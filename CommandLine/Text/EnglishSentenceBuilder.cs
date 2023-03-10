// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.EnglishSentenceBuilder
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine.Text
{
  public class EnglishSentenceBuilder : BaseSentenceBuilder
  {
    public override string OptionWord => "option";

    public override string AndWord => "and";

    public override string RequiredOptionMissingText => "required option is missing";

    public override string ViolatesFormatText => "violates format";

    public override string ViolatesMutualExclusivenessText => "violates mutual exclusiveness";

    public override string ErrorsHeadingText => "ERROR(S):";
  }
}
