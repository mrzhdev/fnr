// Decompiled with JetBrains decompiler
// Type: CommandLine.Text.HeadingInfo
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using CommandLine.Infrastructure;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CommandLine.Text
{
  public class HeadingInfo
  {
    private readonly string _programName;
    private readonly string _version;

    public HeadingInfo(string programName)
      : this(programName, (string) null)
    {
    }

    public HeadingInfo(string programName, string version)
    {
      Assumes.NotNullOrEmpty(programName, nameof (programName));
      this._programName = programName;
      this._version = version;
    }

    public static HeadingInfo Default
    {
      get
      {
        AssemblyTitleAttribute attribute1 = ReflectionHelper.GetAttribute<AssemblyTitleAttribute>();
        string programName = attribute1 == null ? ReflectionHelper.AssemblyFromWhichToPullInformation.GetName().Name : Path.GetFileNameWithoutExtension(attribute1.Title);
        AssemblyInformationalVersionAttribute attribute2 = ReflectionHelper.GetAttribute<AssemblyInformationalVersionAttribute>();
        string version = attribute2 == null ? ReflectionHelper.AssemblyFromWhichToPullInformation.GetName().Version.ToString() : attribute2.InformationalVersion;
        return new HeadingInfo(programName, version);
      }
    }

    public static implicit operator string(HeadingInfo info) => info.ToString();

    public override string ToString()
    {
      bool flag = string.IsNullOrEmpty(this._version);
      StringBuilder stringBuilder = new StringBuilder(this._programName.Length + (!flag ? this._version.Length + 1 : 0));
      stringBuilder.Append(this._programName);
      if (!flag)
      {
        stringBuilder.Append(' ');
        stringBuilder.Append(this._version);
      }
      return stringBuilder.ToString();
    }

    public void WriteMessage(string message, TextWriter writer)
    {
      Assumes.NotNullOrEmpty(message, nameof (message));
      Assumes.NotNull<TextWriter>(writer, nameof (writer));
      StringBuilder stringBuilder = new StringBuilder(this._programName.Length + message.Length + 2);
      stringBuilder.Append(this._programName);
      stringBuilder.Append(": ");
      stringBuilder.Append(message);
      writer.WriteLine(stringBuilder.ToString());
    }

    public void WriteMessage(string message) => this.WriteMessage(message, Console.Out);

    public void WriteError(string message) => this.WriteMessage(message, Console.Error);
  }
}
