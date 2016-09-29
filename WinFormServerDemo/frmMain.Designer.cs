namespace WinFormServerDemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelFrame = new System.Windows.Forms.Panel();
            this.panelCore = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFrame
            // 
            this.panelFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFrame.Controls.Add(this.panelCore);
            this.panelFrame.Location = new System.Drawing.Point(119, 56);
            this.panelFrame.Name = "panelFrame";
            this.panelFrame.Size = new System.Drawing.Size(870, 532);
            this.panelFrame.TabIndex = 0;
            // 
            // panelCore
            // 
            this.panelCore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCore.Location = new System.Drawing.Point(0, 0);
            this.panelCore.Name = "panelCore";
            this.panelCore.Size = new System.Drawing.Size(868, 530);
            this.panelCore.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14F);
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(92, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Core";
            this.label1.Click += new System.EventHandler(this.label_Click);
            this.label1.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(133, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "|";
            this.label2.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14F);
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(147, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Event";
            this.label3.Click += new System.EventHandler(this.label_Click);
            this.label3.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label3.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14F);
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(5, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 46);
            this.label4.TabIndex = 4;
            this.label4.Text = "Environment\r\n  Detection";
            this.label4.Click += new System.EventHandler(this.label_Click);
            this.label4.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label4.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 14F);
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(9, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 46);
            this.label5.TabIndex = 4;
            this.label5.Text = "Integrated\r\n Protector ";
            this.label5.Click += new System.EventHandler(this.label_Click);
            this.label5.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label5.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 14F);
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(20, 271);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 46);
            this.label6.TabIndex = 4;
            this.label6.Text = " Online\r\nMonitor";
            this.label6.Click += new System.EventHandler(this.label_Click);
            this.label6.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label6.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinFormServerDemo.Properties.Resources._1;
            this.pictureBox1.Location = new System.Drawing.Point(24, 391);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelFrame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panelFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelCore;
    }
}

