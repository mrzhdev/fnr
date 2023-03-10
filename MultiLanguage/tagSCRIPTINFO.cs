// Decompiled with JetBrains decompiler
// Type: MultiLanguage.tagSCRIPTINFO
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct tagSCRIPTINFO
  {
    public byte ScriptId;
    public uint uiCodePage;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
    public ushort[] wszDescription;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszFixedWidthFont;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszProportionalFont;
  }
}
