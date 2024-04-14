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
            panel1 = new Panel();
            panel2 = new Panel();
            RMCIconPictureBox = new PictureBox();
            label6 = new Label();
            RMCManagerLbl = new Label();
            OpenRMCFileBtn = new Button();
            SaveRMCFileBTN = new Button();
            QuickSaveRMSGBtn = new Button();
            BroadcastHistoryBtn = new Button();
            ScheduleBroadcastBtn = new Button();
            ComputerListLoadFromFileBtn = new Button();
            ComputerSelectList = new TextBox();
            MessageOpenTxtBtn = new Button();
            SaveMessageTxtBtn = new Button();
            SavePCListTxtBtn = new Button();
            panel3 = new Panel();
            AboutRMCLink = new LinkLabel();
            clearLogBtn = new Button();
            logList = new ListBox();
            label1 = new Label();
            LoadSelectedRMSGBtn = new Button();
            QuickSaveRMSGListBtn = new Button();
            RMSGFileListBox = new ListBox();
            DeleteSelectedRMSGFileBtn = new Button();
            RefreshRMSGListBtn = new Button();
            RenameSelectedRMSGBtn = new Button();
            label5 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            MessageDurationHelpLink = new LinkLabel();
            RMSGHelpLink = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RMCIconPictureBox).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            StartBroadcastBtn.Location = new Point(0, 444);
            StartBroadcastBtn.Margin = new Padding(3, 2, 3, 2);
            StartBroadcastBtn.Name = "StartBroadcastBtn";
            StartBroadcastBtn.Size = new Size(759, 54);
            StartBroadcastBtn.TabIndex = 9;
            StartBroadcastBtn.Text = "Start Message Broadcast";
            StartBroadcastBtn.TextAlign = ContentAlignment.BottomCenter;
            StartBroadcastBtn.UseVisualStyleBackColor = false;
            StartBroadcastBtn.Click += StartBroadcastBtn_Click;
            // 
            // MessageLimitLbl
            // 
            MessageLimitLbl.Font = new Font("Arial", 9F, FontStyle.Italic);
            MessageLimitLbl.ForeColor = Color.White;
            MessageLimitLbl.Location = new Point(441, 43);
            MessageLimitLbl.Name = "MessageLimitLbl";
            MessageLimitLbl.Size = new Size(138, 14);
            MessageLimitLbl.TabIndex = 8;
            MessageLimitLbl.Text = "Length Remaining: 255";
            MessageLimitLbl.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(268, 43);
            label2.Name = "label2";
            label2.Size = new Size(68, 16);
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
            ActiveDirectorySelectBtn.Location = new Point(268, 413);
            ActiveDirectorySelectBtn.Margin = new Padding(3, 2, 3, 2);
            ActiveDirectorySelectBtn.Name = "ActiveDirectorySelectBtn";
            ActiveDirectorySelectBtn.Size = new Size(311, 26);
            ActiveDirectorySelectBtn.TabIndex = 26;
            ActiveDirectorySelectBtn.Text = "Load PC names from Active Directory";
            ActiveDirectorySelectBtn.UseVisualStyleBackColor = false;
            ActiveDirectorySelectBtn.Click += ActiveDirectorySelectBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 10F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(268, 234);
            label4.Name = "label4";
            label4.Size = new Size(93, 16);
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
            expiryHourTime.Location = new Point(269, 209);
            expiryHourTime.Margin = new Padding(3, 2, 3, 2);
            expiryHourTime.Name = "expiryHourTime";
            expiryHourTime.Size = new Size(41, 21);
            expiryHourTime.TabIndex = 40;
            expiryHourTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // MessageTxt
            // 
            MessageTxt.BackColor = Color.FromArgb(73, 68, 80);
            MessageTxt.BorderStyle = BorderStyle.FixedSingle;
            MessageTxt.ForeColor = Color.White;
            MessageTxt.Location = new Point(268, 60);
            MessageTxt.Margin = new Padding(3, 2, 3, 2);
            MessageTxt.MaxLength = 255;
            MessageTxt.Multiline = true;
            MessageTxt.Name = "MessageTxt";
            MessageTxt.ScrollBars = ScrollBars.Vertical;
            MessageTxt.Size = new Size(311, 109);
            MessageTxt.TabIndex = 36;
            MessageTxt.TextChanged += messageTxt_TextChanged;
            MessageTxt.KeyPress += messageTxt_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 9F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(268, 174);
            label3.Name = "label3";
            label3.Size = new Size(106, 30);
            label3.TabIndex = 37;
            label3.Text = "Message duration\r\n(HH:MM:SS)";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // expiryMinutesTime
            // 
            expiryMinutesTime.BackColor = Color.FromArgb(73, 68, 80);
            expiryMinutesTime.BorderStyle = BorderStyle.FixedSingle;
            expiryMinutesTime.Font = new Font("Arial", 9F);
            expiryMinutesTime.ForeColor = Color.White;
            expiryMinutesTime.Location = new Point(315, 209);
            expiryMinutesTime.Margin = new Padding(3, 2, 3, 2);
            expiryMinutesTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expiryMinutesTime.Name = "expiryMinutesTime";
            expiryMinutesTime.Size = new Size(41, 21);
            expiryMinutesTime.TabIndex = 39;
            // 
            // expirySecondsTime
            // 
            expirySecondsTime.BackColor = Color.FromArgb(73, 68, 80);
            expirySecondsTime.BorderStyle = BorderStyle.FixedSingle;
            expirySecondsTime.Font = new Font("Arial", 9F);
            expirySecondsTime.ForeColor = Color.White;
            expirySecondsTime.Location = new Point(361, 209);
            expirySecondsTime.Margin = new Padding(3, 2, 3, 2);
            expirySecondsTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            expirySecondsTime.Name = "expirySecondsTime";
            expirySecondsTime.Size = new Size(41, 21);
            expirySecondsTime.TabIndex = 38;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(48, 37, 50);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(OpenRMCFileBtn);
            panel1.Controls.Add(SaveRMCFileBTN);
            panel1.Controls.Add(QuickSaveRMSGBtn);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(759, 39);
            panel1.TabIndex = 42;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(68, 58, 71);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(RMCIconPictureBox);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(RMCManagerLbl);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(177, 37);
            panel2.TabIndex = 0;
            // 
            // RMCIconPictureBox
            // 
            RMCIconPictureBox.Dock = DockStyle.Left;
            RMCIconPictureBox.Image = Properties.Resources.dsuiext_4118;
            RMCIconPictureBox.Location = new Point(0, 0);
            RMCIconPictureBox.Margin = new Padding(3, 2, 3, 2);
            RMCIconPictureBox.Name = "RMCIconPictureBox";
            RMCIconPictureBox.Size = new Size(47, 35);
            RMCIconPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            RMCIconPictureBox.TabIndex = 1;
            RMCIconPictureBox.TabStop = false;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Arial", 10F);
            label6.ForeColor = Color.Silver;
            label6.Location = new Point(109, 17);
            label6.Name = "label6";
            label6.Size = new Size(63, 16);
            label6.TabIndex = 1;
            label6.Text = "Manager";
            label6.TextAlign = ContentAlignment.TopRight;
            // 
            // RMCManagerLbl
            // 
            RMCManagerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            RMCManagerLbl.AutoSize = true;
            RMCManagerLbl.Font = new Font("Arial", 9F);
            RMCManagerLbl.ForeColor = Color.FromArgb(224, 224, 224);
            RMCManagerLbl.Location = new Point(53, 2);
            RMCManagerLbl.Name = "RMCManagerLbl";
            RMCManagerLbl.Size = new Size(117, 15);
            RMCManagerLbl.TabIndex = 1;
            RMCManagerLbl.Text = "RapidMessageCast\r\n";
            RMCManagerLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // OpenRMCFileBtn
            // 
            OpenRMCFileBtn.BackColor = Color.FromArgb(63, 58, 70);
            OpenRMCFileBtn.BackgroundImageLayout = ImageLayout.Zoom;
            OpenRMCFileBtn.Dock = DockStyle.Right;
            OpenRMCFileBtn.FlatStyle = FlatStyle.Flat;
            OpenRMCFileBtn.Font = new Font("Arial", 9F);
            OpenRMCFileBtn.ForeColor = Color.White;
            OpenRMCFileBtn.Image = Properties.Resources.imageres_53391;
            OpenRMCFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            OpenRMCFileBtn.Location = new Point(240, 0);
            OpenRMCFileBtn.Margin = new Padding(3, 2, 3, 2);
            OpenRMCFileBtn.Name = "OpenRMCFileBtn";
            OpenRMCFileBtn.Size = new Size(164, 37);
            OpenRMCFileBtn.TabIndex = 27;
            OpenRMCFileBtn.Text = "Open .RMSG file";
            OpenRMCFileBtn.UseVisualStyleBackColor = false;
            OpenRMCFileBtn.Click += OpenRMSGFileBtn_Click;
            // 
            // SaveRMCFileBTN
            // 
            SaveRMCFileBTN.BackColor = Color.FromArgb(63, 58, 70);
            SaveRMCFileBTN.BackgroundImageLayout = ImageLayout.Zoom;
            SaveRMCFileBTN.Dock = DockStyle.Right;
            SaveRMCFileBTN.FlatStyle = FlatStyle.Flat;
            SaveRMCFileBTN.Font = new Font("Arial", 9F);
            SaveRMCFileBTN.ForeColor = Color.White;
            SaveRMCFileBTN.Image = Properties.Resources.mspaint_501371;
            SaveRMCFileBTN.ImageAlign = ContentAlignment.MiddleLeft;
            SaveRMCFileBTN.Location = new Point(404, 0);
            SaveRMCFileBTN.Margin = new Padding(3, 2, 3, 2);
            SaveRMCFileBTN.Name = "SaveRMCFileBTN";
            SaveRMCFileBTN.Size = new Size(180, 37);
            SaveRMCFileBTN.TabIndex = 27;
            SaveRMCFileBTN.Text = "Save .RMSG file as...";
            SaveRMCFileBTN.UseVisualStyleBackColor = false;
            SaveRMCFileBTN.Click += SaveRMSGBttn;
            // 
            // QuickSaveRMSGBtn
            // 
            QuickSaveRMSGBtn.BackColor = Color.FromArgb(63, 58, 70);
            QuickSaveRMSGBtn.BackgroundImageLayout = ImageLayout.Zoom;
            QuickSaveRMSGBtn.Dock = DockStyle.Right;
            QuickSaveRMSGBtn.FlatStyle = FlatStyle.Flat;
            QuickSaveRMSGBtn.Font = new Font("Arial", 9F);
            QuickSaveRMSGBtn.ForeColor = Color.White;
            QuickSaveRMSGBtn.Image = Properties.Resources.mspaint_501371;
            QuickSaveRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            QuickSaveRMSGBtn.Location = new Point(584, 0);
            QuickSaveRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            QuickSaveRMSGBtn.Name = "QuickSaveRMSGBtn";
            QuickSaveRMSGBtn.Size = new Size(173, 37);
            QuickSaveRMSGBtn.TabIndex = 27;
            QuickSaveRMSGBtn.Text = "Quick Save .RMSG";
            QuickSaveRMSGBtn.UseVisualStyleBackColor = false;
            QuickSaveRMSGBtn.Click += QuickSaveRMSGBtn_Clicked;
            // 
            // BroadcastHistoryBtn
            // 
            BroadcastHistoryBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BroadcastHistoryBtn.BackColor = Color.FromArgb(63, 58, 70);
            BroadcastHistoryBtn.FlatStyle = FlatStyle.Flat;
            BroadcastHistoryBtn.Font = new Font("Arial", 9F);
            BroadcastHistoryBtn.ForeColor = Color.FromArgb(224, 224, 224);
            BroadcastHistoryBtn.Location = new Point(0, 444);
            BroadcastHistoryBtn.Margin = new Padding(3, 2, 3, 2);
            BroadcastHistoryBtn.Name = "BroadcastHistoryBtn";
            BroadcastHistoryBtn.Size = new Size(92, 53);
            BroadcastHistoryBtn.TabIndex = 26;
            BroadcastHistoryBtn.Text = "Broadcast History";
            BroadcastHistoryBtn.TextAlign = ContentAlignment.BottomCenter;
            BroadcastHistoryBtn.UseVisualStyleBackColor = false;
            // 
            // ScheduleBroadcastBtn
            // 
            ScheduleBroadcastBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ScheduleBroadcastBtn.BackColor = Color.FromArgb(63, 58, 70);
            ScheduleBroadcastBtn.FlatStyle = FlatStyle.Flat;
            ScheduleBroadcastBtn.Font = new Font("Arial", 9F);
            ScheduleBroadcastBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ScheduleBroadcastBtn.Location = new Point(668, 444);
            ScheduleBroadcastBtn.Margin = new Padding(3, 2, 3, 2);
            ScheduleBroadcastBtn.Name = "ScheduleBroadcastBtn";
            ScheduleBroadcastBtn.Size = new Size(91, 53);
            ScheduleBroadcastBtn.TabIndex = 26;
            ScheduleBroadcastBtn.Text = "Schedule Message";
            ScheduleBroadcastBtn.TextAlign = ContentAlignment.BottomCenter;
            ScheduleBroadcastBtn.UseVisualStyleBackColor = false;
            // 
            // ComputerListLoadFromFileBtn
            // 
            ComputerListLoadFromFileBtn.BackColor = Color.FromArgb(63, 58, 70);
            ComputerListLoadFromFileBtn.FlatStyle = FlatStyle.Flat;
            ComputerListLoadFromFileBtn.Font = new Font("Arial", 9F);
            ComputerListLoadFromFileBtn.ForeColor = Color.FromArgb(224, 224, 224);
            ComputerListLoadFromFileBtn.Image = Properties.Resources.imageres_53391;
            ComputerListLoadFromFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            ComputerListLoadFromFileBtn.Location = new Point(268, 382);
            ComputerListLoadFromFileBtn.Margin = new Padding(3, 2, 3, 2);
            ComputerListLoadFromFileBtn.Name = "ComputerListLoadFromFileBtn";
            ComputerListLoadFromFileBtn.Size = new Size(148, 26);
            ComputerListLoadFromFileBtn.TabIndex = 22;
            ComputerListLoadFromFileBtn.Text = "Open from .txt";
            ComputerListLoadFromFileBtn.UseVisualStyleBackColor = false;
            ComputerListLoadFromFileBtn.Click += openSendComputerListToolStripMenuItem_Click;
            // 
            // ComputerSelectList
            // 
            ComputerSelectList.BackColor = Color.FromArgb(73, 68, 80);
            ComputerSelectList.BorderStyle = BorderStyle.FixedSingle;
            ComputerSelectList.Font = new Font("Segoe UI", 8F);
            ComputerSelectList.ForeColor = Color.White;
            ComputerSelectList.Location = new Point(268, 252);
            ComputerSelectList.Margin = new Padding(3, 2, 3, 2);
            ComputerSelectList.Multiline = true;
            ComputerSelectList.Name = "ComputerSelectList";
            ComputerSelectList.ScrollBars = ScrollBars.Vertical;
            ComputerSelectList.Size = new Size(311, 125);
            ComputerSelectList.TabIndex = 36;
            ComputerSelectList.KeyPress += ComputerSelectList_KeyPress;
            // 
            // MessageOpenTxtBtn
            // 
            MessageOpenTxtBtn.BackColor = Color.FromArgb(63, 58, 70);
            MessageOpenTxtBtn.FlatStyle = FlatStyle.Flat;
            MessageOpenTxtBtn.Font = new Font("Arial", 9F);
            MessageOpenTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            MessageOpenTxtBtn.Image = Properties.Resources.imageres_53391;
            MessageOpenTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            MessageOpenTxtBtn.Location = new Point(421, 173);
            MessageOpenTxtBtn.Margin = new Padding(3, 2, 3, 2);
            MessageOpenTxtBtn.Name = "MessageOpenTxtBtn";
            MessageOpenTxtBtn.Size = new Size(158, 26);
            MessageOpenTxtBtn.TabIndex = 27;
            MessageOpenTxtBtn.Text = "Open from .txt";
            MessageOpenTxtBtn.UseVisualStyleBackColor = false;
            MessageOpenTxtBtn.Click += openMessageTextToolStripMenuItem_Click;
            // 
            // SaveMessageTxtBtn
            // 
            SaveMessageTxtBtn.BackColor = Color.FromArgb(63, 58, 70);
            SaveMessageTxtBtn.FlatStyle = FlatStyle.Flat;
            SaveMessageTxtBtn.Font = new Font("Arial", 9F);
            SaveMessageTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            SaveMessageTxtBtn.Image = Properties.Resources.mspaint_501371;
            SaveMessageTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            SaveMessageTxtBtn.Location = new Point(421, 204);
            SaveMessageTxtBtn.Margin = new Padding(3, 2, 3, 2);
            SaveMessageTxtBtn.Name = "SaveMessageTxtBtn";
            SaveMessageTxtBtn.Size = new Size(158, 26);
            SaveMessageTxtBtn.TabIndex = 27;
            SaveMessageTxtBtn.Text = "Save as .txt";
            SaveMessageTxtBtn.UseVisualStyleBackColor = false;
            SaveMessageTxtBtn.Click += SaveMessageBtn_Click;
            // 
            // SavePCListTxtBtn
            // 
            SavePCListTxtBtn.BackColor = Color.FromArgb(63, 58, 70);
            SavePCListTxtBtn.FlatStyle = FlatStyle.Flat;
            SavePCListTxtBtn.Font = new Font("Arial", 9F);
            SavePCListTxtBtn.ForeColor = Color.FromArgb(224, 224, 224);
            SavePCListTxtBtn.Image = Properties.Resources.mspaint_501371;
            SavePCListTxtBtn.ImageAlign = ContentAlignment.MiddleLeft;
            SavePCListTxtBtn.Location = new Point(421, 382);
            SavePCListTxtBtn.Margin = new Padding(3, 2, 3, 2);
            SavePCListTxtBtn.Name = "SavePCListTxtBtn";
            SavePCListTxtBtn.Size = new Size(158, 26);
            SavePCListTxtBtn.TabIndex = 27;
            SavePCListTxtBtn.Text = "Save list as txt";
            SavePCListTxtBtn.UseVisualStyleBackColor = false;
            SavePCListTxtBtn.Click += SaveComputerListBtn_Click;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(AboutRMCLink);
            panel3.Controls.Add(clearLogBtn);
            panel3.Controls.Add(logList);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(584, 39);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(175, 405);
            panel3.TabIndex = 43;
            // 
            // AboutRMCLink
            // 
            AboutRMCLink.AutoSize = true;
            AboutRMCLink.LinkColor = Color.FromArgb(0, 192, 192);
            AboutRMCLink.Location = new Point(104, 1);
            AboutRMCLink.Name = "AboutRMCLink";
            AboutRMCLink.Size = new Size(68, 16);
            AboutRMCLink.TabIndex = 28;
            AboutRMCLink.TabStop = true;
            AboutRMCLink.Text = "About RMC";
            AboutRMCLink.LinkClicked += AboutLabel_LinkClicked;
            // 
            // clearLogBtn
            // 
            clearLogBtn.BackColor = Color.FromArgb(63, 58, 70);
            clearLogBtn.FlatStyle = FlatStyle.Flat;
            clearLogBtn.Font = new Font("Arial", 6.5F);
            clearLogBtn.ForeColor = Color.FromArgb(224, 224, 224);
            clearLogBtn.Location = new Point(2, 21);
            clearLogBtn.Margin = new Padding(3, 2, 3, 2);
            clearLogBtn.Name = "clearLogBtn";
            clearLogBtn.Size = new Size(170, 20);
            clearLogBtn.TabIndex = 48;
            clearLogBtn.Text = "Clear log";
            clearLogBtn.UseVisualStyleBackColor = false;
            clearLogBtn.Click += clearLogBtn_Click;
            // 
            // logList
            // 
            logList.BackColor = Color.FromArgb(73, 68, 80);
            logList.BorderStyle = BorderStyle.FixedSingle;
            logList.Dock = DockStyle.Bottom;
            logList.Font = new Font("Segoe UI", 6.5F);
            logList.ForeColor = Color.White;
            logList.FormattingEnabled = true;
            logList.HorizontalScrollbar = true;
            logList.IntegralHeight = false;
            logList.ItemHeight = 12;
            logList.Location = new Point(0, 45);
            logList.Margin = new Padding(3, 2, 3, 2);
            logList.Name = "logList";
            logList.Size = new Size(173, 358);
            logList.TabIndex = 49;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(1, 2);
            label1.Name = "label1";
            label1.Size = new Size(91, 16);
            label1.TabIndex = 47;
            label1.Text = "Runtime Log:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // LoadSelectedRMSGBtn
            // 
            LoadSelectedRMSGBtn.BackColor = Color.FromArgb(63, 58, 70);
            LoadSelectedRMSGBtn.BackgroundImageLayout = ImageLayout.Zoom;
            LoadSelectedRMSGBtn.FlatStyle = FlatStyle.Flat;
            LoadSelectedRMSGBtn.Font = new Font("Arial", 9F);
            LoadSelectedRMSGBtn.ForeColor = Color.White;
            LoadSelectedRMSGBtn.Image = Properties.Resources.imageres_53391;
            LoadSelectedRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            LoadSelectedRMSGBtn.Location = new Point(5, 355);
            LoadSelectedRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            LoadSelectedRMSGBtn.Name = "LoadSelectedRMSGBtn";
            LoadSelectedRMSGBtn.Size = new Size(258, 26);
            LoadSelectedRMSGBtn.TabIndex = 69;
            LoadSelectedRMSGBtn.Text = "Load Selected .RMSG";
            LoadSelectedRMSGBtn.UseVisualStyleBackColor = false;
            LoadSelectedRMSGBtn.Click += LoadSelectedRMSGBtn_Click;
            // 
            // QuickSaveRMSGListBtn
            // 
            QuickSaveRMSGListBtn.BackColor = Color.FromArgb(63, 58, 70);
            QuickSaveRMSGListBtn.BackgroundImageLayout = ImageLayout.Zoom;
            QuickSaveRMSGListBtn.FlatStyle = FlatStyle.Flat;
            QuickSaveRMSGListBtn.Font = new Font("Arial", 9F);
            QuickSaveRMSGListBtn.ForeColor = Color.White;
            QuickSaveRMSGListBtn.Image = Properties.Resources.mspaint_501371;
            QuickSaveRMSGListBtn.ImageAlign = ContentAlignment.MiddleLeft;
            QuickSaveRMSGListBtn.Location = new Point(5, 385);
            QuickSaveRMSGListBtn.Margin = new Padding(3, 2, 3, 2);
            QuickSaveRMSGListBtn.Name = "QuickSaveRMSGListBtn";
            QuickSaveRMSGListBtn.Size = new Size(258, 26);
            QuickSaveRMSGListBtn.TabIndex = 70;
            QuickSaveRMSGListBtn.Text = "Quick Save .RMSG";
            QuickSaveRMSGListBtn.UseVisualStyleBackColor = false;
            QuickSaveRMSGListBtn.Click += QuickSaveRMSGBtn_Clicked;
            // 
            // RMSGFileListBox
            // 
            RMSGFileListBox.BackColor = Color.FromArgb(73, 68, 80);
            RMSGFileListBox.BorderStyle = BorderStyle.FixedSingle;
            RMSGFileListBox.ForeColor = Color.White;
            RMSGFileListBox.FormattingEnabled = true;
            RMSGFileListBox.IntegralHeight = false;
            RMSGFileListBox.Location = new Point(5, 60);
            RMSGFileListBox.Margin = new Padding(3, 2, 3, 2);
            RMSGFileListBox.Name = "RMSGFileListBox";
            RMSGFileListBox.Size = new Size(258, 291);
            RMSGFileListBox.TabIndex = 68;
            RMSGFileListBox.DoubleClick += RMSGFileListBox_DoubleClick;
            // 
            // DeleteSelectedRMSGFileBtn
            // 
            DeleteSelectedRMSGFileBtn.BackColor = Color.FromArgb(63, 58, 70);
            DeleteSelectedRMSGFileBtn.FlatStyle = FlatStyle.Flat;
            DeleteSelectedRMSGFileBtn.Font = new Font("Arial", 9F);
            DeleteSelectedRMSGFileBtn.ForeColor = Color.FromArgb(224, 224, 224);
            DeleteSelectedRMSGFileBtn.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteSelectedRMSGFileBtn.Location = new Point(181, 413);
            DeleteSelectedRMSGFileBtn.Margin = new Padding(3, 2, 3, 2);
            DeleteSelectedRMSGFileBtn.Name = "DeleteSelectedRMSGFileBtn";
            DeleteSelectedRMSGFileBtn.Size = new Size(82, 26);
            DeleteSelectedRMSGFileBtn.TabIndex = 65;
            DeleteSelectedRMSGFileBtn.Text = "Delete";
            DeleteSelectedRMSGFileBtn.UseVisualStyleBackColor = false;
            DeleteSelectedRMSGFileBtn.Click += DeleteSelectedRMSGFileBtn_Click;
            // 
            // RefreshRMSGListBtn
            // 
            RefreshRMSGListBtn.BackColor = Color.FromArgb(63, 58, 70);
            RefreshRMSGListBtn.FlatStyle = FlatStyle.Flat;
            RefreshRMSGListBtn.Font = new Font("Arial", 9F);
            RefreshRMSGListBtn.ForeColor = Color.FromArgb(224, 224, 224);
            RefreshRMSGListBtn.ImageAlign = ContentAlignment.MiddleLeft;
            RefreshRMSGListBtn.Location = new Point(96, 413);
            RefreshRMSGListBtn.Margin = new Padding(3, 2, 3, 2);
            RefreshRMSGListBtn.Name = "RefreshRMSGListBtn";
            RefreshRMSGListBtn.Size = new Size(82, 26);
            RefreshRMSGListBtn.TabIndex = 66;
            RefreshRMSGListBtn.Text = "Refresh";
            RefreshRMSGListBtn.UseVisualStyleBackColor = false;
            RefreshRMSGListBtn.Click += RefreshRMSGListBtn_Click;
            // 
            // RenameSelectedRMSGBtn
            // 
            RenameSelectedRMSGBtn.BackColor = Color.FromArgb(63, 58, 70);
            RenameSelectedRMSGBtn.FlatStyle = FlatStyle.Flat;
            RenameSelectedRMSGBtn.Font = new Font("Arial", 9F);
            RenameSelectedRMSGBtn.ForeColor = Color.FromArgb(224, 224, 224);
            RenameSelectedRMSGBtn.Image = Properties.Resources.pen;
            RenameSelectedRMSGBtn.ImageAlign = ContentAlignment.MiddleLeft;
            RenameSelectedRMSGBtn.Location = new Point(5, 413);
            RenameSelectedRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            RenameSelectedRMSGBtn.Name = "RenameSelectedRMSGBtn";
            RenameSelectedRMSGBtn.Size = new Size(87, 26);
            RenameSelectedRMSGBtn.TabIndex = 67;
            RenameSelectedRMSGBtn.Text = "Rename";
            RenameSelectedRMSGBtn.TextAlign = ContentAlignment.MiddleRight;
            RenameSelectedRMSGBtn.UseVisualStyleBackColor = false;
            RenameSelectedRMSGBtn.Click += RenameSelectedRMSGBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial", 10F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(5, 42);
            label5.Name = "label5";
            label5.Size = new Size(155, 16);
            label5.TabIndex = 64;
            label5.Text = "Quick Load RMSG File:";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.remotepg_501;
            pictureBox1.Location = new Point(361, 234);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 16);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 71;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.imageres_20;
            pictureBox2.Location = new Point(332, 42);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(24, 16);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 71;
            pictureBox2.TabStop = false;
            // 
            // MessageDurationHelpLink
            // 
            MessageDurationHelpLink.AutoSize = true;
            MessageDurationHelpLink.LinkColor = Color.FromArgb(0, 192, 0);
            MessageDurationHelpLink.Location = new Point(373, 171);
            MessageDurationHelpLink.Name = "MessageDurationHelpLink";
            MessageDurationHelpLink.Size = new Size(12, 16);
            MessageDurationHelpLink.TabIndex = 72;
            MessageDurationHelpLink.TabStop = true;
            MessageDurationHelpLink.Text = "?";
            MessageDurationHelpLink.LinkClicked += MessageDurationHelpLink_LinkClicked;
            // 
            // RMSGHelpLink
            // 
            RMSGHelpLink.AutoSize = true;
            RMSGHelpLink.LinkColor = Color.FromArgb(0, 192, 0);
            RMSGHelpLink.Location = new Point(160, 41);
            RMSGHelpLink.Name = "RMSGHelpLink";
            RMSGHelpLink.Size = new Size(12, 16);
            RMSGHelpLink.TabIndex = 72;
            RMSGHelpLink.TabStop = true;
            RMSGHelpLink.Text = "?";
            RMSGHelpLink.LinkClicked += RMSGHelpLink_LinkClicked;
            // 
            // RMCManager
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 31, 36);
            ClientSize = new Size(759, 498);
            Controls.Add(RMSGHelpLink);
            Controls.Add(MessageDurationHelpLink);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(LoadSelectedRMSGBtn);
            Controls.Add(QuickSaveRMSGListBtn);
            Controls.Add(RMSGFileListBox);
            Controls.Add(DeleteSelectedRMSGFileBtn);
            Controls.Add(RenameSelectedRMSGBtn);
            Controls.Add(label5);
            Controls.Add(SavePCListTxtBtn);
            Controls.Add(ComputerListLoadFromFileBtn);
            Controls.Add(panel3);
            Controls.Add(SaveMessageTxtBtn);
            Controls.Add(panel1);
            Controls.Add(MessageOpenTxtBtn);
            Controls.Add(expirySecondsTime);
            Controls.Add(expiryMinutesTime);
            Controls.Add(expiryHourTime);
            Controls.Add(label3);
            Controls.Add(ComputerSelectList);
            Controls.Add(MessageTxt);
            Controls.Add(ScheduleBroadcastBtn);
            Controls.Add(BroadcastHistoryBtn);
            Controls.Add(ActiveDirectorySelectBtn);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(MessageLimitLbl);
            Controls.Add(StartBroadcastBtn);
            Controls.Add(RefreshRMSGListBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "RMCManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RapidMessageCast Manager -";
            FormClosing += RMCManager_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)expiryHourTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expiryMinutesTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)expirySecondsTime).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)RMCIconPictureBox).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
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
        private Panel panel1;
        private Panel panel2;
        private PictureBox RMCIconPictureBox;
        private Label RMCManagerLbl;
        private Button BroadcastHistoryBtn;
        private Button ScheduleBroadcastBtn;
        private Button ComputerListLoadFromFileBtn;
        private TextBox ComputerSelectList;
        private Button MessageOpenTxtBtn;
        private Button OpenRMCFileBtn;
        private Button SaveRMCFileBTN;
        private Button SaveMessageTxtBtn;
        private Button SavePCListTxtBtn;
        private Panel panel3;
        private Button clearLogBtn;
        public ListBox logList;
        private Label label1;
        private Label label6;
        private LinkLabel AboutRMCLink;
        private Button LoadSelectedRMSGBtn;
        private ListBox RMSGFileListBox;
        private Button DeleteSelectedRMSGFileBtn;
        private Button RefreshRMSGListBtn;
        private Button RenameSelectedRMSGBtn;
        private Label label5;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button QuickSaveRMSGListBtn;
        private Button QuickSaveRMSGBtn;
        private LinkLabel MessageDurationHelpLink;
        private LinkLabel RMSGHelpLink;
    }
}
