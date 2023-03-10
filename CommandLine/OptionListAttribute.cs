// Decompiled with JetBrains decompiler
// Type: CommandLine.OptionListAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class OptionListAttribute : BaseOptionAttribute
  {
    private const char DefaultSeparator = ':';

    public OptionListAttribute()
    {
      this.AutoLongName = true;
      this.Separator = ':';
    }

    public OptionListAttribute(char shortName)
      : base(shortName, (string) null)
    {
    }

    public OptionListAttribute(string longName)
      : base(new char?(), longName)
    {
    }

    public OptionListAttribute(char shortName, string longName)
      : base(shortName, longName)
    {
      this.Separator = ':';
    }

    public OptionListAttribute(char shortName, string longName, char separator)
      : base(shortName, longName)
    {
      this.Separator = separator;
    }

    public char Separator { get; set; }
  }
}
