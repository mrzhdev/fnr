// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangCodePages
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [Guid("359F3443-BD4A-11D0-B188-00AA0038C969")]
  [InterfaceType(1)]
  [ComImport]
  public interface IMLangCodePages
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCharCodePages([In] ushort chSrc, out uint pdwCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetStrCodePages(
      [In] ref ushort pszSrc,
      [In] int cchSrc,
      [In] uint dwPriorityCodePages,
      out uint pdwCodePages,
      out int pcchCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CodePageToCodePages([In] uint uCodePage, out uint pdwCodePages);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CodePagesToCodePage([In] uint dwCodePages, [In] uint uDefaultCodePage, out uint puCodePage);
  }
}
