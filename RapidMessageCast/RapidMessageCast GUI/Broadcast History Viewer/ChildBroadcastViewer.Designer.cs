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
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            MessageInsightsLabel = new Label();
            PCList = new ListBox();
            LogList = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = SystemColors.ControlDark;
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(LogList);
            splitContainer1.Size = new Size(798, 431);
            splitContainer1.SplitterDistance = 195;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(MessageInsightsLabel);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(PCList);
            splitContainer2.Size = new Size(195, 431);
            splitContainer2.SplitterDistance = 146;
            splitContainer2.TabIndex = 2;
            // 
            // MessageInsightsLabel
            // 
            MessageInsightsLabel.Dock = DockStyle.Top;
            MessageInsightsLabel.Location = new Point(0, 0);
            MessageInsightsLabel.Name = "MessageInsightsLabel";
            MessageInsightsLabel.Size = new Size(195, 127);
            MessageInsightsLabel.TabIndex = 7;
            MessageInsightsLabel.Text = "Log Type Numbers Here";
            MessageInsightsLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // PCList
            // 
            PCList.Dock = DockStyle.Fill;
            PCList.FormattingEnabled = true;
            PCList.IntegralHeight = false;
            PCList.Location = new Point(0, 0);
            PCList.Name = "PCList";
            PCList.Size = new Size(195, 281);
            PCList.TabIndex = 3;
            // 
            // LogList
            // 
            LogList.BackColor = Color.Silver;
            LogList.Dock = DockStyle.Fill;
            LogList.Location = new Point(0, 0);
            LogList.Name = "LogList";
            LogList.Size = new Size(599, 431);
            LogList.TabIndex = 2;
            LogList.Text = "";
            LogList.WordWrap = false;
            // 
            // ChildBroadcastViewer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 431);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ChildBroadcastViewer";
            Text = "ChildBroadcastViewer";
            FormClosing += ChildBroadcastViewer_FormClosing;
            Load += ChildBroadcastViewer_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private RichTextBox LogList;
        private SplitContainer splitContainer2;
        private ListBox PCList;
        private Label MessageInsightsLabel;
    }
}