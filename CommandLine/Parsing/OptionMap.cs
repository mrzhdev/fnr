// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.OptionMap
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Extensions;
using CommandLine.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLine.Parsing
{
  internal sealed class OptionMap
  {
    private readonly ParserSettings _settings;
    private readonly Dictionary<string, string> _names;
    private readonly Dictionary<string, OptionInfo> _map;
    private readonly Dictionary<string, OptionMap.MutuallyExclusiveInfo> _mutuallyExclusiveSetMap;

    internal OptionMap(int capacity, ParserSettings settings)
    {
      this._settings = settings;
      IEqualityComparer<string> comparer = this._settings.CaseSensitive ? (IEqualityComparer<string>) StringComparer.Ordinal : (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase;
      this._names = new Dictionary<string, string>(capacity, comparer);
      this._map = new Dictionary<string, OptionInfo>(capacity * 2, comparer);
      if (!this._settings.MutuallyExclusive)
        return;
      this._mutuallyExclusiveSetMap = new Dictionary<string, OptionMap.MutuallyExclusiveInfo>(capacity, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    internal object RawOptions { private get; set; }

    public OptionInfo this[string key]
    {
      get
      {
        OptionInfo optionInfo = (OptionInfo) null;
        if (this._map.ContainsKey(key))
          optionInfo = this._map[key];
        else if (this._names.ContainsKey(key))
          optionInfo = this._map[this._names[key]];
        return optionInfo;
      }
      set
      {
        this._map[key] = value;
        if (!value.HasBothNames)
          return;
        this._names[value.LongName] = new string(value.ShortName.Value, 1);
      }
    }

    public static OptionMap Create(object target, ParserSettings settings)
    {
      IList<Pair<PropertyInfo, BaseOptionAttribute>> pairList = ReflectionHelper.RetrievePropertyList<BaseOptionAttribute>(target);
      if (pairList == null)
        return (OptionMap) null;
      OptionMap optionMap = new OptionMap(pairList.Count, settings);
      foreach (Pair<PropertyInfo, BaseOptionAttribute> pair in (IEnumerable<Pair<PropertyInfo, BaseOptionAttribute>>) pairList)
      {
        if (pair.Left != (PropertyInfo) null && pair.Right != null)
        {
          string key;
          if (pair.Right.AutoLongName)
          {
            key = pair.Left.Name.ToLowerInvariant();
            pair.Right.LongName = key;
          }
          else
            key = pair.Right.UniqueName;
          optionMap[key] = new OptionInfo(pair.Right, pair.Left, settings.ParsingCulture);
        }
      }
      optionMap.RawOptions = target;
      return optionMap;
    }

    public static OptionMap Create(
      object target,
      IList<Pair<PropertyInfo, VerbOptionAttribute>> verbs,
      ParserSettings settings)
    {
      OptionMap optionMap = new OptionMap(verbs.Count, settings);
      foreach (Pair<PropertyInfo, VerbOptionAttribute> verb in (IEnumerable<Pair<PropertyInfo, VerbOptionAttribute>>) verbs)
      {
        OptionInfo optionInfo = new OptionInfo((BaseOptionAttribute) verb.Right, verb.Left, settings.ParsingCulture)
        {
          HasParameterLessCtor = verb.Left.PropertyType.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null
        };
        if (!optionInfo.HasParameterLessCtor && verb.Left.GetValue(target, (object[]) null) == null)
          throw new ParserException("Type {0} must have a parameterless constructor or" + " be already initialized to be used as a verb command.".FormatInvariant((object) verb.Left.PropertyType));
        optionMap[verb.Right.UniqueName] = optionInfo;
      }
      optionMap.RawOptions = target;
      return optionMap;
    }

    public bool EnforceRules() => this.EnforceMutuallyExclusiveMap() && this.EnforceRequiredRule();

    public void SetDefaults()
    {
      foreach (OptionInfo optionInfo in this._map.Values)
        optionInfo.SetDefault(this.RawOptions);
    }

    private static void SetParserStateIfNeeded(
      object options,
      OptionInfo option,
      bool? required,
      bool? mutualExclusiveness)
    {
      IList<Pair<PropertyInfo, ParserStateAttribute>> pairList = ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(options);
      if (pairList.Count == 0)
        return;
      PropertyInfo left = pairList[0].Left;
      if (left.GetValue(options, (object[]) null) == null)
        left.SetValue(options, (object) new ParserState(), (object[]) null);
      IParserState parserState = (IParserState) left.GetValue(options, (object[]) null);
      if (parserState == null)
        return;
      CommandLine.ParsingError parsingError = new CommandLine.ParsingError()
      {
        BadOption = {
          ShortName = option.ShortName,
          LongName = option.LongName
        }
      };
      if (required.HasValue)
        parsingError.ViolatesRequired = required.Value;
      if (mutualExclusiveness.HasValue)
        parsingError.ViolatesMutualExclusiveness = mutualExclusiveness.Value;
      parserState.Errors.Add(parsingError);
    }

    private bool EnforceRequiredRule()
    {
      bool flag = true;
      foreach (OptionInfo option in this._map.Values)
      {
        if (option.Required && (!option.IsDefined || !option.ReceivedValue))
        {
          OptionMap.SetParserStateIfNeeded(this.RawOptions, option, new bool?(true), new bool?());
          flag = false;
        }
      }
      return flag;
    }

    private bool EnforceMutuallyExclusiveMap()
    {
      if (!this._settings.MutuallyExclusive)
        return true;
      foreach (OptionInfo option in this._map.Values)
      {
        if (option.IsDefined && option.MutuallyExclusiveSet != null)
          this.BuildMutuallyExclusiveMap(option);
      }
      foreach (OptionMap.MutuallyExclusiveInfo mutuallyExclusiveInfo in this._mutuallyExclusiveSetMap.Values)
      {
        if (mutuallyExclusiveInfo.Occurrence > 1)
        {
          OptionMap.SetParserStateIfNeeded(this.RawOptions, mutuallyExclusiveInfo.BadOption, new bool?(), new bool?(true));
          return false;
        }
      }
      return true;
    }

    private void BuildMutuallyExclusiveMap(OptionInfo option)
    {
      string mutuallyExclusiveSet = option.MutuallyExclusiveSet;
      if (!this._mutuallyExclusiveSetMap.ContainsKey(mutuallyExclusiveSet))
        this._mutuallyExclusiveSetMap.Add(mutuallyExclusiveSet, new OptionMap.MutuallyExclusiveInfo(option));
      this._mutuallyExclusiveSetMap[mutuallyExclusiveSet].IncrementOccurrence();
    }

    private sealed class MutuallyExclusiveInfo
    {
      private int _count;

      public MutuallyExclusiveInfo(OptionInfo option) => this.BadOption = option;

      public OptionInfo BadOption { get; private set; }

      public int Occurrence => this._count;

      public void IncrementOccurrence() => ++this._count;
    }
  }
}
