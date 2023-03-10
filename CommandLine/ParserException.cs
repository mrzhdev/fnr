// Decompiled with JetBrains decompiler
// Type: CommandLine.ParserException
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Runtime.Serialization;

namespace CommandLine
{
  [Serializable]
  public class ParserException : Exception
  {
    public ParserException()
    {
    }

    public ParserException(string message)
      : base(message)
    {
    }

    public ParserException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ParserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
