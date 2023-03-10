// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.OptionInfo
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace CommandLine.Parsing
{
  [DebuggerDisplay("ShortName = {ShortName}, LongName = {LongName}")]
  internal sealed class OptionInfo
  {
    private readonly CultureInfo _parsingCulture;
    private readonly BaseOptionAttribute _attribute;
    private readonly PropertyInfo _property;
    private readonly PropertyWriter _propertyWriter;
    private readonly bool _required;
    private readonly char? _shortName;
    private readonly string _longName;
    private readonly string _mutuallyExclusiveSet;
    private readonly object _defaultValue;
    private readonly bool _hasDefaultValue;

    public OptionInfo(
      BaseOptionAttribute attribute,
      PropertyInfo property,
      CultureInfo parsingCulture)
    {
      if (attribute == null)
        throw new ArgumentNullException(nameof (attribute), "The attribute is mandatory.");
      if (property == (PropertyInfo) null)
        throw new ArgumentNullException(nameof (property), "The property is mandatory.");
      this._required = attribute.Required;
      this._shortName = attribute.ShortName;
      this._longName = attribute.LongName;
      this._mutuallyExclusiveSet = attribute.MutuallyExclusiveSet;
      this._defaultValue = attribute.DefaultValue;
      this._hasDefaultValue = attribute.HasDefaultValue;
      this._attribute = attribute;
      this._property = property;
      this._parsingCulture = parsingCulture;
      this._propertyWriter = new PropertyWriter(this._property, this._parsingCulture);
    }

    internal OptionInfo(char? shortName, string longName)
    {
      this._shortName = shortName;
      this._longName = longName;
    }

    public char? ShortName => this._shortName;

    public string LongName => this._longName;

    public string MutuallyExclusiveSet => this._mutuallyExclusiveSet;

    public bool Required => this._required;

    public bool IsBoolean => this._property.PropertyType == typeof (bool);

    public bool IsArray => this._property.PropertyType.IsArray;

    public bool IsAttributeArrayCompatible => this._attribute is OptionArrayAttribute;

    public bool IsDefined { get; set; }

    public bool ReceivedValue { get; private set; }

    public bool HasBothNames
    {
      get
      {
        char? shortName = this._shortName;
        return (shortName.HasValue ? new int?((int) shortName.GetValueOrDefault()) : new int?()).HasValue && this._longName != null;
      }
    }

    public bool HasParameterLessCtor { get; set; }

    public object GetValue(object target) => this._property.GetValue(target, (object[]) null);

    public object CreateInstance(object target)
    {
      object instance;
      try
      {
        instance = Activator.CreateInstance(this._property.PropertyType);
        this._property.SetValue(target, instance, (object[]) null);
      }
      catch (Exception ex)
      {
        throw new ParserException("Instance defined for verb command could not be created.", ex);
      }
      return instance;
    }

    public bool SetValue(string value, object options)
    {
      if (this._attribute is OptionListAttribute)
        return this.SetValueList(value, options);
      return ReflectionHelper.IsNullableType(this._property.PropertyType) ? (this.ReceivedValue = this._propertyWriter.WriteNullable(value, options)) : (this.ReceivedValue = this._propertyWriter.WriteScalar(value, options));
    }

    public bool SetValue(IList<string> values, object options)
    {
      Type elementType = this._property.PropertyType.GetElementType();
      Array instance = Array.CreateInstance(elementType, values.Count);
      for (int index = 0; index < instance.Length; ++index)
      {
        try
        {
          instance.SetValue(Convert.ChangeType((object) values[index], elementType, (IFormatProvider) this._parsingCulture), index);
          this._property.SetValue(options, (object) instance, (object[]) null);
        }
        catch (FormatException ex)
        {
          return false;
        }
      }
      return this.ReceivedValue = true;
    }

    public bool SetValue(bool value, object options)
    {
      this._property.SetValue(options, (object) value, (object[]) null);
      return this.ReceivedValue = true;
    }

    public void SetDefault(object options)
    {
      if (!this._hasDefaultValue)
        return;
      try
      {
        this._property.SetValue(options, this._defaultValue, (object[]) null);
      }
      catch (Exception ex)
      {
        throw new ParserException("Bad default value.", ex);
      }
    }

    private bool SetValueList(string value, object options)
    {
      this._property.SetValue(options, (object) new List<string>(), (object[]) null);
      IList<string> stringList = (IList<string>) this._property.GetValue(options, (object[]) null);
      string str1 = value;
      char[] chArray = new char[1]
      {
        ((OptionListAttribute) this._attribute).Separator
      };
      foreach (string str2 in str1.Split(chArray))
        stringList.Add(str2);
      return this.ReceivedValue = true;
    }
  }
}
