﻿// Decompiled with JetBrains decompiler
// Type: MultiLanguage.CMLangStringClass
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [TypeLibType(2)]
  [ComConversionLoss]
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  [Guid("C04D65CF-B70D-11D0-B188-00AA0038C969")]
  [ComImport]
  public class CMLangStringClass : CMLangString, IMLangStringWStr, IMLangStringAStr, IMLangString
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetAStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [In] uint uCodePageIn,
      out uint puCodePageOut,
      out sbyte pszDest,
      [In] int cchDest,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int GetLength();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetLocale(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      out uint plocale,
      out int plLocalePos,
      out int plLocaleLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetMLStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pUnkOuter,
      [In] uint dwClsContext,
      [In] ref Guid piid,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr,
      out int plDestPos,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetStrBufA(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      out uint puDestCodePage,
      [MarshalAs(UnmanagedType.Interface)] out IMLangStringBufA ppDestBuf,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetStrBufW(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      [MarshalAs(UnmanagedType.Interface)] out IMLangStringBufW ppDestBuf,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetWStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      out ushort pszDest,
      [In] int cchDest,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int IMLangStringAStr_GetLength();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringAStr_GetLocale(
      [In] int lSrcPos,
      [In] int lSrcMaxLen,
      out uint plocale,
      out int plLocalePos,
      out int plLocaleLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringAStr_GetMLStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pUnkOuter,
      [In] uint dwClsContext,
      [In] ref Guid piid,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr,
      out int plDestPos,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringAStr_SetLocale([In] int lDestPos, [In] int lDestLen, [In] uint locale);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringAStr_SetMLStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pSrcMLStr,
      [In] int lSrcPos,
      [In] int lSrcLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringAStr_Sync([In] int fNoAccess);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int IMLangStringWStr_GetLength();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringWStr_GetMLStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pUnkOuter,
      [In] uint dwClsContext,
      [In] ref Guid piid,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr,
      out int plDestPos,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringWStr_SetMLStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pSrcMLStr,
      [In] int lSrcPos,
      [In] int lSrcLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IMLangStringWStr_Sync([In] int fNoAccess);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void LockAStr(
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
    public virtual extern void LockWStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [In] int lFlags,
      [In] int cchRequest,
      [Out] IntPtr ppszDest,
      out int pcchDest,
      out int plDestLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetAStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [In] uint uCodePage,
      [In] ref sbyte pszSrc,
      [In] int cchSrc,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetLocale([In] int lDestPos, [In] int lDestLen, [In] uint locale);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetMLStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pSrcMLStr,
      [In] int lSrcPos,
      [In] int lSrcLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetStrBufA(
      [In] int lDestPos,
      [In] int lDestLen,
      [In] uint uCodePage,
      [MarshalAs(UnmanagedType.Interface), In] IMLangStringBufA pSrcBuf,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetStrBufW(
      [In] int lDestPos,
      [In] int lDestLen,
      [MarshalAs(UnmanagedType.Interface), In] IMLangStringBufW pSrcBuf,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetWStr(
      [In] int lDestPos,
      [In] int lDestLen,
      [In] ref ushort pszSrc,
      [In] int cchSrc,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Sync([In] int fNoAccess);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void UnlockAStr(
      [In] ref sbyte pszSrc,
      [In] int cchSrc,
      out int pcchActual,
      out int plActualLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void UnlockWStr(
      [In] ref ushort pszSrc,
      [In] int cchSrc,
      out int pcchActual,
      out int plActualLen);

  }
}
