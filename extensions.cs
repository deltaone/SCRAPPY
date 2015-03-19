using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using System.Runtime.InteropServices;

namespace Extensions
{
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
    {
        source = source.Replace('\n', ' ').Replace("\r", "");
        if (onlyCRLF) return (source);
        return (source.Replace('\t', ' '));
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

    public static string XmlEscape(this string unescaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerText = unescaped;
        return(node.InnerXml);
    }

    public static string XmlUnescape(this string escaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerXml = escaped;
        return(node.InnerText);
    }
}
public class IniFile
{
    public string path;

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
             string key, string def, StringBuilder retVal,
        int size, string filePath);

    public IniFile(string INIPath)
    {
        path = INIPath;
    }
    
    public void WriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, this.path);
    }
    
    public string ReadValue(string Section, string Key, string Default = "")
    {
        StringBuilder temp = new StringBuilder(255);
        int i = GetPrivateProfileString(Section, Key, Default, temp, 255, this.path);
        return temp.ToString();

    }
}
}