// Decompiled with JetBrains decompiler
// Type: CommandLine.ParserState
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;

namespace CommandLine
{
  public sealed class ParserState : IParserState
  {
    internal ParserState() => this.Errors = (IList<ParsingError>) new List<ParsingError>();

    public IList<ParsingError> Errors { get; private set; }
  }
}
