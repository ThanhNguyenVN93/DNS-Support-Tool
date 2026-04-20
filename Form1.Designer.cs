namespace frmdnsflush
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            groupBox1 = new GroupBox();
            txtcmd = new TextBox();
            btnChangeDns = new Button();
            btnflush = new Button();
            groupBox2 = new GroupBox();
            pictureBox1 = new PictureBox();
            lblos = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtcmd);
            groupBox1.Controls.Add(btnChangeDns);
            groupBox1.Controls.Add(btnflush);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(367, 116);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // txtcmd
            // 
            txtcmd.Location = new Point(111, 22);
            txtcmd.Multiline = true;
            txtcmd.Name = "txtcmd";
            txtcmd.ReadOnly = true;
            txtcmd.ScrollBars = ScrollBars.Vertical;
            txtcmd.Size = new Size(245, 84);
            txtcmd.TabIndex = 1;
            // 
            // btnChangeDns
            // 
            btnChangeDns.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChangeDns.Location = new Point(9, 67);
            btnChangeDns.Name = "btnChangeDns";
            btnChangeDns.Size = new Size(85, 39);
            btnChangeDns.TabIndex = 2;
            btnChangeDns.Text = "Change DNS";
            btnChangeDns.UseVisualStyleBackColor = true;
            btnChangeDns.Click += btnChangeDns_Click;
            // 
            // btnflush
            // 
            btnflush.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnflush.Location = new Point(9, 22);
            btnflush.Name = "btnflush";
            btnflush.Size = new Size(85, 39);
            btnflush.TabIndex = 0;
            btnflush.Text = "DNS Flush";
            btnflush.UseVisualStyleBackColor = true;
            btnflush.Click += btnflush_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pictureBox1);
            groupBox2.Controls.Add(lblos);
            groupBox2.Location = new Point(3, 125);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(367, 49);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(296, 17);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // lblos
            // 
            lblos.AutoSize = true;
            lblos.Location = new Point(9, 28);
            lblos.Name = "lblos";
            lblos.Size = new Size(38, 15);
            lblos.TabIndex = 0;
            lblos.Text = "label1";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(371, 180);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(387, 219);
            MinimizeBox = false;
            MinimumSize = new Size(387, 219);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        public System.Windows.Forms.Timer timer1;
        private TextBox txtcmd;
        private Button btnflush;
        private Button btnChangeDns;
        private Label lblos;
        private PictureBox pictureBox1;
    }
}
