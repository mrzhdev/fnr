// Decompiled with JetBrains decompiler
// Type: MultiLanguage.tagMIMECPINFO
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct tagMIMECPINFO
  {
    public uint dwFlags;
    public uint uiCodePage;
    public uint uiFamilyCodePage;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public ushort[] wszDescription;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public ushort[] wszWebCharset;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public ushort[] wszHeaderCharset;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
    public ushort[] wszBodyCharset;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszFixedWidthFont;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public ushort[] wszProportionalFont;
    public byte bGDICharset;
  }
}
