// Decompiled with JetBrains decompiler
// Type: FindAndReplace.EncodingDetector
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using href.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindAndReplace
{
  public class EncodingDetector
  {
    public static Encoding Detect(
      byte[] bytes,
      EncodingDetector.Options opts = EncodingDetector.Options.KlerkSoftBom | EncodingDetector.Options.MLang,
      Encoding defaultEncoding = null)
    {
      Encoding encoding = (Encoding) null;
      if ((opts & EncodingDetector.Options.KlerkSoftBom) == EncodingDetector.Options.KlerkSoftBom)
      {
        StopWatch.Start("DetectEncoding: UsingKlerksSoftBom");
        encoding = EncodingDetector.DetectEncodingUsingKlerksSoftBom(bytes);
        StopWatch.Stop("DetectEncoding: UsingKlerksSoftBom");
      }
      if (encoding != null)
        return encoding;
      if ((opts & EncodingDetector.Options.KlerkSoftHeuristics) == EncodingDetector.Options.KlerkSoftHeuristics)
      {
        StopWatch.Start("DetectEncoding: UsingKlerksSoftHeuristics");
        encoding = EncodingDetector.DetectEncodingUsingKlerksSoftHeuristics(bytes);
        StopWatch.Stop("DetectEncoding: UsingKlerksSoftHeuristics");
      }
      if (encoding != null)
        return encoding;
      if ((opts & EncodingDetector.Options.MLang) == EncodingDetector.Options.MLang)
      {
        StopWatch.Start("DetectEncoding: UsingMLang");
        encoding = EncodingDetector.DetectEncodingUsingMLang(bytes);
        StopWatch.Stop("DetectEncoding: UsingMLang");
      }
      if (encoding == null)
        encoding = defaultEncoding;
      return encoding;
    }

    private static Encoding DetectEncodingUsingKlerksSoftBom(byte[] bytes)
    {
      Encoding encoding = (Encoding) null;
      if (((IEnumerable<byte>) bytes).Count<byte>() >= 4)
        encoding = KlerksSoftEncodingDetector.DetectBOMBytes(bytes);
      return encoding;
    }

    private static Encoding DetectEncodingUsingKlerksSoftHeuristics(byte[] bytes) => KlerksSoftEncodingDetector.DetectUnicodeInByteSampleByHeuristics(bytes);

    private static Encoding DetectEncodingUsingMLang(byte[] bytes)
    {
      try
      {
        Encoding[] encodingArray = EncodingTools.DetectInputCodepages(bytes, 1);
        if (encodingArray.Length > 0)
          return encodingArray[0];
      }
      catch
      {
      }
      return (Encoding) null;
    }

    [Flags]
    public enum Options
    {
      KlerkSoftBom = 1,
      KlerkSoftHeuristics = 2,
      MLang = 4,
    }
  }
}
