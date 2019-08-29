using MouseTail.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MouseTail
{
    public partial class FormMain : Form
    {
        //
        private List<Watchers.FileWatcher> FileWatcherList = new List<Watchers.FileWatcher>();

        #region *** WinForm Events

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dataGridView1.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;

            dataGridView1.Visible = true;
            tabControl1.Visible = false;
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

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var formAbout = new FormAbout();
            formAbout.Show(this);
        }

        #endregion *** WinForm Events

        #region *** Private Methods

        private void UpdateListBox(object sender, string message)
        {
            if (!InvokeRequired)
            {
                Watchers.FileWatcher fileWatcher = (Watchers.FileWatcher)sender;
                UpdateListBox(fileWatcher);
            }
            else
            {
                Invoke(new Action<object, string>(UpdateListBox), sender, message);
            }
        }

        private void UpdateListBox(Watchers.FileWatcher fileWatcher)
        {
            ListBox listBox = (ListBox)fileWatcher.ListBox;

            listBox.Visible = false;
            listBox.Items.Clear();
            listBox.Items.AddRange(fileWatcher.SafeReadAllLines().ToArray<object>());
            listBox.SelectedIndex = listBox.Items.Count - 1;
            listBox.Visible = true;
        }

        private void UpdateDataGrid(object sender, string message)
        {
            if (!InvokeRequired)
            {
                Watchers.FileWatcher fileWatcher = (Watchers.FileWatcher)sender;
                UpdateDataGrid(fileWatcher);
            }
            else
            {
                Invoke(new Action<object, string>(UpdateListBox), sender, message);
            }
        }

        private void UpdateDataGrid(Watchers.FileWatcher fileWatcher)
        {
            string[] lines = fileWatcher.SafeReadAllLines();

            DataTable dt = new DataTable();
            dt.Columns.Add(fileWatcher.FileInfo.Name);

            foreach (var line in lines)
            {
                DataRow dr = dt.NewRow();
                dr[0] = line;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].HeaderCell.Style.Font = new Font("courier new", 9.75F, FontStyle.Bold);

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
                IntegralHeight = false
            };

            TabPage tabPage = new TabPage
            {
                Text = fileWatcher.FileInfo.Name,
                Controls = { listBox }
            };

            tabControl1.TabPages.Add(tabPage);
            fileWatcher.ListBox = listBox;
            FileWatcherList.Add(fileWatcher);

            //UpdateListBox(fileWatcher);
            UpdateDataGrid(fileWatcher);
        }

        #endregion *** Private Methods

        #region *** Private Callback Methods

        private void FileWatcher_OnChanged(object sender, FileSystemEventArgs e)
        {
            //UpdateListBox(sender, null);
            UpdateDataGrid(sender, null);
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