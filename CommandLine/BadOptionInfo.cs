// Decompiled with JetBrains decompiler
// Type: CommandLine.BadOptionInfo
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine
{
  public sealed class BadOptionInfo
  {
    internal BadOptionInfo()
    {
    }

    internal BadOptionInfo(char? shortName, string longName)
    {
      this.ShortName = shortName;
      this.LongName = longName;
    }

    public char? ShortName { get; internal set; }

    public string LongName { get; internal set; }
  }
}
