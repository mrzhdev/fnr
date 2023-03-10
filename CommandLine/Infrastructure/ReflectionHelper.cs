// Decompiled with JetBrains decompiler
// Type: CommandLine.Infrastructure.ReflectionHelper
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLine.Infrastructure
{
  internal static class ReflectionHelper
  {
    static ReflectionHelper()
    {
      Assembly assembly = Assembly.GetEntryAssembly();
      if ((object) assembly == null)
        assembly = Assembly.GetExecutingAssembly();
      ReflectionHelper.AssemblyFromWhichToPullInformation = assembly;
    }

    internal static Assembly AssemblyFromWhichToPullInformation { get; set; }

    public static IList<Pair<PropertyInfo, TAttribute>> RetrievePropertyList<TAttribute>(
      object target)
      where TAttribute : Attribute
    {
      Pair<Type, object> key = new Pair<Type, object>(typeof (Pair<PropertyInfo, TAttribute>), target);
      object obj = ReflectionCache.Instance[key];
      if (obj != null)
        return (IList<Pair<PropertyInfo, TAttribute>>) obj;
      IList<Pair<PropertyInfo, TAttribute>> pairList = (IList<Pair<PropertyInfo, TAttribute>>) new List<Pair<PropertyInfo, TAttribute>>();
      if (target != null)
      {
        foreach (PropertyInfo property in target.GetType().GetProperties())
        {
          if (!(property == (PropertyInfo) null) && property.CanRead && property.CanWrite)
          {
            MethodInfo setMethod = property.GetSetMethod();
            if (!(setMethod == (MethodInfo) null) && !setMethod.IsStatic)
            {
              Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) property, typeof (TAttribute), false);
              if (customAttribute != null)
                pairList.Add(new Pair<PropertyInfo, TAttribute>(property, (TAttribute) customAttribute));
            }
          }
        }
      }
      ReflectionCache.Instance[key] = (object) pairList;
      return pairList;
    }

    public static Pair<MethodInfo, TAttribute> RetrieveMethod<TAttribute>(
      object target)
      where TAttribute : Attribute
    {
      Pair<Type, object> key = new Pair<Type, object>(typeof (Pair<MethodInfo, TAttribute>), target);
      object obj = ReflectionCache.Instance[key];
      if (obj != null)
        return (Pair<MethodInfo, TAttribute>) obj;
      foreach (MethodInfo method in target.GetType().GetMethods())
      {
        if (!method.IsStatic)
        {
          Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) method, typeof (TAttribute), false);
          if (customAttribute != null)
          {
            Pair<MethodInfo, TAttribute> pair = new Pair<MethodInfo, TAttribute>(method, (TAttribute) customAttribute);
            ReflectionCache.Instance[key] = (object) pair;
            return pair;
          }
        }
      }
      return (Pair<MethodInfo, TAttribute>) null;
    }

    public static TAttribute RetrieveMethodAttributeOnly<TAttribute>(object target) where TAttribute : Attribute
    {
      Pair<Type, object> key = new Pair<Type, object>(typeof (TAttribute), target);
      object obj = ReflectionCache.Instance[key];
      if (obj != null)
        return (TAttribute) obj;
      foreach (MethodInfo method in target.GetType().GetMethods())
      {
        if (!method.IsStatic)
        {
          Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) method, typeof (TAttribute), false);
          if (customAttribute != null)
          {
            TAttribute attribute = (TAttribute) customAttribute;
            ReflectionCache.Instance[key] = (object) attribute;
            return attribute;
          }
        }
      }
      return default (TAttribute);
    }

    public static IList<TAttribute> RetrievePropertyAttributeList<TAttribute>(object target) where TAttribute : Attribute
    {
      Pair<Type, object> key = new Pair<Type, object>(typeof (IList<TAttribute>), target);
      object obj = ReflectionCache.Instance[key];
      if (obj != null)
        return (IList<TAttribute>) obj;
      IList<TAttribute> attributeList = (IList<TAttribute>) new List<TAttribute>();
      foreach (PropertyInfo property in target.GetType().GetProperties())
      {
        if (!(property == (PropertyInfo) null) && property.CanRead && property.CanWrite)
        {
          MethodInfo setMethod = property.GetSetMethod();
          if (!(setMethod == (MethodInfo) null) && !setMethod.IsStatic)
          {
            Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) property, typeof (TAttribute), false);
            if (customAttribute != null)
              attributeList.Add((TAttribute) customAttribute);
          }
        }
      }
      ReflectionCache.Instance[key] = (object) attributeList;
      return attributeList;
    }

    public static TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
    {
      object[] customAttributes = ReflectionHelper.AssemblyFromWhichToPullInformation.GetCustomAttributes(typeof (TAttribute), false);
      return customAttributes.Length <= 0 ? default (TAttribute) : (TAttribute) customAttributes[0];
    }

    public static Pair<PropertyInfo, TAttribute> RetrieveOptionProperty<TAttribute>(
      object target,
      string uniqueName)
      where TAttribute : BaseOptionAttribute
    {
      Pair<Type, object> key = new Pair<Type, object>(typeof (Pair<PropertyInfo, BaseOptionAttribute>), target);
      object obj = ReflectionCache.Instance[key];
      if (obj == null)
      {
        if (target == null)
          return (Pair<PropertyInfo, TAttribute>) null;
        foreach (PropertyInfo property in target.GetType().GetProperties())
        {
          if (!(property == (PropertyInfo) null) && property.CanRead && property.CanWrite)
          {
            MethodInfo setMethod = property.GetSetMethod();
            if (!(setMethod == (MethodInfo) null) && !setMethod.IsStatic)
            {
              Attribute customAttribute = Attribute.GetCustomAttribute((MemberInfo) property, typeof (TAttribute), false);
              TAttribute attribute = (TAttribute) customAttribute;
              if ((object) attribute != null && string.CompareOrdinal(uniqueName, attribute.UniqueName) == 0)
              {
                Pair<PropertyInfo, TAttribute> pair = new Pair<PropertyInfo, TAttribute>(property, (TAttribute) customAttribute);
                ReflectionCache.Instance[key] = (object) pair;
                return pair;
              }
            }
          }
        }
      }
      return (Pair<PropertyInfo, TAttribute>) obj;
    }

    public static bool IsNullableType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
  }
}
