// Decompiled with JetBrains decompiler
// Type: CommandLine.ValueListAttribute
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLine
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public sealed class ValueListAttribute : Attribute
  {
    private readonly Type _concreteType;

    public ValueListAttribute(Type concreteType)
      : this()
    {
      if (concreteType == (Type) null)
        throw new ArgumentNullException(nameof (concreteType));
      this._concreteType = typeof (IList<string>).IsAssignableFrom(concreteType) ? concreteType : throw new ParserException("The types are incompatible.");
    }

    private ValueListAttribute() => this.MaximumElements = -1;

    public int MaximumElements { get; set; }

    public Type ConcreteType => this._concreteType;

    internal static IList<string> GetReference(object target)
    {
      Type concreteType;
      PropertyInfo property = ValueListAttribute.GetProperty(target, out concreteType);
      if (property == (PropertyInfo) null || concreteType == (Type) null)
        return (IList<string>) null;
      property.SetValue(target, Activator.CreateInstance(concreteType), (object[]) null);
      return (IList<string>) property.GetValue(target, (object[]) null);
    }

    internal static ValueListAttribute GetAttribute(object target)
    {
      IList<Pair<PropertyInfo, ValueListAttribute>> pairList = ReflectionHelper.RetrievePropertyList<ValueListAttribute>(target);
      if (pairList == null || pairList.Count == 0)
        return (ValueListAttribute) null;
      return pairList.Count <= 1 ? pairList[0].Right : throw new InvalidOperationException();
    }

    private static PropertyInfo GetProperty(object target, out Type concreteType)
    {
      concreteType = (Type) null;
      IList<Pair<PropertyInfo, ValueListAttribute>> pairList = ReflectionHelper.RetrievePropertyList<ValueListAttribute>(target);
      if (pairList == null || pairList.Count == 0)
        return (PropertyInfo) null;
      Pair<PropertyInfo, ValueListAttribute> pair = pairList.Count <= 1 ? pairList[0] : throw new InvalidOperationException();
      concreteType = pair.Right.ConcreteType;
      return pair.Left;
    }
  }
}
