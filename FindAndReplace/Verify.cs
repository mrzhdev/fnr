// Decompiled with JetBrains decompiler
// Type: FindAndReplace.Verify
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FindAndReplace
{
  [Serializable]
  public class Verify
  {
    internal Verify()
    {
    }

    [Serializable]
    public class Argument
    {
      internal Argument()
      {
      }

      [DebuggerStepThrough]
      public static void IsNotEmpty(Guid argument, string argumentName)
      {
        if (argument == Guid.Empty)
          throw new ArgumentException(argumentName + " cannot be empty guid.", argumentName);
      }

      [DebuggerStepThrough]
      public static void IsNotEmpty(string argument, string argumentName)
      {
        if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
          throw new ArgumentException(argumentName + " cannot be empty.", argumentName);
      }

      [DebuggerStepThrough]
      public static void IsWithinLength(string argument, int length, string argumentName)
      {
        if (argument.Trim().Length > length)
          throw new ArgumentException(argumentName + " cannot be more than " + (object) length + " characters", argumentName);
      }

      [DebuggerStepThrough]
      public static void IsNull(object argument, string argumentName)
      {
        if (argument != null)
          throw new ArgumentException(argumentName + " must be null", argumentName);
      }

      [DebuggerStepThrough]
      public static void IsNotNull(object argument, string argumentName)
      {
        if (argument == null)
          throw new ArgumentNullException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositiveOrZero(int argument, string argumentName)
      {
        if (argument < 0)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositive(int argument, string argumentName)
      {
        if (argument <= 0)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositiveOrZero(long argument, string argumentName)
      {
        if (argument < 0L)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositive(long argument, string argumentName)
      {
        if (argument <= 0L)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositiveOrZero(float argument, string argumentName)
      {
        if ((double) argument < 0.0)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositive(float argument, string argumentName)
      {
        if ((double) argument <= 0.0)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositiveOrZero(Decimal argument, string argumentName)
      {
        if (argument < 0M)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositive(Decimal argument, string argumentName)
      {
        if (argument <= 0M)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositiveOrZero(TimeSpan argument, string argumentName)
      {
        if (argument < TimeSpan.Zero)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsPositive(TimeSpan argument, string argumentName)
      {
        if (argument <= TimeSpan.Zero)
          throw new ArgumentOutOfRangeException(argumentName);
      }

      [DebuggerStepThrough]
      public static void IsNotEmpty<T>(IList<T> argument, string argumentName)
      {
        Verify.Argument.IsNotNull((object) argument, argumentName);
        if (argument.Count == 0)
          throw new ArgumentException("List cannot be empty.", argumentName);
      }

      [DebuggerStepThrough]
      public static void IsInRange(int argument, int min, int max, string argumentName)
      {
        if (argument < min || argument > max)
          throw new ArgumentOutOfRangeException(argumentName, argumentName + "must be between " + (object) min + "-" + (object) max + ".");
      }

      public static void AreEqual<T>(T expected, T actual, string argumentName)
      {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
          throw new ArgumentOutOfRangeException(argumentName, argumentName + " must be " + (object) expected + ", but was " + (object) actual + ".");
      }

      public static void IsTrue(bool actual, string argumentName) => Verify.Argument.AreEqual<bool>(true, actual, argumentName);

      public static void IsFalse(bool actual, string argumentName) => Verify.Argument.AreEqual<bool>(false, actual, argumentName);

      public static void AreNotEqual<T>(T expected, T actual, string argumentName)
      {
        if (EqualityComparer<T>.Default.Equals(expected, actual))
          throw new ArgumentOutOfRangeException(argumentName, argumentName + " must not be equal to " + (object) expected + ".");
      }

      [DebuggerStepThrough]
      public static void IsValidID(int? id, string argumentName)
      {
        Verify.Argument.IsNotNull((object) id, argumentName);
        Verify.Argument.IsPositive(id.Value, argumentName);
      }
    }
  }
}
