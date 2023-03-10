// Decompiled with JetBrains decompiler
// Type: CommandLine.OptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Parsing;
using System;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class OptionAttribute : BaseOptionAttribute
  {
    public OptionAttribute() => this.AutoLongName = true;

    public OptionAttribute(char shortName)
      : base(shortName, (string) null)
    {
    }

    public OptionAttribute(string longName)
      : base(new char?(), longName)
    {
    }

    public OptionAttribute(char shortName, string longName)
      : base(shortName, longName)
    {
    }

    internal OptionInfo CreateOptionInfo() => new OptionInfo(this.ShortName, this.LongName);
  }
}
