// Decompiled with JetBrains decompiler
// Type: FindAndReplace.ReplacerEventArgs
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace FindAndReplace
{
  public class ReplacerEventArgs : EventArgs
  {
    public Replacer.ReplaceResultItem ResultItem { get; set; }

    public Stats Stats { get; set; }

    public Status Status { get; set; }

    public bool IsSilent { get; set; }

    public ReplacerEventArgs(
      Replacer.ReplaceResultItem resultItem,
      Stats stats,
      Status status,
      bool isSilent = false)
    {
      this.ResultItem = resultItem;
      this.Stats = stats;
      this.Status = status;
      this.IsSilent = isSilent;
    }
  }
}
