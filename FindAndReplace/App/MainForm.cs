// Decompiled with JetBrains decompiler
// Type: FindAndReplace.App.MainForm
// Assembly: fnr, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F717881E-3C09-4E34-AEE2-8A07A62D558B
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace FindAndReplace.App
{
  public class MainForm : Form
  {
    public const int ExtraWidthWhenResults = 350;
    private Finder _finder;
    private Replacer _replacer;
    private Thread _currentThread;
    public bool _isFindOnly;
    private FormData _lastOperationFormData;
    private IContainer components;
    private TextBox txtFind;
    private Label label1;
    private Label label2;
    private TextBox txtReplace;
    private Button btnReplace;
    private Label label4;
    private TextBox txtDir;
    private Label label5;
    private TextBox txtFileMask;
    private Button btnFindOnly;
    private CheckBox chkIsCaseSensitive;
    private CheckBox chkIncludeSubDirectories;
    private Button btnGenReplaceCommandLine;
    private TextBox txtCommandLine;
    private Label lblCommandLine;
    private Panel pnlCommandLine;
    public DataGridView gvResults;
    private Label lblResults;
    public ProgressBar progressBar;
    private Label lblStatus;
    private Panel pnlGridResults;
    private ErrorProvider errorProvider1;
    private Label txtNoMatches;
    private RichTextBox txtMatchesPreview;
    private Label lblStats;
    private CheckBox chkIsRegEx;
    private Button btnCancel;
    private TextBox txtExcludeFileMask;
    private Label label3;
    private Button btnSelectDir;
    private FolderBrowserDialog folderBrowserDialog1;
    private Button btnSwap;
    private ToolTip toolTip_btnSwap;
    private CheckBox chkSkipBinaryFileDetection;
    private CheckBox chkIncludeFilesWithoutMatches;
    private ToolTip toolTip_chkIncludeFilesWithoutMatches;
    private ToolTip toolTip_chkSkipBinaryFileDetection;
    private CheckBox chkShowEncoding;
    private ToolTip toolTip_chkShowEncoding;

    public MainForm() => this.InitializeComponent();

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this._currentThread == null || !this._currentThread.IsAlive)
        return;
      this._currentThread.Abort();
    }

    private void btnFindOnly_Click(object sender, EventArgs e)
    {
      this._isFindOnly = true;
      if (!this.ValidateForm())
        return;
      this.PrepareFinderGrid();
      this.lblStats.Text = "";
      this.lblStatus.Text = "Getting file list...";
      this.CreateListener(new Finder()
      {
        Dir = this.txtDir.Text,
        IncludeSubDirectories = this.chkIncludeSubDirectories.Checked,
        FileMask = this.txtFileMask.Text,
        FindTextHasRegEx = this.chkIsRegEx.Checked,
        FindText = this.txtFind.Text,
        IsCaseSensitive = this.chkIsCaseSensitive.Checked,
        SkipBinaryFileDetection = this.chkSkipBinaryFileDetection.Checked,
        IncludeFilesWithoutMatches = this.chkIncludeFilesWithoutMatches.Checked,
        ExcludeFileMask = this.txtExcludeFileMask.Text
      });
      this.ShowResultPanel();
      this.SaveToRegistry();
      this._currentThread = new Thread(new ThreadStart(this.DoFindWork));
      this._currentThread.IsBackground = true;
      this._currentThread.Start();
    }

    private void SaveToRegistry()
    {
      FormData formData = new FormData();
      formData.IsFindOnly = this._isFindOnly;
      formData.Dir = this.txtDir.Text;
      formData.IncludeSubDirectories = this.chkIncludeSubDirectories.Checked;
      formData.FileMask = this.txtFileMask.Text;
      formData.ExcludeFileMask = this.txtExcludeFileMask.Text;
      formData.FindText = this.txtFind.Text;
      formData.IsCaseSensitive = this.chkIsCaseSensitive.Checked;
      formData.IsRegEx = this.chkIsRegEx.Checked;
      formData.SkipBinaryFileDetection = this.chkSkipBinaryFileDetection.Checked;
      formData.IncludeFilesWithoutMatches = this.chkIncludeFilesWithoutMatches.Checked;
      formData.ShowEncoding = this.chkShowEncoding.Checked;
      formData.ReplaceText = this.txtReplace.Text;
      formData.SaveToRegistry();
      this._lastOperationFormData = formData;
    }

    private void PrepareFinderGrid()
    {
      this.gvResults.DataSource = (object) null;
      this.gvResults.Rows.Clear();
      this.gvResults.Columns.Clear();
      this.AddResultsColumn("Filename", "Filename", 250);
      this.AddResultsColumn("Path", "Path", 450);
      if (this.chkShowEncoding.Checked)
        this.AddResultsColumn("FileEncoding", "Encoding", 100);
      this.AddResultsColumn("NumMatches", "Matches", 50);
      this.AddResultsColumn("ErrorMessage", "Error", 150);
      this.gvResults.Columns.Add("MatchesPreview", "");
      this.gvResults.Columns[this.gvResults.ColumnCount - 1].Visible = false;
      this.HideMatchesPreviewPanel();
      this.progressBar.Value = 0;
    }

    private void AddResultsColumn(string dataPropertyName, string headerText, int width) => this.gvResults.Columns.Add(new DataGridViewColumn()
    {
      DataPropertyName = dataPropertyName,
      HeaderText = headerText,
      CellTemplate = (DataGridViewCell) new DataGridViewTextBoxCell(),
      Width = width,
      SortMode = DataGridViewColumnSortMode.Automatic
    });

    private void CreateListener(Finder finder)
    {
      this._finder = finder;
      this._finder.FileProcessed += new FileProcessedEventHandler(this.OnFinderFileProcessed);
    }

    private void OnFinderFileProcessed(object sender, FinderEventArgs e)
    {
      if (!this.gvResults.InvokeRequired)
        this.ShowFindResult(e.ResultItem, e.Stats, e.Status);
      else
        this.Invoke((Delegate) new MainForm.SetFindResultCallback(this.ShowFindResult), (object) e.ResultItem, (object) e.Stats, (object) e.Status);
    }

    private void ShowFindResult(Finder.FindResultItem findResultItem, Stats stats, Status status)
    {
      if (stats.Files.Total != 0)
      {
        if (findResultItem.IncludeInResultsList)
        {
          this.gvResults.Rows.Add();
          int num1 = this.gvResults.Rows.Count - 1;
          this.gvResults.Rows[num1].ContextMenuStrip = this.CreateContextMenu(num1);
          int num2 = 0;
          DataGridViewCellCollection cells1 = this.gvResults.Rows[num1].Cells;
          int index1 = num2;
          int num3 = index1 + 1;
          cells1[index1].Value = (object) findResultItem.FileName;
          DataGridViewCellCollection cells2 = this.gvResults.Rows[num1].Cells;
          int index2 = num3;
          int num4 = index2 + 1;
          cells2[index2].Value = (object) findResultItem.FileRelativePath;
          if (this._lastOperationFormData.ShowEncoding)
            this.gvResults.Rows[num1].Cells[num4++].Value = findResultItem.FileEncoding != null ? (object) findResultItem.FileEncoding.WebName : (object) string.Empty;
          DataGridViewCellCollection cells3 = this.gvResults.Rows[num1].Cells;
          int index3 = num4;
          int num5 = index3 + 1;
          cells3[index3].Value = (object) findResultItem.NumMatches;
          DataGridViewCellCollection cells4 = this.gvResults.Rows[num1].Cells;
          int index4 = num5;
          int index5 = index4 + 1;
          cells4[index4].Value = (object) findResultItem.ErrorMessage;
          this.gvResults.Rows[num1].Resizable = DataGridViewTriState.False;
          if (findResultItem.IsSuccess && findResultItem.NumMatches > 0)
          {
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(findResultItem.FilePath, findResultItem.FileEncoding))
              str = streamReader.ReadToEnd();
            List<MatchPreviewLineNumber> forMatchesPreview = Utils.GetLineNumbersForMatchesPreview(str, findResultItem.Matches);
            this.gvResults.Rows[num1].Cells[index5].Value = (object) this.GenerateMatchesPreviewText(str, forMatchesPreview.Select<MatchPreviewLineNumber, int>((Func<MatchPreviewLineNumber, int>) (ln => ln.LineNumber)).ToList<int>());
          }
          if (this.gvResults.Rows.Count == 1)
            this.gvResults.ClearSelection();
        }
        this.progressBar.Maximum = stats.Files.Total;
        this.progressBar.Value = stats.Files.Processed;
        this.lblStatus.Text = "Processing " + (object) stats.Files.Processed + " of " + (object) stats.Files.Total + " files.  Last file: " + findResultItem.FileRelativePath;
        this.ShowStats(stats);
      }
      else
      {
        this.HideResultPanel();
        this.txtNoMatches.Visible = true;
        this.HideStats();
      }
      if (status != Status.Completed && status != Status.Cancelled)
        return;
      if (status == Status.Completed)
        this.lblStatus.Text = "Processed " + (object) stats.Files.Processed + " files.";
      if (status == Status.Cancelled)
        this.lblStatus.Text = "Operation was cancelled.";
      this.EnableButtons();
    }

    private void DisableButtons() => this.UpdateButtons(false);

    private void EnableButtons() => this.UpdateButtons(true);

    private void UpdateButtons(bool enabled)
    {
      this.btnFindOnly.Enabled = enabled;
      this.btnReplace.Enabled = enabled;
      this.btnGenReplaceCommandLine.Enabled = enabled;
      this.btnCancel.Enabled = !enabled;
    }

    private void DoFindWork() => this._finder.Find();

    private void ShowResultPanel()
    {
      this.DisableButtons();
      this.txtNoMatches.Visible = false;
      this.HideCommandLinePanel();
      this.HideMatchesPreviewPanel();
      if (this.pnlGridResults.Visible)
        return;
      this.pnlGridResults.Visible = true;
      if (this.pnlCommandLine.Visible)
      {
        this.Height -= this.pnlCommandLine.Height + 10;
        this.pnlCommandLine.Visible = false;
      }
      this.Height += this.pnlGridResults.Height + 10;
      this.Width += 350;
    }

    private void HideResultPanel()
    {
      if (!this.pnlGridResults.Visible)
        return;
      this.pnlGridResults.Visible = false;
      this.Height -= this.pnlGridResults.Height + 10;
      this.Width -= 350;
    }

    private void ShowCommandLinePanel()
    {
      this.HideResultPanel();
      this.HideMatchesPreviewPanel();
      if (this.pnlCommandLine.Visible)
        return;
      this.pnlCommandLine.Visible = true;
      this.Height += this.pnlCommandLine.Height + 10;
      this.Width += 350;
    }

    private void HideCommandLinePanel()
    {
      if (!this.pnlCommandLine.Visible)
        return;
      this.pnlCommandLine.Visible = false;
      this.Height -= this.pnlCommandLine.Height + 10;
      this.Width -= 350;
    }

    private void ShowMatchesPreviewPanel()
    {
      if (this.txtMatchesPreview.Visible)
        return;
      this.txtMatchesPreview.Visible = true;
      this.Height += this.txtMatchesPreview.Height + 50;
    }

    private void HideMatchesPreviewPanel()
    {
      if (!this.txtMatchesPreview.Visible)
        return;
      this.txtMatchesPreview.Visible = false;
      this.Height -= this.txtMatchesPreview.Height + 50;
    }

    private bool ValidateForm()
    {
      bool flag = true;
      Control control1 = (Control) null;
      foreach (Control control2 in (ArrangedElementCollection) this.Controls)
      {
        control2.Focus();
        if (!this.Validate() || this.errorProvider1.GetError(control2) != "")
        {
          if (flag)
            control1 = control2;
          flag = false;
        }
        else
          this.errorProvider1.SetError(control2, "");
      }
      control1?.Focus();
      if (!flag && this.AutoValidate == AutoValidate.Disable)
        this.AutoValidate = AutoValidate.EnablePreventFocusChange;
      return flag;
    }

    private void btnReplace_Click(object sender, EventArgs e)
    {
      this._isFindOnly = false;
      if (!this.ValidateForm() || string.IsNullOrEmpty(this.txtReplace.Text) && MessageBox.Show((IWin32Window) this, "Are you sure you would like to replace with an empty string?", "Replace Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      Replacer replacer = new Replacer();
      replacer.Dir = this.txtDir.Text;
      replacer.IncludeSubDirectories = this.chkIncludeSubDirectories.Checked;
      replacer.FileMask = this.txtFileMask.Text;
      replacer.ExcludeFileMask = this.txtExcludeFileMask.Text;
      replacer.FindText = this.txtFind.Text;
      replacer.IsCaseSensitive = this.chkIsCaseSensitive.Checked;
      replacer.FindTextHasRegEx = this.chkIsRegEx.Checked;
      replacer.SkipBinaryFileDetection = this.chkSkipBinaryFileDetection.Checked;
      replacer.IncludeFilesWithoutMatches = this.chkIncludeFilesWithoutMatches.Checked;
      replacer.ReplaceText = this.txtReplace.Text;
      this.ShowResultPanel();
      this.lblStats.Text = "";
      this.lblStatus.Text = "Getting file list...";
      this.PrepareReplacerGrid();
      this.txtMatchesPreview.Visible = false;
      this.CreateListener(replacer);
      this.SaveToRegistry();
      this._currentThread = new Thread(new ThreadStart(this.DoReplaceWork));
      this._currentThread.IsBackground = true;
      this._currentThread.Start();
    }

    private void CreateListener(Replacer replacer)
    {
      this._replacer = replacer;
      this._replacer.FileProcessed += new ReplaceFileProcessedEventHandler(this.ReplaceFileProceed);
    }

    private void PrepareReplacerGrid()
    {
      this.gvResults.DataSource = (object) null;
      this.gvResults.Rows.Clear();
      this.gvResults.Columns.Clear();
      this.AddResultsColumn("Filename", "Filename", 250);
      this.AddResultsColumn("Path", "Path", 400);
      if (this.chkShowEncoding.Checked)
        this.AddResultsColumn("FileEncoding", "Encoding", 100);
      this.AddResultsColumn("NumMatches", "Matches", 50);
      this.AddResultsColumn("IsSuccess", "Replaced", 60);
      this.AddResultsColumn("ErrorMessage", "Error", 150);
      this.gvResults.Columns.Add("MatchesPreview", "");
      this.gvResults.Columns[this.gvResults.ColumnCount - 1].Visible = false;
      this.HideMatchesPreviewPanel();
      this.progressBar.Value = 0;
    }

    private void DoReplaceWork() => this._replacer.Replace();

    private void ShowReplaceResult(
      Replacer.ReplaceResultItem replaceResultItem,
      Stats stats,
      Status status)
    {
      if (stats.Files.Total > 0)
      {
        if (replaceResultItem.IncludeInResultsList)
        {
          this.gvResults.Rows.Add();
          int num1 = this.gvResults.Rows.Count - 1;
          this.gvResults.Rows[num1].ContextMenuStrip = this.CreateContextMenu(num1);
          int num2 = 0;
          DataGridViewCellCollection cells1 = this.gvResults.Rows[num1].Cells;
          int index1 = num2;
          int num3 = index1 + 1;
          cells1[index1].Value = (object) replaceResultItem.FileName;
          DataGridViewCellCollection cells2 = this.gvResults.Rows[num1].Cells;
          int index2 = num3;
          int num4 = index2 + 1;
          cells2[index2].Value = (object) replaceResultItem.FileRelativePath;
          if (this._lastOperationFormData.ShowEncoding)
            this.gvResults.Rows[num1].Cells[num4++].Value = replaceResultItem.FileEncoding != null ? (object) replaceResultItem.FileEncoding.WebName : (object) string.Empty;
          DataGridViewCellCollection cells3 = this.gvResults.Rows[num1].Cells;
          int index3 = num4;
          int num5 = index3 + 1;
          cells3[index3].Value = (object) replaceResultItem.NumMatches;
          DataGridViewCellCollection cells4 = this.gvResults.Rows[num1].Cells;
          int index4 = num5;
          int num6 = index4 + 1;
          cells4[index4].Value = replaceResultItem.IsReplaced ? (object) "Yes" : (object) "No";
          DataGridViewCellCollection cells5 = this.gvResults.Rows[num1].Cells;
          int index5 = num6;
          int index6 = index5 + 1;
          cells5[index5].Value = (object) replaceResultItem.ErrorMessage;
          this.gvResults.Rows[num1].Resizable = DataGridViewTriState.False;
          if (replaceResultItem.IsSuccess && replaceResultItem.NumMatches > 0)
          {
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(replaceResultItem.FilePath, replaceResultItem.FileEncoding))
              str = streamReader.ReadToEnd();
            List<MatchPreviewLineNumber> forMatchesPreview = Utils.GetLineNumbersForMatchesPreview(str, replaceResultItem.Matches, this._lastOperationFormData.ReplaceText.Length, true);
            this.gvResults.Rows[num1].Cells[index6].Value = (object) this.GenerateMatchesPreviewText(str, forMatchesPreview.Select<MatchPreviewLineNumber, int>((Func<MatchPreviewLineNumber, int>) (ln => ln.LineNumber)).ToList<int>());
          }
          if (this.gvResults.Rows.Count == 1)
            this.gvResults.ClearSelection();
        }
        this.progressBar.Maximum = stats.Files.Total;
        this.progressBar.Value = stats.Files.Processed;
        this.lblStatus.Text = "Processing " + (object) stats.Files.Processed + " of " + (object) stats.Files.Total + " files.  Last file: " + replaceResultItem.FileRelativePath;
        this.ShowStats(stats, true);
      }
      else
      {
        this.HideResultPanel();
        this.txtNoMatches.Visible = true;
        this.HideStats();
      }
      if (status != Status.Completed && status != Status.Cancelled)
        return;
      if (status == Status.Completed)
        this.lblStatus.Text = "Processed " + (object) stats.Files.Processed + " files.";
      if (status == Status.Cancelled)
        this.lblStatus.Text = "Operation was cancelled.";
      this.EnableButtons();
    }

    private void ReplaceFileProceed(object sender, ReplacerEventArgs e)
    {
      if (!this.gvResults.InvokeRequired)
        this.ShowReplaceResult(e.ResultItem, e.Stats, e.Status);
      else
        this.Invoke((Delegate) new MainForm.SetReplaceResultCallback(this.ShowReplaceResult), (object) e.ResultItem, (object) e.Stats, (object) e.Status);
    }

    private void btnGenReplaceCommandLine_Click(object sender, EventArgs e)
    {
      if (!this.ValidateForm())
        return;
      this.ShowCommandLinePanel();
      this.lblStats.Text = "";
      this.txtCommandLine.Clear();
      this.txtCommandLine.Text = string.Format("\"{0}\" --cl --dir \"{1}\" --fileMask \"{2}\"{3}{4}{5}{6}{7}{8}{9} --find \"{10}\" --replace \"{11}\"", (object) Application.ExecutablePath, (object) this.txtDir.Text.TrimEnd('\\'), (object) this.txtFileMask.Text, string.IsNullOrEmpty(this.txtExcludeFileMask.Text) ? (object) "" : (object) string.Format(" --excludeFileMask \"{0}\"", (object) CommandLineUtils.EncodeText(this.txtExcludeFileMask.Text)), this.chkIncludeSubDirectories.Checked ? (object) " --includeSubDirectories" : (object) "", this.chkIsCaseSensitive.Checked ? (object) " --caseSensitive" : (object) "", this.chkIsRegEx.Checked ? (object) " --useRegEx" : (object) "", this.chkSkipBinaryFileDetection.Checked ? (object) " --skipBinaryFileDetection" : (object) "", this.chkShowEncoding.Checked ? (object) " --showEncoding" : (object) "", this.chkIncludeFilesWithoutMatches.Checked ? (object) " --includeFilesWithoutMatches" : (object) "", (object) CommandLineUtils.EncodeText(this.txtFind.Text), (object) CommandLineUtils.EncodeText(this.txtReplace.Text));
    }

    private void txtDir_Validating(object sender, CancelEventArgs e)
    {
      ValidationResult validationResult = ValidationUtils.IsDirValid(this.txtDir.Text, "Dir");
      if (!validationResult.IsSuccess)
        this.errorProvider1.SetError((Control) this.txtDir, validationResult.ErrorMessage);
      else
        this.errorProvider1.SetError((Control) this.txtDir, "");
    }

    private void txtFileMask_Validating(object sender, CancelEventArgs e)
    {
      ValidationResult validationResult = ValidationUtils.IsNotEmpty(this.txtFileMask.Text, "FileMask");
      if (!validationResult.IsSuccess)
        this.errorProvider1.SetError((Control) this.txtFileMask, validationResult.ErrorMessage);
      else
        this.errorProvider1.SetError((Control) this.txtFileMask, "");
    }

    private void txtFind_Validating(object sender, CancelEventArgs e)
    {
      ValidationResult validationResult1 = ValidationUtils.IsNotEmpty(this.txtFind.Text, "Find");
      if (!validationResult1.IsSuccess)
      {
        this.errorProvider1.SetError((Control) this.txtFind, validationResult1.ErrorMessage);
      }
      else
      {
        ValidationResult validationResult2 = ValidationUtils.IsValidRegExp(this.txtFind.Text, "Find");
        if (this.chkIsRegEx.Checked && !validationResult2.IsSuccess)
          this.errorProvider1.SetError((Control) this.txtFind, validationResult2.ErrorMessage);
        else
          this.errorProvider1.SetError((Control) this.txtFind, "");
      }
    }

    private void gvResults_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      int index = this.gvResults.ColumnCount - 1;
      if (this.gvResults.Rows[e.RowIndex].Cells[index].Value == null)
      {
        this.HideMatchesPreviewPanel();
      }
      else
      {
        this.ShowMatchesPreviewPanel();
        string str1 = this.gvResults.Rows[e.RowIndex].Cells[index].Value.ToString();
        this.txtMatchesPreview.SelectionLength = 0;
        this.txtMatchesPreview.Clear();
        this.txtMatchesPreview.Text = str1;
        Font font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold);
        string str2 = (this._lastOperationFormData.IsFindOnly ? this._lastOperationFormData.FindText : this._lastOperationFormData.ReplaceText).Replace("\r\n", "\n");
        MatchCollection matchCollection = Regex.Matches(this.txtMatchesPreview.Text, !this._lastOperationFormData.IsRegEx || !this._lastOperationFormData.IsFindOnly ? Regex.Escape(str2) : str2, Utils.GetRegExOptions(this._lastOperationFormData.IsCaseSensitive));
        int num1 = 0;
        int num2 = 1000;
        foreach (Match match in matchCollection)
        {
          this.txtMatchesPreview.SelectionStart = match.Index;
          this.txtMatchesPreview.SelectionLength = match.Length;
          this.txtMatchesPreview.SelectionFont = font;
          this.txtMatchesPreview.SelectionColor = Color.CadetBlue;
          ++num1;
          if (num1 > num2)
            break;
        }
        this.txtMatchesPreview.SelectionLength = 0;
      }
    }

    private string GenerateMatchesPreviewText(string content, List<int> rowNumbers)
    {
      string newLine = Environment.NewLine;
      string[] strArray = content.Split(new string[1]
      {
        newLine
      }, StringSplitOptions.None);
      StringBuilder stringBuilder = new StringBuilder();
      rowNumbers = rowNumbers.Distinct<int>().OrderBy<int, int>((Func<int, int>) (r => r)).ToList<int>();
      int num = 0;
      string str = "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
      foreach (int rowNumber in rowNumbers)
      {
        if (rowNumber - num > 1 && num != 0)
        {
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine(str);
          stringBuilder.AppendLine("");
        }
        stringBuilder.AppendLine(strArray[rowNumber]);
        num = rowNumber;
      }
      return stringBuilder.ToString();
    }

    private void MainForm_Load(object sender, EventArgs e) => this.InitWithRegistryData();

    private void gvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex == -1)
        return;
      this.OpenFileUsingExternalApp(e.RowIndex);
    }

    private void OpenFileUsingExternalApp(int rowIndex) => Process.Start(this.txtDir.Text + this.gvResults.Rows[rowIndex].Cells[1].Value.ToString().TrimStart('.'));

    private ContextMenuStrip CreateContextMenu(int rowNumber)
    {
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.ShowImageMargin = false;
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Open");
      MainForm.GVResultEventArgs eventArgs = new MainForm.GVResultEventArgs();
      eventArgs.cellRow = rowNumber;
      toolStripMenuItem1.Click += (EventHandler) ((param0, param1) => this.contextMenu_ClickOpen((object) this, eventArgs));
      ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Open Containing Folder");
      toolStripMenuItem2.Click += (EventHandler) ((param0, param1) => this.contextMenu_ClickOpenFolder((object) this, eventArgs));
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem1);
      contextMenuStrip.Items.Add((ToolStripItem) toolStripMenuItem2);
      return contextMenuStrip;
    }

    private void contextMenu_ClickOpen(object sender, MainForm.GVResultEventArgs e) => this.OpenFileUsingExternalApp(e.cellRow);

    private void contextMenu_ClickOpenFolder(object sender, MainForm.GVResultEventArgs e) => Process.Start("explorer.exe", "/select, " + this.txtDir.Text + this.gvResults.Rows[e.cellRow].Cells[1].Value.ToString().TrimStart('.'));

    private void ShowStats(Stats stats, bool showReplaceStats = false)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Files:");
      stringBuilder.AppendLine("- Total: " + (object) stats.Files.Total);
      stringBuilder.AppendLine("- Processed: " + (object) stats.Files.Processed);
      stringBuilder.AppendLine("- Binary: " + (object) stats.Files.Binary + " (skipped)");
      stringBuilder.AppendLine("- With Matches: " + (object) stats.Files.WithMatches);
      stringBuilder.AppendLine("- Without Matches: " + (object) stats.Files.WithoutMatches);
      stringBuilder.AppendLine("- Failed to Open: " + (object) stats.Files.FailedToRead);
      if (showReplaceStats)
        stringBuilder.AppendLine("- Failed to Write: " + (object) stats.Files.FailedToWrite);
      stringBuilder.AppendLine("");
      stringBuilder.AppendLine("Matches:");
      stringBuilder.AppendLine("- Found: " + (object) stats.Matches.Found);
      if (showReplaceStats)
        stringBuilder.AppendLine("- Replaced: " + (object) stats.Matches.Replaced);
      double totalSeconds1 = stats.Time.Passed.TotalSeconds;
      double totalSeconds2 = stats.Time.Remaining.TotalSeconds;
      if (totalSeconds1 >= 1.0)
      {
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Time:");
        stringBuilder.AppendLine("- Passed: " + Utils.FormatTimeSpan(stats.Time.Passed));
        if (totalSeconds1 >= 3.0 && (int) totalSeconds2 != 0)
          stringBuilder.AppendLine("- Remaining: " + Utils.FormatTimeSpan(stats.Time.Remaining) + " (estimated)");
      }
      this.lblStats.Text = stringBuilder.ToString();
    }

    private void HideStats() => this.lblStats.Text = string.Empty;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (!this._currentThread.IsAlive)
        return;
      if (this._isFindOnly)
        this._finder.CancelFind();
      else
        this._replacer.CancelReplace();
    }

    private void InitWithRegistryData()
    {
      FormData formData = new FormData();
      if (formData.IsEmpty())
        return;
      formData.LoadFromRegistry();
      this.txtDir.Text = formData.Dir;
      this.chkIncludeSubDirectories.Checked = formData.IncludeSubDirectories;
      this.txtFileMask.Text = formData.FileMask;
      this.txtExcludeFileMask.Text = formData.ExcludeFileMask;
      this.txtFind.Text = formData.FindText;
      this.chkIsCaseSensitive.Checked = formData.IsCaseSensitive;
      this.chkIsRegEx.Checked = formData.IsRegEx;
      this.chkSkipBinaryFileDetection.Checked = formData.SkipBinaryFileDetection;
      this.chkIncludeFilesWithoutMatches.Checked = formData.IncludeFilesWithoutMatches;
      this.chkShowEncoding.Checked = formData.ShowEncoding;
      this.txtReplace.Text = formData.ReplaceText;
    }

    private void btnSelectDir_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.SelectedPath = this.txtDir.Text;
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.txtDir.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void txtReplace_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.A)
        return;
      this.txtReplace.SelectAll();
      e.SuppressKeyPress = true;
      e.Handled = true;
    }

    private void txtFind_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.A)
        return;
      this.txtFind.SelectAll();
      e.SuppressKeyPress = true;
      e.Handled = true;
    }

    private void txtCommandLine_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.A)
        return;
      this.txtCommandLine.SelectAll();
      e.SuppressKeyPress = true;
      e.Handled = true;
    }

    private void btnSwap_Click(object sender, EventArgs e)
    {
      string text = this.txtFind.Text;
      this.txtFind.Text = this.txtReplace.Text;
      this.txtReplace.Text = text;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.txtFind = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtReplace = new TextBox();
      this.btnReplace = new Button();
      this.label4 = new Label();
      this.txtDir = new TextBox();
      this.label5 = new Label();
      this.txtFileMask = new TextBox();
      this.btnFindOnly = new Button();
      this.chkIsCaseSensitive = new CheckBox();
      this.chkIncludeSubDirectories = new CheckBox();
      this.btnGenReplaceCommandLine = new Button();
      this.txtCommandLine = new TextBox();
      this.lblCommandLine = new Label();
      this.pnlCommandLine = new Panel();
      this.gvResults = new DataGridView();
      this.lblResults = new Label();
      this.progressBar = new ProgressBar();
      this.lblStatus = new Label();
      this.pnlGridResults = new Panel();
      this.btnCancel = new Button();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.txtNoMatches = new Label();
      this.lblStats = new Label();
      this.chkIsRegEx = new CheckBox();
      this.txtMatchesPreview = new RichTextBox();
      this.label3 = new Label();
      this.txtExcludeFileMask = new TextBox();
      this.btnSelectDir = new Button();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.btnSwap = new Button();
      this.toolTip_btnSwap = new ToolTip(this.components);
      this.chkSkipBinaryFileDetection = new CheckBox();
      this.chkIncludeFilesWithoutMatches = new CheckBox();
      this.toolTip_chkIncludeFilesWithoutMatches = new ToolTip(this.components);
      this.toolTip_chkSkipBinaryFileDetection = new ToolTip(this.components);
      this.chkShowEncoding = new CheckBox();
      this.toolTip_chkShowEncoding = new ToolTip(this.components);
      this.pnlCommandLine.SuspendLayout();
      ((ISupportInitialize) this.gvResults).BeginInit();
      this.pnlGridResults.SuspendLayout();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.SuspendLayout();
      this.txtFind.Location = new Point(83, 93);
      this.txtFind.Multiline = true;
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(575, 74);
      this.txtFind.TabIndex = 9;
      this.txtFind.KeyDown += new KeyEventHandler(this.txtFind_KeyDown);
      this.txtFind.Validating += new CancelEventHandler(this.txtFind_Validating);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(32, 93);
      this.label1.Name = "label1";
      this.label1.Size = new Size(30, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Find:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 219);
      this.label2.Name = "label2";
      this.label2.Size = new Size(50, 13);
      this.label2.TabIndex = 13;
      this.label2.Text = "Replace:";
      this.txtReplace.CausesValidation = false;
      this.txtReplace.Location = new Point(83, 219);
      this.txtReplace.Multiline = true;
      this.txtReplace.Name = "txtReplace";
      this.txtReplace.Size = new Size(575, 74);
      this.txtReplace.TabIndex = 14;
      this.txtReplace.KeyDown += new KeyEventHandler(this.txtReplace_KeyDown);
      this.btnReplace.Location = new Point(583, 299);
      this.btnReplace.Name = "btnReplace";
      this.btnReplace.Size = new Size(75, 23);
      this.btnReplace.TabIndex = 15;
      this.btnReplace.Text = "Replace";
      this.btnReplace.UseVisualStyleBackColor = true;
      this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(39, 19);
      this.label4.Name = "label4";
      this.label4.Size = new Size(23, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Dir:";
      this.errorProvider1.SetIconPadding((Control) this.txtDir, 30);
      this.txtDir.Location = new Point(83, 19);
      this.txtDir.Name = "txtDir";
      this.txtDir.Size = new Size(548, 20);
      this.txtDir.TabIndex = 1;
      this.txtDir.Validating += new CancelEventHandler(this.txtDir_Validating);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 67);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "File Mask:";
      this.txtFileMask.Location = new Point(83, 64);
      this.txtFileMask.Name = "txtFileMask";
      this.txtFileMask.Size = new Size(232, 20);
      this.txtFileMask.TabIndex = 5;
      this.txtFileMask.Text = "*.*";
      this.txtFileMask.Validating += new CancelEventHandler(this.txtFileMask_Validating);
      this.btnFindOnly.Location = new Point(583, 169);
      this.btnFindOnly.Name = "btnFindOnly";
      this.btnFindOnly.Size = new Size(75, 23);
      this.btnFindOnly.TabIndex = 12;
      this.btnFindOnly.Text = "Find Only";
      this.btnFindOnly.UseVisualStyleBackColor = true;
      this.btnFindOnly.Click += new EventHandler(this.btnFindOnly_Click);
      this.chkIsCaseSensitive.AutoSize = true;
      this.chkIsCaseSensitive.Location = new Point(83, 173);
      this.chkIsCaseSensitive.Name = "chkIsCaseSensitive";
      this.chkIsCaseSensitive.Size = new Size(94, 17);
      this.chkIsCaseSensitive.TabIndex = 10;
      this.chkIsCaseSensitive.Text = "Case sensitive";
      this.chkIsCaseSensitive.UseVisualStyleBackColor = true;
      this.chkIncludeSubDirectories.AutoSize = true;
      this.chkIncludeSubDirectories.Checked = true;
      this.chkIncludeSubDirectories.CheckState = CheckState.Checked;
      this.chkIncludeSubDirectories.Location = new Point(83, 41);
      this.chkIncludeSubDirectories.Name = "chkIncludeSubDirectories";
      this.chkIncludeSubDirectories.Size = new Size(132, 17);
      this.chkIncludeSubDirectories.TabIndex = 3;
      this.chkIncludeSubDirectories.Text = "Include sub-directories";
      this.chkIncludeSubDirectories.UseVisualStyleBackColor = true;
      this.btnGenReplaceCommandLine.Location = new Point(484, 326);
      this.btnGenReplaceCommandLine.Name = "btnGenReplaceCommandLine";
      this.btnGenReplaceCommandLine.Size = new Size(174, 23);
      this.btnGenReplaceCommandLine.TabIndex = 16;
      this.btnGenReplaceCommandLine.Text = "Gen Replace Command Line";
      this.btnGenReplaceCommandLine.UseVisualStyleBackColor = true;
      this.btnGenReplaceCommandLine.Click += new EventHandler(this.btnGenReplaceCommandLine_Click);
      this.txtCommandLine.Location = new Point(76, 11);
      this.txtCommandLine.Multiline = true;
      this.txtCommandLine.Name = "txtCommandLine";
      this.txtCommandLine.Size = new Size(930, 74);
      this.txtCommandLine.TabIndex = 15;
      this.txtCommandLine.KeyDown += new KeyEventHandler(this.txtCommandLine_KeyDown);
      this.lblCommandLine.AutoSize = true;
      this.lblCommandLine.Location = new Point(-3, 11);
      this.lblCommandLine.Name = "lblCommandLine";
      this.lblCommandLine.Size = new Size(80, 13);
      this.lblCommandLine.TabIndex = 20;
      this.lblCommandLine.Text = "Command Line:";
      this.pnlCommandLine.Controls.Add((Control) this.lblCommandLine);
      this.pnlCommandLine.Controls.Add((Control) this.txtCommandLine);
      this.pnlCommandLine.Location = new Point(11, 355);
      this.pnlCommandLine.Name = "pnlCommandLine";
      this.pnlCommandLine.Size = new Size(1012, 100);
      this.pnlCommandLine.TabIndex = 21;
      this.pnlCommandLine.Visible = false;
      this.gvResults.AllowUserToAddRows = false;
      this.gvResults.AllowUserToDeleteRows = false;
      this.gvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gvResults.Location = new Point(77, 10);
      this.gvResults.MultiSelect = false;
      this.gvResults.Name = "gvResults";
      this.gvResults.ReadOnly = true;
      this.gvResults.RowHeadersVisible = false;
      this.gvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.gvResults.Size = new Size(930, 129);
      this.gvResults.TabIndex = 18;
      this.gvResults.CellClick += new DataGridViewCellEventHandler(this.gvResults_CellClick);
      this.gvResults.CellDoubleClick += new DataGridViewCellEventHandler(this.gvResults_CellDoubleClick);
      this.lblResults.AutoSize = true;
      this.lblResults.Location = new Point(17, 9);
      this.lblResults.Name = "lblResults";
      this.lblResults.Size = new Size(45, 13);
      this.lblResults.TabIndex = 19;
      this.lblResults.Text = "Results:";
      this.progressBar.Location = new Point(77, 170);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(849, 23);
      this.progressBar.TabIndex = 20;
      this.lblStatus.AutoSize = true;
      this.lblStatus.Location = new Point(74, 154);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(16, 13);
      this.lblStatus.TabIndex = 21;
      this.lblStatus.Text = "...";
      this.pnlGridResults.Controls.Add((Control) this.btnCancel);
      this.pnlGridResults.Controls.Add((Control) this.lblStatus);
      this.pnlGridResults.Controls.Add((Control) this.progressBar);
      this.pnlGridResults.Controls.Add((Control) this.lblResults);
      this.pnlGridResults.Controls.Add((Control) this.gvResults);
      this.pnlGridResults.Location = new Point(10, 356);
      this.pnlGridResults.Name = "pnlGridResults";
      this.pnlGridResults.Size = new Size(1013, 196);
      this.pnlGridResults.TabIndex = 22;
      this.pnlGridResults.Visible = false;
      this.btnCancel.Location = new Point(932, 170);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 26;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      this.txtNoMatches.AutoSize = true;
      this.txtNoMatches.Location = new Point(80, 336);
      this.txtNoMatches.Name = "txtNoMatches";
      this.txtNoMatches.Size = new Size(124, 13);
      this.txtNoMatches.TabIndex = 17;
      this.txtNoMatches.Text = " No matching files found.";
      this.txtNoMatches.Visible = false;
      this.lblStats.AutoSize = true;
      this.lblStats.Location = new Point(693, 94);
      this.lblStats.Name = "lblStats";
      this.lblStats.Size = new Size(0, 13);
      this.lblStats.TabIndex = 25;
      this.chkIsRegEx.AutoSize = true;
      this.chkIsRegEx.Location = new Point(193, 173);
      this.chkIsRegEx.Name = "chkIsRegEx";
      this.chkIsRegEx.Size = new Size(138, 17);
      this.chkIsRegEx.TabIndex = 11;
      this.chkIsRegEx.Text = "Use regular expressions";
      this.chkIsRegEx.UseVisualStyleBackColor = true;
      this.txtMatchesPreview.BackColor = SystemColors.Info;
      this.txtMatchesPreview.Location = new Point(83, 566);
      this.txtMatchesPreview.Name = "txtMatchesPreview";
      this.txtMatchesPreview.ReadOnly = true;
      this.txtMatchesPreview.Size = new Size(930, 166);
      this.txtMatchesPreview.TabIndex = 24;
      this.txtMatchesPreview.Text = "";
      this.txtMatchesPreview.Visible = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(330, 67);
      this.label3.Name = "label3";
      this.label3.Size = new Size(77, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Exclude Mask:";
      this.txtExcludeFileMask.Location = new Point(408, 64);
      this.txtExcludeFileMask.Name = "txtExcludeFileMask";
      this.txtExcludeFileMask.Size = new Size(250, 20);
      this.txtExcludeFileMask.TabIndex = 7;
      this.txtExcludeFileMask.Text = "*.dll, *.exe";
      this.btnSelectDir.CausesValidation = false;
      this.btnSelectDir.Location = new Point(634, 17);
      this.btnSelectDir.Margin = new Padding(0);
      this.btnSelectDir.Name = "btnSelectDir";
      this.btnSelectDir.Size = new Size(24, 23);
      this.btnSelectDir.TabIndex = 2;
      this.btnSelectDir.Text = "...";
      this.btnSelectDir.UseVisualStyleBackColor = true;
      this.btnSelectDir.Click += new EventHandler(this.btnSelectDir_Click);
      this.folderBrowserDialog1.Description = "Select folder with files to find and replace.";
      this.btnSwap.AccessibleDescription = "";
      this.btnSwap.CausesValidation = false;
      this.btnSwap.Location = new Point(521, 181);
      this.btnSwap.Name = "btnSwap";
      this.btnSwap.Size = new Size(32, 23);
      this.btnSwap.TabIndex = 26;
      this.btnSwap.Text = "↑ ↓";
      this.toolTip_btnSwap.SetToolTip((Control) this.btnSwap, "Swap find text and replace text");
      this.btnSwap.UseVisualStyleBackColor = true;
      this.btnSwap.Click += new EventHandler(this.btnSwap_Click);
      this.chkSkipBinaryFileDetection.AutoSize = true;
      this.chkSkipBinaryFileDetection.Location = new Point(346, 173);
      this.chkSkipBinaryFileDetection.Name = "chkSkipBinaryFileDetection";
      this.chkSkipBinaryFileDetection.Size = new Size(141, 17);
      this.chkSkipBinaryFileDetection.TabIndex = 10;
      this.chkSkipBinaryFileDetection.Text = "Skip binary file detection";
      this.toolTip_chkSkipBinaryFileDetection.SetToolTip((Control) this.chkSkipBinaryFileDetection, "Include binary files when searching for the string in 'Find'.");
      this.chkSkipBinaryFileDetection.UseVisualStyleBackColor = true;
      this.chkIncludeFilesWithoutMatches.AutoSize = true;
      this.chkIncludeFilesWithoutMatches.Location = new Point(193, 196);
      this.chkIncludeFilesWithoutMatches.Name = "chkIncludeFilesWithoutMatches";
      this.chkIncludeFilesWithoutMatches.Size = new Size(162, 17);
      this.chkIncludeFilesWithoutMatches.TabIndex = 11;
      this.chkIncludeFilesWithoutMatches.Text = "Include files without matches";
      this.toolTip_chkIncludeFilesWithoutMatches.SetToolTip((Control) this.chkIncludeFilesWithoutMatches, "Show files without matches in results.");
      this.chkIncludeFilesWithoutMatches.UseVisualStyleBackColor = true;
      this.chkShowEncoding.AutoSize = true;
      this.chkShowEncoding.Location = new Point(83, 196);
      this.chkShowEncoding.Name = "chkShowEncoding";
      this.chkShowEncoding.Size = new Size(100, 17);
      this.chkShowEncoding.TabIndex = 27;
      this.chkShowEncoding.TabStop = false;
      this.chkShowEncoding.Text = "Show encoding";
      this.toolTip_chkShowEncoding.SetToolTip((Control) this.chkShowEncoding, "Indicate encoding detected for each file");
      this.chkShowEncoding.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoValidate = AutoValidate.Disable;
      this.ClientSize = new Size(712, 355);
      this.Controls.Add((Control) this.chkShowEncoding);
      this.Controls.Add((Control) this.btnSwap);
      this.Controls.Add((Control) this.btnSelectDir);
      this.Controls.Add((Control) this.txtExcludeFileMask);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.chkIncludeFilesWithoutMatches);
      this.Controls.Add((Control) this.chkIsRegEx);
      this.Controls.Add((Control) this.lblStats);
      this.Controls.Add((Control) this.txtMatchesPreview);
      this.Controls.Add((Control) this.txtNoMatches);
      this.Controls.Add((Control) this.pnlGridResults);
      this.Controls.Add((Control) this.btnGenReplaceCommandLine);
      this.Controls.Add((Control) this.chkIncludeSubDirectories);
      this.Controls.Add((Control) this.chkSkipBinaryFileDetection);
      this.Controls.Add((Control) this.chkIsCaseSensitive);
      this.Controls.Add((Control) this.btnFindOnly);
      this.Controls.Add((Control) this.txtFileMask);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtDir);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnReplace);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtReplace);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtFind);
      this.Controls.Add((Control) this.pnlCommandLine);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (MainForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Find and Replace";
      this.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
      this.Load += new EventHandler(this.MainForm_Load);
      this.pnlCommandLine.ResumeLayout(false);
      this.pnlCommandLine.PerformLayout();
      ((ISupportInitialize) this.gvResults).EndInit();
      this.pnlGridResults.ResumeLayout(false);
      this.pnlGridResults.PerformLayout();
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate void SetFindResultCallback(
      Finder.FindResultItem resultItem,
      Stats stats,
      Status status);

    private delegate void SetReplaceResultCallback(
      Replacer.ReplaceResultItem resultItem,
      Stats stats,
      Status status);

    public class GVResultEventArgs : EventArgs
    {
      public int cellRow { get; set; }
    }
  }
}
