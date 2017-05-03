namespace DuoSoftware.DuoSoftPhone.UI
{
    partial class AudioWiz
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ButtonTestAudio = new System.Windows.Forms.Button();
            this.gbControls = new Infragistics.Win.Misc.UltraGroupBox();
            this.CheckBoxMute = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkboxMuteSpeaker = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.TrackBarMicrophone = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
            this.TrackBarSpeaker = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gbControls)).BeginInit();
            this.gbControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBoxMute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkboxMuteSpeaker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarMicrophone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarSpeaker)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonTestAudio
            // 
            this.ButtonTestAudio.BackColor = System.Drawing.Color.Gray;
            this.ButtonTestAudio.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonTestAudio.ForeColor = System.Drawing.Color.Navy;
            this.ButtonTestAudio.Location = new System.Drawing.Point(305, 129);
            this.ButtonTestAudio.Name = "ButtonTestAudio";
            this.ButtonTestAudio.Size = new System.Drawing.Size(107, 23);
            this.ButtonTestAudio.TabIndex = 41;
            this.ButtonTestAudio.Text = "Test Audio";
            this.ButtonTestAudio.UseVisualStyleBackColor = false;
            this.ButtonTestAudio.Click += new System.EventHandler(this.ButtonTestAudio_Click);
            // 
            // gbControls
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Calibri";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.gbControls.Appearance = appearance1;
            this.gbControls.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.RectangularInset;
            this.gbControls.Controls.Add(this.CheckBoxMute);
            this.gbControls.Controls.Add(this.chkboxMuteSpeaker);
            this.gbControls.Controls.Add(this.ButtonTestAudio);
            this.gbControls.Controls.Add(this.TrackBarMicrophone);
            this.gbControls.Controls.Add(this.TrackBarSpeaker);
            this.gbControls.Controls.Add(this.ultraLabel3);
            this.gbControls.Controls.Add(this.ultraLabel2);
            this.gbControls.Controls.Add(this.ultraLabel1);
            this.gbControls.Controls.Add(this.ultraLabel4);
            this.gbControls.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbControls.ForeColor = System.Drawing.Color.White;
            this.gbControls.Location = new System.Drawing.Point(10, 8);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(434, 163);
            this.gbControls.TabIndex = 48;
            this.gbControls.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            this.gbControls.Click += new System.EventHandler(this.gbControls_Click);
            // 
            // CheckBoxMute
            // 
            appearance6.FontData.BoldAsString = "False";
            this.CheckBoxMute.Appearance = appearance6;
            this.CheckBoxMute.AutoSize = true;
            this.CheckBoxMute.Location = new System.Drawing.Point(305, 102);
            this.CheckBoxMute.Name = "CheckBoxMute";
            this.CheckBoxMute.Size = new System.Drawing.Size(124, 21);
            this.CheckBoxMute.TabIndex = 7;
            this.CheckBoxMute.Text = "Mute Microphone";
            this.CheckBoxMute.CheckedChanged += new System.EventHandler(this.CheckBoxMute_CheckedChanged);
            // 
            // chkboxMuteSpeaker
            // 
            appearance7.FontData.BoldAsString = "False";
            this.chkboxMuteSpeaker.Appearance = appearance7;
            this.chkboxMuteSpeaker.AutoSize = true;
            this.chkboxMuteSpeaker.Location = new System.Drawing.Point(305, 76);
            this.chkboxMuteSpeaker.Name = "chkboxMuteSpeaker";
            this.chkboxMuteSpeaker.Size = new System.Drawing.Size(100, 21);
            this.chkboxMuteSpeaker.TabIndex = 6;
            this.chkboxMuteSpeaker.Text = "Mute Speaker";
            this.chkboxMuteSpeaker.CheckedChanged += new System.EventHandler(this.ChkboxMuteSpeaker_CheckedChanged);
            // 
            // TrackBarMicrophone
            // 
            this.TrackBarMicrophone.LargeChange = 10;
            this.TrackBarMicrophone.Location = new System.Drawing.Point(89, 101);
            this.TrackBarMicrophone.MaxValue = 255;
            this.TrackBarMicrophone.Name = "TrackBarMicrophone";
            this.TrackBarMicrophone.Size = new System.Drawing.Size(214, 23);
            this.TrackBarMicrophone.TabIndex = 5;
            this.TrackBarMicrophone.ViewStyle = Infragistics.Win.UltraWinEditors.TrackBarViewStyle.Vista;
            this.TrackBarMicrophone.ValueChanged += new System.EventHandler(this.TrackBarMicrophone_ValueChanged);
            // 
            // TrackBarSpeaker
            // 
            this.TrackBarSpeaker.LargeChange = 10;
            this.TrackBarSpeaker.Location = new System.Drawing.Point(89, 75);
            this.TrackBarSpeaker.MaxValue = 255;
            this.TrackBarSpeaker.Name = "TrackBarSpeaker";
            this.TrackBarSpeaker.Size = new System.Drawing.Size(214, 23);
            this.TrackBarSpeaker.TabIndex = 4;
            this.TrackBarSpeaker.ViewStyle = Infragistics.Win.UltraWinEditors.TrackBarViewStyle.Vista;
            this.TrackBarSpeaker.ValueChanged += new System.EventHandler(this.TrackBarSpeaker_ValueChanged);
            // 
            // ultraLabel3
            // 
            appearance4.TextHAlignAsString = "Left";
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance4;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(11, 77);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(51, 18);
            this.ultraLabel3.TabIndex = 2;
            this.ultraLabel3.Text = "Speaker";
            // 
            // ultraLabel2
            // 
            appearance5.TextHAlignAsString = "Left";
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance5;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(11, 103);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(75, 18);
            this.ultraLabel2.TabIndex = 1;
            this.ultraLabel2.Text = "Microphone";
            // 
            // ultraLabel1
            // 
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(11, 17);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(56, 18);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Speakers";
            // 
            // ultraLabel4
            // 
            appearance3.TextHAlignAsString = "Left";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance3;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(11, 45);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(80, 18);
            this.ultraLabel4.TabIndex = 3;
            this.ultraLabel4.Text = "Microphones";
            // 
            // AudioWiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(451, 181);
            this.Controls.Add(this.gbControls);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AudioWiz";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AudioWiz";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AudioWiz_FormClosed);
            this.Load += new System.EventHandler(this.AudioWiz_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbControls)).EndInit();
            this.gbControls.ResumeLayout(false);
            this.gbControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheckBoxMute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkboxMuteSpeaker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarMicrophone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarSpeaker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button ButtonTestAudio;
        private Infragistics.Win.Misc.UltraGroupBox gbControls;
        private Infragistics.Win.UltraWinEditors.UltraTrackBar TrackBarMicrophone;
        private Infragistics.Win.UltraWinEditors.UltraTrackBar TrackBarSpeaker;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor CheckBoxMute;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkboxMuteSpeaker;
    }
}