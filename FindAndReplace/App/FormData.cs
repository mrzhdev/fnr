// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.FormData
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using Microsoft.Win32;
using System.Windows.Forms;

namespace FindAndReplace.App
{
  public class FormData
  {
    private static readonly string _versionIndependentRegKey = FormData.GetVersionIndependentRegKey();

    public bool IsFindOnly { get; set; }

    public string Dir { get; set; }

    public bool IncludeSubDirectories { get; set; }

    public string FileMask { get; set; }

    public string ExcludeFileMask { get; set; }

    public string FindText { get; set; }

    public bool IsCaseSensitive { get; set; }

    public bool IsRegEx { get; set; }

    public bool SkipBinaryFileDetection { get; set; }

    public bool ShowEncoding { get; set; }

    public bool IncludeFilesWithoutMatches { get; set; }

    public string ReplaceText { get; set; }

    private static string GetVersionIndependentRegKey()
    {
      string name = Application.UserAppDataRegistry.Name;
      return name.Substring(0, name.LastIndexOf("\\")) + "\\1.0.0.0";
    }

    public void SaveToRegistry()
    {
      this.SaveValueToRegistry("Dir", this.Dir);
      this.SaveValueToRegistry("IncludeSubDirectories", this.IncludeSubDirectories.ToString());
      this.SaveValueToRegistry("FileMask", this.FileMask);
      this.SaveValueToRegistry("ExcludeFileMask", this.ExcludeFileMask);
      this.SaveValueToRegistry("FindText", this.FindText);
      this.SaveValueToRegistry("IsCaseSensitive", this.IsCaseSensitive.ToString());
      this.SaveValueToRegistry("IsRegEx", this.IsRegEx.ToString());
      this.SaveValueToRegistry("SkipBinaryFileDetection", this.SkipBinaryFileDetection.ToString());
      this.SaveValueToRegistry("ShowEncoding", this.ShowEncoding.ToString());
      this.SaveValueToRegistry("IncludeFilesWithoutMatches", this.IncludeFilesWithoutMatches.ToString());
      this.SaveValueToRegistry("ReplaceText", this.ReplaceText);
    }

    public bool IsEmpty() => this.GetValueFromRegistry("Dir") == null;

    public void LoadFromRegistry()
    {
      this.Dir = this.GetValueFromRegistry("Dir");
      this.IncludeSubDirectories = this.GetValueFromRegistry("IncludeSubDirectories") == "True";
      this.FileMask = this.GetValueFromRegistry("Filemask");
      this.ExcludeFileMask = this.GetValueFromRegistry("ExcludeFileMask");
      this.FindText = this.GetValueFromRegistry("FindText");
      this.IsCaseSensitive = this.GetValueFromRegistry("IsCaseSensitive") == "True";
      this.IsRegEx = this.GetValueFromRegistry("IsRegEx") == "True";
      this.SkipBinaryFileDetection = this.GetValueFromRegistry("SkipBinaryFileDetection") == "True";
      this.IncludeFilesWithoutMatches = this.GetValueFromRegistry("IncludeFilesWithoutMatches") == "True";
      this.ShowEncoding = this.GetValueFromRegistry("ShowEncoding") == "True";
      this.ReplaceText = this.GetValueFromRegistry("ReplaceText");
    }

    private void SaveValueToRegistry(string name, string value) => Registry.SetValue(FormData._versionIndependentRegKey, name, (object) value, RegistryValueKind.String);

    private string GetValueFromRegistry(string name) => Registry.GetValue(FormData._versionIndependentRegKey, name, (object) null)?.ToString();
  }
}
