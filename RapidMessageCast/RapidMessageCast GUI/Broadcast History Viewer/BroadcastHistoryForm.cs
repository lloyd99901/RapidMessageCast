using System.Diagnostics;

namespace RapidMessageCast_Manager
{
    public partial class BroadcastHistoryForm : Form
    {

        public BroadcastHistoryForm()
        {
            InitializeComponent();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void BroadcastHistoryForm_Load(object sender, EventArgs e)
        {
            LoadBroadcastHistoryList();
        }

        private void LoadBroadcastHistoryList()
        {
            //clear listbox
            HistoryListBox.Items.Clear();
            //Load the broadcast history from the application directory / broadcast history folder and add it to the HistoryListBox
            string[] files = Directory.GetFiles(Application.StartupPath + "\\BroadcastHistory");
            foreach (string file in files)
            {
                HistoryListBox.Items.Add(Path.GetFileName(file));
            }
        }

        private void HistoryListBox_DoubleClick(object sender, EventArgs e)
        {
            //Get the selected item from the HistoryListBox and open a new ChildBroadcastViewer form with the selected file
            try
            {
                string selectedFile = HistoryListBox.SelectedItem.ToString();
                Broadcast_History_Viewer.ChildBroadcastViewer childForm = new Broadcast_History_Viewer.ChildBroadcastViewer(Application.StartupPath + "\\BroadcastHistory\\" + selectedFile);
                childForm.MdiParent = this;
                childForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshtoolStripButton_Click(object sender, EventArgs e)
        {
            LoadBroadcastHistoryList();
        }

        private void OpenBroadcastHistoryFolder(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = Application.StartupPath + "BroadcastHistory\\",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

    }
}
