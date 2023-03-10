// Decompiled with JetBrains decompiler
// Type: CommandLine.ParsingError
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine
{
  public sealed class ParsingError
  {
    internal ParsingError() => this.BadOption = new BadOptionInfo();

    internal ParsingError(char? shortName, string longName, bool format)
    {
      this.BadOption = new BadOptionInfo(shortName, longName);
      this.ViolatesFormat = format;
    }

    public BadOptionInfo BadOption { get; private set; }

    public bool ViolatesRequired { get; set; }

    public bool ViolatesFormat { get; set; }

    public bool ViolatesMutualExclusiveness { get; set; }
  }
}
