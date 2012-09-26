namespace MXiTunesRemote
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRetriever = new System.Windows.Forms.CheckedListBox();
            this.txtLogMsg = new System.Windows.Forms.TextBox();
            this.lblSelectedRetriever = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "当前选中：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "日志：";
            // 
            // chkRetriever
            // 
            this.chkRetriever.CheckOnClick = true;
            this.chkRetriever.FormattingEnabled = true;
            this.chkRetriever.Location = new System.Drawing.Point(14, 27);
            this.chkRetriever.Name = "chkRetriever";
            this.chkRetriever.Size = new System.Drawing.Size(323, 84);
            this.chkRetriever.TabIndex = 6;
            this.chkRetriever.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkRetriever_ItemCheck);
            this.chkRetriever.SelectedIndexChanged += new System.EventHandler(this.chkRetriever_SelectedIndexChanged);
            // 
            // txtLogMsg
            // 
            this.txtLogMsg.AcceptsReturn = true;
            this.txtLogMsg.Location = new System.Drawing.Point(14, 130);
            this.txtLogMsg.Multiline = true;
            this.txtLogMsg.Name = "txtLogMsg";
            this.txtLogMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogMsg.Size = new System.Drawing.Size(323, 239);
            this.txtLogMsg.TabIndex = 7;
            // 
            // lblSelectedRetriever
            // 
            this.lblSelectedRetriever.AutoSize = true;
            this.lblSelectedRetriever.Location = new System.Drawing.Point(83, 12);
            this.lblSelectedRetriever.Name = "lblSelectedRetriever";
            this.lblSelectedRetriever.Size = new System.Drawing.Size(0, 12);
            this.lblSelectedRetriever.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 381);
            this.Controls.Add(this.lblSelectedRetriever);
            this.Controls.Add(this.txtLogMsg);
            this.Controls.Add(this.chkRetriever);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "MXiTunes Helper";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox chkRetriever;
        private System.Windows.Forms.TextBox txtLogMsg;
        private System.Windows.Forms.Label lblSelectedRetriever;
    }
}

