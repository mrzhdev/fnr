// Decompiled with JetBrains decompiler
// Type: MultiLanguage.tagSCRIPFONTINFO
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct tagSCRIPFONTINFO
  {
    public long scripts;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszFont;
  }
}
