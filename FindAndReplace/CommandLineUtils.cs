// Decompiled with JetBrains decompiler
// Type: FindAndReplace.CommandLineUtils
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace FindAndReplace
{
  public static class CommandLineUtils
  {
    public static string EncodeText(string original) => original.Replace(Environment.NewLine, "\\n").Replace("\"", "\\\"");

    public static string DecodeText(string original, bool hasRegEx = false)
    {
      string str = original;
      if (!hasRegEx)
        str = str.Replace("\\n", Environment.NewLine);
      return str.Replace("\\\"", "\"");
    }
  }
}
