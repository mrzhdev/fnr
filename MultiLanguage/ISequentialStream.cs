// Decompiled with JetBrains decompiler
// Type: MultiLanguage.ISequentialStream
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
  [InterfaceType(1)]
  [ComImport]
  public interface ISequentialStream
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoteRead(IntPtr pv, uint cb, ref uint pcbRead);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoteWrite([In] ref byte pv, [In] uint cb, ref uint pcbWritten);
  }
}
