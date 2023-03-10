// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.Program
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FindAndReplace.App
{
  internal static class Program
  {
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    [DllImport("kernel32", SetLastError = true)]
    private static extern bool AttachConsole(int dwProcessId);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [STAThread]
    private static int Main(string[] args)
    {
      AppDomain.CurrentDomain.AssemblyResolve += new System.ResolveEventHandler(Program.ResolveEventHandler);
      if (args.Length != 0 && args[0] == "--cl")
      {
        int lpdwProcessId;
        int windowThreadProcessId = (int) Program.GetWindowThreadProcessId(Program.GetForegroundWindow(), out lpdwProcessId);
        Process processById = Process.GetProcessById(lpdwProcessId);
        if (processById.ProcessName == "cmd")
          Program.AttachConsole(processById.Id);
        else
          Program.AllocConsole();
        int num = new CommandLineRunner().Run(args);
        Program.FreeConsole();
        return num;
      }
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
      return 0;
    }

    private static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
    {
      string dllName = new AssemblyName(args.Name).Name + ".dll";
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string name = ((IEnumerable<string>) executingAssembly.GetManifestResourceNames()).FirstOrDefault<string>((Func<string, bool>) (rn => rn.EndsWith(dllName)));
      if (name == null)
        return (Assembly) null;
      using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name))
      {
        byte[] numArray = new byte[manifestResourceStream.Length];
        manifestResourceStream.Read(numArray, 0, numArray.Length);
        return Assembly.Load(numArray);
      }
    }
  }
}
