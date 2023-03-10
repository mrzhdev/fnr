// Decompiled with JetBrains decompiler
// Type: CommandLine.AssemblyUsageAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.InteropServices;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  [ComVisible(false)]
  public sealed class AssemblyUsageAttribute : MultilineTextAttribute
  {
    public AssemblyUsageAttribute(string line1)
      : base(line1)
    {
    }

    public AssemblyUsageAttribute(string line1, string line2)
      : base(line1, line2)
    {
    }

    public AssemblyUsageAttribute(string line1, string line2, string line3)
      : base(line1, line2, line3)
    {
    }

    public AssemblyUsageAttribute(string line1, string line2, string line3, string line4)
      : base(line1, line2, line3, line4)
    {
    }

    public AssemblyUsageAttribute(
      string line1,
      string line2,
      string line3,
      string line4,
      string line5)
      : base(line1, line2, line3, line4, line5)
    {
    }
  }
}
