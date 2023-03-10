// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.TargetCapabilitiesExtensions
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;

namespace CommandLine.Parsing
{
  internal static class TargetCapabilitiesExtensions
  {
    public static bool HasVerbs(this object target) => ReflectionHelper.RetrievePropertyList<VerbOptionAttribute>(target).Count > 0;

    public static bool HasHelp(this object target) => ReflectionHelper.RetrieveMethod<HelpOptionAttribute>(target) != null;

    public static bool HasVerbHelp(this object target) => ReflectionHelper.RetrieveMethod<HelpVerbOptionAttribute>(target) != null;

    public static bool CanReceiveParserState(this object target) => ReflectionHelper.RetrievePropertyList<ParserStateAttribute>(target).Count > 0;
  }
}
