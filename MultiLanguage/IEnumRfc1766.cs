// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IEnumRfc1766
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [Guid("3DC39D1D-C030-11D0-B81B-00C04FC9B31F")]
  [InterfaceType(1)]
  [ComImport]
  public interface IEnumRfc1766
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumRfc1766 ppEnum);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Next([In] uint celt, out tagRFC1766INFO rgelt, out uint pceltFetched);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Reset();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Skip([In] uint celt);
  }
}
