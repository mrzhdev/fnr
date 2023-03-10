// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangFontLink2
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [Guid("DCCFC162-2B38-11D2-B7EC-00C04F8F5D9A")]
  [InterfaceType(1)]
  [ComConversionLoss]
  [ComImport]
  public interface IMLangFontLink2 : IMLangCodePages
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetCharCodePages([In] ushort chSrc, out uint pdwCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetStrCodePages(
      [In] ref ushort pszSrc,
      [In] int cchSrc,
      [In] uint dwPriorityCodePages,
      out uint pdwCodePages,
      out int pcchCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void CodePageToCodePages([In] uint uCodePage, out uint pdwCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void CodePagesToCodePage([In] uint dwCodePages, [In] uint uDefaultCodePage, out uint puCodePage);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFontCodePages(
      [ComAliasName("MultiLanguage.wireHDC"), In] ref _RemotableHandle hDC,
      [ComAliasName("MultiLanguage.wireHFONT"), In] ref _RemotableHandle hFont,
      out uint pdwCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ReleaseFont([ComAliasName("MultiLanguage.wireHFONT"), In] ref _RemotableHandle hFont);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ResetFontMapping();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MapFont([ComAliasName("MultiLanguage.wireHDC"), In] ref _RemotableHandle hDC, [In] uint dwCodePages, [In] ushort chSrc, [ComAliasName("MultiLanguage.wireHFONT"), Out] IntPtr pFont);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFontUnicodeRanges(
      [ComAliasName("MultiLanguage.wireHDC"), In] ref _RemotableHandle hDC,
      [In, Out] ref uint puiRanges,
      out tagUNICODERANGE pUranges);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetScriptFontInfo(
      [In] byte sid,
      [In] uint dwFlags,
      [In, Out] ref uint puiFonts,
      out tagSCRIPFONTINFO pScriptFont);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CodePageToScriptID([In] uint uiCodePage, out byte pSid);
  }
}
