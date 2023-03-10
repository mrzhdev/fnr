// Decompiled with JetBrains decompiler
// Type: FindAndReplace.KlerksSoftEncodingDetector
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FindAndReplace
{
  public class KlerksSoftEncodingDetector
  {
    private const long _defaultHeuristicSampleSize = 10240;

    public static Encoding DetectTextFileEncoding(
      string InputFilename,
      Encoding DefaultEncoding)
    {
      using (FileStream InputFileStream = File.OpenRead(InputFilename))
        return KlerksSoftEncodingDetector.DetectTextFileEncoding(InputFileStream, DefaultEncoding);
    }

    public static Encoding DetectTextFileEncoding(
      FileStream InputFileStream,
      Encoding DefaultEncoding)
    {
      return KlerksSoftEncodingDetector.DetectTextFileEncoding(InputFileStream, DefaultEncoding, 10240L);
    }

    public static Encoding DetectTextFileEncoding(
      FileStream InputFileStream,
      Encoding DefaultEncoding,
      long HeuristicSampleSize)
    {
      if (InputFileStream == null)
        throw new ArgumentNullException("Must provide a valid Filestream!", nameof (InputFileStream));
      if (!InputFileStream.CanRead)
        throw new ArgumentException("Provided file stream is not readable!", nameof (InputFileStream));
      long num = InputFileStream.CanSeek ? InputFileStream.Position : throw new ArgumentException("Provided file stream cannot seek!", nameof (InputFileStream));
      InputFileStream.Position = 0L;
      byte[] numArray1 = new byte[(int)(InputFileStream.Length > 4L ? new IntPtr(4) : checked ((IntPtr) InputFileStream.Length))];
      InputFileStream.Read(numArray1, 0, numArray1.Length);
      Encoding encoding = KlerksSoftEncodingDetector.DetectBOMBytes(numArray1);
      if (encoding != null)
      {
        InputFileStream.Position = num;
        return encoding;
      }
      byte[] numArray2 = new byte[(int)(HeuristicSampleSize > InputFileStream.Length ? checked ((IntPtr) InputFileStream.Length) : checked ((IntPtr) HeuristicSampleSize))];
      Array.Copy((Array) numArray1, (Array) numArray2, numArray1.Length);
      if (InputFileStream.Length > (long) numArray1.Length)
        InputFileStream.Read(numArray2, numArray1.Length, numArray2.Length - numArray1.Length);
      InputFileStream.Position = num;
      return KlerksSoftEncodingDetector.DetectUnicodeInByteSampleByHeuristics(numArray2) ?? DefaultEncoding;
    }

    public static Encoding DetectTextByteArrayEncoding(
      byte[] TextData,
      Encoding DefaultEncoding)
    {
      if (TextData == null)
        throw new ArgumentNullException("Must provide a valid text data byte array!", nameof (TextData));
      return KlerksSoftEncodingDetector.DetectBOMBytes(TextData) ?? KlerksSoftEncodingDetector.DetectUnicodeInByteSampleByHeuristics(TextData) ?? DefaultEncoding;
    }

    public static Encoding DetectBOMBytes(byte[] BOMBytes)
    {
      if (BOMBytes == null)
        throw new ArgumentNullException("Must provide a valid BOM byte array!", nameof (BOMBytes));
      if (BOMBytes.Length < 2)
        return (Encoding) null;
      if (BOMBytes[0] == byte.MaxValue && BOMBytes[1] == (byte) 254 && (BOMBytes.Length < 4 || BOMBytes[2] != (byte) 0 || BOMBytes[3] != (byte) 0))
        return Encoding.Unicode;
      if (BOMBytes[0] == (byte) 254 && BOMBytes[1] == byte.MaxValue)
        return Encoding.BigEndianUnicode;
      if (BOMBytes.Length < 3)
        return (Encoding) null;
      if (BOMBytes[0] == (byte) 239 && BOMBytes[1] == (byte) 187 && BOMBytes[2] == (byte) 191)
        return Encoding.UTF8;
      if (BOMBytes[0] == (byte) 43 && BOMBytes[1] == (byte) 47 && BOMBytes[2] == (byte) 118)
        return Encoding.UTF7;
      if (BOMBytes.Length < 4)
        return (Encoding) null;
      if (BOMBytes[0] == byte.MaxValue && BOMBytes[1] == (byte) 254 && (BOMBytes[2] == (byte) 0 && BOMBytes[3] == (byte) 0))
        return Encoding.UTF32;
      return BOMBytes[0] == (byte) 0 && BOMBytes[1] == (byte) 0 && (BOMBytes[2] == (byte) 254 && BOMBytes[3] == byte.MaxValue) ? Encoding.GetEncoding(12001) : (Encoding) null;
    }

    public static Encoding DetectUnicodeInByteSampleByHeuristics(byte[] SampleBytes)
    {
      long num1 = 0;
      long num2 = 0;
      long num3 = 0;
      long num4 = 0;
      long num5 = 0;
      long currentPos = 0;
      int num6 = 0;
      for (; currentPos < (long) SampleBytes.Length; ++currentPos)
      {
        if (SampleBytes[currentPos] == (byte) 0)
        {
          if (currentPos % 2L == 0L)
            ++num2;
          else
            ++num1;
        }
        if (KlerksSoftEncodingDetector.IsCommonUSASCIIByte(SampleBytes[currentPos]))
          ++num5;
        if (num6 == 0)
        {
          int num7 = KlerksSoftEncodingDetector.DetectSuspiciousUTF8SequenceLength(SampleBytes, currentPos);
          if (num7 > 0)
          {
            ++num3;
            num4 += (long) num7;
            num6 = num7 - 1;
          }
        }
        else
          --num6;
      }
      if ((double) num2 * 2.0 / (double) SampleBytes.Length < 0.2 && (double) num1 * 2.0 / (double) SampleBytes.Length > 0.6)
        return Encoding.Unicode;
      if ((double) num1 * 2.0 / (double) SampleBytes.Length < 0.2 && (double) num2 * 2.0 / (double) SampleBytes.Length > 0.6)
        return Encoding.BigEndianUnicode;
      return new Regex("\\A([\\x09\\x0A\\x0D\\x20-\\x7E]|[\\xC2-\\xDF][\\x80-\\xBF]|\\xE0[\\xA0-\\xBF][\\x80-\\xBF]|[\\xE1-\\xEC\\xEE\\xEF][\\x80-\\xBF]{2}|\\xED[\\x80-\\x9F][\\x80-\\xBF]|\\xF0[\\x90-\\xBF][\\x80-\\xBF]{2}|[\\xF1-\\xF3][\\x80-\\xBF]{3}|\\xF4[\\x80-\\x8F][\\x80-\\xBF]{2})*\\z").IsMatch(Encoding.ASCII.GetString(SampleBytes)) && (double) num3 * 500000.0 / (double) SampleBytes.Length >= 1.0 && ((long) SampleBytes.Length - num4 == 0L || (double) num5 * 1.0 / (double) ((long) SampleBytes.Length - num4) >= 0.8) ? Encoding.UTF8 : (Encoding) null;
    }

    private static bool IsCommonUSASCIIByte(byte testByte) => testByte == (byte) 10 || testByte == (byte) 13 || testByte == (byte) 9 || (testByte >= (byte) 32 && testByte <= (byte) 47 || testByte >= (byte) 48 && testByte <= (byte) 57) || (testByte >= (byte) 58 && testByte <= (byte) 64 || testByte >= (byte) 65 && testByte <= (byte) 90 || (testByte >= (byte) 91 && testByte <= (byte) 96 || testByte >= (byte) 97 && testByte <= (byte) 122)) || testByte >= (byte) 123 && testByte <= (byte) 126;

    private static int DetectSuspiciousUTF8SequenceLength(byte[] SampleBytes, long currentPos)
    {
      int num = 0;
      if ((long) SampleBytes.Length > currentPos + 1L && SampleBytes[currentPos] == (byte) 194)
      {
        if (SampleBytes[currentPos + 1L] == (byte) 129 || SampleBytes[currentPos + 1L] == (byte) 141 || SampleBytes[currentPos + 1L] == (byte) 143)
          num = 2;
        else if (SampleBytes[currentPos + 1L] == (byte) 144 || SampleBytes[currentPos + 1L] == (byte) 157)
          num = 2;
        else if (SampleBytes[currentPos + 1L] >= (byte) 160 && SampleBytes[currentPos + 1L] <= (byte) 191)
          num = 2;
      }
      else if ((long) SampleBytes.Length > currentPos + 1L && SampleBytes[currentPos] == (byte) 195)
      {
        if (SampleBytes[currentPos + 1L] >= (byte) 128 && SampleBytes[currentPos + 1L] <= (byte) 191)
          num = 2;
      }
      else if ((long) SampleBytes.Length > currentPos + 1L && SampleBytes[currentPos] == (byte) 197)
      {
        if (SampleBytes[currentPos + 1L] == (byte) 146 || SampleBytes[currentPos + 1L] == (byte) 147)
          num = 2;
        else if (SampleBytes[currentPos + 1L] == (byte) 160 || SampleBytes[currentPos + 1L] == (byte) 161)
          num = 2;
        else if (SampleBytes[currentPos + 1L] == (byte) 184 || SampleBytes[currentPos + 1L] == (byte) 189 || SampleBytes[currentPos + 1L] == (byte) 190)
          num = 2;
      }
      else if ((long) SampleBytes.Length > currentPos + 1L && SampleBytes[currentPos] == (byte) 198)
      {
        if (SampleBytes[currentPos + 1L] == (byte) 146)
          num = 2;
      }
      else if ((long) SampleBytes.Length > currentPos + 1L && SampleBytes[currentPos] == (byte) 203)
      {
        if (SampleBytes[currentPos + 1L] == (byte) 134 || SampleBytes[currentPos + 1L] == (byte) 156)
          num = 2;
      }
      else if ((long) SampleBytes.Length > currentPos + 2L && SampleBytes[currentPos] == (byte) 226)
      {
        if (SampleBytes[currentPos + 1L] == (byte) 128)
        {
          if (SampleBytes[currentPos + 2L] == (byte) 147 || SampleBytes[currentPos + 2L] == (byte) 148)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 152 || SampleBytes[currentPos + 2L] == (byte) 153 || SampleBytes[currentPos + 2L] == (byte) 154)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 156 || SampleBytes[currentPos + 2L] == (byte) 157 || SampleBytes[currentPos + 2L] == (byte) 158)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 160 || SampleBytes[currentPos + 2L] == (byte) 161 || SampleBytes[currentPos + 2L] == (byte) 162)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 166)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 176)
            num = 3;
          if (SampleBytes[currentPos + 2L] == (byte) 185 || SampleBytes[currentPos + 2L] == (byte) 186)
            num = 3;
        }
        else if (SampleBytes[currentPos + 1L] == (byte) 130 && SampleBytes[currentPos + 2L] == (byte) 172)
          num = 3;
        else if (SampleBytes[currentPos + 1L] == (byte) 132 && SampleBytes[currentPos + 2L] == (byte) 162)
          num = 3;
      }
      return num;
    }
  }
}
