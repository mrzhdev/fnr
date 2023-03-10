// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.CommandLineRunner
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FindAndReplace.App
{
  public class CommandLineRunner
  {
    private CommandLineOptions _options;

    public int Run(string[] args)
    {
      this._options = new CommandLineOptions();
      if (!Parser.Default.ParseArguments(args, (object) this._options))
      {
        Console.ReadKey();
        Environment.Exit(1);
      }
      List<ValidationResult> source = new List<ValidationResult>();
      source.Add(ValidationUtils.IsDirValid(this._options.Dir, "dir"));
      source.Add(ValidationUtils.IsNotEmpty(this._options.FileMask, "fileMask"));
      source.Add(ValidationUtils.IsNotEmpty(this._options.FindText, "find"));
      source.Add(ValidationUtils.IsNotEmpty(this._options.FindText, "find"));
      if (this._options.IsFindTextHasRegEx)
        source.Add(ValidationUtils.IsValidRegExp(this._options.FindText, "find"));
      if (!string.IsNullOrEmpty(this._options.AlwaysUseEncoding))
        source.Add(ValidationUtils.IsValidEncoding(this._options.AlwaysUseEncoding, "alwaysUseEncoding"));
      if (!string.IsNullOrEmpty(this._options.DefaultEncodingIfNotDetected))
        source.Add(ValidationUtils.IsValidEncoding(this._options.DefaultEncodingIfNotDetected, "alwaysUseEncoding"));
      if (!string.IsNullOrEmpty(this._options.LogFile))
        Console.SetOut((TextWriter) new StreamWriter((Stream) new FileStream(this._options.LogFile, FileMode.Create)));
      Console.WriteLine("");
      CommandLineRunner.DosErrorLevel dosErrorLevel;
      if (source.Any<ValidationResult>((Func<ValidationResult, bool>) (vr => !vr.IsSuccess)))
      {
        foreach (ValidationResult validationResult in source)
        {
          if (!validationResult.IsSuccess)
            Console.WriteLine(string.Format("{0}: {1}", (object) validationResult.FieldName, (object) validationResult.ErrorMessage));
        }
        Console.WriteLine("");
        dosErrorLevel = CommandLineRunner.DosErrorLevel.FatalError;
      }
      else
      {
        dosErrorLevel = CommandLineRunner.DosErrorLevel.Success;
        bool findTextHasRegEx = this._options.IsFindTextHasRegEx;
        if (this._options.ReplaceText == null)
        {
          Finder finder = new Finder();
          finder.Dir = this._options.Dir;
          finder.IncludeSubDirectories = this._options.IncludeSubDirectories;
          finder.FileMask = this._options.FileMask;
          finder.ExcludeFileMask = this._options.ExcludeFileMask;
          finder.FindText = CommandLineUtils.DecodeText(this._options.FindText, findTextHasRegEx);
          finder.IsCaseSensitive = this._options.IsCaseSensitive;
          finder.FindTextHasRegEx = findTextHasRegEx;
          finder.SkipBinaryFileDetection = this._options.SkipBinaryFileDetection;
          finder.IncludeFilesWithoutMatches = this._options.IncludeFilesWithoutMatches;
          finder.AlwaysUseEncoding = this.GetEncoding(this._options.AlwaysUseEncoding);
          finder.DefaultEncodingIfNotDetected = this.GetEncoding(this._options.DefaultEncodingIfNotDetected);
          finder.IsSilent = this._options.Silent;
          finder.FileProcessed += new FileProcessedEventHandler(this.OnFinderFileProcessed);
          Finder.FindResult findResult = finder.Find();
          if (this._options.SetErrorLevelIfAnyFileErrors && findResult.Stats.Files.FailedToRead > 0)
            dosErrorLevel = CommandLineRunner.DosErrorLevel.ErrorsInSomeFiles;
        }
        else
        {
          Replacer replacer = new Replacer();
          replacer.Dir = this._options.Dir;
          replacer.IncludeSubDirectories = this._options.IncludeSubDirectories;
          replacer.FileMask = this._options.FileMask;
          replacer.ExcludeFileMask = this._options.ExcludeFileMask;
          replacer.FindText = CommandLineUtils.DecodeText(this._options.FindText, findTextHasRegEx);
          replacer.IsCaseSensitive = this._options.IsCaseSensitive;
          replacer.FindTextHasRegEx = this._options.IsFindTextHasRegEx;
          replacer.SkipBinaryFileDetection = this._options.SkipBinaryFileDetection;
          replacer.IncludeFilesWithoutMatches = this._options.IncludeFilesWithoutMatches;
          replacer.ReplaceText = CommandLineUtils.DecodeText(this._options.ReplaceText);
          replacer.AlwaysUseEncoding = this.GetEncoding(this._options.AlwaysUseEncoding);
          replacer.DefaultEncodingIfNotDetected = this.GetEncoding(this._options.DefaultEncodingIfNotDetected);
          replacer.IsSilent = this._options.Silent;
          replacer.FileProcessed += new ReplaceFileProcessedEventHandler(this.OnReplacerFileProcessed);
          Replacer.ReplaceResult replaceResult = replacer.Replace();
          if (this._options.SetErrorLevelIfAnyFileErrors && (replaceResult.Stats.Files.FailedToRead > 0 || replaceResult.Stats.Files.FailedToWrite > 0))
            dosErrorLevel = CommandLineRunner.DosErrorLevel.ErrorsInSomeFiles;
        }
      }
      if (!string.IsNullOrEmpty(this._options.LogFile))
        Console.Out.Close();
      return (int) dosErrorLevel;
    }

    private Encoding GetEncoding(string encodingName) => string.IsNullOrEmpty(encodingName) ? (Encoding) null : Encoding.GetEncoding(encodingName);

    private void OnFinderFileProcessed(object sender, FinderEventArgs e)
    {
      if (e.ResultItem.IncludeInResultsList && !e.IsSilent)
        this.PrintFinderResultRow(e.ResultItem, e.Stats);
      if (e.Stats.Files.Processed != e.Stats.Files.Total || e.IsSilent)
        return;
      this.PrintStatistics(e.Stats);
    }

    private void OnReplacerFileProcessed(object sender, ReplacerEventArgs e)
    {
      if (e.ResultItem.IncludeInResultsList && !e.IsSilent)
        this.PrintReplacerResultRow(e.ResultItem, e.Stats);
      if (e.Stats.Files.Processed != e.Stats.Files.Total || e.IsSilent)
        return;
      this.PrintStatistics(e.Stats, true);
    }

    public void PrintFinderResultRow(Finder.FindResultItem item, Stats stats)
    {
      this.PrintFileAndEncoding((ResultItem) item);
      if (item.IsSuccess)
        CommandLineRunner.PrintNameValuePair("Matches", item.NumMatches.ToString());
      else
        CommandLineRunner.PrintNameValuePair("Error", item.ErrorMessage);
      Console.WriteLine();
    }

    private void PrintFileAndEncoding(ResultItem item)
    {
      CommandLineRunner.PrintNameValuePair("File", item.FileRelativePath);
      if (!this._options.ShowEncoding || item.FileEncoding == null)
        return;
      CommandLineRunner.PrintNameValuePair("Encoding", item.FileEncoding.WebName);
    }

    public void PrintReplacerResultRow(Replacer.ReplaceResultItem item, Stats stats)
    {
      this.PrintFileAndEncoding((ResultItem) item);
      if (!item.FailedToOpen)
        CommandLineRunner.PrintNameValuePair("Matches", item.NumMatches.ToString());
      CommandLineRunner.PrintNameValuePair("Replaced", item.IsReplaced ? "Yes" : "No");
      if (!item.IsSuccess)
        CommandLineRunner.PrintNameValuePair("Error", item.ErrorMessage);
      Console.WriteLine();
    }

    private static void PrintNameValuePair(string name, string value) => Console.WriteLine((name + ":").PadRight(10) + value);

    public void PrintStatistics(Stats stats, bool isReplacerStats = false)
    {
      Console.WriteLine("");
      Console.WriteLine("====================================");
      Console.WriteLine("Stats");
      Console.WriteLine("");
      Console.WriteLine("Files:");
      Console.WriteLine("- Total: " + (object) stats.Files.Total);
      Console.WriteLine("- Binary: " + (object) stats.Files.Binary + " (skipped)");
      Console.WriteLine("- With Matches: " + (object) stats.Files.WithMatches);
      Console.WriteLine("- Without Matches: " + (object) stats.Files.WithoutMatches);
      Console.WriteLine("- Failed to Open: " + (object) stats.Files.FailedToRead);
      if (isReplacerStats)
        Console.WriteLine("- Failed to Write: " + (object) stats.Files.FailedToWrite);
      Console.WriteLine("");
      Console.WriteLine("Matches:");
      Console.WriteLine("- Found: " + (object) stats.Matches.Found);
      if (isReplacerStats)
        Console.WriteLine("- Replaced: " + (object) stats.Matches.Replaced);
      Console.WriteLine("");
      Console.WriteLine("Duration: " + Math.Round(stats.Time.Passed.TotalSeconds, 3).ToString() + " secs");
      Console.WriteLine("====================================");
    }

    public enum DosErrorLevel
    {
      Success,
      FatalError,
      ErrorsInSomeFiles,
    }
  }
}
