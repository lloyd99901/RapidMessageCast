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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RMCManager));
            StartBroadcastBtn = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            RMCManagerLbl = new Label();
            versionLbl = new Label();
            OpenRMCFileBtn = new Button();
            SaveRMCFileBTN = new Button();
            QuickSaveRMSGBtn = new Button();
            BroadcastHistoryBtn = new Button();
            ScheduleBroadcastBtn = new Button();
            Rmsgcontextstrip = new ContextMenuStrip(components);
            renameSelectRMSGFileToolStripMenuItem = new ToolStripMenuItem();
            deleteSelectedItemToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            pictureBox3 = new PictureBox();
            EmergencyModeCheckbox = new CheckBox();
            EmergencyHelpLink = new LinkLabel();
            MessageDurationHelpLink = new LinkLabel();
            SaveMessageTxtBtn = new Button();
            MessageOpenTxtBtn = new Button();
            expirySecondsTime = new NumericUpDown();
            expiryMinutesTime = new NumericUpDown();
            expiryHourTime = new NumericUpDown();
            label3 = new Label();
            MessageTxt = new TextBox();
            label2 = new Label();
            MessageLimitLbl = new Label();
            tabPage2 = new TabPage();
            pictureBox4 = new PictureBox();
            PCCountLbl = new Label();
            SavePCListTxtBtn = new Button();
            ComputerListLoadFromFileBtn = new Button();
            ComputerSelectList = new TextBox();
            ActiveDirectorySelectBtn = new Button();
            label4 = new Label();
            tabPage3 = new TabPage();
            clearLogBtn = new Button();
            label1 = new Label();
            logList = new ListBox();
            tabPage4 = new TabPage();
            AboutText = new TextBox();
            IconsLinkLabel = new LinkLabel();
            pictureBox2 = new PictureBox();
            label6 = new Label();
            label7 = new Label();
            RenameSelectedRMSGBtn = new Button();
            RMSGHelpLink = new LinkLabel();
            LoadSelectedRMSGBtn = new Button();
            RMSGFileListBox = new ListBox();
            DeleteSelectedRMSGFileBtn = new Button();
            label5 = new Label();
            RefreshRMSGListBtn = new Button();
            GreenButtonTimer = new System.Windows.Forms.Timer(components);
            pictureBox5 = new PictureBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            Rmsgcontextstrip.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
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
            StartBroadcastBtn.Location = new Point(0, 361);
            StartBroadcastBtn.Margin = new Padding(3, 2, 3, 2);
            StartBroadcastBtn.Name = "StartBroadcastBtn";
            StartBroadcastBtn.Size = new Size(681, 68);
            StartBroadcastBtn.TabIndex = 9;
            StartBroadcastBtn.Text = "Start Message Broadcast";
            StartBroadcastBtn.TextAlign = ContentAlignment.BottomCenter;
            StartBroadcastBtn.UseVisualStyleBackColor = false;
            StartBroadcastBtn.Click += StartBroadcastBtn_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 32, 32);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(OpenRMCFileBtn);
            panel1.Controls.Add(SaveRMCFileBTN);
            panel1.Controls.Add(QuickSaveRMSGBtn);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(681, 48);
            panel1.TabIndex = 42;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(48, 48, 48);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(RMCManagerLbl);
            panel2.Controls.Add(versionLbl);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(206, 46);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resources.RMC_GUI_Icon;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 44);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // RMCManagerLbl
            // 
            RMCManagerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            RMCManagerLbl.AutoSize = true;
            RMCManagerLbl.Font = new Font("Arial", 9F);
            RMCManagerLbl.ForeColor = Color.FromArgb(224, 224, 224);
            RMCManagerLbl.Location = new Point(66, 4);
            RMCManagerLbl.Name = "RMCManagerLbl";
            RMCManagerLbl.Size = new Size(136, 17);
            RMCManagerLbl.TabIndex = 1;
            RMCManagerLbl.Text = "RapidMessageCast\r\n";
            RMCManagerLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // versionLbl
            // 
            versionLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            versionLbl.Font = new Font("Arial", 10F);
            versionLbl.ForeColor = Color.Silver;
            versionLbl.Location = new Point(69, 21);
            versionLbl.Name = "versionLbl";
            versionLbl.Size = new Size(130, 23);
            versionLbl.TabIndex = 1;
            versionLbl.Text = "verNumb";
            versionLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // OpenRMCFileBtn
            // 
            OpenRMCFileBtn.BackColor = Color.FromArgb(48, 48, 48);
            OpenRMCFileBtn.BackgroundImageLayout = ImageLayout.Zoom;
            OpenRMCFileBtn.Dock = DockStyle.Right;
            OpenRMCFileBtn.FlatStyle = FlatStyle.Flat;
            OpenRMCFileBtn.Font = new Font("Arial", 9F);
            OpenRMCFileBtn.ForeColor = Color.White;
            OpenRMCFileBtn.Image = Properties.Resources.icons8_external_link_24;
            OpenRMCFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            OpenRMCFileBtn.Location = new Point(268, 0);
            OpenRMCFileBtn.Margin = new Padding(3, 2, 3, 2);
            OpenRMCFileBtn.Name = "OpenRMCFileBtn";
            OpenRMCFileBtn.Size = new Size(110, 46);
            OpenRMCFileBtn.TabIndex = 27;
            OpenRMCFileBtn.Text = "Open";
            OpenRMCFileBtn.UseVisualStyleBackColor = false;
            OpenRMCFileBtn.Click += OpenRMSGFileBtn_Click;
            // 
            // SaveRMCFileBTN
            // 
            SaveRMCFileBTN.BackColor = Color.FromArgb(48, 48, 48);
            SaveRMCFileBTN.BackgroundImageLayout = ImageLayout.Zoom;
            SaveRMCFileBTN.Dock = DockStyle.Right;
            SaveRMCFileBTN.FlatStyle = FlatStyle.Flat;
            SaveRMCFileBTN.Font = new Font("Arial", 9F);
            SaveRMCFileBTN.ForeColor = Color.White;
            SaveRMCFileBTN.Image = Properties.Resources.icons8_save_24;
            SaveRMCFileBTN.ImageAlign = ContentAlignment.MiddleLeft;
            SaveRMCFileBTN.Location = new Point(378, 0);
            SaveRMCFileBTN.Margin = new Padding(3, 2, 3, 2);
            SaveRMCFileBTN.Name = "SaveRMCFileBTN";
            SaveRMCFileBTN.Size = new Size(132, 46);
            SaveRMCFileBTN.TabIndex = 27;
            SaveRMCFileBTN.Text = "Save as...";
            SaveRMCFileBTN.UseVisualStyleBackColor = false;
            SaveRMCFileBTN.Click += SaveRMSGBttn;
            // 
            // QuickSaveRMSGBtn
            // 
            QuickSaveRMSGBtn.BackColor = Color.FromArgb(48, 48, 48);
            QuickSaveRMSGBtn.BackgroundImageLayout = ImageLayout.Zoom;
            QuickSaveRMSGBtn.Dock = DockStyle.Right;
            QuickSaveRMSGBtn.FlatStyle = FlatStyle.Flat;
            QuickSaveRMSGBtn.Font = new Font("Arial", 9F);
            QuickSaveRMSGBtn.ForeColor = Color.White;
            QuickSaveRMSGBtn.Image = Properties.Resources.icons8_save_24;
            QuickSaveRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            QuickSaveRMSGBtn.Location = new Point(510, 0);
            QuickSaveRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            QuickSaveRMSGBtn.Name = "QuickSaveRMSGBtn";
            QuickSaveRMSGBtn.Size = new Size(169, 46);
            QuickSaveRMSGBtn.TabIndex = 27;
            QuickSaveRMSGBtn.Text = "Quick Save .RMSG";
            QuickSaveRMSGBtn.TextAlign = ContentAlignment.MiddleRight;
            QuickSaveRMSGBtn.UseVisualStyleBackColor = false;
            QuickSaveRMSGBtn.Click += QuickSaveRMSGBtn_Clicked;
            // 
            // BroadcastHistoryBtn
            // 
            BroadcastHistoryBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BroadcastHistoryBtn.BackColor = Color.FromArgb(48, 48, 48);
            BroadcastHistoryBtn.FlatStyle = FlatStyle.Flat;
            BroadcastHistoryBtn.Font = new Font("Arial", 9F);
            BroadcastHistoryBtn.ForeColor = Color.FromArgb(224, 224, 224);
            BroadcastHistoryBtn.Image = Properties.Resources.icons8_broadcast_24;
            BroadcastHistoryBtn.ImageAlign = ContentAlignment.TopCenter;
            BroadcastHistoryBtn.Location = new Point(0, 362);
            BroadcastHistoryBtn.Margin = new Padding(3, 2, 3, 2);
            BroadcastHistoryBtn.Name = "BroadcastHistoryBtn";
            BroadcastHistoryBtn.Size = new Size(105, 66);
            BroadcastHistoryBtn.TabIndex = 26;
            BroadcastHistoryBtn.Text = "Broadcast History";
            BroadcastHistoryBtn.TextAlign = ContentAlignment.BottomCenter;
            BroadcastHistoryBtn.UseVisualStyleBackColor = false;
            // 
            // ScheduleBroadcastBtn
            // 
            ScheduleBroadcastBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ScheduleBroadcastBtn.BackColor = Color.FromArgb(48, 48, 48);
            ScheduleBroadcastBtn.FlatStyle = FlatStyle.Flat;
            ScheduleBroadcastBtn.Font = new Font("Arial", 9F);
            ScheduleBroadcastBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ScheduleBroadcastBtn.Image = Properties.Resources.icons8_schedule_24;
            ScheduleBroadcastBtn.ImageAlign = ContentAlignment.TopCenter;
            ScheduleBroadcastBtn.Location = new Point(577, 362);
            ScheduleBroadcastBtn.Margin = new Padding(3, 2, 3, 2);
            ScheduleBroadcastBtn.Name = "ScheduleBroadcastBtn";
            ScheduleBroadcastBtn.Size = new Size(104, 66);
            ScheduleBroadcastBtn.TabIndex = 26;
            ScheduleBroadcastBtn.Text = "Schedule Message";
            ScheduleBroadcastBtn.TextAlign = ContentAlignment.BottomCenter;
            ScheduleBroadcastBtn.UseVisualStyleBackColor = false;
            // 
            // Rmsgcontextstrip
            // 
            Rmsgcontextstrip.ImageScalingSize = new Size(20, 20);
            Rmsgcontextstrip.Items.AddRange(new ToolStripItem[] { renameSelectRMSGFileToolStripMenuItem, deleteSelectedItemToolStripMenuItem, refreshToolStripMenuItem });
            Rmsgcontextstrip.Name = "Rmsgcontextstrip";
            Rmsgcontextstrip.Size = new Size(228, 76);
            // 
            // renameSelectRMSGFileToolStripMenuItem
            // 
            renameSelectRMSGFileToolStripMenuItem.Name = "renameSelectRMSGFileToolStripMenuItem";
            renameSelectRMSGFileToolStripMenuItem.Size = new Size(227, 24);
            renameSelectRMSGFileToolStripMenuItem.Text = "Rename Selected Item";
            renameSelectRMSGFileToolStripMenuItem.Click += RenameSelectedRMSGBtn_Click;
            // 
            // deleteSelectedItemToolStripMenuItem
            // 
            deleteSelectedItemToolStripMenuItem.Name = "deleteSelectedItemToolStripMenuItem";
            deleteSelectedItemToolStripMenuItem.Size = new Size(227, 24);
            deleteSelectedItemToolStripMenuItem.Text = "Delete Selected Item";
            deleteSelectedItemToolStripMenuItem.Click += DeleteSelectedRMSGFileBtn_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(227, 24);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.Click += RefreshRMSGListBtn_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new Point(304, 50);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(374, 306);
            tabControl1.TabIndex = 73;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(30, 30, 30);
            tabPage1.Controls.Add(pictureBox3);
            tabPage1.Controls.Add(EmergencyModeCheckbox);
            tabPage1.Controls.Add(EmergencyHelpLink);
            tabPage1.Controls.Add(MessageDurationHelpLink);
            tabPage1.Controls.Add(SaveMessageTxtBtn);
            tabPage1.Controls.Add(MessageOpenTxtBtn);
            tabPage1.Controls.Add(expirySecondsTime);
            tabPage1.Controls.Add(expiryMinutesTime);
            tabPage1.Controls.Add(expiryHourTime);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(MessageTxt);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(MessageLimitLbl);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(366, 273);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Message";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.icons8_message_24;
            pictureBox3.Location = new Point(6, 0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(24, 24);
            pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox3.TabIndex = 87;
            pictureBox3.TabStop = false;
            // 
            // EmergencyModeCheckbox
            // 
            EmergencyModeCheckbox.AutoSize = true;
            EmergencyModeCheckbox.ForeColor = Color.White;
            EmergencyModeCheckbox.Location = new Point(180, 178);
            EmergencyModeCheckbox.Name = "EmergencyModeCheckbox";
            EmergencyModeCheckbox.Size = new Size(147, 24);
            EmergencyModeCheckbox.TabIndex = 85;
            EmergencyModeCheckbox.Text = "Emergency Mode";
            EmergencyModeCheckbox.UseVisualStyleBackColor = true;
            EmergencyModeCheckbox.CheckedChanged += EmergencyModeCheckbox_CheckedChanged;
            // 
            // EmergencyHelpLink
            // 
            EmergencyHelpLink.AutoSize = true;
            EmergencyHelpLink.LinkColor = Color.FromArgb(0, 192, 0);
            EmergencyHelpLink.Location = new Point(326, 179);
            EmergencyHelpLink.Name = "EmergencyHelpLink";
            EmergencyHelpLink.Size = new Size(16, 20);
            EmergencyHelpLink.TabIndex = 86;
            EmergencyHelpLink.TabStop = true;
            EmergencyHelpLink.Text = "?";
            EmergencyHelpLink.LinkClicked += EmergencyHelpLink_LinkClicked;
            // 
            // MessageDurationHelpLink
            // 
            MessageDurationHelpLink.AutoSize = true;
            MessageDurationHelpLink.LinkColor = Color.FromArgb(0, 192, 0);
            MessageDurationHelpLink.Location = new Point(143, 218);
            MessageDurationHelpLink.Name = "MessageDurationHelpLink";
            MessageDurationHelpLink.Size = new Size(16, 20);
            MessageDurationHelpLink.TabIndex = 83;
            MessageDurationHelpLink.TabStop = true;
            MessageDurationHelpLink.Text = "?";
            MessageDurationHelpLink.LinkClicked += MessageDurationHelpLink_LinkClicked;
            // 
            // SaveMessageTxtBtn
            // 
            SaveMessageTxtBtn.BackColor = Color.FromArgb(48, 48, 48);
            SaveMessageTxtBtn.FlatStyle = FlatStyle.Flat;
            SaveMessageTxtBtn.Font = new Font("Arial", 9F);
            SaveMessageTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            SaveMessageTxtBtn.Image = Properties.Resources.icons8_save_24;
            SaveMessageTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            SaveMessageTxtBtn.Location = new Point(180, 239);
            SaveMessageTxtBtn.Margin = new Padding(3, 2, 3, 2);
            SaveMessageTxtBtn.Name = "SaveMessageTxtBtn";
            SaveMessageTxtBtn.Size = new Size(181, 32);
            SaveMessageTxtBtn.TabIndex = 75;
            SaveMessageTxtBtn.Text = "Save as .txt";
            SaveMessageTxtBtn.UseVisualStyleBackColor = false;
            SaveMessageTxtBtn.Click += SaveMessageBtn_Click;
            // 
            // MessageOpenTxtBtn
            // 
            MessageOpenTxtBtn.BackColor = Color.FromArgb(48, 48, 48);
            MessageOpenTxtBtn.FlatStyle = FlatStyle.Flat;
            MessageOpenTxtBtn.Font = new Font("Arial", 9F);
            MessageOpenTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            MessageOpenTxtBtn.Image = Properties.Resources.icons8_external_link_24;
            MessageOpenTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            MessageOpenTxtBtn.Location = new Point(180, 204);
            MessageOpenTxtBtn.Margin = new Padding(3, 2, 3, 2);
            MessageOpenTxtBtn.Name = "MessageOpenTxtBtn";
            MessageOpenTxtBtn.Size = new Size(181, 32);
            MessageOpenTxtBtn.TabIndex = 76;
            MessageOpenTxtBtn.Text = "Open from .txt";
            MessageOpenTxtBtn.UseVisualStyleBackColor = false;
            MessageOpenTxtBtn.Click += openMessageTextToolStripMenuItem_Click;
            // 
            // expirySecondsTime
            // 
            expirySecondsTime.BackColor = Color.FromArgb(48, 48, 48);
            expirySecondsTime.BorderStyle = BorderStyle.FixedSingle;
            expirySecondsTime.Font = new Font("Arial", 9F);
            expirySecondsTime.ForeColor = Color.White;
            expirySecondsTime.Location = new Point(112, 242);
            expirySecondsTime.Margin = new Padding(3, 2, 3, 2);
            expirySecondsTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expirySecondsTime.Name = "expirySecondsTime";
            expirySecondsTime.Size = new Size(47, 25);
            expirySecondsTime.TabIndex = 79;
            // 
            // expiryMinutesTime
            // 
            expiryMinutesTime.BackColor = Color.FromArgb(48, 48, 48);
            expiryMinutesTime.BorderStyle = BorderStyle.FixedSingle;
            expiryMinutesTime.Font = new Font("Arial", 9F);
            expiryMinutesTime.ForeColor = Color.White;
            expiryMinutesTime.Location = new Point(59, 242);
            expiryMinutesTime.Margin = new Padding(3, 2, 3, 2);
            expiryMinutesTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expiryMinutesTime.Name = "expiryMinutesTime";
            expiryMinutesTime.Size = new Size(47, 25);
            expiryMinutesTime.TabIndex = 80;
            // 
            // expiryHourTime
            // 
            expiryHourTime.BackColor = Color.FromArgb(48, 48, 48);
            expiryHourTime.BorderStyle = BorderStyle.FixedSingle;
            expiryHourTime.Font = new Font("Arial", 9F);
            expiryHourTime.ForeColor = Color.White;
            expiryHourTime.Location = new Point(6, 242);
            expiryHourTime.Margin = new Padding(3, 2, 3, 2);
            expiryHourTime.Name = "expiryHourTime";
            expiryHourTime.Size = new Size(47, 25);
            expiryHourTime.TabIndex = 81;
            expiryHourTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 9F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(22, 196);
            label3.Name = "label3";
            label3.Size = new Size(123, 34);
            label3.TabIndex = 78;
            label3.Text = "Message duration\r\n(HH:MM:SS)";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MessageTxt
            // 
            MessageTxt.BackColor = Color.FromArgb(45, 45, 45);
            MessageTxt.BorderStyle = BorderStyle.FixedSingle;
            MessageTxt.ForeColor = Color.White;
            MessageTxt.Location = new Point(5, 26);
            MessageTxt.Margin = new Padding(3, 2, 3, 2);
            MessageTxt.MaxLength = 255;
            MessageTxt.Multiline = true;
            MessageTxt.Name = "MessageTxt";
            MessageTxt.ScrollBars = ScrollBars.Vertical;
            MessageTxt.Size = new Size(355, 151);
            MessageTxt.TabIndex = 77;
            MessageTxt.TextChanged += messageTxt_TextChanged;
            MessageTxt.KeyPress += messageTxt_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(36, 4);
            label2.Name = "label2";
            label2.Size = new Size(79, 19);
            label2.TabIndex = 73;
            label2.Text = "Message:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // MessageLimitLbl
            // 
            MessageLimitLbl.Font = new Font("Arial", 9F, FontStyle.Italic);
            MessageLimitLbl.ForeColor = Color.White;
            MessageLimitLbl.Location = new Point(203, 5);
            MessageLimitLbl.Name = "MessageLimitLbl";
            MessageLimitLbl.Size = new Size(158, 18);
            MessageLimitLbl.TabIndex = 74;
            MessageLimitLbl.Text = "Length Remaining: 255";
            MessageLimitLbl.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(30, 30, 30);
            tabPage2.Controls.Add(pictureBox4);
            tabPage2.Controls.Add(PCCountLbl);
            tabPage2.Controls.Add(SavePCListTxtBtn);
            tabPage2.Controls.Add(ComputerListLoadFromFileBtn);
            tabPage2.Controls.Add(ComputerSelectList);
            tabPage2.Controls.Add(ActiveDirectorySelectBtn);
            tabPage2.Controls.Add(label4);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(366, 273);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Send to PC's";
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.icons8_pc_24;
            pictureBox4.Location = new Point(6, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(24, 24);
            pictureBox4.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox4.TabIndex = 78;
            pictureBox4.TabStop = false;
            // 
            // PCCountLbl
            // 
            PCCountLbl.Font = new Font("Arial", 9F, FontStyle.Italic);
            PCCountLbl.ForeColor = Color.White;
            PCCountLbl.Location = new Point(202, 6);
            PCCountLbl.Name = "PCCountLbl";
            PCCountLbl.Size = new Size(158, 18);
            PCCountLbl.TabIndex = 77;
            PCCountLbl.Text = "PC Count: 0";
            PCCountLbl.TextAlign = ContentAlignment.MiddleRight;
            // 
            // SavePCListTxtBtn
            // 
            SavePCListTxtBtn.BackColor = Color.FromArgb(48, 48, 48);
            SavePCListTxtBtn.FlatStyle = FlatStyle.Flat;
            SavePCListTxtBtn.Font = new Font("Arial", 9F);
            SavePCListTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            SavePCListTxtBtn.Image = Properties.Resources.icons8_save_24;
            SavePCListTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            SavePCListTxtBtn.Location = new Point(180, 204);
            SavePCListTxtBtn.Margin = new Padding(3, 2, 3, 2);
            SavePCListTxtBtn.Name = "SavePCListTxtBtn";
            SavePCListTxtBtn.Size = new Size(181, 32);
            SavePCListTxtBtn.TabIndex = 75;
            SavePCListTxtBtn.Text = "Save list as txt";
            SavePCListTxtBtn.UseVisualStyleBackColor = false;
            SavePCListTxtBtn.Click += SaveComputerListBtn_Click;
            // 
            // ComputerListLoadFromFileBtn
            // 
            ComputerListLoadFromFileBtn.BackColor = Color.FromArgb(48, 48, 48);
            ComputerListLoadFromFileBtn.FlatStyle = FlatStyle.Flat;
            ComputerListLoadFromFileBtn.Font = new Font("Arial", 9F);
            ComputerListLoadFromFileBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ComputerListLoadFromFileBtn.Image = Properties.Resources.icons8_external_link_24;
            ComputerListLoadFromFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            ComputerListLoadFromFileBtn.Location = new Point(5, 204);
            ComputerListLoadFromFileBtn.Margin = new Padding(3, 2, 3, 2);
            ComputerListLoadFromFileBtn.Name = "ComputerListLoadFromFileBtn";
            ComputerListLoadFromFileBtn.Size = new Size(169, 32);
            ComputerListLoadFromFileBtn.TabIndex = 73;
            ComputerListLoadFromFileBtn.Text = "Open from .txt";
            ComputerListLoadFromFileBtn.UseVisualStyleBackColor = false;
            ComputerListLoadFromFileBtn.Click += openSendComputerListToolStripMenuItem_Click;
            // 
            // ComputerSelectList
            // 
            ComputerSelectList.BackColor = Color.FromArgb(45, 45, 45);
            ComputerSelectList.BorderStyle = BorderStyle.FixedSingle;
            ComputerSelectList.Font = new Font("Segoe UI", 8F);
            ComputerSelectList.ForeColor = Color.White;
            ComputerSelectList.Location = new Point(5, 26);
            ComputerSelectList.Margin = new Padding(3, 2, 3, 2);
            ComputerSelectList.Multiline = true;
            ComputerSelectList.Name = "ComputerSelectList";
            ComputerSelectList.ScrollBars = ScrollBars.Vertical;
            ComputerSelectList.Size = new Size(355, 174);
            ComputerSelectList.TabIndex = 76;
            ComputerSelectList.TextChanged += ComputerSelectList_TextChanged;
            ComputerSelectList.KeyPress += ComputerSelectList_KeyPress;
            // 
            // ActiveDirectorySelectBtn
            // 
            ActiveDirectorySelectBtn.BackColor = Color.FromArgb(48, 48, 48);
            ActiveDirectorySelectBtn.BackgroundImageLayout = ImageLayout.None;
            ActiveDirectorySelectBtn.FlatStyle = FlatStyle.Flat;
            ActiveDirectorySelectBtn.Font = new Font("Arial", 9F);
            ActiveDirectorySelectBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ActiveDirectorySelectBtn.Image = Properties.Resources.icons8_directory_24;
            ActiveDirectorySelectBtn.ImageAlign = ContentAlignment.MiddleLeft;
            ActiveDirectorySelectBtn.Location = new Point(5, 239);
            ActiveDirectorySelectBtn.Margin = new Padding(3, 2, 3, 2);
            ActiveDirectorySelectBtn.Name = "ActiveDirectorySelectBtn";
            ActiveDirectorySelectBtn.Size = new Size(356, 32);
            ActiveDirectorySelectBtn.TabIndex = 74;
            ActiveDirectorySelectBtn.Text = "Load PC names from Active Directory";
            ActiveDirectorySelectBtn.UseVisualStyleBackColor = false;
            ActiveDirectorySelectBtn.Click += ActiveDirectorySelectBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 10F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(36, 4);
            label4.Name = "label4";
            label4.Size = new Size(109, 19);
            label4.TabIndex = 72;
            label4.Text = "Send to PC's:";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.FromArgb(30, 30, 30);
            tabPage3.Controls.Add(clearLogBtn);
            tabPage3.Controls.Add(label1);
            tabPage3.Controls.Add(logList);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(366, 273);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Logs";
            // 
            // clearLogBtn
            // 
            clearLogBtn.BackColor = Color.FromArgb(48, 48, 48);
            clearLogBtn.Dock = DockStyle.Bottom;
            clearLogBtn.FlatStyle = FlatStyle.Flat;
            clearLogBtn.Font = new Font("Arial", 6.5F);
            clearLogBtn.ForeColor = Color.FromArgb(224, 224, 224);
            clearLogBtn.Location = new Point(0, 26);
            clearLogBtn.Margin = new Padding(3, 2, 3, 2);
            clearLogBtn.Name = "clearLogBtn";
            clearLogBtn.Size = new Size(366, 25);
            clearLogBtn.TabIndex = 53;
            clearLogBtn.Text = "Clear log";
            clearLogBtn.UseVisualStyleBackColor = false;
            clearLogBtn.Click += clearLogBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 4);
            label1.Name = "label1";
            label1.Size = new Size(146, 19);
            label1.TabIndex = 52;
            label1.Text = "RMC Runtime Log:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // logList
            // 
            logList.BackColor = Color.FromArgb(45, 45, 45);
            logList.BorderStyle = BorderStyle.FixedSingle;
            logList.Dock = DockStyle.Bottom;
            logList.Font = new Font("Segoe UI", 6.5F);
            logList.ForeColor = Color.White;
            logList.FormattingEnabled = true;
            logList.HorizontalScrollbar = true;
            logList.IntegralHeight = false;
            logList.ItemHeight = 13;
            logList.Location = new Point(0, 51);
            logList.Margin = new Padding(3, 2, 3, 2);
            logList.Name = "logList";
            logList.Size = new Size(366, 222);
            logList.TabIndex = 50;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.FromArgb(30, 30, 30);
            tabPage4.Controls.Add(AboutText);
            tabPage4.Controls.Add(IconsLinkLabel);
            tabPage4.Controls.Add(pictureBox2);
            tabPage4.Controls.Add(label6);
            tabPage4.Controls.Add(label7);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(366, 273);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "About";
            // 
            // AboutText
            // 
            AboutText.BackColor = Color.FromArgb(45, 45, 45);
            AboutText.BorderStyle = BorderStyle.FixedSingle;
            AboutText.Font = new Font("Segoe UI Variable Text", 8F);
            AboutText.ForeColor = Color.White;
            AboutText.Location = new Point(4, 60);
            AboutText.Margin = new Padding(3, 2, 3, 2);
            AboutText.MaxLength = 255;
            AboutText.Multiline = true;
            AboutText.Name = "AboutText";
            AboutText.ScrollBars = ScrollBars.Vertical;
            AboutText.Size = new Size(358, 181);
            AboutText.TabIndex = 78;
            AboutText.Text = resources.GetString("AboutText.Text");
            AboutText.WordWrap = false;
            // 
            // IconsLinkLabel
            // 
            IconsLinkLabel.AutoSize = true;
            IconsLinkLabel.LinkColor = Color.FromArgb(0, 192, 192);
            IconsLinkLabel.Location = new Point(252, 245);
            IconsLinkLabel.Name = "IconsLinkLabel";
            IconsLinkLabel.Size = new Size(109, 20);
            IconsLinkLabel.TabIndex = 4;
            IconsLinkLabel.TabStop = true;
            IconsLinkLabel.Text = "Icons by Icons8";
            IconsLinkLabel.LinkClicked += IconsLinkLabel_LinkClicked;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.RMC_GUI_Icon;
            pictureBox2.Location = new Point(3, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(72, 55);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Arial", 12F);
            label6.ForeColor = Color.FromArgb(224, 224, 224);
            label6.Location = new Point(81, 8);
            label6.Name = "label6";
            label6.Size = new Size(183, 23);
            label6.TabIndex = 1;
            label6.Text = "RapidMessageCast\r\n";
            label6.TextAlign = ContentAlignment.TopRight;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label7.Font = new Font("Arial", 10F);
            label7.ForeColor = Color.Silver;
            label7.Location = new Point(227, 31);
            label7.Name = "label7";
            label7.Size = new Size(130, 23);
            label7.TabIndex = 1;
            label7.Text = "by lloyd99901";
            label7.TextAlign = ContentAlignment.TopRight;
            // 
            // RenameSelectedRMSGBtn
            // 
            RenameSelectedRMSGBtn.BackColor = Color.FromArgb(48, 48, 48);
            RenameSelectedRMSGBtn.FlatStyle = FlatStyle.Flat;
            RenameSelectedRMSGBtn.Font = new Font("Arial", 9F);
            RenameSelectedRMSGBtn.ForeColor = Color.FromArgb(224, 224, 224);
            RenameSelectedRMSGBtn.Image = Properties.Resources.icons8_rename_24;
            RenameSelectedRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            RenameSelectedRMSGBtn.Location = new Point(107, 324);
            RenameSelectedRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            RenameSelectedRMSGBtn.Name = "RenameSelectedRMSGBtn";
            RenameSelectedRMSGBtn.Size = new Size(105, 32);
            RenameSelectedRMSGBtn.TabIndex = 77;
            RenameSelectedRMSGBtn.Text = "Rename";
            RenameSelectedRMSGBtn.TextAlign = ContentAlignment.MiddleRight;
            RenameSelectedRMSGBtn.UseVisualStyleBackColor = false;
            RenameSelectedRMSGBtn.Click += RenameSelectedRMSGBtn_Click;
            // 
            // RMSGHelpLink
            // 
            RMSGHelpLink.AutoSize = true;
            RMSGHelpLink.LinkColor = Color.FromArgb(0, 192, 0);
            RMSGHelpLink.Location = new Point(207, 50);
            RMSGHelpLink.Name = "RMSGHelpLink";
            RMSGHelpLink.Size = new Size(16, 20);
            RMSGHelpLink.TabIndex = 81;
            RMSGHelpLink.TabStop = true;
            RMSGHelpLink.Text = "?";
            RMSGHelpLink.LinkClicked += RMSGHelpLink_LinkClicked;
            // 
            // LoadSelectedRMSGBtn
            // 
            LoadSelectedRMSGBtn.BackColor = Color.FromArgb(48, 48, 48);
            LoadSelectedRMSGBtn.BackgroundImageLayout = ImageLayout.Zoom;
            LoadSelectedRMSGBtn.FlatStyle = FlatStyle.Flat;
            LoadSelectedRMSGBtn.Font = new Font("Arial", 9F);
            LoadSelectedRMSGBtn.ForeColor = Color.White;
            LoadSelectedRMSGBtn.Image = Properties.Resources.icons8_send_letter_24;
            LoadSelectedRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            LoadSelectedRMSGBtn.Location = new Point(4, 288);
            LoadSelectedRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            LoadSelectedRMSGBtn.Name = "LoadSelectedRMSGBtn";
            LoadSelectedRMSGBtn.Size = new Size(295, 32);
            LoadSelectedRMSGBtn.TabIndex = 79;
            LoadSelectedRMSGBtn.Text = "Load Selected .RMSG";
            LoadSelectedRMSGBtn.UseVisualStyleBackColor = false;
            LoadSelectedRMSGBtn.Click += LoadSelectedRMSGBtn_Click;
            // 
            // RMSGFileListBox
            // 
            RMSGFileListBox.BackColor = Color.FromArgb(45, 45, 45);
            RMSGFileListBox.BorderStyle = BorderStyle.FixedSingle;
            RMSGFileListBox.ContextMenuStrip = Rmsgcontextstrip;
            RMSGFileListBox.ForeColor = Color.White;
            RMSGFileListBox.FormattingEnabled = true;
            RMSGFileListBox.IntegralHeight = false;
            RMSGFileListBox.Location = new Point(4, 72);
            RMSGFileListBox.Margin = new Padding(3, 2, 3, 2);
            RMSGFileListBox.Name = "RMSGFileListBox";
            RMSGFileListBox.Size = new Size(295, 212);
            RMSGFileListBox.TabIndex = 78;
            RMSGFileListBox.DoubleClick += RMSGFileListBox_DoubleClick;
            // 
            // DeleteSelectedRMSGFileBtn
            // 
            DeleteSelectedRMSGFileBtn.BackColor = Color.FromArgb(48, 48, 48);
            DeleteSelectedRMSGFileBtn.FlatStyle = FlatStyle.Flat;
            DeleteSelectedRMSGFileBtn.Font = new Font("Arial", 9F);
            DeleteSelectedRMSGFileBtn.ForeColor = Color.FromArgb(224, 224, 224);
            DeleteSelectedRMSGFileBtn.Image = Properties.Resources.icons8_delete_24;
            DeleteSelectedRMSGFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteSelectedRMSGFileBtn.Location = new Point(217, 324);
            DeleteSelectedRMSGFileBtn.Margin = new Padding(3, 2, 3, 2);
            DeleteSelectedRMSGFileBtn.Name = "DeleteSelectedRMSGFileBtn";
            DeleteSelectedRMSGFileBtn.Size = new Size(82, 32);
            DeleteSelectedRMSGFileBtn.TabIndex = 75;
            DeleteSelectedRMSGFileBtn.Text = "Delete";
            DeleteSelectedRMSGFileBtn.TextAlign = ContentAlignment.MiddleRight;
            DeleteSelectedRMSGFileBtn.UseVisualStyleBackColor = false;
            DeleteSelectedRMSGFileBtn.Click += DeleteSelectedRMSGFileBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial", 10F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(30, 51);
            label5.Name = "label5";
            label5.Size = new Size(180, 19);
            label5.TabIndex = 74;
            label5.Text = "Quick Load RMSG File:";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // RefreshRMSGListBtn
            // 
            RefreshRMSGListBtn.BackColor = Color.FromArgb(48, 48, 48);
            RefreshRMSGListBtn.FlatStyle = FlatStyle.Flat;
            RefreshRMSGListBtn.Font = new Font("Arial", 9F);
            RefreshRMSGListBtn.ForeColor = Color.FromArgb(224, 224, 224);
            RefreshRMSGListBtn.Image = Properties.Resources.icons8_refresh_24;
            RefreshRMSGListBtn.ImageAlign = ContentAlignment.MiddleLeft;
            RefreshRMSGListBtn.Location = new Point(4, 324);
            RefreshRMSGListBtn.Margin = new Padding(3, 2, 3, 2);
            RefreshRMSGListBtn.Name = "RefreshRMSGListBtn";
            RefreshRMSGListBtn.Size = new Size(98, 32);
            RefreshRMSGListBtn.TabIndex = 76;
            RefreshRMSGListBtn.Text = "Refresh";
            RefreshRMSGListBtn.TextAlign = ContentAlignment.MiddleRight;
            RefreshRMSGListBtn.UseVisualStyleBackColor = false;
            RefreshRMSGListBtn.Click += RefreshRMSGListBtn_Click;
            // 
            // GreenButtonTimer
            // 
            GreenButtonTimer.Interval = 2000;
            GreenButtonTimer.Tick += GreenButtonTimer_Tick;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.icons8_message_24;
            pictureBox5.Location = new Point(5, 48);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(24, 24);
            pictureBox5.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox5.TabIndex = 88;
            pictureBox5.TabStop = false;
            // 
            // RMCManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(681, 429);
            Controls.Add(pictureBox5);
            Controls.Add(RenameSelectedRMSGBtn);
            Controls.Add(RMSGHelpLink);
            Controls.Add(LoadSelectedRMSGBtn);
            Controls.Add(RMSGFileListBox);
            Controls.Add(DeleteSelectedRMSGFileBtn);
            Controls.Add(label5);
            Controls.Add(RefreshRMSGListBtn);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Controls.Add(ScheduleBroadcastBtn);
            Controls.Add(BroadcastHistoryBtn);
            Controls.Add(StartBroadcastBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "RMCManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RapidMessageCast GUI -";
            FormClosing += RMCManager_FormClosing;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            Rmsgcontextstrip.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button StartBroadcastBtn;
        private Panel panel1;
        private Button BroadcastHistoryBtn;
        private Button ScheduleBroadcastBtn;
        private Button OpenRMCFileBtn;
        private Button SaveRMCFileBTN;
        private Button QuickSaveRMSGBtn;
        private ContextMenuStrip Rmsgcontextstrip;
        private ToolStripMenuItem renameSelectRMSGFileToolStripMenuItem;
        private ToolStripMenuItem deleteSelectedItemToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private LinkLabel MessageDurationHelpLink;
        private Button SaveMessageTxtBtn;
        private Button MessageOpenTxtBtn;
        private NumericUpDown expirySecondsTime;
        private NumericUpDown expiryMinutesTime;
        private NumericUpDown expiryHourTime;
        private Label label3;
        private TextBox MessageTxt;
        private Label MessageLimitLbl;
        private Button SavePCListTxtBtn;
        private Button ComputerListLoadFromFileBtn;
        private TextBox ComputerSelectList;
        private Button ActiveDirectorySelectBtn;
        private Label label4;
        private TabPage tabPage3;
        private Button clearLogBtn;
        private Label label1;
        public ListBox logList;
        private Button RenameSelectedRMSGBtn;
        private LinkLabel RMSGHelpLink;
        private Button LoadSelectedRMSGBtn;
        private ListBox RMSGFileListBox;
        private Button DeleteSelectedRMSGFileBtn;
        private Label label5;
        private Button RefreshRMSGListBtn;
        private Panel panel2;
        private Label RMCManagerLbl;
        private Label label2;
        private PictureBox pictureBox1;
        private Label PCCountLbl;
        private Label versionLbl;
        private System.Windows.Forms.Timer GreenButtonTimer;
        private CheckBox EmergencyModeCheckbox;
        private LinkLabel EmergencyHelpLink;
        private TabPage tabPage4;
        private PictureBox pictureBox2;
        private Label label6;
        private LinkLabel IconsLinkLabel;
        private Label label7;
        private TextBox AboutText;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
    }
}
