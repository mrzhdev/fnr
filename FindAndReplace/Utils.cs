// Decompiled with JetBrains decompiler
// Type: FindAndReplace.Utils
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FindAndReplace
{
  public static class Utils
  {
    public static RegexOptions GetRegExOptions(bool isCaseSensitive)
    {
      RegexOptions regexOptions = (RegexOptions) (0 | 2);
      if (!isCaseSensitive)
        regexOptions |= RegexOptions.IgnoreCase;
      return regexOptions;
    }

    public static string[] GetFilesInDirectory(
      string dir,
      string fileMask,
      bool includeSubDirectories,
      string excludeMask)
    {
      SearchOption searchOption = includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
      List<string> source = new List<string>();
      string str1 = fileMask;
      char[] chArray1 = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray1))
        source.AddRange((IEnumerable<string>) Directory.GetFiles(dir, str2.Trim(), searchOption));
      List<string> stringList1 = source.Distinct<string>().ToList<string>();
      if (!string.IsNullOrEmpty(excludeMask))
      {
        List<string> stringList2 = new List<string>();
        string str2 = excludeMask;
        char[] chArray2 = new char[1]{ ',' };
        foreach (string pattern in ((IEnumerable<string>) str2.Split(chArray2)).ToList<string>().Select<string, string>((Func<string, string>) (fm => Utils.WildcardToRegex(fm.Trim()))).ToList<string>())
        {
          foreach (string path in stringList1)
          {
            string fileName = Path.GetFileName(path);
            if (fileName != null && !Regex.IsMatch(fileName, pattern))
              stringList2.Add(path);
          }
          stringList1 = stringList2;
          stringList2 = new List<string>();
        }
      }
      stringList1.Sort();
      return stringList1.ToArray();
    }

    public static FileGetter CreateFileGetter(
      string dir,
      string fileMask,
      bool includeSubDirectories,
      string excludeMask)
    {
      SearchOption searchOption = includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
      List<string> list = ((IEnumerable<string>) fileMask.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (fm => fm.Trim())).ToList<string>();
      List<string> stringList = (List<string>) null;
      if (!string.IsNullOrEmpty(excludeMask))
        stringList = ((IEnumerable<string>) excludeMask.Split(',')).ToList<string>().Select<string, string>((Func<string, string>) (fm => fm.Trim())).ToList<string>();
      return new FileGetter()
      {
        DirPath = dir,
        FileMasks = list,
        ExcludeFileMasks = stringList,
        SearchOption = searchOption,
        UseBlockingCollection = false
      };
    }

    public static bool IsBinaryFile(string fileContent) => fileContent.Contains("\0\0\0\0");

    public static bool IsBinaryFile(byte[] bytes) => Utils.IsBinaryFile(Encoding.Default.GetString(bytes));

    public static List<MatchPreviewLineNumber> GetLineNumbersForMatchesPreview(
      string fileContent,
      List<LiteMatch> matches,
      int replaceStrLength = 0,
      bool isReplace = false)
    {
      string newLine = Environment.NewLine;
      string[] strArray = fileContent.Split(new string[1]
      {
        newLine
      }, StringSplitOptions.None);
      List<MatchPreviewLineNumber> source = new List<MatchPreviewLineNumber>();
      int replacedTextLength = 0;
      foreach (LiteMatch match in matches)
      {
        int num1 = Utils.DetectMatchLine(((IEnumerable<string>) strArray).ToArray<string>(), Utils.GetMatchIndex(match.Index, replacedTextLength, isReplace));
        int num2 = Utils.DetectMatchLine(((IEnumerable<string>) strArray).ToArray<string>(), Utils.GetMatchIndex(match.Index + replaceStrLength, replacedTextLength, isReplace));
        replacedTextLength += match.Length;
        for (int index = num1 - 2; index <= num2 + 2; ++index)
        {
          if (index >= 0 && index < ((IEnumerable<string>) strArray).Count<string>())
            source.Add(new MatchPreviewLineNumber()
            {
              LineNumber = index,
              HasMatch = index >= num1 && index <= num2
            });
        }
      }
      return source.Distinct<MatchPreviewLineNumber>((IEqualityComparer<MatchPreviewLineNumber>) new LineNumberComparer()).OrderBy<MatchPreviewLineNumber, int>((Func<MatchPreviewLineNumber, int>) (ln => ln.LineNumber)).ToList<MatchPreviewLineNumber>();
    }

    public static string FormatTimeSpan(TimeSpan timeSpan)
    {
      string str = string.Empty;
      int hours = timeSpan.Hours;
      int minutes = timeSpan.Minutes;
      int seconds = timeSpan.Seconds;
      if (hours > 0)
      {
        str += string.Format("{0}h ", (object) hours);
        if (minutes > 0)
        {
          str += string.Format("{0}m ", (object) minutes);
          if (seconds > 0)
            str += string.Format("{0}s ", (object) seconds);
        }
        else if (seconds > 0)
          str = str + string.Format("{0}m ", (object) minutes) + string.Format("{0}s ", (object) seconds);
      }
      else
      {
        if (minutes > 0)
          str += string.Format("{0}m ", (object) minutes);
        if (seconds > 0)
          str += string.Format("{0}s ", (object) seconds);
      }
      return str;
    }

    private static int DetectMatchLine(string[] lines, int position)
    {
      int num = 2;
      int index1 = 0;
      for (int index2 = lines[0].Length + num; index2 <= position; index2 += lines[index1].Length + num)
        ++index1;
      return index1;
    }

    internal static string WildcardToRegex(string pattern) => string.Format("^{0}$", (object) Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", "."));

    public static byte[] ReadFileContentSample(string filePath, int maxSize = 10240)
    {
      byte[] buffer;
      using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        long length = Math.Min(fileStream.Length, (long) maxSize);
        buffer = new byte[length];
        fileStream.Read(buffer, 0, (int) length);
      }
      return buffer;
    }

    private static int GetMatchIndex(int originalIndex, int replacedTextLength, bool isReplace = false) => !isReplace ? originalIndex : originalIndex - replacedTextLength;

    public static List<LiteMatch> FindMatches(
      string fileContent,
      string findText,
      bool findTextHasRegEx,
      RegexOptions regexOptions)
    {
      MatchCollection matchCollection = findTextHasRegEx ? Regex.Matches(fileContent, findText, regexOptions) : Regex.Matches(fileContent, Regex.Escape(findText), regexOptions);
      List<LiteMatch> liteMatchList = new List<LiteMatch>();
      foreach (Match match in matchCollection)
        liteMatchList.Add(new LiteMatch()
        {
          Index = match.Index,
          Length = match.Length
        });
      return liteMatchList;
    }
  }
}
