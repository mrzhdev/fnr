// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.Properties.Resources
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace FindAndReplace.App.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) FindAndReplace.App.Properties.Resources.resourceMan, (object) null))
          FindAndReplace.App.Properties.Resources.resourceMan = new ResourceManager("FindAndReplace.App.Properties.Resources", typeof (FindAndReplace.App.Properties.Resources).Assembly);
        return FindAndReplace.App.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => FindAndReplace.App.Properties.Resources.resourceCulture;
      set => FindAndReplace.App.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap swap_icon => (Bitmap) FindAndReplace.App.Properties.Resources.ResourceManager.GetObject(nameof (swap_icon), FindAndReplace.App.Properties.Resources.resourceCulture);
  }
}
