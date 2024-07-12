namespace caro_project
{
    partial class chessRoom
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.pcbIcon = new System.Windows.Forms.PictureBox();
            this.chessBoard = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Username = new System.Windows.Forms.TextBox();
            this.pcbAvt = new System.Windows.Forms.PictureBox();
            this.txbIp = new System.Windows.Forms.TextBox();
            this.prbCoolDown = new System.Windows.Forms.ProgressBar();
            this.txbPlayerName = new System.Windows.Forms.TextBox();
            this.tmCoolDown = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbAvt)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pcbIcon);
            this.panel2.Location = new System.Drawing.Point(724, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(544, 392);
            this.panel2.TabIndex = 5;
            // 
            // pcbIcon
            // 
            this.pcbIcon.BackgroundImage = global::caro_project.Properties.Resources.caroIcon;
            this.pcbIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbIcon.Location = new System.Drawing.Point(0, 19);
            this.pcbIcon.Name = "pcbIcon";
            this.pcbIcon.Size = new System.Drawing.Size(544, 373);
            this.pcbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbIcon.TabIndex = 0;
            this.pcbIcon.TabStop = false;
            // 
            // chessBoard
            // 
            this.chessBoard.Location = new System.Drawing.Point(12, 63);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(706, 664);
            this.chessBoard.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.Username);
            this.panel3.Controls.Add(this.pcbAvt);
            this.panel3.Controls.Add(this.txbIp);
            this.panel3.Controls.Add(this.prbCoolDown);
            this.panel3.Controls.Add(this.txbPlayerName);
            this.panel3.Location = new System.Drawing.Point(724, 442);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(544, 285);
            this.panel3.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "CoolDown";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Lượt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username";
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(94, 23);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(250, 22);
            this.Username.TabIndex = 5;
            // 
            // pcbAvt
            // 
            this.pcbAvt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbAvt.Location = new System.Drawing.Point(350, 3);
            this.pcbAvt.Name = "pcbAvt";
            this.pcbAvt.Size = new System.Drawing.Size(191, 186);
            this.pcbAvt.TabIndex = 3;
            this.pcbAvt.TabStop = false;
            // 
            // txbIp
            // 
            this.txbIp.Enabled = false;
            this.txbIp.Location = new System.Drawing.Point(94, 157);
            this.txbIp.Name = "txbIp";
            this.txbIp.Size = new System.Drawing.Size(250, 22);
            this.txbIp.TabIndex = 2;
            this.txbIp.Text = "127.0.0.1";
            // 
            // prbCoolDown
            // 
            this.prbCoolDown.Location = new System.Drawing.Point(94, 106);
            this.prbCoolDown.Name = "prbCoolDown";
            this.prbCoolDown.Size = new System.Drawing.Size(250, 25);
            this.prbCoolDown.Step = 0;
            this.prbCoolDown.TabIndex = 1;
            // 
            // txbPlayerName
            // 
            this.txbPlayerName.Enabled = false;
            this.txbPlayerName.Location = new System.Drawing.Point(94, 65);
            this.txbPlayerName.Name = "txbPlayerName";
            this.txbPlayerName.Size = new System.Drawing.Size(250, 22);
            this.txbPlayerName.TabIndex = 0;
            // 
            // tmCoolDown
            // 
            this.tmCoolDown.Tick += new System.EventHandler(this.tmCoolDown_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1280, 28);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(60, 24);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // chessRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 759);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chessBoard);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Name = "chessRoom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.chessRoom_FormClosing);
            this.Load += new System.EventHandler(this.chessRoom_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbAvt)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pcbIcon;
        private System.Windows.Forms.Panel chessBoard;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pcbAvt;
        private System.Windows.Forms.TextBox txbIp;
        private System.Windows.Forms.ProgressBar prbCoolDown;
        private System.Windows.Forms.TextBox txbPlayerName;
        private System.Windows.Forms.Timer tmCoolDown;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}