// Decompiled with JetBrains decompiler
// Type: CommandLine.Extensions.CharExtensions
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine.Extensions
{
  internal static class CharExtensions
  {
    public static bool IsWhiteSpace(this char c)
    {
      switch (c)
      {
        case '\t':
        case '\v':
        case '\f':
        case ' ':
          return true;
        default:
          return c > '\u007F' && char.IsWhiteSpace(c);
      }
    }

    public static bool IsLineTerminator(this char c)
    {
      switch (c)
      {
        case '\n':
        case '\r':
        case '\u2028':
        case '\u2029':
          return true;
        default:
          return false;
      }
    }
  }
}
