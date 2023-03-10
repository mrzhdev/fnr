// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.PropertyWriter
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace CommandLine.Parsing
{
  internal sealed class PropertyWriter
  {
    private readonly CultureInfo _parsingCulture;

    public PropertyWriter(PropertyInfo property, CultureInfo parsingCulture)
    {
      this._parsingCulture = parsingCulture;
      this.Property = property;
    }

    public PropertyInfo Property { get; private set; }

    public bool WriteScalar(string value, object target)
    {
      try
      {
        object obj = !this.Property.PropertyType.IsEnum ? Convert.ChangeType((object) value, this.Property.PropertyType, (IFormatProvider) this._parsingCulture) : Enum.Parse(this.Property.PropertyType, value, true);
        this.Property.SetValue(target, obj, (object[]) null);
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
      catch (FormatException ex)
      {
        return false;
      }
      catch (ArgumentException ex)
      {
        return false;
      }
      catch (OverflowException ex)
      {
        return false;
      }
      return true;
    }

    public bool WriteNullable(string value, object target)
    {
      NullableConverter nullableConverter = new NullableConverter(this.Property.PropertyType);
      try
      {
        this.Property.SetValue(target, nullableConverter.ConvertFromString((ITypeDescriptorContext) null, this._parsingCulture, value), (object[]) null);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }
  }
}
