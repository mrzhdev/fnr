// Decompiled with JetBrains decompiler
// Type: CommandLine.BaseOptionAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Extensions;
using System;

namespace CommandLine
{
  public abstract class BaseOptionAttribute : Attribute
  {
    internal const string DefaultMutuallyExclusiveSet = "Default";
    private char? _shortName;
    private object _defaultValue;
    private string _metaValue;
    private bool _hasMetaValue;
    private string _mutuallyExclusiveSet;

    protected BaseOptionAttribute()
    {
    }

    protected BaseOptionAttribute(char shortName, string longName)
    {
      this._shortName = new char?(shortName);
      if (this._shortName.Value.IsWhiteSpace() || this._shortName.Value.IsLineTerminator())
        throw new ArgumentException("shortName with whitespace or line terminator character is not allowed.", nameof (shortName));
      this.UniqueName = new string(shortName, 1);
      this.LongName = longName;
    }

    protected BaseOptionAttribute(char? shortName, string longName)
    {
      this._shortName = shortName;
      char? shortName1 = this._shortName;
      if ((shortName1.HasValue ? new int?((int) shortName1.GetValueOrDefault()) : new int?()).HasValue)
        this.UniqueName = !this._shortName.Value.IsWhiteSpace() && !this._shortName.Value.IsLineTerminator() ? new string(this._shortName.Value, 1) : throw new ArgumentException("shortName with whitespace or line terminator character is not allowed.", nameof (shortName));
      this.LongName = longName;
      if (this.UniqueName != null)
        return;
      this.UniqueName = this.LongName != null ? this.LongName : throw new ArgumentNullException(nameof (longName), "The option must have short, long name or both.");
    }

    public virtual char? ShortName
    {
      get => this._shortName;
      internal set => this._shortName = value;
    }

    public string LongName { get; internal set; }

    public string MutuallyExclusiveSet
    {
      get => this._mutuallyExclusiveSet;
      set => this._mutuallyExclusiveSet = string.IsNullOrEmpty(value) ? "Default" : value;
    }

    public virtual bool Required { get; set; }

    public virtual object DefaultValue
    {
      get => this._defaultValue;
      set
      {
        this._defaultValue = value;
        this.HasDefaultValue = true;
      }
    }

    public virtual string MetaValue
    {
      get => this._metaValue;
      set
      {
        this._metaValue = value;
        this._hasMetaValue = !string.IsNullOrEmpty(this._metaValue);
      }
    }

    public string HelpText { get; set; }

    internal string UniqueName { get; private set; }

    internal bool HasShortName
    {
      get
      {
        char? shortName = this._shortName;
        return (shortName.HasValue ? new int?((int) shortName.GetValueOrDefault()) : new int?()).HasValue;
      }
    }

    internal bool HasLongName => !string.IsNullOrEmpty(this.LongName);

    internal bool HasDefaultValue { get; private set; }

    internal bool HasMetaValue => this._hasMetaValue;

    internal bool AutoLongName { get; set; }
  }
}
