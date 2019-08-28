using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MouseTail
{
    public partial class Form1 : Form
    {
        // 
        private List<Watchers.FileWatcher> FileWatcherList = new List<Watchers.FileWatcher>();

        #region *** WinForm Events

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;
            toolStripStatusLabel1.Text = FileWatcherList[i].FileInfo.Name;
        }

        #endregion *** WinForm Events

        #region *** Private Methods

        private void UpdateListBox(object sender, string message)
        {
            if (!InvokeRequired)
            {
                Watchers.FileWatcher fileWatcher = (Watchers.FileWatcher)sender;
                ListBox listBox = (ListBox)fileWatcher.ListBox;
                listBox.Items.Add($"{message}");
            }
            else
            {
                Invoke(new Action<object, string>(UpdateListBox), sender, message);
            }
        }

        private void OpenFile()
        {
            openFileDialog1.InitialDirectory = @"D:\test";
            openFileDialog1.ShowDialog();

            Watchers.FileWatcher fileWatcher = new Watchers.FileWatcher(openFileDialog1.FileName);
            fileWatcher.OnChanged += FileWatcher_OnChanged;
            fileWatcher.OnCreated += FileWatcher_OnCreated;
            fileWatcher.OnDeleted += FileWatcher_OnDeleted;
            fileWatcher.OnRenamed += FileWatcher_OnRenamed;

            ListBox listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                Items =
                {
                    $"Hello {DateTime.Now}",
                    $" count {this.FileWatcherList.Count}"
                }
            };

            TabPage tabPage = new TabPage
            {
                Text = fileWatcher.FileInfo.Name,
                Controls = { listBox }
            };

            tabControl1.TabPages.Add(tabPage);
            fileWatcher.ListBox = listBox;
            FileWatcherList.Add(fileWatcher);
        }

        #endregion *** Private Methods

        #region *** Private Callback Methods

        private void FileWatcher_OnChanged(object sender, FileSystemEventArgs e)
        {
            UpdateListBox(sender, $"[{DateTime.Now}] CHANGED - {e.Name}");
        }

        private void FileWatcher_OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now}] CREATED - {e.Name}");
        }

        private void FileWatcher_OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] DELETED - {e.Name}");
        }

        private void FileWatcher_OnRenamed(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] RENAMED - {e.Name}");
        }

        #endregion *** Private Callback Methods
    }
}