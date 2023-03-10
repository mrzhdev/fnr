// Decompiled with JetBrains decompiler
// Type: FindAndReplace.StopWatch
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FindAndReplace
{
  [Serializable]
  public class StopWatch
  {
    private static ConcurrentDictionary<string, StopWatch> _stopWatches;
    private DateTime _endTime;
    private DateTime _startTime;
    private bool _started;
    private int _stopCount;
    private TimeSpan _timeSpan;

    public StopWatch() => this.Reset();

    public TimeSpan ElapsedSpan
    {
      get
      {
        if (!this.IsStarted())
          throw new InvalidOperationException("Stopwatch needs to be running");
        return this._timeSpan.Add(DateTime.Now.Subtract(this._startTime));
      }
    }

    public int ElapsedMilliseconds
    {
      get
      {
        if (!this.IsStarted())
          throw new InvalidOperationException("Stopwatch needs to be running");
        return (int) this._timeSpan.Add(DateTime.Now.Subtract(this._startTime)).TotalMilliseconds;
      }
    }

    public int Milliseconds
    {
      get => (int) this._timeSpan.TotalMilliseconds;
      set
      {
      }
    }

    public int AverageMilliseconds
    {
      get
      {
        int num = this._stopCount;
        if (this._stopCount < 1)
          num = 1;
        return this.Milliseconds / num;
      }
      set
      {
      }
    }

    public int StopCount
    {
      get => this._stopCount;
      set => this._stopCount = value;
    }

    public TimeSpan Span
    {
      get => this._timeSpan;
      set => this._timeSpan = value;
    }

    public static ConcurrentDictionary<string, StopWatch> Collection
    {
      get => StopWatch._stopWatches;
      set => StopWatch._stopWatches = value;
    }

    public void Reset()
    {
      this._started = false;
      this._stopCount = 0;
      this._timeSpan = new TimeSpan(0L);
    }

    public bool IsStarted() => this._started;

    public void Start()
    {
      this._startTime = DateTime.Now;
      this._started = true;
    }

    public void Stop()
    {
      if (!this._started)
        throw new InvalidOperationException("Timer could not be stopped.  Start() function must be called before Stop().");
      this._endTime = DateTime.Now;
      this._timeSpan = this._timeSpan.Add(this._endTime.Subtract(this._startTime));
      ++this._stopCount;
      this._started = false;
    }

    public static void Start(string key, bool reset = false)
    {
      if (StopWatch._stopWatches == null)
        StopWatch._stopWatches = new ConcurrentDictionary<string, StopWatch>();
      if (!StopWatch._stopWatches.ContainsKey(key))
        StopWatch._stopWatches[key] = new StopWatch();
      if (reset)
        StopWatch._stopWatches[key].Reset();
      StopWatch._stopWatches[key].Start();
    }

    public static void Stop(string key) => StopWatch._stopWatches[key].Stop();

    public static void PrintCollection(int? totalMilliseconds)
    {
      StringBuilder sb = new StringBuilder();
      StopWatch.PrintCollection(totalMilliseconds, sb);
      Console.Write(sb.ToString());
    }

    public static void PrintCollection(int? totalMilliseconds, StringBuilder sb)
    {
      sb.AppendLine("Print Stop Watches: " + (object) DateTime.Now);
      foreach (string key in (IEnumerable<string>) StopWatch._stopWatches.Keys)
      {
        StopWatch stopWatch = StopWatch._stopWatches[key];
        Decimal num1 = Math.Round((Decimal) stopWatch.Milliseconds / 1000M, 1);
        string str1 = key + ": " + (object) num1 + " secs, " + (object) stopWatch.Milliseconds + " millisecs";
        Decimal num2 = Math.Round((Decimal) stopWatch.Milliseconds / (Decimal) stopWatch.StopCount, 1);
        string str2 = str1 + ", " + (object) stopWatch.StopCount + " stops, Avg Duration " + (object) num2 + " millisecs";
        if (totalMilliseconds.HasValue)
        {
          Decimal num3 = Math.Round((Decimal) stopWatch.Milliseconds / (Decimal) totalMilliseconds.Value * 100M, 1);
          str2 = str2 + ", " + (object) num3 + "%";
        }
        sb.AppendLine(str2);
      }
      sb.AppendLine("Total: " + (object) totalMilliseconds + " millisecs");
    }
  }
}
