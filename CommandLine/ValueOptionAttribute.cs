// Decompiled with JetBrains decompiler
// Type: CommandLine.ValueOptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class ValueOptionAttribute : Attribute
  {
    private readonly int _index;

    public ValueOptionAttribute(int index) => this._index = index;

    public int Index => this._index;
  }
}
