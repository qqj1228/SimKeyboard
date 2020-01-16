namespace SimKeyboard {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.grpBoxHotKey = new System.Windows.Forms.GroupBox();
            this.txtBoxHotKey = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.grpBoxInput = new System.Windows.Forms.GroupBox();
            this.cmbBoxInput = new System.Windows.Forms.ComboBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.grpBoxHotKey.SuspendLayout();
            this.grpBoxInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxHotKey
            // 
            this.grpBoxHotKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxHotKey.Controls.Add(this.txtBoxHotKey);
            this.grpBoxHotKey.Location = new System.Drawing.Point(13, 13);
            this.grpBoxHotKey.Name = "grpBoxHotKey";
            this.grpBoxHotKey.Size = new System.Drawing.Size(409, 45);
            this.grpBoxHotKey.TabIndex = 0;
            this.grpBoxHotKey.TabStop = false;
            this.grpBoxHotKey.Text = "全局热键";
            // 
            // txtBoxHotKey
            // 
            this.txtBoxHotKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxHotKey.Location = new System.Drawing.Point(3, 17);
            this.txtBoxHotKey.Name = "txtBoxHotKey";
            this.txtBoxHotKey.ReadOnly = true;
            this.txtBoxHotKey.Size = new System.Drawing.Size(403, 21);
            this.txtBoxHotKey.TabIndex = 0;
            this.txtBoxHotKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBoxHotKey_KeyDown);
            this.txtBoxHotKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtBoxHotKey_KeyUp);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(347, 115);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "启用";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // grpBoxInput
            // 
            this.grpBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxInput.Controls.Add(this.cmbBoxInput);
            this.grpBoxInput.Location = new System.Drawing.Point(16, 64);
            this.grpBoxInput.Name = "grpBoxInput";
            this.grpBoxInput.Size = new System.Drawing.Size(406, 45);
            this.grpBoxInput.TabIndex = 1;
            this.grpBoxInput.TabStop = false;
            this.grpBoxInput.Text = "模拟输入";
            // 
            // cmbBoxInput
            // 
            this.cmbBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBoxInput.FormattingEnabled = true;
            this.cmbBoxInput.Location = new System.Drawing.Point(3, 17);
            this.cmbBoxInput.Name = "cmbBoxInput";
            this.cmbBoxInput.Size = new System.Drawing.Size(400, 20);
            this.cmbBoxInput.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(13, 116);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(29, 12);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "准备";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 146);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.grpBoxInput);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpBoxHotKey);
            this.Name = "Form1";
            this.Text = "模拟键盘";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpBoxHotKey.ResumeLayout(false);
            this.grpBoxHotKey.PerformLayout();
            this.grpBoxInput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxHotKey;
        private System.Windows.Forms.TextBox txtBoxHotKey;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox grpBoxInput;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ComboBox cmbBoxInput;
    }
}

