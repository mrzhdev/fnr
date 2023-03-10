// Decompiled with JetBrains decompiler
// Type: FindAndReplace.LineNumberComparer
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;

namespace FindAndReplace
{
  public class LineNumberComparer : IEqualityComparer<MatchPreviewLineNumber>
  {
    public bool Equals(MatchPreviewLineNumber x, MatchPreviewLineNumber y) => x.LineNumber == y.LineNumber;

    public int GetHashCode(MatchPreviewLineNumber obj) => obj.LineNumber.GetHashCode();
  }
}
