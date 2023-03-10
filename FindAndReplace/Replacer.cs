// Decompiled with JetBrains decompiler
// Type: FindAndReplace.Replacer
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FindAndReplace
{
  public class Replacer
  {
    public string Dir { get; set; }

    public bool IncludeSubDirectories { get; set; }

    public string FileMask { get; set; }

    public string ExcludeFileMask { get; set; }

    public string FindText { get; set; }

    public bool IsCaseSensitive { get; set; }

    public bool FindTextHasRegEx { get; set; }

    public bool SkipBinaryFileDetection { get; set; }

    public bool IncludeFilesWithoutMatches { get; set; }

    public string ReplaceText { get; set; }

    public Encoding AlwaysUseEncoding { get; set; }

    public Encoding DefaultEncodingIfNotDetected { get; set; }

    public bool IsCancelRequested { get; set; }

    public bool IsSupressOutput { get; set; }

    public bool IsSilent { get; set; }

    public Replacer.ReplaceResult Replace()
    {
      Verify.Argument.IsNotEmpty(this.Dir, "Dir");
      Verify.Argument.IsNotEmpty(this.FileMask, "FileMask");
      Verify.Argument.IsNotEmpty(this.FindText, "FindText");
      Verify.Argument.IsNotNull((object) this.ReplaceText, "ReplaceText");
      Status status1 = Status.Processing;
      DateTime now1 = DateTime.Now;
      string[] filesInDirectory = Utils.GetFilesInDirectory(this.Dir, this.FileMask, this.IncludeSubDirectories, this.ExcludeFileMask);
      List<Replacer.ReplaceResultItem> replaceResultItemList = new List<Replacer.ReplaceResultItem>();
      Stats stats = new Stats();
      stats.Files.Total = filesInDirectory.Length;
      DateTime now2 = DateTime.Now;
      foreach (string filePath in filesInDirectory)
      {
        Replacer.ReplaceResultItem resultItem = this.ReplaceTextInFile(filePath);
        ++stats.Files.Processed;
        stats.Matches.Found += resultItem.NumMatches;
        if (resultItem.IsSuccess)
        {
          if (resultItem.NumMatches > 0)
          {
            ++stats.Files.WithMatches;
            stats.Matches.Replaced += resultItem.NumMatches;
          }
          else
            ++stats.Files.WithoutMatches;
        }
        else
        {
          if (resultItem.FailedToOpen)
            ++stats.Files.FailedToRead;
          if (resultItem.IsBinaryFile)
            ++stats.Files.Binary;
          if (resultItem.FailedToWrite)
            ++stats.Files.FailedToWrite;
        }
        if (resultItem.IncludeInResultsList)
          replaceResultItemList.Add(resultItem);
        stats.UpdateTime(now1, now2);
        if (this.IsCancelRequested)
          status1 = Status.Cancelled;
        if (stats.Files.Total == stats.Files.Processed)
          status1 = Status.Completed;
        this.OnFileProcessed(new ReplacerEventArgs(resultItem, stats, status1, this.IsSilent));
        if (status1 == Status.Cancelled)
          break;
      }
      if (filesInDirectory.Length == 0)
      {
        Status status2 = Status.Completed;
        this.OnFileProcessed(new ReplacerEventArgs(new Replacer.ReplaceResultItem(), stats, status2, this.IsSilent));
      }
      return new Replacer.ReplaceResult()
      {
        ResultItems = replaceResultItemList,
        Stats = stats
      };
    }

    private Replacer.ReplaceResultItem ReplaceTextInFile(string filePath)
    {
      string str1 = string.Empty;
      Replacer.ReplaceResultItem replaceResultItem = new Replacer.ReplaceResultItem();
      replaceResultItem.IsSuccess = true;
      replaceResultItem.IncludeFilesWithoutMatches = this.IncludeFilesWithoutMatches;
      replaceResultItem.FileName = Path.GetFileName(filePath);
      replaceResultItem.FilePath = filePath;
      replaceResultItem.FileRelativePath = "." + filePath.Substring(this.Dir.Length);
      byte[] numArray;
      try
      {
        numArray = Utils.ReadFileContentSample(filePath);
      }
      catch (Exception ex)
      {
        replaceResultItem.IsSuccess = false;
        replaceResultItem.FailedToOpen = true;
        replaceResultItem.ErrorMessage = ex.Message;
        return replaceResultItem;
      }
      if (!this.SkipBinaryFileDetection && replaceResultItem.IsSuccess && Utils.IsBinaryFile(numArray))
      {
        replaceResultItem.IsSuccess = false;
        replaceResultItem.IsBinaryFile = true;
        return replaceResultItem;
      }
      if (!replaceResultItem.IsSuccess)
        return replaceResultItem;
      Encoding encoding = this.DetectEncoding(numArray);
      if (encoding == null)
      {
        replaceResultItem.IsSuccess = false;
        replaceResultItem.FailedToOpen = true;
        replaceResultItem.ErrorMessage = "Could not detect file encoding.";
        return replaceResultItem;
      }
      replaceResultItem.FileEncoding = encoding;
      using (StreamReader streamReader = new StreamReader(filePath, encoding))
        str1 = streamReader.ReadToEnd();
      RegexOptions regExOptions = Utils.GetRegExOptions(this.IsCaseSensitive);
      List<LiteMatch> matches = Utils.FindMatches(str1, this.FindText, this.FindTextHasRegEx, regExOptions);
      replaceResultItem.NumMatches = matches.Count;
      replaceResultItem.Matches = matches;
      if (matches.Count > 0)
      {
        string pattern = this.FindText;
        if (!this.FindTextHasRegEx)
          pattern = Regex.Escape(this.FindText);
        string str2 = Regex.Replace(str1, pattern, this.ReplaceText, regExOptions);
        try
        {
          using (StreamWriter streamWriter = new StreamWriter(filePath, false, encoding))
            streamWriter.Write(str2);
        }
        catch (Exception ex)
        {
          replaceResultItem.IsSuccess = false;
          replaceResultItem.FailedToWrite = true;
          replaceResultItem.ErrorMessage = ex.Message;
        }
      }
      return replaceResultItem;
    }

    private Encoding DetectEncoding(byte[] sampleBytes) => this.AlwaysUseEncoding != null ? this.AlwaysUseEncoding : EncodingDetector.Detect(sampleBytes, defaultEncoding: this.DefaultEncodingIfNotDetected);

    public void CancelReplace() => this.IsCancelRequested = true;

    public event ReplaceFileProcessedEventHandler FileProcessed;

    protected virtual void OnFileProcessed(ReplacerEventArgs e)
    {
      if (this.FileProcessed == null)
        return;
      this.FileProcessed((object) this, e);
    }

    public class ReplaceResultItem : ResultItem
    {
      public bool FailedToWrite { get; set; }
    }

    public class ReplaceResult
    {
      public List<Replacer.ReplaceResultItem> ResultItems { get; set; }

      public Stats Stats { get; set; }
    }
  }
}
