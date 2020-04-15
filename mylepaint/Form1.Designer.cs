namespace LePaint
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTool = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProp = new System.Windows.Forms.ToolStripMenuItem();
            this.showOtherToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fireIndustriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerIndustriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.constructionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutLePaintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.drawingOnToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // drawingOnToolStripMenuItem
            // 
            this.drawingOnToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.drawingOnToolStripMenuItem.Name = "drawingOnToolStripMenuItem";
            this.drawingOnToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.drawingOnToolStripMenuItem.Text = "Edit";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(57, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTool,
            this.menuProp,
            this.showOtherToolsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // menuTool
            // 
            this.menuTool.Name = "menuTool";
            this.menuTool.Size = new System.Drawing.Size(166, 22);
            this.menuTool.Text = "ToolBox";
            this.menuTool.Click += new System.EventHandler(this.menuTool_Click);
            // 
            // menuProp
            // 
            this.menuProp.Name = "menuProp";
            this.menuProp.Size = new System.Drawing.Size(166, 22);
            this.menuProp.Text = "Property Window";
            this.menuProp.Click += new System.EventHandler(this.menuProp_Click);
            // 
            // showOtherToolsToolStripMenuItem
            // 
            this.showOtherToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fireIndustriesToolStripMenuItem,
            this.powerIndustriesToolStripMenuItem,
            this.constructionToolStripMenuItem});
            this.showOtherToolsToolStripMenuItem.Name = "showOtherToolsToolStripMenuItem";
            this.showOtherToolsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.showOtherToolsToolStripMenuItem.Text = "Show Other Tools";
            // 
            // fireIndustriesToolStripMenuItem
            // 
            this.fireIndustriesToolStripMenuItem.Name = "fireIndustriesToolStripMenuItem";
            this.fireIndustriesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.fireIndustriesToolStripMenuItem.Text = "Fire Industries";
            this.fireIndustriesToolStripMenuItem.Click += new System.EventHandler(this.fireIndustriesToolStripMenuItem_Click);
            // 
            // powerIndustriesToolStripMenuItem
            // 
            this.powerIndustriesToolStripMenuItem.Name = "powerIndustriesToolStripMenuItem";
            this.powerIndustriesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.powerIndustriesToolStripMenuItem.Text = "Power Industries";
            this.powerIndustriesToolStripMenuItem.Click += new System.EventHandler(this.powerIndustriesToolStripMenuItem_Click);
            // 
            // constructionToolStripMenuItem
            // 
            this.constructionToolStripMenuItem.Name = "constructionToolStripMenuItem";
            this.constructionToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.constructionToolStripMenuItem.Text = "Construction";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutLePaintToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutLePaintToolStripMenuItem
            // 
            this.aboutLePaintToolStripMenuItem.Name = "aboutLePaintToolStripMenuItem";
            this.aboutLePaintToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.aboutLePaintToolStripMenuItem.Text = "About LePaint";
            this.aboutLePaintToolStripMenuItem.Click += new System.EventHandler(this.aboutLePaintToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("testToolStripMenuItem.BackgroundImage")));
            this.testToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.testToolStripMenuItem.Checked = true;
            this.testToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.testToolStripMenuItem.Text = "test";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 441);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 441);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "LePaint";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem drawingOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutLePaintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuTool;
        private System.Windows.Forms.ToolStripMenuItem menuProp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem showOtherToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fireIndustriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerIndustriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem constructionToolStripMenuItem;
    }
}

