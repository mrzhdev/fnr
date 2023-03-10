// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IEnumScript
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("AE5F1430-388B-11D2-8380-00C04F8F5DA1")]
  [ComImport]
  public interface IEnumScript
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumScript ppEnum);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Next([In] uint celt, out tagSCRIPTINFO rgelt, out uint pceltFetched);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Reset();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Skip([In] uint celt);
  }
}
