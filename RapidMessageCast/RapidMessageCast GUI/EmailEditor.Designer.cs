namespace RapidMessageCast_Manager
{
    partial class EmailEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailEditor));
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            RMCManagerLbl = new Label();
            button1 = new Button();
            SaveRMCFileBTN = new Button();
            FastBroadcastModeCheckbox = new CheckBox();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            richTextBox1 = new RichTextBox();
            button2 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(32, 32, 32);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(437, 36);
            panel1.TabIndex = 44;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(48, 48, 48);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(RMCManagerLbl);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(122, 34);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resources.icons8_email_24;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // RMCManagerLbl
            // 
            RMCManagerLbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            RMCManagerLbl.AutoSize = true;
            RMCManagerLbl.Font = new Font("Arial", 11F);
            RMCManagerLbl.ForeColor = Color.FromArgb(224, 224, 224);
            RMCManagerLbl.Location = new Point(30, 7);
            RMCManagerLbl.Name = "RMCManagerLbl";
            RMCManagerLbl.Size = new Size(87, 17);
            RMCManagerLbl.TabIndex = 1;
            RMCManagerLbl.Text = "Email Editor";
            RMCManagerLbl.TextAlign = ContentAlignment.TopRight;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(48, 48, 48);
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Right;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Arial", 8F);
            button1.ForeColor = Color.White;
            button1.Image = Properties.Resources.icons8_external_link_24;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(292, 0);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(143, 34);
            button1.TabIndex = 92;
            button1.Text = "Edit RMCEmail File...";
            button1.TextAlign = ContentAlignment.MiddleRight;
            button1.UseVisualStyleBackColor = false;
            // 
            // SaveRMCFileBTN
            // 
            SaveRMCFileBTN.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveRMCFileBTN.BackColor = Color.FromArgb(48, 48, 48);
            SaveRMCFileBTN.BackgroundImageLayout = ImageLayout.Zoom;
            SaveRMCFileBTN.FlatStyle = FlatStyle.Flat;
            SaveRMCFileBTN.Font = new Font("Arial", 8F);
            SaveRMCFileBTN.ForeColor = Color.White;
            SaveRMCFileBTN.Image = Properties.Resources.Send4;
            SaveRMCFileBTN.ImageAlign = ContentAlignment.MiddleLeft;
            SaveRMCFileBTN.Location = new Point(303, 206);
            SaveRMCFileBTN.Margin = new Padding(3, 2, 3, 2);
            SaveRMCFileBTN.Name = "SaveRMCFileBTN";
            SaveRMCFileBTN.Size = new Size(122, 34);
            SaveRMCFileBTN.TabIndex = 28;
            SaveRMCFileBTN.Text = "Send to RMC";
            SaveRMCFileBTN.TextAlign = ContentAlignment.MiddleRight;
            SaveRMCFileBTN.UseVisualStyleBackColor = false;
            // 
            // FastBroadcastModeCheckbox
            // 
            FastBroadcastModeCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            FastBroadcastModeCheckbox.AutoSize = true;
            FastBroadcastModeCheckbox.ForeColor = Color.White;
            FastBroadcastModeCheckbox.Location = new Point(76, 206);
            FastBroadcastModeCheckbox.Margin = new Padding(3, 2, 3, 2);
            FastBroadcastModeCheckbox.Name = "FastBroadcastModeCheckbox";
            FastBroadcastModeCheckbox.Size = new Size(88, 19);
            FastBroadcastModeCheckbox.TabIndex = 86;
            FastBroadcastModeCheckbox.Text = "HTML Body";
            FastBroadcastModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 10F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 47);
            label1.Name = "label1";
            label1.Size = new Size(58, 16);
            label1.TabIndex = 90;
            label1.Text = "Subject:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(76, 45);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(349, 23);
            textBox1.TabIndex = 91;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 10F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 76);
            label2.Name = "label2";
            label2.Size = new Size(43, 16);
            label2.TabIndex = 90;
            label2.Text = "Body:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(76, 76);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(349, 125);
            richTextBox1.TabIndex = 92;
            richTextBox1.Text = "";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.BackColor = Color.FromArgb(48, 48, 48);
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Arial", 8F);
            button2.ForeColor = Color.White;
            button2.Image = Properties.Resources.icons8_save_24;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(219, 206);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(78, 34);
            button2.TabIndex = 28;
            button2.Text = "Save...";
            button2.TextAlign = ContentAlignment.MiddleRight;
            button2.UseVisualStyleBackColor = false;
            // 
            // EmailEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(437, 251);
            Controls.Add(richTextBox1);
            Controls.Add(button2);
            Controls.Add(SaveRMCFileBTN);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(FastBroadcastModeCheckbox);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(453, 290);
            Name = "EmailEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RMC - Email Editor";
            Load += EmailEditor_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button SaveRMCFileBTN;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label RMCManagerLbl;
        private CheckBox FastBroadcastModeCheckbox;
        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private RichTextBox richTextBox1;
        private Button button2;
    }
}