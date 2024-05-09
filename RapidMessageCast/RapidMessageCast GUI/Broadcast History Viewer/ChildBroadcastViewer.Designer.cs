namespace RapidMessageCast_Manager.Broadcast_History_Viewer
{
    partial class ChildBroadcastViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChildBroadcastViewer));
            toolStrip1 = new ToolStrip();
            LogList = new RichTextBox();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(902, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // LogList
            // 
            LogList.BackColor = Color.Silver;
            LogList.Dock = DockStyle.Fill;
            LogList.Location = new Point(0, 25);
            LogList.Name = "LogList";
            LogList.Size = new Size(902, 506);
            LogList.TabIndex = 1;
            LogList.Text = "";
            LogList.WordWrap = false;
            // 
            // ChildBroadcastViewer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(902, 531);
            Controls.Add(LogList);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ChildBroadcastViewer";
            Text = "ChildBroadcastViewer";
            FormClosing += ChildBroadcastViewer_FormClosing;
            Load += ChildBroadcastViewer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private RichTextBox LogList;
    }
}