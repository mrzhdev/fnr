// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.OneCharStringEnumerator
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;

namespace CommandLine.Parsing
{
  internal sealed class OneCharStringEnumerator : IArgumentEnumerator
  {
    private readonly string _data;
    private string _currentElement;
    private int _index;

    public OneCharStringEnumerator(string value)
    {
      Assumes.NotNullOrEmpty(value, nameof (value));
      this._data = value;
      this._index = -1;
    }

    public string Current
    {
      get
      {
        if (this._index == -1)
          throw new InvalidOperationException();
        if (this._index >= this._data.Length)
          throw new InvalidOperationException();
        return this._currentElement;
      }
    }

    public string Next
    {
      get
      {
        if (this._index == -1)
          throw new InvalidOperationException();
        if (this._index > this._data.Length)
          throw new InvalidOperationException();
        return this.IsLast ? (string) null : this._data.Substring(this._index + 1, 1);
      }
    }

    public bool IsLast => this._index == this._data.Length - 1;

    public bool MoveNext()
    {
      if (this._index < this._data.Length - 1)
      {
        ++this._index;
        this._currentElement = this._data.Substring(this._index, 1);
        return true;
      }
      this._index = this._data.Length;
      return false;
    }

    public string GetRemainingFromNext()
    {
      if (this._index == -1)
        throw new InvalidOperationException();
      if (this._index > this._data.Length)
        throw new InvalidOperationException();
      return this._data.Substring(this._index + 1);
    }

    public bool MovePrevious() => throw new NotSupportedException();
  }
}
