// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.StringArrayEnumerator
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;

namespace CommandLine.Parsing
{
  internal sealed class StringArrayEnumerator : IArgumentEnumerator
  {
    private readonly int _endIndex;
    private readonly string[] _data;
    private int _index;

    public StringArrayEnumerator(string[] value)
    {
      Assumes.NotNull<string[]>(value, nameof (value));
      this._data = value;
      this._index = -1;
      this._endIndex = value.Length;
    }

    public string Current
    {
      get
      {
        if (this._index == -1)
          throw new InvalidOperationException();
        return this._index < this._endIndex ? this._data[this._index] : throw new InvalidOperationException();
      }
    }

    public string Next
    {
      get
      {
        if (this._index == -1)
          throw new InvalidOperationException();
        if (this._index > this._endIndex)
          throw new InvalidOperationException();
        return this.IsLast ? (string) null : this._data[this._index + 1];
      }
    }

    public bool IsLast => this._index == this._endIndex - 1;

    public bool MoveNext()
    {
      if (this._index >= this._endIndex)
        return false;
      ++this._index;
      return this._index < this._endIndex;
    }

    public string GetRemainingFromNext() => throw new NotSupportedException();

    public bool MovePrevious()
    {
      if (this._index <= 0)
        throw new InvalidOperationException();
      if (this._index > this._endIndex)
        return false;
      --this._index;
      return this._index <= this._endIndex;
    }
  }
}
