// Decompiled with JetBrains decompiler
// Type: CommandLine.HelpVerbOptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Extensions;
using CommandLine.Infrastructure;
using System;
using System.Reflection;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class HelpVerbOptionAttribute : BaseOptionAttribute
  {
    private const string DefaultHelpText = "Display more information on a specific command.";

    public HelpVerbOptionAttribute()
      : this("help")
    {
      this.HelpText = "Display more information on a specific command.";
    }

    public HelpVerbOptionAttribute(string longName)
      : base(new char?(), longName)
    {
      this.HelpText = "Display more information on a specific command.";
    }

    public override char? ShortName
    {
      get => new char?();
      internal set => throw new InvalidOperationException("Verb commands do not support short name by design.");
    }

    public override bool Required
    {
      get => false;
      set => throw new InvalidOperationException("Verb commands cannot be mandatory since are mutually exclusive by design.");
    }

    internal static void InvokeMethod(
      object target,
      Pair<MethodInfo, HelpVerbOptionAttribute> helpInfo,
      string verb,
      out string text)
    {
      text = (string) null;
      MethodInfo left = helpInfo.Left;
      if (!HelpVerbOptionAttribute.CheckMethodSignature(left))
        throw new MemberAccessException("{0} has an incorrect signature. Help verb command requires a method that accepts and returns a string.".FormatInvariant((object) left.Name));
      text = (string) left.Invoke(target, new object[1]
      {
        (object) verb
      });
    }

    private static bool CheckMethodSignature(MethodInfo value) => value.ReturnType == typeof (string) && value.GetParameters().Length == 1 && value.GetParameters()[0].ParameterType == typeof (string);
  }
}
