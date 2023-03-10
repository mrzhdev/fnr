// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.HelpText
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Extensions;
using CommandLine.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommandLine.Text
{
  public class HelpText
  {
    private const int BuilderCapacity = 128;
    private const int DefaultMaximumLength = 80;
    private const string DefaultRequiredWord = "Required.";
    private readonly StringBuilder _preOptionsHelp;
    private readonly StringBuilder _postOptionsHelp;
    private readonly BaseSentenceBuilder _sentenceBuilder;
    private int? _maximumDisplayWidth;
    private string _heading;
    private string _copyright;
    private bool _additionalNewLineAfterOption;
    private StringBuilder _optionsHelp;
    private bool _addDashesToOption;

    public HelpText()
    {
      this._preOptionsHelp = new StringBuilder(128);
      this._postOptionsHelp = new StringBuilder(128);
      this._sentenceBuilder = BaseSentenceBuilder.CreateBuiltIn();
    }

    public HelpText(BaseSentenceBuilder sentenceBuilder)
      : this()
    {
      Assumes.NotNull<BaseSentenceBuilder>(sentenceBuilder, nameof (sentenceBuilder));
      this._sentenceBuilder = sentenceBuilder;
    }

    public HelpText(string heading)
      : this()
    {
      Assumes.NotNullOrEmpty(heading, nameof (heading));
      this._heading = heading;
    }

    public HelpText(BaseSentenceBuilder sentenceBuilder, string heading)
      : this(heading)
    {
      Assumes.NotNull<BaseSentenceBuilder>(sentenceBuilder, nameof (sentenceBuilder));
      this._sentenceBuilder = sentenceBuilder;
    }

    public HelpText(string heading, string copyright)
      : this()
    {
      Assumes.NotNullOrEmpty(heading, nameof (heading));
      Assumes.NotNullOrEmpty(copyright, nameof (copyright));
      this._heading = heading;
      this._copyright = copyright;
    }

    public HelpText(BaseSentenceBuilder sentenceBuilder, string heading, string copyright)
      : this(heading, copyright)
    {
      Assumes.NotNull<BaseSentenceBuilder>(sentenceBuilder, nameof (sentenceBuilder));
      this._sentenceBuilder = sentenceBuilder;
    }

    public HelpText(string heading, string copyright, object options)
      : this()
    {
      Assumes.NotNullOrEmpty(heading, nameof (heading));
      Assumes.NotNullOrEmpty(copyright, nameof (copyright));
      Assumes.NotNull<object>(options, nameof (options));
      this._heading = heading;
      this._copyright = copyright;
      this.DoAddOptions(options, "Required.", this.MaximumDisplayWidth, false);
    }

    public HelpText(
      BaseSentenceBuilder sentenceBuilder,
      string heading,
      string copyright,
      object options)
      : this(heading, copyright, options)
    {
      Assumes.NotNull<BaseSentenceBuilder>(sentenceBuilder, nameof (sentenceBuilder));
      this._sentenceBuilder = sentenceBuilder;
    }

    public event EventHandler<FormatOptionHelpTextEventArgs> FormatOptionHelpText;

    public string Heading
    {
      get => this._heading;
      set
      {
        Assumes.NotNullOrEmpty(value, nameof (value));
        this._heading = value;
      }
    }

    public string Copyright
    {
      get => this._heading;
      set
      {
        Assumes.NotNullOrEmpty(value, nameof (value));
        this._copyright = value;
      }
    }

    public int MaximumDisplayWidth
    {
      get => !this._maximumDisplayWidth.HasValue ? 80 : this._maximumDisplayWidth.Value;
      set => this._maximumDisplayWidth = new int?(value);
    }

    public bool AddDashesToOption
    {
      get => this._addDashesToOption;
      set => this._addDashesToOption = value;
    }

    public bool AdditionalNewLineAfterOption
    {
      get => this._additionalNewLineAfterOption;
      set => this._additionalNewLineAfterOption = value;
    }

    public BaseSentenceBuilder SentenceBuilder => this._sentenceBuilder;

    public static HelpText AutoBuild(object options) => HelpText.AutoBuild(options, (Action<HelpText>) null);

    public static HelpText AutoBuild(
      object options,
      Action<HelpText> onError,
      bool verbsIndex = false)
    {
      HelpText helpText = new HelpText()
      {
        Heading = (string) HeadingInfo.Default,
        Copyright = (string) CopyrightInfo.Default,
        AdditionalNewLineAfterOption = true,
        AddDashesToOption = !verbsIndex
      };
      if (onError != null && ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(options) != null)
        onError(helpText);
      ReflectionHelper.GetAttribute<AssemblyLicenseAttribute>()?.AddToHelpText(helpText, true);
      ReflectionHelper.GetAttribute<AssemblyUsageAttribute>()?.AddToHelpText(helpText, true);
      helpText.AddOptions(options);
      return helpText;
    }

    public static HelpText AutoBuild(object options, string verb)
    {
      bool found;
      object optionsInstanceByName = Parser.InternalGetVerbOptionsInstanceByName(verb, options, out found);
      bool verbsIndex = verb == null || !found;
      object target = verbsIndex ? options : optionsInstanceByName;
      return HelpText.AutoBuild(target, (Action<HelpText>) (current => HelpText.DefaultParsingErrorsHandler(target, current)), verbsIndex);
    }

    public static void DefaultParsingErrorsHandler(object options, HelpText current)
    {
      IList<Pair<PropertyInfo, ParserStateAttribute>> pairList = ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(options);
      if (pairList.Count == 0)
        return;
      IParserState parserState = (IParserState) pairList[0].Left.GetValue(options, (object[]) null);
      if (parserState == null || parserState.Errors.Count == 0)
        return;
      string str1 = current.RenderParsingErrorsText(options, 2);
      if (string.IsNullOrEmpty(str1))
        return;
      current.AddPreOptionsLine(Environment.NewLine + current.SentenceBuilder.ErrorsHeadingText);
      string str2 = str1;
      string[] separator = new string[1]
      {
        Environment.NewLine
      };
      foreach (string str3 in str2.Split(separator, StringSplitOptions.None))
        current.AddPreOptionsLine(str3);
    }

    public static implicit operator string(HelpText info) => info.ToString();

    public void AddPreOptionsLine(string value) => this.AddPreOptionsLine(value, this.MaximumDisplayWidth);

    public void AddPostOptionsLine(string value) => this.AddLine(this._postOptionsHelp, value);

    public void AddOptions(object options) => this.AddOptions(options, "Required.");

    public void AddOptions(object options, string requiredWord)
    {
      Assumes.NotNull<object>(options, nameof (options));
      Assumes.NotNullOrEmpty(requiredWord, nameof (requiredWord));
      this.AddOptions(options, requiredWord, this.MaximumDisplayWidth);
    }

    public void AddOptions(object options, string requiredWord, int maximumLength)
    {
      Assumes.NotNull<object>(options, nameof (options));
      Assumes.NotNullOrEmpty(requiredWord, nameof (requiredWord));
      this.DoAddOptions(options, requiredWord, maximumLength);
    }

    public string RenderParsingErrorsText(object options, int indent)
    {
      IList<Pair<PropertyInfo, ParserStateAttribute>> pairList = ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(options);
      if (pairList.Count == 0)
        return string.Empty;
      IParserState parserState = (IParserState) pairList[0].Left.GetValue(options, (object[]) null);
      if (parserState == null || parserState.Errors.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (CommandLine.ParsingError error in (IEnumerable<CommandLine.ParsingError>) parserState.Errors)
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append(indent.Spaces());
        char? shortName = error.BadOption.ShortName;
        if ((shortName.HasValue ? new int?((int) shortName.GetValueOrDefault()) : new int?()).HasValue)
        {
          stringBuilder2.Append('-');
          stringBuilder2.Append((object) error.BadOption.ShortName);
          if (!string.IsNullOrEmpty(error.BadOption.LongName))
            stringBuilder2.Append('/');
        }
        if (!string.IsNullOrEmpty(error.BadOption.LongName))
        {
          stringBuilder2.Append("--");
          stringBuilder2.Append(error.BadOption.LongName);
        }
        stringBuilder2.Append(" ");
        stringBuilder2.Append(error.ViolatesRequired ? this._sentenceBuilder.RequiredOptionMissingText : this._sentenceBuilder.OptionWord);
        if (error.ViolatesFormat)
        {
          stringBuilder2.Append(" ");
          stringBuilder2.Append(this._sentenceBuilder.ViolatesFormatText);
        }
        if (error.ViolatesMutualExclusiveness)
        {
          if (error.ViolatesFormat || error.ViolatesRequired)
          {
            stringBuilder2.Append(" ");
            stringBuilder2.Append(this._sentenceBuilder.AndWord);
          }
          stringBuilder2.Append(" ");
          stringBuilder2.Append(this._sentenceBuilder.ViolatesMutualExclusivenessText);
        }
        stringBuilder2.Append('.');
        stringBuilder1.AppendLine(stringBuilder2.ToString());
      }
      return stringBuilder1.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(HelpText.GetLength(this._heading) + HelpText.GetLength(this._copyright) + HelpText.GetLength(this._preOptionsHelp) + HelpText.GetLength(this._optionsHelp) + 10);
      stringBuilder.Append(this._heading);
      if (!string.IsNullOrEmpty(this._copyright))
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(this._copyright);
      }
      if (this._preOptionsHelp.Length > 0)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append((object) this._preOptionsHelp);
      }
      if (this._optionsHelp != null && this._optionsHelp.Length > 0)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append((object) this._optionsHelp);
      }
      if (this._postOptionsHelp.Length > 0)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append((object) this._postOptionsHelp);
      }
      return stringBuilder.ToString();
    }

    protected virtual void OnFormatOptionHelpText(FormatOptionHelpTextEventArgs e)
    {
      EventHandler<FormatOptionHelpTextEventArgs> formatOptionHelpText = this.FormatOptionHelpText;
      if (formatOptionHelpText == null)
        return;
      formatOptionHelpText((object) this, e);
    }

    private static int GetLength(string value) => value == null ? 0 : value.Length;

    private static int GetLength(StringBuilder value) => value == null ? 0 : value.Length;

    private static void AddLine(StringBuilder builder, string value, int maximumLength)
    {
      Assumes.NotNull<string>(value, nameof (value));
      if (builder.Length > 0)
        builder.Append(Environment.NewLine);
      do
      {
        int val1 = 0;
        string[] strArray = value.Split(' ');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].Length < maximumLength - val1)
          {
            builder.Append(strArray[index]);
            val1 += strArray[index].Length;
            if (maximumLength - val1 > 1 && index != strArray.Length - 1)
            {
              builder.Append(" ");
              ++val1;
            }
          }
          else
          {
            if (strArray[index].Length >= maximumLength && val1 == 0)
            {
              builder.Append(strArray[index].Substring(0, maximumLength));
              val1 = maximumLength;
              break;
            }
            break;
          }
        }
        value = value.Substring(Math.Min(val1, value.Length));
        if (value.Length > 0)
          builder.Append(Environment.NewLine);
      }
      while (value.Length > maximumLength);
      builder.Append(value);
    }

    private void DoAddOptions(
      object options,
      string requiredWord,
      int maximumLength,
      bool fireEvent = true)
    {
      IList<BaseOptionAttribute> baseOptionAttributeList = ReflectionHelper.RetrievePropertyAttributeList<BaseOptionAttribute>(options);
      HelpOptionAttribute helpOptionAttribute = ReflectionHelper.RetrieveMethodAttributeOnly<HelpOptionAttribute>(options);
      if (helpOptionAttribute != null)
        baseOptionAttributeList.Add((BaseOptionAttribute) helpOptionAttribute);
      if (baseOptionAttributeList.Count == 0)
        return;
      int maxLength = this.GetMaxLength((IEnumerable<BaseOptionAttribute>) baseOptionAttributeList);
      this._optionsHelp = new StringBuilder(128);
      int widthOfHelpText = maximumLength - (maxLength + 6);
      foreach (BaseOptionAttribute option in (IEnumerable<BaseOptionAttribute>) baseOptionAttributeList)
        this.AddOption(requiredWord, maxLength, option, widthOfHelpText, fireEvent);
    }

    private void AddPreOptionsLine(string value, int maximumLength) => HelpText.AddLine(this._preOptionsHelp, value, maximumLength);

    private void AddOption(
      string requiredWord,
      int maxLength,
      BaseOptionAttribute option,
      int widthOfHelpText,
      bool fireEvent = true)
    {
      this._optionsHelp.Append("  ");
      StringBuilder stringBuilder = new StringBuilder(maxLength);
      if (option.HasShortName)
      {
        if (this._addDashesToOption)
          stringBuilder.Append('-');
        stringBuilder.AppendFormat("{0}", (object) option.ShortName);
        if (option.HasMetaValue)
          stringBuilder.AppendFormat(" {0}", (object) option.MetaValue);
        if (option.HasLongName)
          stringBuilder.Append(", ");
      }
      if (option.HasLongName)
      {
        if (this._addDashesToOption)
          stringBuilder.Append("--");
        stringBuilder.AppendFormat("{0}", (object) option.LongName);
        if (option.HasMetaValue)
          stringBuilder.AppendFormat("={0}", (object) option.MetaValue);
      }
      this._optionsHelp.Append(stringBuilder.Length < maxLength ? stringBuilder.ToString().PadRight(maxLength) : stringBuilder.ToString());
      this._optionsHelp.Append("    ");
      if (option.HasDefaultValue)
        option.HelpText = "(Default: {0}) ".FormatLocal(option.DefaultValue) + option.HelpText;
      if (option.Required)
        option.HelpText = "{0} ".FormatInvariant((object) requiredWord) + option.HelpText;
      if (fireEvent)
      {
        FormatOptionHelpTextEventArgs e = new FormatOptionHelpTextEventArgs(option);
        this.OnFormatOptionHelpText(e);
        option.HelpText = e.Option.HelpText;
      }
      if (!string.IsNullOrEmpty(option.HelpText))
      {
        do
        {
          int val1 = 0;
          string[] strArray = option.HelpText.Split(' ');
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (strArray[index].Length < widthOfHelpText - val1)
            {
              this._optionsHelp.Append(strArray[index]);
              val1 += strArray[index].Length;
              if (widthOfHelpText - val1 > 1 && index != strArray.Length - 1)
              {
                this._optionsHelp.Append(" ");
                ++val1;
              }
            }
            else
            {
              if (strArray[index].Length >= widthOfHelpText && val1 == 0)
              {
                this._optionsHelp.Append(strArray[index].Substring(0, widthOfHelpText));
                val1 = widthOfHelpText;
                break;
              }
              break;
            }
          }
          option.HelpText = option.HelpText.Substring(Math.Min(val1, option.HelpText.Length)).Trim();
          if (option.HelpText.Length > 0)
          {
            this._optionsHelp.Append(Environment.NewLine);
            this._optionsHelp.Append(new string(' ', maxLength + 6));
          }
        }
        while (option.HelpText.Length > widthOfHelpText);
      }
      this._optionsHelp.Append(option.HelpText);
      this._optionsHelp.Append(Environment.NewLine);
      if (!this._additionalNewLineAfterOption)
        return;
      this._optionsHelp.Append(Environment.NewLine);
    }

    private void AddLine(StringBuilder builder, string value)
    {
      Assumes.NotNull<string>(value, nameof (value));
      HelpText.AddLine(builder, value, this.MaximumDisplayWidth);
    }

    private int GetMaxLength(IEnumerable<BaseOptionAttribute> optionList)
    {
      int val1 = 0;
      foreach (BaseOptionAttribute option in optionList)
      {
        int val2 = 0;
        bool hasShortName = option.HasShortName;
        bool hasLongName = option.HasLongName;
        int num1 = 0;
        if (option.HasMetaValue)
          num1 = option.MetaValue.Length + 1;
        if (hasShortName)
        {
          int num2 = val2 + 1;
          if (this.AddDashesToOption)
            ++num2;
          val2 = num2 + num1;
        }
        if (hasLongName)
        {
          int num2 = val2 + option.LongName.Length;
          if (this.AddDashesToOption)
            num2 += 2;
          val2 = num2 + num1;
        }
        if (hasShortName && hasLongName)
          val2 += 2;
        val1 = Math.Max(val1, val2);
      }
      return val1;
    }
  }
}
