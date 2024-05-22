using System.Diagnostics;

//--RapidMessageCast Software--
//BroadcastHistoryForm.cs - RapidMessageCast Manager

//Copyright (c) 2024 Lunar/lloyd99901

//MIT License
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

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
                string? selectedFile = HistoryListBox.SelectedItem?.ToString();
                Broadcast_History_Viewer.ChildBroadcastViewer childForm = new(Application.StartupPath + "\\BroadcastHistory\\" + selectedFile)
                {
                    MdiParent = this
                };
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
            ProcessStartInfo startInfo = new()
            {
                Arguments = Application.StartupPath + "BroadcastHistory\\",
                FileName = "explorer.exe"
            };
            Process.Start(startInfo);
        }

    }
}
