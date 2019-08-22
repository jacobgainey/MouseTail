using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MouseTail
{
    public partial class Form1 : Form
    {
        private List<Watcher.FileWatcher> FileWatcherList = new List<Watcher.FileWatcher>();

        public Form1()
        {
            InitializeComponent();
        }

        //public void UpdateListBox(string message)
        //{
        //    if (!InvokeRequired)
        //    {
        //        listBox1.Items.Add($"{message}");
        //    }
        //    else
        //    {
        //        Invoke(new Action<string>(UpdateListBox), message);
        //    }
        //}

        private void FileWatcher_OnChanged(object sender, FileSystemEventArgs e)
        {
            //UpdateListBox($"[{DateTime.Now}] CHANGED - {e.Name}");
        }

        private static void FileWatcher_OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now}] CREATED - {e.Name}");
        }

        private static void FileWatcher_OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now}] DELETED - {e.Name}");
        }

        private static void FileWatcher_OnRenamed(object sender, FileSystemEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now}] RENAMED - {e.Name}");
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFile();
            TabPage tabPage = new TabPage("New");
            ListBox listBox = new ListBox();

            listBox.Dock = DockStyle.Fill;
            listBox.Items.Add($"Hello {DateTime.Now}");
            listBox.IntegralHeight = false;

            tabPage.Controls.Add(listBox);
            tabControl1.TabPages.Add(tabPage);

        }

        private void OpenFile()
        {
            openFileDialog1.InitialDirectory = @"D:\test";
            openFileDialog1.ShowDialog();

            Watcher.FileWatcher fileWatcher = new Watcher.FileWatcher(openFileDialog1.FileName);
            fileWatcher.OnChanged += FileWatcher_OnChanged;
            fileWatcher.OnCreated += FileWatcher_OnCreated;
            fileWatcher.OnDeleted += FileWatcher_OnDeleted;
            fileWatcher.OnRenamed += FileWatcher_OnRenamed;

            FileWatcherList.Add(fileWatcher);
        }
    }
}