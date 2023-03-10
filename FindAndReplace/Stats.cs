// Decompiled with JetBrains decompiler
// Type: FindAndReplace.Stats
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;

namespace FindAndReplace
{
  public class Stats
  {
    public Stats.StatsFiles Files { get; set; }

    public Stats.StatsMatches Matches { get; set; }

    public Stats.StatsTime Time { get; set; }

    public Stats()
    {
      this.Files = new Stats.StatsFiles();
      this.Matches = new Stats.StatsMatches();
      this.Time = new Stats.StatsTime();
    }

    public void UpdateTime(DateTime startTime, DateTime startTimeProcessingFiles)
    {
      DateTime now = DateTime.Now;
      this.Time.Passed = now.Subtract(startTime);
      this.Time.Remaining = TimeSpan.FromSeconds(now.Subtract(startTimeProcessingFiles).TotalSeconds / (double) this.Files.Processed * (double) (this.Files.Total - this.Files.Processed));
    }

    public class StatsFiles
    {
      public int Total { get; set; }

      public int Processed { get; set; }

      public int Binary { get; set; }

      public int WithMatches { get; set; }

      public int WithoutMatches { get; set; }

      public int FailedToRead { get; set; }

      public int FailedToWrite { get; set; }
    }

    public class StatsMatches
    {
      public int Found { get; set; }

      public int Replaced { get; set; }
    }

    public class StatsTime
    {
      public TimeSpan Passed { get; set; }

      public TimeSpan Remaining { get; set; }
    }
  }
}
