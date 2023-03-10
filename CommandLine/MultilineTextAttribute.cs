// Decompiled with JetBrains decompiler
// Type: CommandLine.MultilineTextAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using CommandLine.Text;
using System;
using System.Text;

namespace CommandLine
{
  public abstract class MultilineTextAttribute : Attribute
  {
    private readonly string _line1;
    private readonly string _line2;
    private readonly string _line3;
    private readonly string _line4;
    private readonly string _line5;

    protected MultilineTextAttribute(string line1)
    {
      Assumes.NotNullOrEmpty(line1, nameof (line1));
      this._line1 = line1;
    }

    protected MultilineTextAttribute(string line1, string line2)
      : this(line1)
    {
      Assumes.NotNullOrEmpty(line2, nameof (line2));
      this._line2 = line2;
    }

    protected MultilineTextAttribute(string line1, string line2, string line3)
      : this(line1, line2)
    {
      Assumes.NotNullOrEmpty(line3, nameof (line3));
      this._line3 = line3;
    }

    protected MultilineTextAttribute(string line1, string line2, string line3, string line4)
      : this(line1, line2, line3)
    {
      Assumes.NotNullOrEmpty(line4, nameof (line4));
      this._line4 = line4;
    }

    protected MultilineTextAttribute(
      string line1,
      string line2,
      string line3,
      string line4,
      string line5)
      : this(line1, line2, line3, line4)
    {
      Assumes.NotNullOrEmpty(line5, nameof (line5));
      this._line5 = line5;
    }

    public virtual string Value
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(string.Empty);
        string[] strArray = new string[5]
        {
          this._line1,
          this._line2,
          this._line3,
          this._line4,
          this._line5
        };
        for (int index = 0; index < this.GetLastLineWithText(strArray); ++index)
          stringBuilder.AppendLine(strArray[index]);
        return stringBuilder.ToString();
      }
    }

    public string Line1 => this._line1;

    public string Line2 => this._line2;

    public string Line3 => this._line3;

    public string Line4 => this._line4;

    public string Line5 => this._line5;

    internal void AddToHelpText(Action<string> action) => Array.ForEach<string>(new string[5]
    {
      this._line1,
      this._line2,
      this._line3,
      this._line4,
      this._line5
    }, (Action<string>) (line =>
    {
      if (string.IsNullOrEmpty(line))
        return;
      action(line);
    }));

    internal void AddToHelpText(HelpText helpText, bool before)
    {
      if (before)
        this.AddToHelpText(new Action<string>(helpText.AddPreOptionsLine));
      else
        this.AddToHelpText(new Action<string>(helpText.AddPostOptionsLine));
    }

    protected virtual int GetLastLineWithText(string[] value) => Array.FindLastIndex<string>(value, (Predicate<string>) (str => !string.IsNullOrEmpty(str))) + 1;
  }
}
