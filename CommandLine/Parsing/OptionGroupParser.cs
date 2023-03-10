// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.OptionGroupParser
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;

namespace CommandLine.Parsing
{
  internal sealed class OptionGroupParser : ArgumentParser
  {
    private readonly bool _ignoreUnkwnownArguments;

    public OptionGroupParser(bool ignoreUnkwnownArguments) => this._ignoreUnkwnownArguments = ignoreUnkwnownArguments;

    public override PresentParserState Parse(
      IArgumentEnumerator argumentEnumerator,
      OptionMap map,
      object options)
    {
      OneCharStringEnumerator stringEnumerator = new OneCharStringEnumerator(argumentEnumerator.Current.Substring(1));
      while (stringEnumerator.MoveNext())
      {
        OptionInfo option = map[stringEnumerator.Current];
        if (option == null)
          return !this._ignoreUnkwnownArguments ? PresentParserState.Failure : PresentParserState.MoveOnNextElement;
        option.IsDefined = true;
        ArgumentParser.EnsureOptionArrayAttributeIsNotBoundToScalar(option);
        if (!option.IsBoolean)
        {
          if (argumentEnumerator.IsLast && stringEnumerator.IsLast)
            return PresentParserState.Failure;
          if (!stringEnumerator.IsLast)
          {
            if (!option.IsArray)
            {
              bool flag = option.SetValue(stringEnumerator.GetRemainingFromNext(), options);
              if (!flag)
                this.DefineOptionThatViolatesFormat(option);
              return ArgumentParser.BooleanToParserState(flag);
            }
            ArgumentParser.EnsureOptionAttributeIsArrayCompatible(option);
            IList<string> nextInputValues = ArgumentParser.GetNextInputValues(argumentEnumerator);
            nextInputValues.Insert(0, stringEnumerator.GetRemainingFromNext());
            bool flag1 = option.SetValue(nextInputValues, options);
            if (!flag1)
              this.DefineOptionThatViolatesFormat(option);
            return ArgumentParser.BooleanToParserState(flag1, true);
          }
          if (!argumentEnumerator.IsLast && !ArgumentParser.IsInputValue(argumentEnumerator.Next))
            return PresentParserState.Failure;
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
        if (!stringEnumerator.IsLast && map[stringEnumerator.Next] == null || !option.SetValue(true, options))
          return PresentParserState.Failure;
      }
      return PresentParserState.Success;
    }
  }
}
