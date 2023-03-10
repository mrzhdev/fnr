// Decompiled with JetBrains decompiler
// Type: href.Utils.EncodingTools
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using FindAndReplace;
using MultiLanguage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace href.Utils
{
  public static class EncodingTools
  {
    public static int[] PreferedEncodingsForStream;
    public static int[] PreferedEncodings;
    public static int[] AllEncodings;

    static EncodingTools()
    {
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      intList1.Add(Encoding.ASCII.CodePage);
      intList3.Add(Encoding.ASCII.CodePage);
      intList2.Add(Encoding.ASCII.CodePage);
      intList2.Add(Encoding.Default.CodePage);
      if (Encoding.Default.IsSingleByte)
      {
        intList1.Add(Encoding.Default.CodePage);
        intList3.Add(Encoding.Default.CodePage);
      }
      intList2.Add(50220);
      intList3.Add(50220);
      intList1.Add(Encoding.Unicode.CodePage);
      foreach (EncodingInfo encoding in Encoding.GetEncodings())
      {
        if (!intList1.Contains(encoding.CodePage) && Encoding.GetEncoding(encoding.CodePage).GetPreamble().Length > 0)
          intList1.Add(encoding.CodePage);
      }
      EncodingTools.PreferedEncodingsForStream = intList1.ToArray();
      foreach (EncodingInfo encoding in Encoding.GetEncodings())
      {
        if (encoding.GetEncoding().IsSingleByte)
        {
          if (!intList2.Contains(encoding.CodePage))
            intList2.Add(encoding.CodePage);
          if (encoding.CodePage <= 1258)
            intList3.Add(encoding.CodePage);
        }
      }
      foreach (EncodingInfo encoding in Encoding.GetEncodings())
      {
        if (!encoding.GetEncoding().IsSingleByte)
        {
          if (!intList2.Contains(encoding.CodePage))
            intList2.Add(encoding.CodePage);
          if (encoding.CodePage <= 1258)
            intList3.Add(encoding.CodePage);
        }
      }
      intList3.Add(Encoding.Unicode.CodePage);
      EncodingTools.PreferedEncodings = intList3.ToArray();
      EncodingTools.AllEncodings = intList2.ToArray();
    }

    public static bool IsAscii(string data)
    {
      switch (data)
      {
        case "":
        case null:
          return true;
        default:
          foreach (char ch in data)
          {
            if (ch > '\u007F')
              return false;
          }
          return true;
      }
    }

    public static Encoding GetMostEfficientEncoding(string input) => EncodingTools.GetMostEfficientEncoding(input, EncodingTools.PreferedEncodings);

    public static Encoding GetMostEfficientEncodingForStream(string input) => EncodingTools.GetMostEfficientEncoding(input, EncodingTools.PreferedEncodingsForStream);

    public static Encoding GetMostEfficientEncoding(string input, int[] preferedEncodings)
    {
      Encoding encoding = EncodingTools.DetectOutgoingEncoding(input, preferedEncodings, true);
      if (encoding.CodePage == Encoding.Unicode.CodePage)
      {
        int byteCount1 = Encoding.UTF7.GetByteCount(input);
        encoding = Encoding.UTF7;
        int num = byteCount1;
        int byteCount2 = Encoding.UTF8.GetByteCount(input);
        if (byteCount2 < num)
        {
          encoding = Encoding.UTF8;
          num = byteCount2;
        }
        if (Encoding.Unicode.GetByteCount(input) < num)
          encoding = Encoding.Unicode;
      }
      return encoding;
    }

    public static Encoding DetectOutgoingEncoding(string input) => EncodingTools.DetectOutgoingEncoding(input, EncodingTools.PreferedEncodings, true);

    public static Encoding DetectOutgoingStreamEncoding(string input) => EncodingTools.DetectOutgoingEncoding(input, EncodingTools.PreferedEncodingsForStream, true);

    public static Encoding[] DetectOutgoingEncodings(string input) => EncodingTools.DetectOutgoingEncodings(input, EncodingTools.PreferedEncodings, true);

    public static Encoding[] DetectOutgoingStreamEncodings(string input) => EncodingTools.DetectOutgoingEncodings(input, EncodingTools.PreferedEncodingsForStream, true);

    private static Encoding DetectOutgoingEncoding(
      string input,
      int[] preferedEncodings,
      bool preserveOrder)
    {
      switch (input)
      {
        case "":
          return Encoding.ASCII;
        case null:
          throw new ArgumentNullException(nameof (input));
        default:
          Encoding encoding = Encoding.ASCII;
          IMultiLanguage3 multiLanguage3 = (IMultiLanguage3) new CMultiLanguageClass();
          if (multiLanguage3 == null)
            throw new COMException("Failed to get IMultilang3");
          try
          {
            int[] source = new int[preferedEncodings != null ? preferedEncodings.Length : Encoding.GetEncodings().Length];
            uint length = (uint) source.Length;
            ushort lpSpecialChar = 63;
            IntPtr num1 = preferedEncodings == null ? IntPtr.Zero : Marshal.AllocCoTaskMem(4 * preferedEncodings.Length);
            IntPtr num2 = Marshal.AllocCoTaskMem(4 * source.Length);
            try
            {
              if (preferedEncodings != null)
                Marshal.Copy(preferedEncodings, 0, num1, preferedEncodings.Length);
              Marshal.Copy(source, 0, num2, source.Length);
              MLCPF dwFlags = MLCPF.MLDETECTF_VALID_NLS;
              if (preserveOrder)
                dwFlags |= MLCPF.MLDETECTF_PRESERVE_ORDER;
              if (preferedEncodings != null)
                dwFlags |= MLCPF.MLDETECTF_PREFERRED_ONLY;
              multiLanguage3.DetectOutboundCodePage(dwFlags, input, (uint) input.Length, num1, preferedEncodings == null ? 0U : (uint) preferedEncodings.Length, num2, ref length, ref lpSpecialChar);
              if (length > 0U)
              {
                int[] destination = new int[(int)(IntPtr) length];
                Marshal.Copy(num2, destination, 0, destination.Length);
                encoding = Encoding.GetEncoding(destination[0]);
              }
            }
            finally
            {
              if (num1 != IntPtr.Zero)
                Marshal.FreeCoTaskMem(num1);
              Marshal.FreeCoTaskMem(num2);
            }
          }
          finally
          {
            Marshal.FinalReleaseComObject((object) multiLanguage3);
          }
          return encoding;
      }
    }

    public static Encoding[] DetectOutgoingEncodings(
      string input,
      int[] preferedEncodings,
      bool preserveOrder)
    {
      switch (input)
      {
        case "":
          return new Encoding[1]{ Encoding.ASCII };
        case null:
          throw new ArgumentNullException(nameof (input));
        default:
          List<Encoding> encodingList = new List<Encoding>();
          IMultiLanguage3 multiLanguage3 = (IMultiLanguage3) new CMultiLanguageClass();
          if (multiLanguage3 == null)
            throw new COMException("Failed to get IMultilang3");
          try
          {
            int[] source = new int[preferedEncodings.Length];
            uint length = (uint) source.Length;
            ushort lpSpecialChar = 63;
            IntPtr num1 = Marshal.AllocCoTaskMem(4 * preferedEncodings.Length);
            IntPtr num2 = preferedEncodings == null ? IntPtr.Zero : Marshal.AllocCoTaskMem(4 * source.Length);
            try
            {
              if (preferedEncodings != null)
                Marshal.Copy(preferedEncodings, 0, num1, preferedEncodings.Length);
              Marshal.Copy(source, 0, num2, source.Length);
              MLCPF dwFlags = MLCPF.MLDETECTF_VALID_NLS | MLCPF.MLDETECTF_PREFERRED_ONLY;
              if (preserveOrder)
                dwFlags |= MLCPF.MLDETECTF_PRESERVE_ORDER;
              if (preferedEncodings != null)
                dwFlags |= MLCPF.MLDETECTF_PREFERRED_ONLY;
              multiLanguage3.DetectOutboundCodePage(dwFlags, input, (uint) input.Length, num1, preferedEncodings == null ? 0U : (uint) preferedEncodings.Length, num2, ref length, ref lpSpecialChar);
              if (length > 0U)
              {
                int[] destination = new int[(int)(IntPtr)length];
                Marshal.Copy(num2, destination, 0, destination.Length);
                for (int index = 0; (long) index < (long) length; ++index)
                  encodingList.Add(Encoding.GetEncoding(destination[index]));
              }
            }
            finally
            {
              if (num1 != IntPtr.Zero)
                Marshal.FreeCoTaskMem(num1);
              Marshal.FreeCoTaskMem(num2);
            }
          }
          finally
          {
            Marshal.FinalReleaseComObject((object) multiLanguage3);
          }
          return encodingList.ToArray();
      }
    }

    public static Encoding DetectInputCodepage(byte[] input)
    {
      try
      {
        Encoding[] encodingArray = EncodingTools.DetectInputCodepages(input, 1);
        return encodingArray.Length > 0 ? encodingArray[0] : Encoding.Default;
      }
      catch (COMException ex)
      {
        return Encoding.Default;
      }
    }

    public static Encoding[] DetectInputCodepages(byte[] input, int maxEncodings)
    {
      StopWatch.Start("DetectInputCodepages_" + (object) Thread.CurrentThread.ManagedThreadId);
      if (maxEncodings < 1)
        throw new ArgumentOutOfRangeException("at least one encoding must be returend", nameof (maxEncodings));
      if (input == null)
        throw new ArgumentNullException(nameof (input));
      if (input.Length == 0)
        return new Encoding[1]{ Encoding.ASCII };
      if (input.Length < 256)
      {
        byte[] numArray = new byte[256];
        int num = 256 / input.Length;
        for (int index = 0; index < num; ++index)
          Array.Copy((Array) input, 0, (Array) numArray, input.Length * index, input.Length);
        int length = 256 % input.Length;
        if (length > 0)
          Array.Copy((Array) input, 0, (Array) numArray, num * input.Length, length);
        input = numArray;
      }
      List<Encoding> encodingList = new List<Encoding>();
      IMultiLanguage2 multiLanguage2 = (IMultiLanguage2) new CMultiLanguageClass();
      if (multiLanguage2 == null)
        throw new COMException("Failed to get IMultilang2");
      try
      {
        DetectEncodingInfo[] detectEncodingInfoArray = new DetectEncodingInfo[maxEncodings];
        int length1 = detectEncodingInfoArray.Length;
        int length2 = input.Length;
        MLDETECTCP flags = MLDETECTCP.MLDETECTCP_NONE;
        StopWatch.Start("multilang2.DetectInputCodepage_" + (object) Thread.CurrentThread.ManagedThreadId);
        multiLanguage2.DetectInputCodepage(flags, 0U, ref input[0], ref length2, ref detectEncodingInfoArray[0], ref length1);
        StopWatch.Stop("multilang2.DetectInputCodepage_" + (object) Thread.CurrentThread.ManagedThreadId);
        if (length1 > 0)
        {
          for (int index = 0; index < length1; ++index)
            encodingList.Add(Encoding.GetEncoding((int) detectEncodingInfoArray[index].nCodePage));
        }
      }
      finally
      {
        Marshal.FinalReleaseComObject((object) multiLanguage2);
      }
      StopWatch.Stop("DetectInputCodepages_" + (object) Thread.CurrentThread.ManagedThreadId);
      return encodingList.ToArray();
    }

    public static string ReadTextFile(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      using (Stream stream = (Stream) File.Open(path, FileMode.Open))
      {
        byte[] numArray = new byte[stream.Length];
        return EncodingTools.DetectInputCodepage(numArray).GetString(numArray);
      }
    }

    public static StreamReader OpenTextFile(string path) => path != null ? EncodingTools.OpenTextStream((Stream) File.Open(path, FileMode.Open)) : throw new ArgumentNullException(nameof (path));

    public static StreamReader OpenTextStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (!stream.CanSeek)
        throw new ArgumentException("the stream must support seek operations", nameof (stream));
      Encoding encoding1 = Encoding.Default;
      stream.Seek(0L, SeekOrigin.Begin);
      byte[] numArray = new byte[Math.Min(stream.Length, 512L)];
      stream.Read(numArray, 0, numArray.Length);
      Encoding encoding2 = EncodingTools.DetectInputCodepage(numArray);
      stream.Seek(0L, SeekOrigin.Begin);
      return new StreamReader(stream, encoding2);
    }
  }
}
