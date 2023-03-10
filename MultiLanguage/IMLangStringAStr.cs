// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangStringAStr
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [Guid("C04D65D2-B70D-11D0-B188-00AA0038C969")]
  [InterfaceType(1)]
  [ComConversionLoss]
  [ComImport]
  public interface IMLangStringAStr : IMLangString
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void Sync([In] int fNoAccess);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new int GetLength();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void SetMLStr([In] int lDestPos, [In] int lDestLen, [MarshalAs(UnmanagedType.IUnknown), In] object pSrcMLStr, [In] int lSrcPos, [In] int lSrcLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    new void GetMLStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pUnkOuter,
      [In] uint dwClsContext,
      [In] ref Guid piid,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr,
      out int plDestPos,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetAStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [In] uint uCodePage,
      [In] ref sbyte pszSrc,
      [In] int cchSrc,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetStrBufA(
      [In] int lDestPos,
      [In] int lDestLen,
      [In] uint uCodePage,
      [MarshalAs(UnmanagedType.Interface), In] IMLangStringBufA pSrcBuf,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [In] uint uCodePageIn,
      out uint puCodePageOut,
      out sbyte pszDest,
      [In] int cchDest,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStrBufA(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      out uint puDestCodePage,
      [MarshalAs(UnmanagedType.Interface)] out IMLangStringBufA ppDestBuf,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void LockAStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [In] int lFlags,
      [In] uint uCodePageIn,
      [In] int cchRequest,
      out uint puCodePageOut,
      [Out] IntPtr ppszDest,
      out int pcchDest,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void UnlockAStr([In] ref sbyte pszSrc, [In] int cchSrc, out int pcchActual, out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetLocale([In] int lDestPos, [In] int lDestLen, [In] uint locale);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetLocale(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      out uint plocale,
      out int plLocalePos,
      out int plLocaleLen);
  }
}
