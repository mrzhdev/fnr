// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangString
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("C04D65CE-B70D-11D0-B188-00AA0038C969")]
  [ComImport]
  public interface IMLangString
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Sync([In] int fNoAccess);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetLength();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetMLStr([In] int lDestPos, [In] int lDestLen, [MarshalAs(UnmanagedType.IUnknown), In] object pSrcMLStr, [In] int lSrcPos, [In] int lSrcLen);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetMLStr(
      [In] int lSrcPos,
      [In] int lSrcLen,
      [MarshalAs(UnmanagedType.IUnknown), In] object pUnkOuter,
      [In] uint dwClsContext,
      [In] ref Guid piid,
      [MarshalAs(UnmanagedType.IUnknown)] out object ppDestMLStr,
      out int plDestPos,
      out int plDestLen);
  }
}
