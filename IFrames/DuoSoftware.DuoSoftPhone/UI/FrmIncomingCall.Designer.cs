namespace DuoSoftware.DuoSoftPhone.UI
{
    partial class FrmIncomingCall
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonReject = new Infragistics.Win.Misc.UltraButton();
            this.buttonAnswer = new Infragistics.Win.Misc.UltraButton();
            this.phoner8ClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.callToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.answerCallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rejectCallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.phoner8ClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            appearance2.BackColor = System.Drawing.Color.Black;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraGroupBox1.Appearance = appearance2;
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.RectangularInset;
            this.ultraGroupBox1.ContextMenuStrip = this.phoner8ClickMenu;
            this.ultraGroupBox1.Controls.Add(this.buttonReject);
            this.ultraGroupBox1.Controls.Add(this.buttonAnswer);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(209, 70);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Incoming Call";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // buttonReject
            // 
            appearance4.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.EndCallM;
            appearance4.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonReject.Appearance = appearance4;
            this.buttonReject.ImageSize = new System.Drawing.Size(70, 50);
            this.buttonReject.Location = new System.Drawing.Point(108, 21);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(93, 42);
            this.buttonReject.TabIndex = 2;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // buttonAnswer
            // 
            appearance3.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.AnswerM;
            appearance3.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonAnswer.Appearance = appearance3;
            this.buttonAnswer.ImageSize = new System.Drawing.Size(70, 50);
            this.buttonAnswer.Location = new System.Drawing.Point(8, 21);
            this.buttonAnswer.Name = "buttonAnswer";
            this.buttonAnswer.Size = new System.Drawing.Size(93, 42);
            this.buttonAnswer.TabIndex = 1;
            this.buttonAnswer.Click += new System.EventHandler(this.buttonAnswer_Click);
            // 
            // phoner8ClickMenu
            // 
            this.phoner8ClickMenu.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.phoner8ClickMenu.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoner8ClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.callToolStripMenuItem});
            this.phoner8ClickMenu.Name = "phoner8ClickMenu";
            this.phoner8ClickMenu.Size = new System.Drawing.Size(93, 26);
            // 
            // callToolStripMenuItem
            // 
            this.callToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.answerCallToolStripMenuItem,
            this.rejectCallToolStripMenuItem});
            this.callToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.callToolStripMenuItem.Name = "callToolStripMenuItem";
            this.callToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.callToolStripMenuItem.Text = "Call";
            // 
            // answerCallToolStripMenuItem
            // 
            this.answerCallToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.answerCallToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.answerCallToolStripMenuItem.Name = "answerCallToolStripMenuItem";
            this.answerCallToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.answerCallToolStripMenuItem.ShowShortcutKeys = false;
            this.answerCallToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.answerCallToolStripMenuItem.Text = "AnswerCall";
            this.answerCallToolStripMenuItem.Click += new System.EventHandler(this.buttonAnswer_Click);
            // 
            // rejectCallToolStripMenuItem
            // 
            this.rejectCallToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rejectCallToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.rejectCallToolStripMenuItem.Name = "rejectCallToolStripMenuItem";
            this.rejectCallToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.rejectCallToolStripMenuItem.ShowShortcutKeys = false;
            this.rejectCallToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rejectCallToolStripMenuItem.Text = "RejectCall";
            this.rejectCallToolStripMenuItem.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // FrmIncomingCall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(209, 70);
            this.ContextMenuStrip = this.phoner8ClickMenu;
            this.ControlBox = false;
            this.Controls.Add(this.ultraGroupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmIncomingCall";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Incoming Call";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmIncomingCall_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.phoner8ClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton buttonReject;
        private Infragistics.Win.Misc.UltraButton buttonAnswer;
        private System.Windows.Forms.ContextMenuStrip phoner8ClickMenu;
        private System.Windows.Forms.ToolStripMenuItem callToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem answerCallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rejectCallToolStripMenuItem;


    }
}