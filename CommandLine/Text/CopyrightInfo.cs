// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.CopyrightInfo
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace CommandLine.Text
{
  public class CopyrightInfo
  {
    private const string DefaultCopyrightWord = "Copyright";
    private const string SymbolLower = "(c)";
    private const string SymbolUpper = "(C)";
    private readonly AssemblyCopyrightAttribute _attribute;
    private readonly bool _isSymbolUpper;
    private readonly int[] _copyrightYears;
    private readonly string _author;
    private readonly int _builderSize;

    public CopyrightInfo(string author, int year)
      : this(true, author, year)
    {
    }

    public CopyrightInfo(string author, params int[] years)
      : this(true, author, years)
    {
    }

    public CopyrightInfo(bool isSymbolUpper, string author, params int[] copyrightYears)
    {
      Assumes.NotNullOrEmpty(author, nameof (author));
      Assumes.NotZeroLength<int>(copyrightYears, nameof (copyrightYears));
      this._isSymbolUpper = isSymbolUpper;
      this._author = author;
      this._copyrightYears = copyrightYears;
      this._builderSize = 12 + author.Length + 4 * copyrightYears.Length + 10;
    }

    protected CopyrightInfo()
    {
    }

    private CopyrightInfo(AssemblyCopyrightAttribute attribute) => this._attribute = attribute;

    public static CopyrightInfo Default
    {
      get
      {
        AssemblyCopyrightAttribute attribute = ReflectionHelper.GetAttribute<AssemblyCopyrightAttribute>();
        if (attribute != null)
          return new CopyrightInfo(attribute);
        return new CopyrightInfo((ReflectionHelper.GetAttribute<AssemblyCompanyAttribute>() ?? throw new InvalidOperationException("CopyrightInfo::Default requires that you define AssemblyCopyrightAttribute or AssemblyCompanyAttribute.")).Company, DateTime.Now.Year);
      }
    }

    protected virtual string CopyrightWord => "Copyright";

    public static implicit operator string(CopyrightInfo info) => info.ToString();

    public override string ToString()
    {
      if (this._attribute != null)
        return this._attribute.Copyright;
      StringBuilder stringBuilder = new StringBuilder(this._builderSize);
      stringBuilder.Append(this.CopyrightWord);
      stringBuilder.Append(' ');
      stringBuilder.Append(this._isSymbolUpper ? "(C)" : "(c)");
      stringBuilder.Append(' ');
      stringBuilder.Append(this.FormatYears(this._copyrightYears));
      stringBuilder.Append(' ');
      stringBuilder.Append(this._author);
      return stringBuilder.ToString();
    }

    protected virtual string FormatYears(int[] years)
    {
      if (years.Length == 1)
        return years[0].ToString((IFormatProvider) CultureInfo.InvariantCulture);
      StringBuilder stringBuilder = new StringBuilder(years.Length * 6);
      for (int index1 = 0; index1 < years.Length; ++index1)
      {
        stringBuilder.Append(years[index1].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        int index2 = index1 + 1;
        if (index2 < years.Length)
          stringBuilder.Append(years[index2] - years[index1] > 1 ? " - " : ", ");
      }
      return stringBuilder.ToString();
    }
  }
}
