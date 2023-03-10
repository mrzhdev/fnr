// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.ArgumentParser
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Extensions;
using System;
using System.Collections.Generic;

namespace CommandLine.Parsing
{
  internal abstract class ArgumentParser
  {
    protected ArgumentParser() => this.PostParsingState = new List<CommandLine.ParsingError>();

    public List<CommandLine.ParsingError> PostParsingState { get; private set; }

    public static bool CompareShort(string argument, char? option, bool caseSensitive) => string.Compare(argument, ArgumentParser.ToOption(option), caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;

    public static bool CompareLong(string argument, string option, bool caseSensitive) => string.Compare(argument, ArgumentParser.ToOption(option), caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) == 0;

    public static ArgumentParser Create(string argument, bool ignoreUnknownArguments = false)
    {
      if (argument.IsNumeric())
        return (ArgumentParser) null;
      if (ArgumentParser.IsDash(argument))
        return (ArgumentParser) null;
      if (ArgumentParser.IsLongOption(argument))
        return (ArgumentParser) new LongOptionParser(ignoreUnknownArguments);
      return ArgumentParser.IsShortOption(argument) ? (ArgumentParser) new OptionGroupParser(ignoreUnknownArguments) : (ArgumentParser) null;
    }

    public static bool IsInputValue(string argument) => argument.IsNumeric() || argument.Length <= 0 || ArgumentParser.IsDash(argument) || !ArgumentParser.IsShortOption(argument);

    public abstract PresentParserState Parse(
      IArgumentEnumerator argumentEnumerator,
      OptionMap map,
      object options);

    internal static IList<string> InternalWrapperOfGetNextInputValues(IArgumentEnumerator ae) => ArgumentParser.GetNextInputValues(ae);

    protected static IList<string> GetNextInputValues(IArgumentEnumerator ae)
    {
      IList<string> stringList = (IList<string>) new List<string>();
      while (ae.MoveNext() && ArgumentParser.IsInputValue(ae.Current))
        stringList.Add(ae.Current);
      if (!ae.MovePrevious())
        throw new ParserException();
      return stringList;
    }

    protected static PresentParserState BooleanToParserState(bool value) => ArgumentParser.BooleanToParserState(value, false);

    protected static PresentParserState BooleanToParserState(
      bool value,
      bool addMoveNextIfTrue)
    {
      if (value && !addMoveNextIfTrue)
        return PresentParserState.Success;
      return value ? PresentParserState.Success | PresentParserState.MoveOnNextElement : PresentParserState.Failure;
    }

    protected static void EnsureOptionAttributeIsArrayCompatible(OptionInfo option)
    {
      if (!option.IsAttributeArrayCompatible)
        throw new ParserException();
    }

    protected static void EnsureOptionArrayAttributeIsNotBoundToScalar(OptionInfo option)
    {
      if (!option.IsArray && option.IsAttributeArrayCompatible)
        throw new ParserException();
    }

    protected void DefineOptionThatViolatesFormat(OptionInfo option) => this.PostParsingState.Add(new CommandLine.ParsingError(option.ShortName, option.LongName, true));

    private static string ToOption(string value) => "--" + value;

    private static string ToOption(char? value) => "-" + (object) value;

    private static bool IsDash(string value) => string.CompareOrdinal(value, "-") == 0;

    private static bool IsShortOption(string value) => value[0] == '-';

    private static bool IsLongOption(string value) => value[0] == '-' && value[1] == '-';
  }
}
