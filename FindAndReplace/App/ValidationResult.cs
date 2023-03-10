// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.ValidationResult
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

namespace FindAndReplace.App
{
  public class ValidationResult
  {
    public bool IsSuccess { get; set; }

    public string ErrorMessage { get; set; }

    public string FieldName { get; set; }
  }
}
