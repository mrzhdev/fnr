// Decompiled with JetBrains decompiler
// Type: CommandLine.VerbOptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class VerbOptionAttribute : BaseOptionAttribute
  {
    public VerbOptionAttribute(string longName)
      : base(new char?(), longName)
    {
      Assumes.NotNullOrEmpty(longName, nameof (longName));
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
  }
}
