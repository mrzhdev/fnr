﻿// Decompiled with JetBrains decompiler
// Type: MultiLanguage.IMLangConvertCharset
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiLanguage
{
  [InterfaceType(1)]
  [Guid("D66D6F98-CDAA-11D0-B822-00C04FC9B31F")]
  [ComImport]
  public interface IMLangConvertCharset
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Initialize([In] uint uiSrcCodePage, [In] uint uiDstCodePage, [In] uint dwProperty);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetSourceCodePage(out uint puiSrcCodePage);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDestinationCodePage(out uint puiDstCodePage);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetProperty(out uint pdwProperty);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DoConversion([In] ref byte pSrcStr, [In, Out] ref uint pcSrcSize, [In] ref byte pDstStr, [In, Out] ref uint pcDstSize);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DoConversionToUnicode(
      [In] ref sbyte pSrcStr,
      [In, Out] ref uint pcSrcSize,
      [In] ref ushort pDstStr,
      [In, Out] ref uint pcDstSize);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DoConversionFromUnicode(
      [In] ref ushort pSrcStr,
      [In, Out] ref uint pcSrcSize,
      [In] ref sbyte pDstStr,
      [In, Out] ref uint pcDstSize);
  }
}
