namespace RapidMessageCast_Manager
{
    partial class FilterPCListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterPCListForm));
            splitContainer1 = new SplitContainer();
            MessagePCList = new TextBox();
            BroadcastToImage = new PictureBox();
            RegexlogList = new ListBox();
            RegexFilterLabel = new Label();
            TempRegexTxt = new TextBox();
            AddRegexBtn = new Button();
            DeleteSelectedRegexBtn = new Button();
            AllRegexFiltersListbox = new ListBox();
            ApplyBtn = new Button();
            GlobalRegexLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BroadcastToImage).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(MessagePCList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(BroadcastToImage);
            splitContainer1.Panel2.Controls.Add(RegexlogList);
            splitContainer1.Panel2.Controls.Add(RegexFilterLabel);
            splitContainer1.Panel2.Controls.Add(TempRegexTxt);
            splitContainer1.Panel2.Controls.Add(AddRegexBtn);
            splitContainer1.Panel2.Controls.Add(DeleteSelectedRegexBtn);
            splitContainer1.Panel2.Controls.Add(AllRegexFiltersListbox);
            splitContainer1.Panel2.Controls.Add(ApplyBtn);
            splitContainer1.Panel2.Controls.Add(GlobalRegexLabel);
            splitContainer1.Size = new Size(713, 466);
            splitContainer1.SplitterDistance = 338;
            splitContainer1.TabIndex = 0;
            // 
            // MessagePCList
            // 
            MessagePCList.BackColor = Color.FromArgb(45, 45, 45);
            MessagePCList.BorderStyle = BorderStyle.FixedSingle;
            MessagePCList.Dock = DockStyle.Fill;
            MessagePCList.Font = new Font("Segoe UI", 8F);
            MessagePCList.ForeColor = Color.White;
            MessagePCList.Location = new Point(0, 0);
            MessagePCList.Margin = new Padding(3, 2, 3, 2);
            MessagePCList.Multiline = true;
            MessagePCList.Name = "MessagePCList";
            MessagePCList.ScrollBars = ScrollBars.Both;
            MessagePCList.Size = new Size(338, 466);
            MessagePCList.TabIndex = 78;
            // 
            // BroadcastToImage
            // 
            BroadcastToImage.Image = Properties.Resources.icons8_pc_24;
            BroadcastToImage.Location = new Point(335, 3);
            BroadcastToImage.Name = "BroadcastToImage";
            BroadcastToImage.Size = new Size(24, 24);
            BroadcastToImage.SizeMode = PictureBoxSizeMode.AutoSize;
            BroadcastToImage.TabIndex = 85;
            BroadcastToImage.TabStop = false;
            // 
            // RegexlogList
            // 
            RegexlogList.BackColor = Color.FromArgb(45, 45, 45);
            RegexlogList.BorderStyle = BorderStyle.FixedSingle;
            RegexlogList.Dock = DockStyle.Bottom;
            RegexlogList.Font = new Font("Segoe UI", 6.5F);
            RegexlogList.ForeColor = Color.White;
            RegexlogList.FormattingEnabled = true;
            RegexlogList.HorizontalScrollbar = true;
            RegexlogList.IntegralHeight = false;
            RegexlogList.ItemHeight = 13;
            RegexlogList.Items.AddRange(new object[] { "Regex Log - No Errors" });
            RegexlogList.Location = new Point(0, 310);
            RegexlogList.Margin = new Padding(3, 2, 3, 2);
            RegexlogList.Name = "RegexlogList";
            RegexlogList.Size = new Size(371, 88);
            RegexlogList.TabIndex = 84;
            // 
            // RegexFilterLabel
            // 
            RegexFilterLabel.AutoSize = true;
            RegexFilterLabel.Font = new Font("Arial", 10F);
            RegexFilterLabel.ForeColor = Color.White;
            RegexFilterLabel.Location = new Point(7, 214);
            RegexFilterLabel.Name = "RegexFilterLabel";
            RegexFilterLabel.Size = new Size(351, 19);
            RegexFilterLabel.TabIndex = 83;
            RegexFilterLabel.Text = "Regex Filter: (Type your Regex here, then add)";
            RegexFilterLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // TempRegexTxt
            // 
            TempRegexTxt.Location = new Point(7, 236);
            TempRegexTxt.Name = "TempRegexTxt";
            TempRegexTxt.Size = new Size(352, 27);
            TempRegexTxt.TabIndex = 82;
            // 
            // AddRegexBtn
            // 
            AddRegexBtn.BackColor = Color.FromArgb(48, 48, 48);
            AddRegexBtn.BackgroundImageLayout = ImageLayout.Zoom;
            AddRegexBtn.FlatStyle = FlatStyle.Flat;
            AddRegexBtn.Font = new Font("Arial", 9F);
            AddRegexBtn.ForeColor = Color.White;
            AddRegexBtn.Image = Properties.Resources.icons8_send_letter_24;
            AddRegexBtn.ImageAlign = ContentAlignment.MiddleLeft;
            AddRegexBtn.Location = new Point(7, 268);
            AddRegexBtn.Margin = new Padding(3, 2, 3, 2);
            AddRegexBtn.Name = "AddRegexBtn";
            AddRegexBtn.Size = new Size(352, 32);
            AddRegexBtn.TabIndex = 81;
            AddRegexBtn.Text = "Add Regex";
            AddRegexBtn.UseVisualStyleBackColor = false;
            AddRegexBtn.Click += AddRegexBtn_Click;
            // 
            // DeleteSelectedRegexBtn
            // 
            DeleteSelectedRegexBtn.BackColor = Color.FromArgb(48, 48, 48);
            DeleteSelectedRegexBtn.FlatStyle = FlatStyle.Flat;
            DeleteSelectedRegexBtn.Font = new Font("Arial", 9F);
            DeleteSelectedRegexBtn.ForeColor = Color.FromArgb(224, 224, 224);
            DeleteSelectedRegexBtn.Image = Properties.Resources.icons8_delete_24;
            DeleteSelectedRegexBtn.ImageAlign = ContentAlignment.MiddleLeft;
            DeleteSelectedRegexBtn.Location = new Point(7, 180);
            DeleteSelectedRegexBtn.Margin = new Padding(3, 2, 3, 2);
            DeleteSelectedRegexBtn.Name = "DeleteSelectedRegexBtn";
            DeleteSelectedRegexBtn.Size = new Size(352, 32);
            DeleteSelectedRegexBtn.TabIndex = 80;
            DeleteSelectedRegexBtn.Text = "Delete Selected";
            DeleteSelectedRegexBtn.UseVisualStyleBackColor = false;
            DeleteSelectedRegexBtn.Click += DeleteSelectedRegexBtn_Click;
            // 
            // AllRegexFiltersListbox
            // 
            AllRegexFiltersListbox.FormattingEnabled = true;
            AllRegexFiltersListbox.Location = new Point(7, 31);
            AllRegexFiltersListbox.Name = "AllRegexFiltersListbox";
            AllRegexFiltersListbox.Size = new Size(352, 144);
            AllRegexFiltersListbox.TabIndex = 77;
            // 
            // ApplyBtn
            // 
            ApplyBtn.BackColor = Color.FromArgb(53, 48, 70);
            ApplyBtn.BackgroundImageLayout = ImageLayout.Center;
            ApplyBtn.Dock = DockStyle.Bottom;
            ApplyBtn.FlatStyle = FlatStyle.Flat;
            ApplyBtn.Font = new Font("Arial", 9F, FontStyle.Bold);
            ApplyBtn.ForeColor = Color.PaleGreen;
            ApplyBtn.Image = Properties.Resources.Send4;
            ApplyBtn.ImageAlign = ContentAlignment.TopCenter;
            ApplyBtn.Location = new Point(0, 398);
            ApplyBtn.Margin = new Padding(3, 2, 3, 2);
            ApplyBtn.Name = "ApplyBtn";
            ApplyBtn.Size = new Size(371, 68);
            ApplyBtn.TabIndex = 76;
            ApplyBtn.Text = "Apply";
            ApplyBtn.TextAlign = ContentAlignment.BottomCenter;
            ApplyBtn.UseVisualStyleBackColor = false;
            ApplyBtn.Click += ApplyBtn_Click;
            // 
            // GlobalRegexLabel
            // 
            GlobalRegexLabel.AutoSize = true;
            GlobalRegexLabel.Font = new Font("Arial", 10F);
            GlobalRegexLabel.ForeColor = Color.White;
            GlobalRegexLabel.Location = new Point(7, 9);
            GlobalRegexLabel.Name = "GlobalRegexLabel";
            GlobalRegexLabel.Size = new Size(188, 19);
            GlobalRegexLabel.TabIndex = 75;
            GlobalRegexLabel.Text = "All Applied Regex Filters:";
            GlobalRegexLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // FilterPCListForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(713, 466);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(731, 467);
            Name = "FilterPCListForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Filter PC...";
            Load += FilterPCListForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)BroadcastToImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label GlobalRegexLabel;
        public Button ApplyBtn;
        private ListBox AllRegexFiltersListbox;
        private Button AddRegexBtn;
        private Button DeleteSelectedRegexBtn;
        private Label RegexFilterLabel;
        private TextBox TempRegexTxt;
        public ListBox RegexlogList;
        private PictureBox BroadcastToImage;
        public TextBox MessagePCList;
    }
}