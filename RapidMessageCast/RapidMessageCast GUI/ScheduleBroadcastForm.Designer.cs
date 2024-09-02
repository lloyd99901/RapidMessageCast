namespace RapidMessageCast_Manager
{
    partial class ScheduleBroadcastForm
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
            panel1 = new Panel();
            SaveRMCFileBTN = new Button();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label4 = new Label();
            RMCManagerLbl = new Label();
            monthCalendar1 = new MonthCalendar();
            LoadSelectedRMSGBtn = new Button();
            RMSGFileListBox = new ListBox();
            label5 = new Label();
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label3 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 32, 32);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(SaveRMCFileBTN);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(894, 36);
            panel1.TabIndex = 43;
            // 
            // SaveRMCFileBTN
            // 
            SaveRMCFileBTN.BackColor = Color.FromArgb(48, 48, 48);
            SaveRMCFileBTN.BackgroundImageLayout = ImageLayout.Zoom;
            SaveRMCFileBTN.Dock = DockStyle.Right;
            SaveRMCFileBTN.FlatStyle = FlatStyle.Flat;
            SaveRMCFileBTN.Font = new Font("Arial", 8F);
            SaveRMCFileBTN.ForeColor = Color.White;
            SaveRMCFileBTN.Image = Properties.Resources.icons8_save_24;
            SaveRMCFileBTN.ImageAlign = ContentAlignment.MiddleLeft;
            SaveRMCFileBTN.Location = new Point(766, 0);
            SaveRMCFileBTN.Margin = new Padding(3, 2, 3, 2);
            SaveRMCFileBTN.Name = "SaveRMCFileBTN";
            SaveRMCFileBTN.Size = new Size(126, 34);
            SaveRMCFileBTN.TabIndex = 28;
            SaveRMCFileBTN.Text = "Save and apply schedule";
            SaveRMCFileBTN.TextAlign = ContentAlignment.MiddleRight;
            SaveRMCFileBTN.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(48, 48, 48);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(RMCManagerLbl);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(237, 34);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resources.icons8_broadcast_24;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(18, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 9F);
            label4.ForeColor = Color.Silver;
            label4.Location = new Point(49, 19);
            label4.Name = "label4";
            label4.Size = new Size(180, 15);
            label4.TabIndex = 1;
            label4.Text = "Send messages on a schedule";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // RMCManagerLbl
            // 
            RMCManagerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            RMCManagerLbl.AutoSize = true;
            RMCManagerLbl.Font = new Font("Arial", 11F);
            RMCManagerLbl.ForeColor = Color.FromArgb(224, 224, 224);
            RMCManagerLbl.Location = new Point(23, 2);
            RMCManagerLbl.Name = "RMCManagerLbl";
            RMCManagerLbl.Size = new Size(74, 17);
            RMCManagerLbl.TabIndex = 1;
            RMCManagerLbl.Text = "Scheduler";
            RMCManagerLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(276, 55);
            monthCalendar1.Margin = new Padding(8, 7, 8, 7);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 44;
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
            LoadSelectedRMSGBtn.Location = new Point(7, 290);
            LoadSelectedRMSGBtn.Margin = new Padding(3, 2, 3, 2);
            LoadSelectedRMSGBtn.Name = "LoadSelectedRMSGBtn";
            LoadSelectedRMSGBtn.Size = new Size(259, 24);
            LoadSelectedRMSGBtn.TabIndex = 94;
            LoadSelectedRMSGBtn.Text = "Load Selected .RMSG";
            LoadSelectedRMSGBtn.UseVisualStyleBackColor = false;
            // 
            // RMSGFileListBox
            // 
            RMSGFileListBox.BackColor = Color.FromArgb(45, 45, 45);
            RMSGFileListBox.BorderStyle = BorderStyle.FixedSingle;
            RMSGFileListBox.ForeColor = Color.White;
            RMSGFileListBox.FormattingEnabled = true;
            RMSGFileListBox.IntegralHeight = false;
            RMSGFileListBox.ItemHeight = 15;
            RMSGFileListBox.Location = new Point(7, 55);
            RMSGFileListBox.Margin = new Padding(3, 2, 3, 2);
            RMSGFileListBox.Name = "RMSGFileListBox";
            RMSGFileListBox.Size = new Size(259, 233);
            RMSGFileListBox.TabIndex = 93;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial", 10F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(4, 39);
            label5.Name = "label5";
            label5.Size = new Size(260, 16);
            label5.TabIndex = 89;
            label5.Text = "Select the message you want scheduled";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(276, 39);
            label1.Name = "label1";
            label1.Size = new Size(97, 16);
            label1.TabIndex = 89;
            label1.Text = "Select the day";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(276, 217);
            label2.Name = "label2";
            label2.Size = new Size(70, 16);
            label2.TabIndex = 89;
            label2.Text = "Schedule:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Every X minutes", "Every X hours", "Daily", "Weekly", "Monthly", "Yearly", "Once only" });
            comboBox1.Location = new Point(381, 230);
            comboBox1.Margin = new Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(117, 23);
            comboBox1.TabIndex = 95;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 10F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(294, 232);
            label3.Name = "label3";
            label3.Size = new Size(79, 48);
            label3.TabIndex = 89;
            label3.Text = "Frequency:\r\n\r\nTime:";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // ScheduleBroadcastForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(894, 359);
            Controls.Add(comboBox1);
            Controls.Add(LoadSelectedRMSGBtn);
            Controls.Add(RMSGFileListBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label5);
            Controls.Add(monthCalendar1);
            Controls.Add(panel1);
            Margin = new Padding(3, 2, 3, 2);
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "ScheduleBroadcastForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ScheduleBroadcastForm";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label RMCManagerLbl;
        private MonthCalendar monthCalendar1;
        private Button LoadSelectedRMSGBtn;
        private ListBox RMSGFileListBox;
        private Label label5;
        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label4;
        private Button SaveRMCFileBTN;
    }
}