// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.BaseSentenceBuilder
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine.Text
{
  public abstract class BaseSentenceBuilder
  {
    public abstract string OptionWord { get; }

    public abstract string AndWord { get; }

    public abstract string RequiredOptionMissingText { get; }

    public abstract string ViolatesFormatText { get; }

    public abstract string ViolatesMutualExclusivenessText { get; }

    public abstract string ErrorsHeadingText { get; }

    public static BaseSentenceBuilder CreateBuiltIn() => (BaseSentenceBuilder) new EnglishSentenceBuilder();
  }
}
