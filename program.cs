using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml;

using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using CSScriptLibrary;

using Extensions;

namespace SCRAPPY { 

public class MAIN
{
    // debug related
    public static bool debug = false;    
    // set AssemlyInfo.cs version to "1.0.*"
    public static Version buildVersion = Assembly.GetExecutingAssembly().GetName().Version;
    public static DateTime buildDate = new DateTime(2000, 1, 1).AddDays(buildVersion.Build).AddSeconds(buildVersion.Revision * 2);
    // path related
    public static string fileExecutable = Assembly.GetExecutingAssembly().Location;
    public static string folderExecutable = Path.GetDirectoryName(fileExecutable) + @"\";
    public static string folderCurrent = Directory.GetCurrentDirectory() + @"\";
    public static string folderTemplates = Path.Combine(folderExecutable, "templates") + @"\";
    public static string folderRules = Path.Combine(folderExecutable, "rules") + @"\";
    public static string folderDebug = Path.Combine(folderExecutable, "debug") + @"\";
    public static string folderCache = Path.Combine(folderExecutable, "cache") + @"\";    
    // log related
    public static string fileLog = Path.Combine(folderDebug, "debug.log");
    // ini related
    public static IniFile INI = new IniFile(Path.Combine(folderExecutable, "scrappy.ini"));
        
    //-------------------------------------------------------------------------    
    public static void log(string message)
    {
        File.AppendAllText(fileLog, message + "\n"); 
    }

    public static void log(string format, params object[] argList)
    {
        File.AppendAllText(fileLog, String.Format(format, argList) + "\n"); 
    }

    public static void logDebug(string message)
    {
        if (debug) File.AppendAllText(fileLog, "DEBUG: " + message + "\n"); 
    }

    public static void logDebug(string format, params object[] argList)
    {
        if (debug) File.AppendAllText(fileLog, "DEBUG: " + String.Format(format, argList) + "\n");
    }

    public static void print(string message)
    {
        Console.Write(message + "\n");
        logDebug(message);
    }

    public static void printf(string format, params object[] argList)
    {
        Console.Write(String.Format(format, argList) + "\n");
        logDebug(format, argList);
    }

    public static string DumpObject(object data, string path = null)
    {
        if (data.GetType() != typeof(string))
            data = (string)(JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings { }));        
        if (!path.IsEmpty()) File.WriteAllText(Path.Combine(folderDebug, path + ".dbg"), (string) data);
        return ((string)data);
    }

    public static string BuildTemplate(string templateFileName, Hashtable args, Hashtable argsDefault = null)
    {
        string result = "";
        try
        {
            VelocityContext context = new VelocityContext();
            StringWriter writer = new StringWriter();
            if(argsDefault != null) foreach (DictionaryEntry a in argsDefault) context.Put((string)a.Key, a.Value);
            foreach (DictionaryEntry a in args) context.Put((string)a.Key, a.Value);
            Velocity.MergeTemplate(templateFileName, Encoding.UTF8.WebName, context, writer);
            result = writer.GetStringBuilder().ToString();
        }
        catch (Exception ex) { print("\nEXCEPTION:\n" + ex.Message + "\n" + ex.StackTrace); }
        return (result);
    }

    private static string _GetRuleBody(string url, ref string ruleWildcard)
    {
        string rule = "";
        bool found = false;

        List<string> filePaths = new List<string>(Directory.GetFiles(folderRules, "*.cs"));

        var index = filePaths.FindIndex(item => item.EndsWith("default.cs"));
        if (index > 0) filePaths.Swap(0, index);

        foreach (string filePath in filePaths)
        {
            var data = File.ReadAllLines(filePath);
            logDebug("_getRuleBody() - parse rule file: " + filePath);
            foreach (string line in data)
            {
                Match match = Regex.Match(line, "^\\s*<\\[(.+)\\]>\\s*$", RegexOptions.IgnoreCase);
                if (!found)
                {
                    if (!match.Success) continue;
                    ruleWildcard = match.Groups[1].Value;
                    string pattern = "^" + Regex.Escape(match.Groups[1].Value).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
                    logDebug("_getRuleBody() -- section: [" + match.Groups[1].Value + "] => regexp: " + pattern);
                    match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
                    if (!match.Success) continue;
                    logDebug("_getRuleBody() section found: " + pattern);
                    found = true;
                    continue;
                }
                if (match.Success) break;
                rule += line + "\r\n";
            }
            if (found) break;
        }
        return (rule);
    }
    
    private static void _ProcessFile(string inFile, string outFile)
    {
        print("Input file: " + inFile);
        print("Output file: " + outFile);

        var pageText = File.ReadAllText(inFile);
        if (debug) DumpObject(pageText, "source");
        
        // split header and body
        Match match = Regex.Match(pageText, "^\\s*\\[HEADER BEGIN\\]\\s*(.*?)\\s*\\[HEADER END\\]\\s*(.+)\\s*$",
                                    RegexOptions.Singleline | RegexOptions.IgnoreCase); 
        if (!match.Success)
        {
            print("ERROR: Malformed input file (no header)!");
            return;
        }
        else if(match.Groups[1].Value.Length > 256)
        {
            print("ERROR: Malformed input file (URL length > 256)!");
            return;
        }
        
        pageText = match.Groups[2].Value;
        string pageURL = match.Groups[1].Value;
        if (debug) DumpObject(pageText, "page");
      
        // try read rule body
        logDebug("_ProcessFile() page url: " + pageURL);
        string ruleWildcard = "";
        string ruleBody = _GetRuleBody(pageURL, ref ruleWildcard);
        if (ruleBody.IsEmpty())
        {
            print("ERROR: Rule not found!");
            return;
        }
        if (debug) DumpObject(ruleBody, "rule");

        // build rule
        var args = new Hashtable {{"ProcessorCodeText", ruleBody}};
        string scriptCode = BuildTemplate("processor.cs", args);
        if (scriptCode.IsEmpty()) return;

        if (debug) DumpObject(scriptCode, "script");

        // execute rule
        AsmHelper helper;
        args = new Hashtable {
                        {"TEMPLATE", "default.st" },
                        {"PageTitle", "Scrapped page"},
                        {"PageDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")},
                        {"PageURL", pageURL}};
        string asmFileName = Path.Combine(folderCache, scriptCode.GetMD5() + scriptCode.Length.ToString("X") + ".dll");
        try
        {
            if (File.Exists(asmFileName))
            {
                logDebug("_ProcessFile() use precompilled assembly '{0}'!", asmFileName);                                
                helper = new AsmHelper(Assembly.LoadFrom(asmFileName));
            }
            else
            {
                logDebug("_ProcessFile() compille assembly to '{0}'!", asmFileName);
                helper = new AsmHelper(CSScript.LoadCode(scriptCode, asmFileName, true, null));                
            }
            args = (Hashtable)helper.Invoke("Script.Process", pageText, args);
            print("Scraping page - OK!");
        }
        catch (Exception ex)
        {
            print("\nEXCEPTION:\n" + ex.Message + "\n" + ex.StackTrace);
            return;
        }
        if (debug) DumpObject(args, "args");

        // build resulting text
        string result = BuildTemplate((string)(args.ContainsKey("TEMPLATE") ? args["TEMPLATE"]: "default.st"), args);
        if (result.IsEmpty()) return;

        if (debug) DumpObject(result, "result");

        // store result file
        File.WriteAllText(outFile, result);
        print("Save result - OK!");
    }

    private static void _SetupFolders()
    {
        List<string> filePaths;
        // create folder structure
        if (!Directory.Exists(folderDebug)) Directory.CreateDirectory(folderDebug);
        if (!Directory.Exists(folderRules)) Directory.CreateDirectory(folderRules);
        if (!Directory.Exists(folderTemplates)) Directory.CreateDirectory(folderTemplates);
        // cleanup debug folder
        filePaths = new List<string>(Directory.GetFiles(folderDebug, "*.dbg"));
        foreach (string filePath in filePaths) File.Delete(filePath);
        File.Delete(fileLog);
        // cleanup cache folder
        DateTime deadLine = new DateTime(1, 1, 1);
        filePaths = new List<string>(Directory.GetFiles(folderRules, "*.cs"));
        foreach (string filePath in filePaths)
        {
            DateTime deadLineNew = File.GetLastWriteTimeUtc(filePath);
            if (deadLineNew > deadLine) deadLine = deadLineNew;
        }
        filePaths = new List<string>(Directory.GetFiles(folderCache, "*.dll"));
        filePaths.AddRange(Directory.GetFiles(folderCache, "*.pdb"));
        foreach (string filePath in filePaths) if (File.GetLastWriteTimeUtc(filePath) < deadLine) File.Delete(filePath);
    }
       
    private static void Main(string[] args)
    {
        _SetupFolders();
        debug = INI.ReadValue("main", "debug", "0") == "1" ? true : false;
        print("Written by de1ta0ne" +
            "\nVersion: " + buildVersion.Major.ToString() + "." + buildVersion.Minor.ToString() + " build " + buildVersion.Build +
            " [" + buildDate.ToString("dd.MM.yyyy") + "]" + //  HH:mm:ss
            "\nDescription: build html page from templates using scraped data ...\n");

        if (args.Length < 1 || !File.Exists(args[0]))
        {
            print("Usage:\n\n" +
                  "    scrappy.exe <INPUT FORMATTED TEXT FILE> [OUTPUT HTML FILE]\n" +
                  "\n\n(c) 2015");
        }
        else
        {
            Velocity.SetProperty("file.resource.loader.path", folderTemplates);
            Velocity.Init();
            _ProcessFile(args[0], args.Length >= 2 ? args[1] : args[0]);            
            print("\nDone!");
        }

        // Console.ReadKey(true);
    }
}

} // namespace