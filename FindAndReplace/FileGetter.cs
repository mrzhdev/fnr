// Decompiled with JetBrains decompiler
// Type: FindAndReplace.FileGetter
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FindAndReplace
{
  public class FileGetter
  {
    public Task _task;
    public BlockingCollection<string> FileCollection = new BlockingCollection<string>();
    public ConcurrentQueue<string> FileQueue = new ConcurrentQueue<string>();
    private int _fileCount;

    public bool UseBlockingCollection { get; set; }

    public string DirPath { get; set; }

    public List<string> FileMasks { get; set; }

    public List<string> ExcludeFileMasks { get; set; }

    public List<string> ExcludeFileMasksRegEx { get; set; }

    public SearchOption SearchOption { get; set; }

    public bool IsCancelRequested { get; set; }

    public bool IsCancelled { get; set; }

    public int FileCount => this._fileCount;

    public bool IsFileCountFinal { get; set; }

    public FileGetter() => this.UseBlockingCollection = true;

    public void RunAsync() => this._task = Task.Factory.StartNew(new Action(this.Run));

    private void Run()
    {
      this.IsCancelled = false;
      this._fileCount = 0;
      StopWatch.Start("FileGetter.Run");
      foreach (string fileMask in this.FileMasks)
      {
        StopWatch.Start("FileGetter.Run Directory.EnumerateFiles");
        IEnumerable<string> strings = Directory.EnumerateFiles(this.DirPath, fileMask, this.SearchOption);
        StopWatch.Stop("FileGetter.Run Directory.EnumerateFiles");
        foreach (string filePath in strings)
        {
          StopWatch.Start("FileGetter.Run IsMatchWithExcludeFileMasks");
          bool flag = this.IsMatchWithExcludeFileMasks(filePath);
          StopWatch.Stop("FileGetter.Run IsMatchWithExcludeFileMasks");
          if (!flag)
          {
            ++this._fileCount;
            if (this.UseBlockingCollection)
            {
              this.FileCollection.Add(filePath);
            }
            else
            {
              StopWatch.Start("FileGetter.Run FileQueue.Enqueue");
              this.FileQueue.Enqueue(filePath);
              StopWatch.Stop("FileGetter.Run FileQueue.Enqueue");
            }
          }
          if (this.IsCancelRequested)
            break;
        }
        if (this.IsCancelRequested)
          break;
      }
      if (this.IsCancelRequested)
        this.IsCancelled = true;
      else
        this.IsFileCountFinal = true;
      if (this.UseBlockingCollection)
        this.FileCollection.CompleteAdding();
      StopWatch.Stop("FileGetter.Run");
      StopWatch.PrintCollection(new int?(StopWatch.Collection["FileGetter.Run"].Milliseconds));
    }

    public List<string> RunSync()
    {
      this.RunAsync();
      List<string> stringList = new List<string>();
      while (true)
      {
        string result;
        while (!this.UseBlockingCollection)
        {
          bool flag = false;
          if (this.FileQueue.TryDequeue(out result))
          {
            stringList.Add(result);
            flag = true;
          }
          if (!this.FileQueue.IsEmpty || !this.IsFileCountFinal)
          {
            if (!flag)
              Thread.Sleep(100);
          }
          else
            goto label_11;
        }
        try
        {
          result = this.FileCollection.Take();
        }
        catch (InvalidOperationException ex)
        {
          if (!this.FileCollection.IsCompleted)
            throw;
          else
            break;
        }
        stringList.Add(result);
      }
label_11:
      return stringList;
    }

    public List<string> RunSync2()
    {
      this.RunAsync();
      List<string> stringList = new List<string>();
      while (!this.IsFileCountFinal)
      {
        string result;
        if (this.FileQueue.TryDequeue(out result))
          stringList.Add(result);
      }
      return stringList;
    }

    private bool IsMatchWithExcludeFileMasks(string filePath)
    {
      if (this.ExcludeFileMasks == null || this.ExcludeFileMasks.Count == 0)
        return false;
      if (this.ExcludeFileMasksRegEx == null)
        this.ExcludeFileMasksRegEx = this.ExcludeFileMasks.Select<string, string>(new Func<string, string>(Utils.WildcardToRegex)).ToList<string>();
      foreach (string pattern in this.ExcludeFileMasksRegEx)
      {
        if (Regex.IsMatch(filePath, pattern))
          return true;
      }
      return false;
    }

    public void Cancel() => this.IsCancelRequested = true;
  }
}
