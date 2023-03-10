// Decompiled with JetBrains decompiler
// Type: FindAndReplace.Finder
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
  public class Finder
  {
    public string Dir { get; set; }

    public bool IncludeSubDirectories { get; set; }

    public string FileMask { get; set; }

    public string ExcludeFileMask { get; set; }

    public string FindText { get; set; }

    public bool IsCaseSensitive { get; set; }

    public bool FindTextHasRegEx { get; set; }

    public bool SkipBinaryFileDetection { get; set; }

    public Encoding AlwaysUseEncoding { get; set; }

    public Encoding DefaultEncodingIfNotDetected { get; set; }

    public bool IncludeFilesWithoutMatches { get; set; }

    public bool IsSilent { get; set; }

    public bool IsCancelRequested { get; set; }

    public Finder.FindResult Find()
    {
      Verify.Argument.IsNotEmpty(this.Dir, "Dir");
      Verify.Argument.IsNotEmpty(this.FileMask, "FileMask");
      Verify.Argument.IsNotEmpty(this.FindText, "FindText");
      Status status1 = Status.Processing;
      StopWatch.Start("GetFilesInDirectory");
      DateTime now1 = DateTime.Now;
      string[] filesInDirectory = Utils.GetFilesInDirectory(this.Dir, this.FileMask, this.IncludeSubDirectories, this.ExcludeFileMask);
      List<Finder.FindResultItem> findResultItemList = new List<Finder.FindResultItem>();
      Stats stats = new Stats();
      stats.Files.Total = filesInDirectory.Length;
      StopWatch.Stop("GetFilesInDirectory");
      DateTime now2 = DateTime.Now;
      foreach (string filePath in filesInDirectory)
      {
        ++stats.Files.Processed;
        Finder.FindResultItem inFile = this.FindInFile(filePath);
        if (inFile.IsSuccess)
        {
          stats.Matches.Found += inFile.Matches.Count;
          if (inFile.Matches.Count > 0)
            ++stats.Files.WithMatches;
          else
            ++stats.Files.WithoutMatches;
        }
        else
        {
          if (inFile.FailedToOpen)
            ++stats.Files.FailedToRead;
          if (inFile.IsBinaryFile)
            ++stats.Files.Binary;
        }
        stats.UpdateTime(now1, now2);
        if (this.IsCancelRequested)
          status1 = Status.Cancelled;
        if (stats.Files.Total == stats.Files.Processed)
          status1 = Status.Completed;
        if (inFile.IncludeInResultsList)
          findResultItemList.Add(inFile);
        this.OnFileProcessed(new FinderEventArgs(inFile, stats, status1, this.IsSilent));
        if (status1 == Status.Cancelled)
          break;
      }
      if (filesInDirectory.Length == 0)
      {
        Status status2 = Status.Completed;
        this.OnFileProcessed(new FinderEventArgs(new Finder.FindResultItem(), stats, status2, this.IsSilent));
      }
      return new Finder.FindResult()
      {
        Items = findResultItemList,
        Stats = stats
      };
    }

    private Finder.FindResultItem FindInFile(string filePath)
    {
      Finder.FindResultItem findResultItem = new Finder.FindResultItem();
      findResultItem.IsSuccess = true;
      findResultItem.IncludeFilesWithoutMatches = this.IncludeFilesWithoutMatches;
      findResultItem.FileName = Path.GetFileName(filePath);
      findResultItem.FilePath = filePath;
      findResultItem.FileRelativePath = "." + filePath.Substring(this.Dir.Length);
      StopWatch.Start("ReadSampleFileContent");
      byte[] numArray;
      try
      {
        numArray = Utils.ReadFileContentSample(filePath);
      }
      catch (Exception ex)
      {
        StopWatch.Stop("ReadSampleFileContent");
        findResultItem.IsSuccess = false;
        findResultItem.FailedToOpen = true;
        findResultItem.ErrorMessage = ex.Message;
        return findResultItem;
      }
      StopWatch.Stop("ReadSampleFileContent");
      if (!this.SkipBinaryFileDetection)
      {
        StopWatch.Start("IsBinaryFile");
        if (findResultItem.IsSuccess && Utils.IsBinaryFile(numArray))
        {
          StopWatch.Stop("IsBinaryFile");
          findResultItem.IsSuccess = false;
          findResultItem.IsBinaryFile = true;
          return findResultItem;
        }
        StopWatch.Stop("IsBinaryFile");
      }
      Encoding encoding = this.DetectEncoding(numArray);
      if (encoding == null)
      {
        findResultItem.IsSuccess = false;
        findResultItem.FailedToOpen = true;
        findResultItem.ErrorMessage = "Could not detect file encoding.";
        return findResultItem;
      }
      findResultItem.FileEncoding = encoding;
      StopWatch.Start("ReadFullFileContent");
      string end;
      using (StreamReader streamReader = new StreamReader(filePath, encoding))
        end = streamReader.ReadToEnd();
      StopWatch.Stop("ReadFullFileContent");
      StopWatch.Start("FindMatches");
      RegexOptions regExOptions = Utils.GetRegExOptions(this.IsCaseSensitive);
      findResultItem.Matches = Utils.FindMatches(end, this.FindText, this.FindTextHasRegEx, regExOptions);
      StopWatch.Stop("FindMatches");
      findResultItem.NumMatches = findResultItem.Matches.Count;
      return findResultItem;
    }

    private Encoding DetectEncoding(byte[] sampleBytes) => this.AlwaysUseEncoding != null ? this.AlwaysUseEncoding : EncodingDetector.Detect(sampleBytes, defaultEncoding: this.DefaultEncodingIfNotDetected);

    public void CancelFind() => this.IsCancelRequested = true;

    public event FileProcessedEventHandler FileProcessed;

    protected virtual void OnFileProcessed(FinderEventArgs e)
    {
      if (this.FileProcessed == null)
        return;
      this.FileProcessed((object) this, e);
    }

    public class FindResultItem : ResultItem
    {
    }

    public class FindResult
    {
      public List<Finder.FindResultItem> Items { get; set; }

      public Stats Stats { get; set; }

      public List<Finder.FindResultItem> ItemsWithMatches => this.Items.Where<Finder.FindResultItem>((Func<Finder.FindResultItem, bool>) (r => r.NumMatches > 0)).ToList<Finder.FindResultItem>();
    }
  }
}
