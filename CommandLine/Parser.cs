// Decompiled with JetBrains decompiler
// Type: CommandLine.Parser
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using CommandLine.Parsing;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CommandLine
{
  public sealed class Parser : IDisposable
  {
    public const int DefaultExitCodeFail = 1;
    private static readonly Parser DefaultParser = new Parser(true);
    private readonly ParserSettings _settings;
    private bool _disposed;

    public Parser() => this._settings = new ParserSettings()
    {
      Consumed = true
    };

    [Obsolete("Use constructor that accepts Action<ParserSettings>.")]
    public Parser(ParserSettings settings)
    {
      Assumes.NotNull<ParserSettings>(settings, nameof (settings), "The command line parser settings instance cannot be null.");
      this._settings = !settings.Consumed ? settings : throw new InvalidOperationException("The command line parserSettings instance cannnot be used more than once.");
      this._settings.Consumed = true;
    }

    public Parser(Action<ParserSettings> configuration)
    {
      Assumes.NotNull<Action<ParserSettings>>(configuration, nameof (configuration), "The command line parser settings delegate cannot be null.");
      this._settings = new ParserSettings();
      configuration(this.Settings);
      this._settings.Consumed = true;
    }

    private Parser(bool singleton)
      : this((Action<ParserSettings>) (with =>
      {
        with.CaseSensitive = false;
        with.MutuallyExclusive = false;
        with.HelpWriter = Console.Error;
        with.ParsingCulture = CultureInfo.InvariantCulture;
      }))
    {
    }

    ~Parser() => this.Dispose(false);

    public static Parser Default => Parser.DefaultParser;

    public ParserSettings Settings => this._settings;

    public bool ParseArguments(string[] args, object options)
    {
      Assumes.NotNull<string[]>(args, nameof (args), "The arguments string array cannot be null.");
      Assumes.NotNull<object>(options, nameof (options), "The target options instance cannot be null.");
      return this.DoParseArguments(args, options);
    }

    public bool ParseArguments(string[] args, object options, Action<string, object> onVerbCommand)
    {
      Assumes.NotNull<string[]>(args, nameof (args), "The arguments string array cannot be null.");
      Assumes.NotNull<object>(options, nameof (options), "The target options instance cannot be null.");
      Assumes.NotNull<object>(options, nameof (onVerbCommand), "Delegate executed to capture verb command instance reference cannot be null.");
      object verbInstance = (object) null;
      bool argumentsVerbs = this.DoParseArgumentsVerbs(args, options, ref verbInstance);
      onVerbCommand(((IEnumerable<string>) args).FirstOrDefault<string>() ?? string.Empty, argumentsVerbs ? verbInstance : (object) null);
      return argumentsVerbs;
    }

    public bool ParseArgumentsStrict(string[] args, object options, Action onFail = null)
    {
      Assumes.NotNull<string[]>(args, nameof (args), "The arguments string array cannot be null.");
      Assumes.NotNull<object>(options, nameof (options), "The target options instance cannot be null.");
      if (this.DoParseArguments(args, options))
        return true;
      this.InvokeAutoBuildIfNeeded(options);
      if (onFail == null)
        Environment.Exit(1);
      else
        onFail();
      return false;
    }

    public bool ParseArgumentsStrict(
      string[] args,
      object options,
      Action<string, object> onVerbCommand,
      Action onFail = null)
    {
      Assumes.NotNull<string[]>(args, nameof (args), "The arguments string array cannot be null.");
      Assumes.NotNull<object>(options, nameof (options), "The target options instance cannot be null.");
      Assumes.NotNull<object>(options, nameof (onVerbCommand), "Delegate executed to capture verb command instance reference cannot be null.");
      object verbInstance = (object) null;
      if (!this.DoParseArgumentsVerbs(args, options, ref verbInstance))
      {
        onVerbCommand(((IEnumerable<string>) args).FirstOrDefault<string>() ?? string.Empty, (object) null);
        this.InvokeAutoBuildIfNeeded(options);
        if (onFail == null)
          Environment.Exit(1);
        else
          onFail();
        return false;
      }
      onVerbCommand(((IEnumerable<string>) args).FirstOrDefault<string>() ?? string.Empty, verbInstance);
      return true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal static object InternalGetVerbOptionsInstanceByName(
      string verb,
      object target,
      out bool found)
    {
      found = false;
      if (string.IsNullOrEmpty(verb))
        return target;
      Pair<PropertyInfo, VerbOptionAttribute> pair = ReflectionHelper.RetrieveOptionProperty<VerbOptionAttribute>(target, verb);
      found = pair != null;
      return !found ? target : pair.Left.GetValue(target, (object[]) null);
    }

    private static void SetParserStateIfNeeded(object options, IEnumerable<ParsingError> errors)
    {
      if (!options.CanReceiveParserState())
        return;
      PropertyInfo left = ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(options)[0].Left;
      object obj = left.GetValue(options, (object[]) null);
      if (obj != null)
      {
        if (!(obj is IParserState))
          throw new InvalidOperationException("Cannot apply ParserStateAttribute to a property that does not implement IParserState or is not accessible.");
        if (!(obj is ParserState))
          throw new InvalidOperationException("ParserState instance cannot be supplied.");
      }
      else
      {
        try
        {
          left.SetValue(options, (object) new ParserState(), (object[]) null);
        }
        catch (Exception ex)
        {
          throw new InvalidOperationException("Cannot apply ParserStateAttribute to a property that does not implement IParserState or is not accessible.", ex);
        }
      }
      IParserState parserState = (IParserState) left.GetValue(options, (object[]) null);
      foreach (ParsingError error in errors)
        parserState.Errors.Add(error);
    }

    private static StringComparison GetStringComparison(ParserSettings settings) => !settings.CaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    private bool DoParseArguments(string[] args, object options)
    {
      Pair<MethodInfo, HelpOptionAttribute> pair = ReflectionHelper.RetrieveMethod<HelpOptionAttribute>(options);
      TextWriter helpWriter = this._settings.HelpWriter;
      if (pair == null || helpWriter == null)
        return this.DoParseArgumentsCore(args, options);
      if (!this.ParseHelp(args, pair.Right) && this.DoParseArgumentsCore(args, options))
        return true;
      string text;
      HelpOptionAttribute.InvokeMethod(options, pair, out text);
      helpWriter.Write(text);
      return false;
    }

    private bool DoParseArgumentsCore(string[] args, object options)
    {
      bool flag = false;
      OptionMap map = OptionMap.Create(options, this._settings);
      map.SetDefaults();
      ValueMapper valueMapper = new ValueMapper(options, this._settings.ParsingCulture);
      StringArrayEnumerator stringArrayEnumerator = new StringArrayEnumerator(args);
      while (stringArrayEnumerator.MoveNext())
      {
        string current = stringArrayEnumerator.Current;
        if (!string.IsNullOrEmpty(current))
        {
          ArgumentParser argumentParser = ArgumentParser.Create(current, this._settings.IgnoreUnknownArguments);
          if (argumentParser != null)
          {
            PresentParserState presentParserState = argumentParser.Parse((IArgumentEnumerator) stringArrayEnumerator, map, options);
            if ((presentParserState & PresentParserState.Failure) == PresentParserState.Failure)
            {
              Parser.SetParserStateIfNeeded(options, (IEnumerable<ParsingError>) argumentParser.PostParsingState);
              flag = true;
            }
            else if ((presentParserState & PresentParserState.MoveOnNextElement) == PresentParserState.MoveOnNextElement)
              stringArrayEnumerator.MoveNext();
          }
          else if (valueMapper.CanReceiveValues && !valueMapper.MapValueItem(current))
            flag = true;
        }
      }
      return !(flag | !map.EnforceRules());
    }

    private bool DoParseArgumentsVerbs(string[] args, object options, ref object verbInstance)
    {
      IList<Pair<PropertyInfo, VerbOptionAttribute>> verbs = ReflectionHelper.RetrievePropertyList<VerbOptionAttribute>(options);
      Pair<MethodInfo, HelpVerbOptionAttribute> helpInfo = ReflectionHelper.RetrieveMethod<HelpVerbOptionAttribute>(options);
      if (args.Length == 0)
      {
        if (helpInfo != null || this._settings.HelpWriter != null)
          this.DisplayHelpVerbText(options, helpInfo, (string) null);
        return false;
      }
      OptionMap optionMap = OptionMap.Create(options, verbs, this._settings);
      if (this.TryParseHelpVerb(args, options, helpInfo, optionMap))
        return false;
      OptionInfo optionInfo = optionMap[((IEnumerable<string>) args).First<string>()];
      if (optionInfo == null)
      {
        if (helpInfo != null)
          this.DisplayHelpVerbText(options, helpInfo, (string) null);
        return false;
      }
      verbInstance = optionInfo.GetValue(options);
      if (verbInstance == null)
        verbInstance = optionInfo.CreateInstance(options);
      bool argumentsCore = this.DoParseArgumentsCore(((IEnumerable<string>) args).Skip<string>(1).ToArray<string>(), verbInstance);
      if (!argumentsCore && helpInfo != null)
        this.DisplayHelpVerbText(options, helpInfo, ((IEnumerable<string>) args).First<string>());
      return argumentsCore;
    }

    private bool ParseHelp(string[] args, HelpOptionAttribute helpOption)
    {
      bool caseSensitive = this._settings.CaseSensitive;
      foreach (string str in args)
      {
        char? shortName = helpOption.ShortName;
        if ((shortName.HasValue ? new int?((int) shortName.GetValueOrDefault()) : new int?()).HasValue && ArgumentParser.CompareShort(str, helpOption.ShortName, caseSensitive) || !string.IsNullOrEmpty(helpOption.LongName) && ArgumentParser.CompareLong(str, helpOption.LongName, caseSensitive))
          return true;
      }
      return false;
    }

    private bool TryParseHelpVerb(
      string[] args,
      object options,
      Pair<MethodInfo, HelpVerbOptionAttribute> helpInfo,
      OptionMap optionMap)
    {
      TextWriter helpWriter = this._settings.HelpWriter;
      if (helpInfo == null || helpWriter == null || string.Compare(args[0], helpInfo.Right.LongName, Parser.GetStringComparison(this._settings)) != 0)
        return false;
      string str = ((IEnumerable<string>) args).FirstOrDefault<string>();
      if (str != null)
      {
        OptionInfo option = optionMap[str];
        if (option != null && option.GetValue(options) == null)
          option.CreateInstance(options);
      }
      this.DisplayHelpVerbText(options, helpInfo, str);
      return true;
    }

    private void DisplayHelpVerbText(
      object options,
      Pair<MethodInfo, HelpVerbOptionAttribute> helpInfo,
      string verb)
    {
      string text;
      if (verb == null)
        HelpVerbOptionAttribute.InvokeMethod(options, helpInfo, (string) null, out text);
      else
        HelpVerbOptionAttribute.InvokeMethod(options, helpInfo, verb, out text);
      if (this._settings.HelpWriter == null)
        return;
      this._settings.HelpWriter.Write(text);
    }

    private void InvokeAutoBuildIfNeeded(object options)
    {
      if (this._settings.HelpWriter == null || options.HasHelp() || options.HasVerbHelp())
        return;
      this._settings.HelpWriter.Write((string) HelpText.AutoBuild(options, (Action<HelpText>) (current => HelpText.DefaultParsingErrorsHandler(options, current)), options.HasVerbs()));
    }

    private void Dispose(bool disposing)
    {
      if (this._disposed || !disposing)
        return;
      if (this._settings != null)
        this._settings.Dispose();
      this._disposed = true;
    }
  }
}
