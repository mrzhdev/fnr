// Decompiled with JetBrains decompiler
// Type: CommandLine.Infrastructure.PopsicleSetter
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine.Infrastructure
{
  internal static class PopsicleSetter
  {
    public static void Set<T>(bool consumed, ref T field, T value)
    {
      if (consumed)
        throw new InvalidOperationException();
      field = value;
    }
  }
}
