// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangFontLink
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("359F3441-BD4A-11D0-B188-00AA0038C969")]
  [ComConversionLoss]
  [ComImport]
  public interface IMLangFontLink : IMLangCodePages
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
    void MapFont(
      [ComAliasName("MultiLanguage.wireHDC"), In] ref _RemotableHandle hDC,
      [In] uint dwCodePages,
      [ComAliasName("MultiLanguage.wireHFONT"), In] ref _RemotableHandle hSrcFont,
      [ComAliasName("MultiLanguage.wireHFONT"), Out] IntPtr phDestFont);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ReleaseFont([ComAliasName("MultiLanguage.wireHFONT"), In] ref _RemotableHandle hFont);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ResetFontMapping();
  }
}
