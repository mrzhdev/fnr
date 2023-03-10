// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.PresentParserState
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine.Parsing
{
  [Flags]
  internal enum PresentParserState : ushort
  {
    Undefined = 0,
    Success = 1,
    Failure = 2,
    MoveOnNextElement = 4,
  }
}
