using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapidMessageCast_Manager
{
    public partial class BroadcastHistoryForm : Form
    {

        public BroadcastHistoryForm()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
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
            string selectedFile = HistoryListBox.SelectedItem.ToString();
            Broadcast_History_Viewer.ChildBroadcastViewer childForm = new Broadcast_History_Viewer.ChildBroadcastViewer(Application.StartupPath + "\\BroadcastHistory\\" + selectedFile);
            childForm.MdiParent = this;
            childForm.Show();
        }
    }
}
