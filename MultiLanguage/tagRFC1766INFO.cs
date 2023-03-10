// Decompiled with JetBrains decompiler
// Type: MultiLanguage.tagRFC1766INFO
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct tagRFC1766INFO
  {
    public uint lcid;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wszRfc1766;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszLocaleName;
  }
}
