// Decompiled with JetBrains decompiler
// Type: MultiLanguage.tagSTATSTG
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct tagSTATSTG
  {
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pwcsName;
    public uint type;
    public _ULARGE_INTEGER cbSize;
    public _FILETIME mtime;
    public _FILETIME ctime;
    public _FILETIME atime;
    public uint grfMode;
    public uint grfLocksSupported;
    public Guid clsid;
    public uint grfStateBits;
    public uint reserved;
  }
}
