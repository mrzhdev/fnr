// Decompiled with JetBrains decompiler
// Type: CommandLine.Infrastructure.Assumes
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine.Infrastructure
{
  internal static class Assumes
  {
    public static void NotNull<T>(T value, string paramName) where T : class
    {
      if ((object) value == null)
        throw new ArgumentNullException(paramName);
    }

    public static void NotNull<T>(T value, string paramName, string message) where T : class
    {
      if ((object) value == null)
        throw new ArgumentNullException(paramName, message);
    }

    public static void NotNullOrEmpty(string value, string paramName)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentException(paramName);
    }

    public static void NotZeroLength<T>(T[] array, string paramName)
    {
      if (array.Length == 0)
        throw new ArgumentOutOfRangeException(paramName);
    }
  }
}
