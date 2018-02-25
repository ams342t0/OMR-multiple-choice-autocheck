namespace projectY
{
    partial class Form1
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
            this.panel = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.iMAGEFILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fOLDERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fOLDERToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sHEETFORMATToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aNSWERKEYSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.eXITToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPTIONSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.listResults = new System.Windows.Forms.ListBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.dATABASEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.source = new System.Windows.Forms.ToolStripStatusLabel();
            this.sheet = new System.Windows.Forms.ToolStripStatusLabel();
            this.database = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.panel)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(12, 28);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(364, 407);
            this.panel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panel.TabIndex = 4;
            this.panel.TabStop = false;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iMAGEFILEToolStripMenuItem,
            this.oPTIONSToolStripMenuItem,
            this.mnuClearLog});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(709, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // iMAGEFILEToolStripMenuItem
            // 
            this.iMAGEFILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fOLDERToolStripMenuItem,
            this.toolStripSeparator1,
            this.sHEETFORMATToolStripMenuItem,
            this.toolStripSeparator2,
            this.aNSWERKEYSToolStripMenuItem,
            this.toolStripSeparator3,
            this.dATABASEToolStripMenuItem,
            this.toolStripSeparator5,
            this.eXITToolStripMenuItem});
            this.iMAGEFILEToolStripMenuItem.Name = "iMAGEFILEToolStripMenuItem";
            this.iMAGEFILEToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.iMAGEFILEToolStripMenuItem.Text = "FILE";
            // 
            // fOLDERToolStripMenuItem
            // 
            this.fOLDERToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem,
            this.fOLDERToolStripMenuItem2,
            this.toolStripSeparator4});
            this.fOLDERToolStripMenuItem.Name = "fOLDERToolStripMenuItem";
            this.fOLDERToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.fOLDERToolStripMenuItem.Text = "SOURCE";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fILEToolStripMenuItem.Text = "FILE...";
            this.fILEToolStripMenuItem.Click += new System.EventHandler(this.fILEToolStripMenuItem_Click);
            // 
            // fOLDERToolStripMenuItem2
            // 
            this.fOLDERToolStripMenuItem2.Name = "fOLDERToolStripMenuItem2";
            this.fOLDERToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.fOLDERToolStripMenuItem2.Text = "FOLDER...";
            this.fOLDERToolStripMenuItem2.Click += new System.EventHandler(this.fOLDERToolStripMenuItem2_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // sHEETFORMATToolStripMenuItem
            // 
            this.sHEETFORMATToolStripMenuItem.Name = "sHEETFORMATToolStripMenuItem";
            this.sHEETFORMATToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.sHEETFORMATToolStripMenuItem.Text = "SHEET FORMAT...";
            this.sHEETFORMATToolStripMenuItem.Click += new System.EventHandler(this.sHEETFORMATToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // aNSWERKEYSToolStripMenuItem
            // 
            this.aNSWERKEYSToolStripMenuItem.Name = "aNSWERKEYSToolStripMenuItem";
            this.aNSWERKEYSToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aNSWERKEYSToolStripMenuItem.Text = "ANSWER KEYS...";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // eXITToolStripMenuItem
            // 
            this.eXITToolStripMenuItem.Name = "eXITToolStripMenuItem";
            this.eXITToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.eXITToolStripMenuItem.Text = "EXIT";
            // 
            // oPTIONSToolStripMenuItem
            // 
            this.oPTIONSToolStripMenuItem.Name = "oPTIONSToolStripMenuItem";
            this.oPTIONSToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.oPTIONSToolStripMenuItem.Text = "OPTIONS";
            // 
            // mnuClearLog
            // 
            this.mnuClearLog.Name = "mnuClearLog";
            this.mnuClearLog.Size = new System.Drawing.Size(80, 20);
            this.mnuClearLog.Text = "CLEAR LOG";
            this.mnuClearLog.Click += new System.EventHandler(this.cLEARLOGToolStripMenuItem_Click);
            // 
            // listResults
            // 
            this.listResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listResults.FormattingEnabled = true;
            this.listResults.Location = new System.Drawing.Point(380, 93);
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(314, 342);
            this.listResults.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(382, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(314, 56);
            this.button1.TabIndex = 8;
            this.button1.Text = "START";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dATABASEToolStripMenuItem
            // 
            this.dATABASEToolStripMenuItem.Name = "dATABASEToolStripMenuItem";
            this.dATABASEToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.dATABASEToolStripMenuItem.Text = "DATABASE...";
            this.dATABASEToolStripMenuItem.Click += new System.EventHandler(this.dATABASEToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(165, 6);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status,
            this.source,
            this.sheet,
            this.database});
            this.statusStrip1.Location = new System.Drawing.Point(0, 438);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(709, 24);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(122, 19);
            this.status.Text = "toolStripStatusLabel1";
            // 
            // source
            // 
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(118, 19);
            this.source.Text = "toolStripStatusLabel1";
            // 
            // sheet
            // 
            this.sheet.Name = "sheet";
            this.sheet.Size = new System.Drawing.Size(118, 19);
            this.sheet.Text = "toolStripStatusLabel1";
            // 
            // database
            // 
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(118, 19);
            this.database.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 462);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listResults);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "P Y";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox panel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem iMAGEFILEToolStripMenuItem;
        private System.Windows.Forms.ListBox listResults;
        private System.Windows.Forms.ToolStripMenuItem fOLDERToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sHEETFORMATToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fOLDERToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aNSWERKEYSToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem eXITToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oPTIONSToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem mnuClearLog;
        private System.Windows.Forms.ToolStripMenuItem dATABASEToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripStatusLabel source;
        private System.Windows.Forms.ToolStripStatusLabel sheet;
        private System.Windows.Forms.ToolStripStatusLabel database;
    }
}

