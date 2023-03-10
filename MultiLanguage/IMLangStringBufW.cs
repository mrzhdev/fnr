// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangStringBufW
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [ComConversionLoss]
  [Guid("D24ACD21-BA72-11D0-B188-00AA0038C969")]
  [ComImport]
  public interface IMLangStringBufW
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStatus(out int plFlags, out int pcchBuf);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void LockBuf([In] int cchOffset, [In] int cchMaxLock, [Out] IntPtr ppszBuf, out int pcchBuf);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void UnlockBuf([In] ref ushort pszBuf, [In] int cchOffset, [In] int cchWrite);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Insert([In] int cchOffset, [In] int cchMaxInsert, out int pcchActual);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Delete([In] int cchOffset, [In] int cchDelete);
  }
}
