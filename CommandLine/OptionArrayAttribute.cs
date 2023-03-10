// Decompiled with JetBrains decompiler
// Type: CommandLine.OptionArrayAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class OptionArrayAttribute : BaseOptionAttribute
  {
    public OptionArrayAttribute() => this.AutoLongName = true;

    public OptionArrayAttribute(char shortName)
      : base(shortName, (string) null)
    {
    }

    public OptionArrayAttribute(string longName)
      : base(new char?(), longName)
    {
    }

    public OptionArrayAttribute(char shortName, string longName)
      : base(shortName, longName)
    {
    }
  }
}
