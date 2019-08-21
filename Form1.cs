using System;
using System.IO;
using System.Windows.Forms;

namespace MouseTail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Watcher.FileWatcher fileWatcher1 = new Watcher.FileWatcher(@"D:\test\test1.txt");
            fileWatcher1.OnChanged += FileWatcher_OnChanged;
            fileWatcher1.OnCreated += FileWatcher_OnCreated;
            fileWatcher1.OnDeleted += FileWatcher_OnDeleted;
            fileWatcher1.OnRenamed += FileWatcher_OnRenamed;
            Watcher.FileWatcher fileWatcher2 = new Watcher.FileWatcher(@"D:\test\test2.txt");
            fileWatcher2.OnChanged += FileWatcher_OnChanged;
            fileWatcher2.OnCreated += FileWatcher_OnCreated;
            fileWatcher2.OnDeleted += FileWatcher_OnDeleted;
            fileWatcher2.OnRenamed += FileWatcher_OnRenamed;
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