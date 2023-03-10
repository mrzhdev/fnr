// Decompiled with JetBrains decompiler
// Type: CommandLine.Parsing.ValueMapper
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CommandLine.Parsing
{
  internal sealed class ValueMapper
  {
    private readonly CultureInfo _parsingCulture;
    private readonly object _target;
    private IList<string> _valueList;
    private ValueListAttribute _valueListAttribute;
    private IList<Pair<PropertyInfo, ValueOptionAttribute>> _valueOptionAttributeList;
    private int _valueOptionIndex;

    public ValueMapper(object target, CultureInfo parsingCulture)
    {
      this._target = target;
      this._parsingCulture = parsingCulture;
      this.InitializeValueList();
      this.InitializeValueOption();
    }

    public bool CanReceiveValues => this.IsValueListDefined || this.IsValueOptionDefined;

    private bool IsValueListDefined => this._valueListAttribute != null;

    private bool IsValueOptionDefined => this._valueOptionAttributeList.Count > 0;

    public bool MapValueItem(string item)
    {
      if (this.IsValueOptionDefined && this._valueOptionIndex < this._valueOptionAttributeList.Count)
      {
        PropertyWriter propertyWriter = new PropertyWriter(this._valueOptionAttributeList[this._valueOptionIndex++].Left, this._parsingCulture);
        return !ReflectionHelper.IsNullableType(propertyWriter.Property.PropertyType) ? propertyWriter.WriteScalar(item, this._target) : propertyWriter.WriteNullable(item, this._target);
      }
      return this.IsValueListDefined && this.AddValueItem(item);
    }

    private bool AddValueItem(string item)
    {
      if (this._valueListAttribute.MaximumElements == 0 || this._valueList.Count == this._valueListAttribute.MaximumElements)
        return false;
      this._valueList.Add(item);
      return true;
    }

    private void InitializeValueList()
    {
      this._valueListAttribute = ValueListAttribute.GetAttribute(this._target);
      if (!this.IsValueListDefined)
        return;
      this._valueList = ValueListAttribute.GetReference(this._target);
    }

    private void InitializeValueOption()
    {
      IList<Pair<PropertyInfo, ValueOptionAttribute>> source = ReflectionHelper.RetrievePropertyList<ValueOptionAttribute>(this._target);
      this._valueOptionAttributeList = source.All<Pair<PropertyInfo, ValueOptionAttribute>>((Func<Pair<PropertyInfo, ValueOptionAttribute>, bool>) (x => x.Right.Index == 0)) ? source : (IList<Pair<PropertyInfo, ValueOptionAttribute>>) source.OrderBy<Pair<PropertyInfo, ValueOptionAttribute>, int>((Func<Pair<PropertyInfo, ValueOptionAttribute>, int>) (x => x.Right.Index)).ToList<Pair<PropertyInfo, ValueOptionAttribute>>();
    }
  }
}
