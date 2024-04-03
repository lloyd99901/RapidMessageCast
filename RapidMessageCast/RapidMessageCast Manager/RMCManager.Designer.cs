namespace RapidMessageCast_Manager
{
    partial class RMCManager
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMCManager));
            StartBroadcastBtn = new Button();
            MessageLimitLbl = new Label();
            label2 = new Label();
            ActiveDirectorySelectBtn = new Button();
            label4 = new Label();
            expiryHourTime = new NumericUpDown();
            MessageTxt = new TextBox();
            label3 = new Label();
            expiryMinutesTime = new NumericUpDown();
            expirySecondsTime = new NumericUpDown();
            MainLoadStrip = new ToolStrip();
            OpenDropDown = new ToolStripDropDownButton();
            OpenRMSGFileBtn = new ToolStripMenuItem();
            openMessageTextToolStripMenuItem = new ToolStripMenuItem();
            openSendComputerListToolStripMenuItem = new ToolStripMenuItem();
            loadComputerListFromActiveDirectoryToolStripMenuItem = new ToolStripMenuItem();
            SaveDropDown = new ToolStripDropDownButton();
            SaveMSGBtn = new ToolStripMenuItem();
            SaveMessageBtn = new ToolStripMenuItem();
            SaveComputerListBtn = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            Quickloadbtn = new ToolStripButton();
            ExitToolButton = new ToolStripButton();
            AboutStripButton = new ToolStripButton();
            panel1 = new Panel();
            domainText = new Label();
            DomainImageBox = new PictureBox();
            panel2 = new Panel();
            RMCIconPictureBox = new PictureBox();
            RMCManagerLbl = new Label();
            BroadcastHistoryBtn = new Button();
            ScheduleBroadcastBtn = new Button();
            ComputerListLoadFromFileBtn = new Button();
            ComputerSelectList = new TextBox();
            label1 = new Label();
            logList = new ListBox();
            clearLogBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).BeginInit();
            MainLoadStrip.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DomainImageBox).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RMCIconPictureBox).BeginInit();
            SuspendLayout();
            // 
            // StartBroadcastBtn
            // 
            StartBroadcastBtn.BackColor = Color.FromArgb(53, 48, 70);
            StartBroadcastBtn.BackgroundImageLayout = ImageLayout.Center;
            StartBroadcastBtn.Dock = DockStyle.Bottom;
            StartBroadcastBtn.FlatStyle = FlatStyle.Flat;
            StartBroadcastBtn.Font = new Font("Arial", 9F, FontStyle.Bold);
            StartBroadcastBtn.ForeColor = Color.PaleGreen;
            StartBroadcastBtn.Image = Properties.Resources.Send4;
            StartBroadcastBtn.ImageAlign = ContentAlignment.TopCenter;
            StartBroadcastBtn.Location = new Point(0, 348);
            StartBroadcastBtn.Name = "StartBroadcastBtn";
            StartBroadcastBtn.Size = new Size(727, 68);
            StartBroadcastBtn.TabIndex = 9;
            StartBroadcastBtn.Text = "Send Message Broadcast";
            StartBroadcastBtn.TextAlign = ContentAlignment.BottomCenter;
            StartBroadcastBtn.UseVisualStyleBackColor = false;
            StartBroadcastBtn.Click += StartBroadcastBtn_Click;
            // 
            // MessageLimitLbl
            // 
            MessageLimitLbl.Font = new Font("Arial", 9F, FontStyle.Italic);
            MessageLimitLbl.ForeColor = Color.White;
            MessageLimitLbl.Location = new Point(129, 59);
            MessageLimitLbl.Name = "MessageLimitLbl";
            MessageLimitLbl.Size = new Size(158, 17);
            MessageLimitLbl.TabIndex = 8;
            MessageLimitLbl.Text = "Length Remaining: 255";
            MessageLimitLbl.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(79, 19);
            label2.TabIndex = 7;
            label2.Text = "Message:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // ActiveDirectorySelectBtn
            // 
            ActiveDirectorySelectBtn.BackColor = Color.FromArgb(63, 58, 70);
            ActiveDirectorySelectBtn.BackgroundImageLayout = ImageLayout.None;
            ActiveDirectorySelectBtn.FlatStyle = FlatStyle.Flat;
            ActiveDirectorySelectBtn.Font = new Font("Arial", 9F);
            ActiveDirectorySelectBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ActiveDirectorySelectBtn.Location = new Point(378, 268);
            ActiveDirectorySelectBtn.Name = "ActiveDirectorySelectBtn";
            ActiveDirectorySelectBtn.Size = new Size(122, 48);
            ActiveDirectorySelectBtn.TabIndex = 26;
            ActiveDirectorySelectBtn.Text = "Load from Active Directory";
            ActiveDirectorySelectBtn.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 10F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(293, 57);
            label4.Name = "label4";
            label4.Size = new Size(109, 19);
            label4.TabIndex = 7;
            label4.Text = "Send to PC's:";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // expiryHourTime
            // 
            expiryHourTime.BackColor = Color.FromArgb(73, 68, 80);
            expiryHourTime.BorderStyle = BorderStyle.FixedSingle;
            expiryHourTime.Font = new Font("Arial", 9F);
            expiryHourTime.ForeColor = Color.White;
            expiryHourTime.Location = new Point(12, 288);
            expiryHourTime.Name = "expiryHourTime";
            expiryHourTime.Size = new Size(47, 25);
            expiryHourTime.TabIndex = 40;
            expiryHourTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // MessageTxt
            // 
            MessageTxt.BackColor = Color.FromArgb(73, 68, 80);
            MessageTxt.BorderStyle = BorderStyle.FixedSingle;
            MessageTxt.ForeColor = Color.White;
            MessageTxt.Location = new Point(12, 79);
            MessageTxt.MaxLength = 255;
            MessageTxt.Multiline = true;
            MessageTxt.Name = "MessageTxt";
            MessageTxt.ScrollBars = ScrollBars.Vertical;
            MessageTxt.Size = new Size(275, 186);
            MessageTxt.TabIndex = 36;
            MessageTxt.TextChanged += messageTxt_TextChanged;
            MessageTxt.KeyPress += messageTxt_KeyPress;
            // 
            // label3
            // 
            label3.Font = new Font("Arial", 9F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 268);
            label3.Name = "label3";
            label3.Size = new Size(275, 17);
            label3.TabIndex = 37;
            label3.Text = "Message duration (HH:MM:SS)";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // expiryMinutesTime
            // 
            expiryMinutesTime.BackColor = Color.FromArgb(73, 68, 80);
            expiryMinutesTime.BorderStyle = BorderStyle.FixedSingle;
            expiryMinutesTime.Font = new Font("Arial", 9F);
            expiryMinutesTime.ForeColor = Color.White;
            expiryMinutesTime.Location = new Point(65, 288);
            expiryMinutesTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expiryMinutesTime.Name = "expiryMinutesTime";
            expiryMinutesTime.Size = new Size(47, 25);
            expiryMinutesTime.TabIndex = 39;
            // 
            // expirySecondsTime
            // 
            expirySecondsTime.BackColor = Color.FromArgb(73, 68, 80);
            expirySecondsTime.BorderStyle = BorderStyle.FixedSingle;
            expirySecondsTime.Font = new Font("Arial", 9F);
            expirySecondsTime.ForeColor = Color.White;
            expirySecondsTime.Location = new Point(118, 288);
            expirySecondsTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expirySecondsTime.Name = "expirySecondsTime";
            expirySecondsTime.Size = new Size(47, 25);
            expirySecondsTime.TabIndex = 38;
            // 
            // MainLoadStrip
            // 
            MainLoadStrip.BackColor = Color.FromArgb(68, 67, 80);
            MainLoadStrip.Dock = DockStyle.Bottom;
            MainLoadStrip.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MainLoadStrip.GripStyle = ToolStripGripStyle.Hidden;
            MainLoadStrip.ImageScalingSize = new Size(20, 20);
            MainLoadStrip.Items.AddRange(new ToolStripItem[] { OpenDropDown, SaveDropDown, toolStripSeparator1, Quickloadbtn, ExitToolButton, AboutStripButton });
            MainLoadStrip.Location = new Point(0, 321);
            MainLoadStrip.Name = "MainLoadStrip";
            MainLoadStrip.Size = new Size(727, 27);
            MainLoadStrip.TabIndex = 41;
            MainLoadStrip.Text = "MainLoadStrip";
            // 
            // OpenDropDown
            // 
            OpenDropDown.DropDownItems.AddRange(new ToolStripItem[] { OpenRMSGFileBtn, openMessageTextToolStripMenuItem, openSendComputerListToolStripMenuItem, loadComputerListFromActiveDirectoryToolStripMenuItem });
            OpenDropDown.ForeColor = Color.FromArgb(224, 224, 224);
            OpenDropDown.Image = Properties.Resources.OpenWith_100;
            OpenDropDown.ImageTransparentColor = Color.Magenta;
            OpenDropDown.Name = "OpenDropDown";
            OpenDropDown.Size = new Size(86, 24);
            OpenDropDown.Text = "Load...";
            // 
            // OpenRMSGFileBtn
            // 
            OpenRMSGFileBtn.Image = Properties.Resources.ntlanui2_115;
            OpenRMSGFileBtn.Name = "OpenRMSGFileBtn";
            OpenRMSGFileBtn.Size = new Size(349, 26);
            OpenRMSGFileBtn.Text = "Open .rmsg file";
            OpenRMSGFileBtn.Click += OpenRMSGFileBtn_Click;
            // 
            // openMessageTextToolStripMenuItem
            // 
            openMessageTextToolStripMenuItem.Image = Properties.Resources.OpenWith_100;
            openMessageTextToolStripMenuItem.Name = "openMessageTextToolStripMenuItem";
            openMessageTextToolStripMenuItem.Size = new Size(349, 26);
            openMessageTextToolStripMenuItem.Text = "Open message text";
            openMessageTextToolStripMenuItem.Click += openMessageTextToolStripMenuItem_Click;
            // 
            // openSendComputerListToolStripMenuItem
            // 
            openSendComputerListToolStripMenuItem.Image = Properties.Resources.remotepg_501;
            openSendComputerListToolStripMenuItem.Name = "openSendComputerListToolStripMenuItem";
            openSendComputerListToolStripMenuItem.Size = new Size(349, 26);
            openSendComputerListToolStripMenuItem.Text = "Load send computer list from file";
            openSendComputerListToolStripMenuItem.Click += openSendComputerListToolStripMenuItem_Click;
            // 
            // loadComputerListFromActiveDirectoryToolStripMenuItem
            // 
            loadComputerListFromActiveDirectoryToolStripMenuItem.Image = Properties.Resources.ieframe_31073;
            loadComputerListFromActiveDirectoryToolStripMenuItem.Name = "loadComputerListFromActiveDirectoryToolStripMenuItem";
            loadComputerListFromActiveDirectoryToolStripMenuItem.Size = new Size(349, 26);
            loadComputerListFromActiveDirectoryToolStripMenuItem.Text = "Load computer list from Active Directory";
            // 
            // SaveDropDown
            // 
            SaveDropDown.DropDownItems.AddRange(new ToolStripItem[] { SaveMSGBtn, SaveMessageBtn, SaveComputerListBtn });
            SaveDropDown.ForeColor = Color.FromArgb(224, 224, 224);
            SaveDropDown.Image = Properties.Resources.psr_210;
            SaveDropDown.ImageTransparentColor = Color.Magenta;
            SaveDropDown.Name = "SaveDropDown";
            SaveDropDown.Size = new Size(87, 24);
            SaveDropDown.Text = "Save...";
            // 
            // SaveMSGBtn
            // 
            SaveMSGBtn.Image = Properties.Resources.ntlanui2_115;
            SaveMSGBtn.Name = "SaveMSGBtn";
            SaveMSGBtn.Size = new Size(247, 26);
            SaveMSGBtn.Text = "Save .rmsg file";
            SaveMSGBtn.Click += SaveMSG;
            // 
            // SaveMessageBtn
            // 
            SaveMessageBtn.Image = Properties.Resources.OpenWith_100;
            SaveMessageBtn.Name = "SaveMessageBtn";
            SaveMessageBtn.Size = new Size(247, 26);
            SaveMessageBtn.Text = "Save message text";
            SaveMessageBtn.Click += SaveMessageBtn_Click;
            // 
            // SaveComputerListBtn
            // 
            SaveComputerListBtn.Image = Properties.Resources.remotepg_501;
            SaveComputerListBtn.Name = "SaveComputerListBtn";
            SaveComputerListBtn.Size = new Size(247, 26);
            SaveComputerListBtn.Text = "Save send computer list";
            SaveComputerListBtn.Click += SaveComputerListBtn_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 27);
            // 
            // Quickloadbtn
            // 
            Quickloadbtn.ForeColor = Color.FromArgb(224, 224, 224);
            Quickloadbtn.Image = Properties.Resources.OpenWith_100;
            Quickloadbtn.ImageTransparentColor = Color.Magenta;
            Quickloadbtn.Name = "Quickloadbtn";
            Quickloadbtn.Size = new Size(181, 24);
            Quickloadbtn.Text = "Quick load default.msg";
            // 
            // ExitToolButton
            // 
            ExitToolButton.Alignment = ToolStripItemAlignment.Right;
            ExitToolButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ExitToolButton.ForeColor = Color.FromArgb(224, 224, 224);
            ExitToolButton.Image = (Image)resources.GetObject("ExitToolButton.Image");
            ExitToolButton.ImageTransparentColor = Color.Magenta;
            ExitToolButton.Name = "ExitToolButton";
            ExitToolButton.Size = new Size(36, 24);
            ExitToolButton.Text = "Exit";
            ExitToolButton.Click += ExitToolButton_Click;
            // 
            // AboutStripButton
            // 
            AboutStripButton.Alignment = ToolStripItemAlignment.Right;
            AboutStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            AboutStripButton.ForeColor = Color.FromArgb(224, 224, 224);
            AboutStripButton.Image = (Image)resources.GetObject("AboutStripButton.Image");
            AboutStripButton.ImageTransparentColor = Color.Magenta;
            AboutStripButton.Name = "AboutStripButton";
            AboutStripButton.RightToLeft = RightToLeft.No;
            AboutStripButton.Size = new Size(86, 24);
            AboutStripButton.Text = "About RMC";
            AboutStripButton.Click += AboutStripButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(48, 37, 50);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(domainText);
            panel1.Controls.Add(DomainImageBox);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(727, 54);
            panel1.TabIndex = 42;
            // 
            // domainText
            // 
            domainText.Dock = DockStyle.Right;
            domainText.Font = new Font("Arial", 8F);
            domainText.ForeColor = Color.FromArgb(200, 200, 240);
            domainText.Location = new Point(418, 0);
            domainText.Name = "domainText";
            domainText.Size = new Size(256, 52);
            domainText.TabIndex = 1;
            domainText.Text = "Domain Status:";
            domainText.TextAlign = ContentAlignment.MiddleRight;
            // 
            // DomainImageBox
            // 
            DomainImageBox.Dock = DockStyle.Right;
            DomainImageBox.Image = Properties.Resources.ieframe_31073;
            DomainImageBox.Location = new Point(674, 0);
            DomainImageBox.Name = "DomainImageBox";
            DomainImageBox.Size = new Size(51, 52);
            DomainImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            DomainImageBox.TabIndex = 2;
            DomainImageBox.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(68, 58, 71);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(RMCIconPictureBox);
            panel2.Controls.Add(RMCManagerLbl);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(217, 52);
            panel2.TabIndex = 0;
            // 
            // RMCIconPictureBox
            // 
            RMCIconPictureBox.Dock = DockStyle.Left;
            RMCIconPictureBox.Image = Properties.Resources.dsuiext_4118;
            RMCIconPictureBox.Location = new Point(0, 0);
            RMCIconPictureBox.Name = "RMCIconPictureBox";
            RMCIconPictureBox.Size = new Size(57, 50);
            RMCIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            RMCIconPictureBox.TabIndex = 1;
            RMCIconPictureBox.TabStop = false;
            // 
            // RMCManagerLbl
            // 
            RMCManagerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            RMCManagerLbl.AutoSize = true;
            RMCManagerLbl.Font = new Font("Arial", 10F);
            RMCManagerLbl.ForeColor = Color.FromArgb(224, 224, 224);
            RMCManagerLbl.Location = new Point(63, 7);
            RMCManagerLbl.Name = "RMCManagerLbl";
            RMCManagerLbl.Size = new Size(149, 38);
            RMCManagerLbl.TabIndex = 1;
            RMCManagerLbl.Text = "RapidMessageCast\r\nManager";
            RMCManagerLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // BroadcastHistoryBtn
            // 
            BroadcastHistoryBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BroadcastHistoryBtn.BackColor = Color.FromArgb(63, 58, 70);
            BroadcastHistoryBtn.FlatStyle = FlatStyle.Flat;
            BroadcastHistoryBtn.Font = new Font("Arial", 9F);
            BroadcastHistoryBtn.ForeColor = Color.FromArgb(224, 224, 224);
            BroadcastHistoryBtn.Location = new Point(0, 348);
            BroadcastHistoryBtn.Name = "BroadcastHistoryBtn";
            BroadcastHistoryBtn.Size = new Size(105, 68);
            BroadcastHistoryBtn.TabIndex = 26;
            BroadcastHistoryBtn.Text = "Broadcast History";
            BroadcastHistoryBtn.UseVisualStyleBackColor = false;
            // 
            // ScheduleBroadcastBtn
            // 
            ScheduleBroadcastBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ScheduleBroadcastBtn.BackColor = Color.FromArgb(63, 58, 70);
            ScheduleBroadcastBtn.FlatStyle = FlatStyle.Flat;
            ScheduleBroadcastBtn.Font = new Font("Arial", 9F);
            ScheduleBroadcastBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ScheduleBroadcastBtn.Location = new Point(623, 348);
            ScheduleBroadcastBtn.Name = "ScheduleBroadcastBtn";
            ScheduleBroadcastBtn.Size = new Size(104, 68);
            ScheduleBroadcastBtn.TabIndex = 26;
            ScheduleBroadcastBtn.Text = "Schedule Message";
            ScheduleBroadcastBtn.UseVisualStyleBackColor = false;
            // 
            // ComputerListLoadFromFileBtn
            // 
            ComputerListLoadFromFileBtn.BackColor = Color.FromArgb(63, 58, 70);
            ComputerListLoadFromFileBtn.FlatStyle = FlatStyle.Flat;
            ComputerListLoadFromFileBtn.Font = new Font("Arial", 9F);
            ComputerListLoadFromFileBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ComputerListLoadFromFileBtn.Location = new Point(293, 268);
            ComputerListLoadFromFileBtn.Name = "ComputerListLoadFromFileBtn";
            ComputerListLoadFromFileBtn.Size = new Size(79, 48);
            ComputerListLoadFromFileBtn.TabIndex = 22;
            ComputerListLoadFromFileBtn.Text = "Load from file";
            ComputerListLoadFromFileBtn.UseVisualStyleBackColor = false;
            ComputerListLoadFromFileBtn.Click += openSendComputerListToolStripMenuItem_Click;
            // 
            // ComputerSelectList
            // 
            ComputerSelectList.BackColor = Color.FromArgb(73, 68, 80);
            ComputerSelectList.BorderStyle = BorderStyle.FixedSingle;
            ComputerSelectList.Font = new Font("Segoe UI", 8F);
            ComputerSelectList.ForeColor = Color.White;
            ComputerSelectList.Location = new Point(293, 79);
            ComputerSelectList.Multiline = true;
            ComputerSelectList.Name = "ComputerSelectList";
            ComputerSelectList.ScrollBars = ScrollBars.Vertical;
            ComputerSelectList.Size = new Size(207, 186);
            ComputerSelectList.TabIndex = 36;
            ComputerSelectList.TextChanged += messageTxt_TextChanged;
            ComputerSelectList.KeyPress += ComputerSelectList_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(506, 57);
            label1.Name = "label1";
            label1.Size = new Size(41, 19);
            label1.TabIndex = 7;
            label1.Text = "Log:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // logList
            // 
            logList.BackColor = Color.FromArgb(73, 68, 80);
            logList.BorderStyle = BorderStyle.FixedSingle;
            logList.Font = new Font("Segoe UI", 6F);
            logList.ForeColor = Color.White;
            logList.FormattingEnabled = true;
            logList.HorizontalScrollbar = true;
            logList.IntegralHeight = false;
            logList.ItemHeight = 12;
            logList.Location = new Point(506, 79);
            logList.Name = "logList";
            logList.Size = new Size(214, 237);
            logList.TabIndex = 43;
            // 
            // clearLogBtn
            // 
            clearLogBtn.BackColor = Color.FromArgb(63, 58, 70);
            clearLogBtn.FlatStyle = FlatStyle.Flat;
            clearLogBtn.Font = new Font("Arial", 7F);
            clearLogBtn.ForeColor = Color.FromArgb(224, 224, 224);
            clearLogBtn.Location = new Point(623, 54);
            clearLogBtn.Name = "clearLogBtn";
            clearLogBtn.Size = new Size(97, 25);
            clearLogBtn.TabIndex = 22;
            clearLogBtn.Text = "Clear";
            clearLogBtn.UseVisualStyleBackColor = false;
            clearLogBtn.Click += clearLogBtn_Click;
            // 
            // RMCManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 31, 36);
            ClientSize = new Size(727, 416);
            Controls.Add(logList);
            Controls.Add(panel1);
            Controls.Add(MainLoadStrip);
            Controls.Add(expirySecondsTime);
            Controls.Add(expiryMinutesTime);
            Controls.Add(expiryHourTime);
            Controls.Add(label3);
            Controls.Add(ComputerSelectList);
            Controls.Add(MessageTxt);
            Controls.Add(ScheduleBroadcastBtn);
            Controls.Add(BroadcastHistoryBtn);
            Controls.Add(ActiveDirectorySelectBtn);
            Controls.Add(clearLogBtn);
            Controls.Add(ComputerListLoadFromFileBtn);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(MessageLimitLbl);
            Controls.Add(StartBroadcastBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "RMCManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RapidMessageCast Manager -";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).EndInit();
            MainLoadStrip.ResumeLayout(false);
            MainLoadStrip.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DomainImageBox).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)RMCIconPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button StartBroadcastBtn;
        private Label MessageLimitLbl;
        private Label label2;
        private Button ActiveDirectorySelectBtn;
        private Label label4;
        private NumericUpDown expiryHourTime;
        private TextBox MessageTxt;
        private Label label3;
        private NumericUpDown expiryMinutesTime;
        private NumericUpDown expirySecondsTime;
        private ToolStrip MainLoadStrip;
        private Panel panel1;
        private Panel panel2;
        private PictureBox RMCIconPictureBox;
        private Label RMCManagerLbl;
        private ToolStripDropDownButton SaveDropDown;
        private ToolStripDropDownButton OpenDropDown;
        private ToolStripMenuItem OpenRMSGFileBtn;
        private ToolStripMenuItem openMessageTextToolStripMenuItem;
        private ToolStripMenuItem openSendComputerListToolStripMenuItem;
        private ToolStripMenuItem loadComputerListFromActiveDirectoryToolStripMenuItem;
        private Button BroadcastHistoryBtn;
        private Button ScheduleBroadcastBtn;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton AboutStripButton;
        private ToolStripMenuItem SaveMSGBtn;
        private ToolStripMenuItem SaveMessageBtn;
        private ToolStripMenuItem SaveComputerListBtn;
        private ToolStripButton ExitToolButton;
        private ToolStripButton Quickloadbtn;
        private Button ComputerListLoadFromFileBtn;
        private Label domainText;
        private PictureBox DomainImageBox;
        private TextBox ComputerSelectList;
        private Label label1;
        public ListBox logList;
        private Button clearLogBtn;
    }
}
