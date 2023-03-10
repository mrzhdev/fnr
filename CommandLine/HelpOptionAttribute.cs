// Decompiled with JetBrains decompiler
// Type: CommandLine.HelpOptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.Reflection;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class HelpOptionAttribute : BaseOptionAttribute
  {
    private const string DefaultHelpText = "Display this help screen.";

    public HelpOptionAttribute()
      : this("help")
    {
      this.HelpText = "Display this help screen.";
    }

    public HelpOptionAttribute(char shortName)
      : base(shortName, (string) null)
    {
      this.HelpText = "Display this help screen.";
    }

    public HelpOptionAttribute(string longName)
      : base(new char?(), longName)
    {
      this.HelpText = "Display this help screen.";
    }

    public HelpOptionAttribute(char shortName, string longName)
      : base(shortName, longName)
    {
      this.HelpText = "Display this help screen.";
    }

    public override bool Required
    {
      get => false;
      set => throw new InvalidOperationException();
    }

    internal static void InvokeMethod(
      object target,
      Pair<MethodInfo, HelpOptionAttribute> pair,
      out string text)
    {
      text = (string) null;
      MethodInfo left = pair.Left;
      text = HelpOptionAttribute.CheckMethodSignature(left) ? (string) left.Invoke(target, (object[]) null) : throw new MemberAccessException();
    }

    private static bool CheckMethodSignature(MethodInfo value) => value.ReturnType == typeof (string) && value.GetParameters().Length == 0;
  }
}
