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
using SCRAPPY;

public static class ExtensionMethods
{   // https://msdn.microsoft.com/en-us/library/bb383977.aspx
    public static void Swap<T>(this List<T> list, int index1, int index2)
    {
        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public static bool IsEmpty(this string source)
    {   // bool isTrulyEmpty = String.IsNullOrWhiteSpace(source); // DOTNET4
        if (String.IsNullOrEmpty(source) || source.Trim().Length == 0) return (true);
        return (false);
    }

    public static string Clean(this string source, bool onlyCRLF)
    {   // source = Regex.Replace(source, @"[\s\r\n]+", " ").Trim();
        source = source.Replace(@"[\r\n]+", " ");
        if (onlyCRLF) return (source);
        return (source.Replace(@"[\t]+", " "));
    }

    public static string Clean(this string source)
    {
        return (Clean(source, false));
    }

    public static string Place(this string placeholder, string source)
    {
        if ((source.IsEmpty())) return ("");
        return (String.Format(placeholder, source));
    }

    public static string GetMD5(this string source)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
        return (BitConverter.ToString(checkSum).Replace("-", String.Empty));
    }

    public static string HTMLStripTags(this string source)
    { // http://haacked.com/archive/2005/04/22/Matching_HTML_With_Regex.aspx/	
        return Regex.Replace(source, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", 
					string.Empty, RegexOptions.Singleline);
    }
	
    public static string XMLEscape(this string unescaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerText = unescaped;
        return(node.InnerXml);
    }

    public static string XMLUnescape(this string escaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerXml = escaped;
        return(node.InnerText);
    }
}

public class Script
{
    public static string GetStringByRegex(string text, string pattern, string template)
    {
        Match match = Regex.Match(text, pattern, RegexOptions.Singleline);
        if (!match.Success) return ("");
        for (int k = 0; k < match.Groups.Count; k++)                    
                template = template.Replace("$" + k.ToString(), match.Groups[k].Value);        
        return (template);
    }

    public static string GetStringByWildcard(string text, string wildcard, string template)
    {
        string pattern = Regex.Escape(wildcard).Replace(@"\?", ".").Replace(@"\*", ".*?").Replace(@"\(", "(").Replace(@"\)", ")");
        return(GetStringByRegex(text, pattern, template));
    }

    public static string[] GetStringsByRegex(string text, string pattern, string template)
    {
        MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Singleline);
        var result = new string[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            string tpl = template;
            for (int k = 0; k < matches[i].Groups.Count; k++)                    
                tpl = tpl.Replace("$" + k.ToString(), matches[i].Groups[k].Value);
            result[i] = tpl;
        }
        return (result);
    }

    public static string[] GetStringsByWildcard(string text, string wildcard, string template)
    {
        string pattern = Regex.Escape(wildcard).Replace(@"\?", ".").Replace(@"\*", ".*?").Replace(@"\(", "(").Replace(@"\)", ")");
        return (GetStringsByRegex(text, pattern, template));
    }
	
    public static List<Hashtable> ExtractToHashtables(string[] strings, string[,] columns)
    {
        var table = new List<Hashtable>();
        foreach (string line in strings)
        {
            var row = new Hashtable();
            for (int i = 0; i <= columns.GetUpperBound(0); i++)
			{
				row[columns[i, 0]] = GetStringByRegex(line, columns[i, 1], columns[i, 2]);				
			}
            table.Add(row);
        }
        return (table);
    }

    public static List<string[]> ExtractToArrays(string[] strings, string[,] columns)
    {
        var table = new List<string[]>();
        // build header record        
        var row = new string[columns.GetUpperBound(0) + 1];
        for (int i = 0; i <= columns.GetUpperBound(0); i++) row[i] = columns[i, 0];
        table.Add(row);
        // build main body
        foreach (string line in strings)
        {
            row = new string[columns.GetUpperBound(0) + 1];
            for (int i = 0; i <= columns.GetUpperBound(0); i++) 
			{
                row[i] = GetStringByRegex(line, columns[i, 1], columns[i, 2]);
			}
            table.Add(row);
        }
        return (table);
    }

    public static List<Hashtable> HTMLStripTags(List<Hashtable> table, string columns)
    {
        string[] c = Regex.Split(columns, @"\s*,\s*");
        if(c.Length < 1) return(table);
        foreach(var row in table) 
            foreach (var column in c)
                row[column] = ((string)row[column]).HTMLStripTags();
        return(table);
    }

    public static List<string[]> HTMLStripTags(List<string[]> table, string columns)
    {
        string[] c = Regex.Split(columns, @"\s*,\s*");
        if(c.Length < 1) return(table);
        foreach(var row in table) 
            foreach (string column in c)
                row[Convert.ToInt32(column)] = ((string)row[Convert.ToInt32(column)]).HTMLStripTags();
        return(table);
    }

//---------------------------------------------------------------
$ProcessorCodeText
//---------------------------------------------------------------        
}
