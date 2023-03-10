// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangLineBreakConsole
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("F5BE2EE1-BFD7-11D0-B188-00AA0038C969")]
  [ComImport]
  public interface IMLangLineBreakConsole
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BreakLineML(
      [MarshalAs(UnmanagedType.Interface), In] CMLangString pSrcMLStr,
      [In] int lSrcPos,
      [In] int lSrcLen,
      [In] int cMinColumns,
      [In] int cMaxColumns,
      out int plLineLen,
      out int plSkipLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BreakLineW(
      [In] uint locale,
      [In] ref ushort pszSrc,
      [In] int cchSrc,
      [In] int cMaxColumns,
      out int pcchLine,
      out int pcchSkip);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BreakLineA(
      [In] uint locale,
      [In] uint uCodePage,
      [In] ref sbyte pszSrc,
      [In] int cchSrc,
      [In] int cMaxColumns,
      out int pcchLine,
      out int pcchSkip);
  }
}
