// Decompiled with JetBrains decompiler
// Type: CommandLine.Extensions.StringExtensions
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Globalization;

namespace CommandLine.Extensions
{
  internal static class StringExtensions
  {
    public static string Spaces(this int value) => new string(' ', value);

    public static bool IsNumeric(this string value) => Decimal.TryParse(value, out Decimal _);

    public static string FormatInvariant(this string value, params object[] arguments) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, value, arguments);

    public static string FormatLocal(this string value, params object[] arguments) => string.Format((IFormatProvider) CultureInfo.CurrentCulture, value, arguments);
  }
}
