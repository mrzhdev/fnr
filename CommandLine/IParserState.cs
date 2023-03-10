// Decompiled with JetBrains decompiler
// Type: CommandLine.IParserState
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.Collections.Generic;

namespace CommandLine
{
  public interface IParserState
  {
    IList<ParsingError> Errors { get; }
  }
}
