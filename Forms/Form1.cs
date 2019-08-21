using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MouseTail
{
    public partial class Form1 : Form
    {
        private List<Watcher.FileWatcher> FileWatcherList;

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Watcher.FileWatcher fileWatcher = new Watcher.FileWatcher(@"D:\test\test1.txt");
            fileWatcher.OnChanged += FileWatcher_OnChanged;
            fileWatcher.OnCreated += FileWatcher_OnCreated;
            fileWatcher.OnDeleted += FileWatcher_OnDeleted;
            fileWatcher.OnRenamed += FileWatcher_OnRenamed;

            FileWatcherList.Add(fileWatcher);
        }

        public void UpdateListBox(string message)
        {
            if (!InvokeRequired)
            {
                listBox1.Items.Add($"{message}");
            }
            else
            {
                Invoke(new Action<string>(UpdateListBox), message);
            }
        }

        private void FileWatcher_OnChanged(object sender, FileSystemEventArgs e)
        {
            UpdateListBox($"[{DateTime.Now}] CHANGED - {e.Name}");
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
    }
}