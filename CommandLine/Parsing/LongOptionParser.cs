// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.LongOptionParser
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;

namespace CommandLine.Parsing
{
  internal sealed class LongOptionParser : ArgumentParser
  {
    private readonly bool _ignoreUnkwnownArguments;

    public LongOptionParser(bool ignoreUnkwnownArguments) => this._ignoreUnkwnownArguments = ignoreUnkwnownArguments;

    public override PresentParserState Parse(
      IArgumentEnumerator argumentEnumerator,
      OptionMap map,
      object options)
    {
      string[] strArray = argumentEnumerator.Current.Substring(2).Split(new char[1]
      {
        '='
      }, 2);
      OptionInfo option = map[strArray[0]];
      if (option == null)
        return !this._ignoreUnkwnownArguments ? PresentParserState.Failure : PresentParserState.MoveOnNextElement;
      option.IsDefined = true;
      ArgumentParser.EnsureOptionArrayAttributeIsNotBoundToScalar(option);
      if (!option.IsBoolean)
      {
        if (strArray.Length == 1 && (argumentEnumerator.IsLast || !ArgumentParser.IsInputValue(argumentEnumerator.Next)))
          return PresentParserState.Failure;
        if (strArray.Length == 2)
        {
          if (!option.IsArray)
          {
            bool flag = option.SetValue(strArray[1], options);
            if (!flag)
              this.DefineOptionThatViolatesFormat(option);
            return ArgumentParser.BooleanToParserState(flag);
          }
          ArgumentParser.EnsureOptionAttributeIsArrayCompatible(option);
          IList<string> nextInputValues = ArgumentParser.GetNextInputValues(argumentEnumerator);
          nextInputValues.Insert(0, strArray[1]);
          bool flag1 = option.SetValue(nextInputValues, options);
          if (!flag1)
            this.DefineOptionThatViolatesFormat(option);
          return ArgumentParser.BooleanToParserState(flag1);
        }
        if (!option.IsArray)
        {
          bool flag = option.SetValue(argumentEnumerator.Next, options);
          if (!flag)
            this.DefineOptionThatViolatesFormat(option);
          return ArgumentParser.BooleanToParserState(flag, true);
        }
        ArgumentParser.EnsureOptionAttributeIsArrayCompatible(option);
        IList<string> nextInputValues1 = ArgumentParser.GetNextInputValues(argumentEnumerator);
        bool flag2 = option.SetValue(nextInputValues1, options);
        if (!flag2)
          this.DefineOptionThatViolatesFormat(option);
        return ArgumentParser.BooleanToParserState(flag2);
      }
      if (strArray.Length == 2)
        return PresentParserState.Failure;
      bool flag3 = option.SetValue(true, options);
      if (!flag3)
        this.DefineOptionThatViolatesFormat(option);
      return ArgumentParser.BooleanToParserState(flag3);
    }
  }
}
