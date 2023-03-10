// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IEnumCodePage
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("275C23E3-3747-11D0-9FEA-00AA003F8646")]
  [ComImport]
  public interface IEnumCodePage
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumCodePage ppEnum);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Next([In] uint celt, out tagMIMECPINFO rgelt, out uint pceltFetched);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Reset();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Skip([In] uint celt);
  }
}
