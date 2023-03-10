// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.IArgumentEnumerator
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine.Parsing
{
  internal interface IArgumentEnumerator
  {
    string Current { get; }

    string Next { get; }

    bool IsLast { get; }

    bool MoveNext();

    bool MovePrevious();

    string GetRemainingFromNext();
  }
}
