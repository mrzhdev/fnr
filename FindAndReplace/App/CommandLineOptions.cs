// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.CommandLineOptions
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine;
using CommandLine.Text;
using System;

namespace FindAndReplace.App
{
  public class CommandLineOptions
  {
    [ParserState]
    public IParserState LastParserState { get; set; }

    [Option("cl", HelpText = "Required to run on command line.")]
    public bool UseCommandLine { get; set; }

    [Option("find", HelpText = "Text to find.", Required = true)]
    public string FindText { get; set; }

    [Option("replace", HelpText = "Replacement text.")]
    public string ReplaceText { get; set; }

    [Option("caseSensitive", HelpText = "Case-sensitive.")]
    public bool IsCaseSensitive { get; set; }

    [Option("useRegEx", HelpText = "Find text has Regular Expression.")]
    public bool IsFindTextHasRegEx { get; set; }

    [Option("dir", HelpText = "Directory path.", Required = true)]
    public string Dir { get; set; }

    [Option("includeSubDirectories", HelpText = "Include files in SubDirectories.")]
    public bool IncludeSubDirectories { get; set; }

    [Option("fileMask", HelpText = "File mask.", Required = true)]
    public string FileMask { get; set; }

    [Option("excludeFileMask", HelpText = "Exclude file mask.")]
    public string ExcludeFileMask { get; set; }

    [Option("skipBinaryFileDetection", HelpText = "Ignore detection of binary files.")]
    public bool SkipBinaryFileDetection { get; set; }

    [Option("showEncoding", HelpText = "Display detected encoding information for each fle.")]
    public bool ShowEncoding { get; set; }

    [Option("alwaysUseEncoding", HelpText = "Skip encoding detection and always use specified encoding.")]
    public string AlwaysUseEncoding { get; set; }

    [Option("defaultEncodingIfNotDetected", HelpText = "If encoding is not detected in some very rare cases, use this one.")]
    public string DefaultEncodingIfNotDetected { get; set; }

    [Option("includeFilesWithoutMatches", HelpText = "Include files without matches in results.")]
    public bool IncludeFilesWithoutMatches { get; set; }

    [Option("setErrorLevelIfAnyFileErrors", HelpText = "Return ErrorLevel 2 if any files have read/write errors.")]
    public bool SetErrorLevelIfAnyFileErrors { get; set; }

    [Option("silent", HelpText = "Supress the command window output.")]
    public bool Silent { get; set; }

    [Option("logFile", HelpText = "Path to log file where to save command output.")]
    public string LogFile { get; set; }

    [HelpOption("help", HelpText = "Display this help screen.")]
    public string GetUsage()
    {
      HelpText help = new HelpText("Find And Replace");
      help.Copyright = (string) new CopyrightInfo("ENTech Solutions", DateTime.Now.Year);
      if (this.LastParserState != null && this.LastParserState.Errors.Count > 0)
      {
        this.HandleParsingErrorsInHelp(help);
      }
      else
      {
        help.AddPreOptionsLine("Usage: \n\nfnr.exe --cl --find \"Text To Find\" --replace \"Text To Replace\"  --caseSensitive  --dir \"Directory Path\" --fileMask \"*.*\"  --includeSubDirectories --useRegEx");
        help.AddPreOptionsLine("\n");
        help.AddPreOptionsLine("Mask new line and quote characters using \\n and \\\".");
        help.AddOptions((object) this);
      }
      return (string) help;
    }

    private void HandleParsingErrorsInHelp(HelpText help)
    {
      string str = help.RenderParsingErrorsText((object) this, 2);
      if (string.IsNullOrEmpty(str))
        return;
      help.MaximumDisplayWidth = 160;
      help.AddPreOptionsLine("\n" + "ERROR(S):");
      help.AddPreOptionsLine(str);
      help.AddPreOptionsLine("Use 'fnr.exe --cl --help' to see help for this command.");
    }
  }
}
