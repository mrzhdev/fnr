// Decompiled with JetBrains decompiler
// Type: CommandLine.Infrastructure.Pair`2
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace CommandLine.Infrastructure
{
  internal sealed class Pair<TLeft, TRight>
    where TLeft : class
    where TRight : class
  {
    private readonly TLeft _left;
    private readonly TRight _right;

    public Pair(TLeft left, TRight right)
    {
      this._left = left;
      this._right = right;
    }

    public TLeft Left => this._left;

    public TRight Right => this._right;

    public override int GetHashCode() => ((object) this._left == null ? 0 : this._left.GetHashCode()) ^ ((object) this._right == null ? 0 : this._right.GetHashCode());

    public override bool Equals(object obj) => obj is Pair<TLeft, TRight> pair && object.Equals((object) this._left, (object) pair._left) && object.Equals((object) this._right, (object) pair._right);
  }
}
