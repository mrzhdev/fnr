// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.ValidationUtils
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FindAndReplace.App
{
  public static class ValidationUtils
  {
    public static ValidationResult IsDirValid(string dir, string itemName)
    {
      ValidationResult validationResult = new ValidationResult()
      {
        IsSuccess = true,
        FieldName = itemName
      };
      if (dir.Trim() == "")
      {
        validationResult.IsSuccess = false;
        validationResult.ErrorMessage = "Dir is required";
        return validationResult;
      }
      if (!new Regex("^(([a-zA-Z]:)|(\\\\{2}[^\\/\\\\:*?<>|]+))(\\\\([^\\/\\\\:*?<>|]*))*(\\\\)?$").IsMatch(dir))
      {
        validationResult.IsSuccess = false;
        validationResult.ErrorMessage = "Dir is invalid";
        return validationResult;
      }
      if (Directory.Exists(dir))
        return validationResult;
      validationResult.IsSuccess = false;
      validationResult.ErrorMessage = "Dir does not exist";
      return validationResult;
    }

    public static ValidationResult IsNotEmpty(string text, string itemName)
    {
      ValidationResult validationResult = new ValidationResult()
      {
        IsSuccess = true,
        FieldName = itemName
      };
      if (!(text.Trim() == ""))
        return validationResult;
      validationResult.IsSuccess = false;
      validationResult.ErrorMessage = string.Format("{0} is required", (object) itemName);
      return validationResult;
    }

    public static ValidationResult IsValidRegExp(string text, string itemName)
    {
      ValidationResult validationResult = new ValidationResult()
      {
        IsSuccess = true,
        FieldName = itemName
      };
      try
      {
        Regex.Match("", text);
      }
      catch (ArgumentException ex)
      {
        validationResult.IsSuccess = false;
        validationResult.ErrorMessage = "Invalid regular expression";
      }
      return validationResult;
    }

    public static ValidationResult IsValidEncoding(
      string encodingName,
      string itemName)
    {
      ValidationResult validationResult = new ValidationResult()
      {
        IsSuccess = true,
        FieldName = itemName
      };
      try
      {
        Encoding.GetEncoding(encodingName);
      }
      catch (ArgumentException ex)
      {
        validationResult.IsSuccess = false;
        validationResult.ErrorMessage = "Invalid encoding name";
      }
      return validationResult;
    }
  }
}
