namespace frmdnsflush
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtcmd = new System.Windows.Forms.TextBox();
            this.btnChangeDns = new System.Windows.Forms.Button();
            this.btnflush = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblos = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtcmd);
            this.groupBox1.Controls.Add(this.btnChangeDns);
            this.groupBox1.Controls.Add(this.btnflush);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtcmd
            // 
            this.txtcmd.Location = new System.Drawing.Point(111, 22);
            this.txtcmd.Multiline = true;
            this.txtcmd.Name = "txtcmd";
            this.txtcmd.ReadOnly = true;
            this.txtcmd.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtcmd.Size = new System.Drawing.Size(245, 84);
            this.txtcmd.TabIndex = 1;
            // 
            // btnChangeDns
            // 
            this.btnChangeDns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnChangeDns.Location = new System.Drawing.Point(9, 67);
            this.btnChangeDns.Name = "btnChangeDns";
            this.btnChangeDns.Size = new System.Drawing.Size(85, 39);
            this.btnChangeDns.TabIndex = 2;
            this.btnChangeDns.Text = "Change DNS";
            this.btnChangeDns.UseVisualStyleBackColor = true;
            this.btnChangeDns.Click += new System.EventHandler(this.btnChangeDns_Click);
            // 
            // btnflush
            // 
            this.btnflush.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnflush.Location = new System.Drawing.Point(9, 22);
            this.btnflush.Name = "btnflush";
            this.btnflush.Size = new System.Drawing.Size(85, 39);
            this.btnflush.TabIndex = 0;
            this.btnflush.Text = "DNS Flush";
            this.btnflush.UseVisualStyleBackColor = true;
            this.btnflush.Click += new System.EventHandler(this.btnflush_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.lblos);
            this.groupBox2.Location = new System.Drawing.Point(3, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 49);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(296, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblos
            // 
            this.lblos.AutoSize = true;
            this.lblos.Location = new System.Drawing.Point(9, 28);
            this.lblos.Name = "lblos";
            this.lblos.Size = new System.Drawing.Size(38, 15);
            this.lblos.TabIndex = 0;
            this.lblos.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 180);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.MaximumSize = new System.Drawing.Size(387, 219);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(387, 219);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtcmd;
        private System.Windows.Forms.Button btnflush;
        private System.Windows.Forms.Button btnChangeDns;
        private System.Windows.Forms.Label lblos;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
