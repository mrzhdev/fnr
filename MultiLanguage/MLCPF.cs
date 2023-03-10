// Decompiled with JetBrains decompiler
// Type: MultiLanguage.MLCPF
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace MultiLanguage
{
  [Flags]
  public enum MLCPF
  {
    MLDETECTF_MAILNEWS = 1,
    MLDETECTF_BROWSER = 2,
    MLDETECTF_VALID = 4,
    MLDETECTF_VALID_NLS = 8,
    MLDETECTF_PRESERVE_ORDER = 16, // 0x00000010
    MLDETECTF_PREFERRED_ONLY = 32, // 0x00000020
    MLDETECTF_FILTER_SPECIALCHAR = 64, // 0x00000040
    MLDETECTF_EURO_UTF8 = 128, // 0x00000080
  }
}
