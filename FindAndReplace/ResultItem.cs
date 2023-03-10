// Decompiled with JetBrains decompiler
// Type: FindAndReplace.ResultItem
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;
using System.Text;

namespace FindAndReplace
{
  public abstract class ResultItem
  {
    public string FileName { get; set; }

    public string FilePath { get; set; }

    public string FileRelativePath { get; set; }

    public Encoding FileEncoding { get; set; }

    public int NumMatches { get; set; }

    public List<LiteMatch> Matches { get; set; }

    public bool IsSuccess { get; set; }

    public bool IsBinaryFile { get; set; }

    public bool FailedToOpen { get; set; }

    public string ErrorMessage { get; set; }

    internal bool IncludeFilesWithoutMatches { get; set; }

    public bool IncludeInResultsList => this.IsSuccess && this.NumMatches == 0 && this.IncludeFilesWithoutMatches || (this.IsSuccess && this.NumMatches > 0 || !this.IsSuccess && !string.IsNullOrEmpty(this.ErrorMessage));

    public bool IsReplaced => this.IsSuccess && this.NumMatches > 0;
  }
}
