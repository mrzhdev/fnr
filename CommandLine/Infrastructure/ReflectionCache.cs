// Decompiled with JetBrains decompiler
// Type: CommandLine.Infrastructure.ReflectionCache
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;

namespace CommandLine.Infrastructure
{
  internal sealed class ReflectionCache
  {
    private static readonly ReflectionCache Singleton = new ReflectionCache();
    private readonly IDictionary<Pair<Type, object>, WeakReference> _cache;

    private ReflectionCache() => this._cache = (IDictionary<Pair<Type, object>, WeakReference>) new Dictionary<Pair<Type, object>, WeakReference>();

    public static ReflectionCache Instance => ReflectionCache.Singleton;

    public object this[Pair<Type, object> key]
    {
      get
      {
        if (key == null)
          throw new ArgumentNullException(nameof (key));
        return !this._cache.ContainsKey(key) ? (object) null : this._cache[key].Target;
      }
      set
      {
        if (key == null)
          throw new ArgumentNullException(nameof (key));
        this._cache[key] = new WeakReference(value);
      }
    }
  }
}
