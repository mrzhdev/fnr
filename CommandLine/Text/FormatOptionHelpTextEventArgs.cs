// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.FormatOptionHelpTextEventArgs
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace CommandLine.Text
{
  public class FormatOptionHelpTextEventArgs : EventArgs
  {
    private readonly BaseOptionAttribute _option;

    public FormatOptionHelpTextEventArgs(BaseOptionAttribute option) => this._option = option;

    public BaseOptionAttribute Option => this._option;
  }
}
