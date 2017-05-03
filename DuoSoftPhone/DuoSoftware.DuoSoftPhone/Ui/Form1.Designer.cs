using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.Ui
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
            this.pnlVolume = new System.Windows.Forms.Panel();
            this.gbVolume = new VIBlend.WinForms.Controls.vGroupBox();
            this.vCircularProgressBar1 = new VIBlend.WinForms.Controls.vCircularProgressBar();
            this.btnVolumeClose = new System.Windows.Forms.PictureBox();
            this.chkSpeakerMute = new VIBlend.WinForms.Controls.vCheckBox();
            this.CheckBoxMute = new VIBlend.WinForms.Controls.vCheckBox();
            this.TrackBarMicrophone = new VIBlend.WinForms.Controls.vTrackBar();
            this.TrackBarSpeaker = new VIBlend.WinForms.Controls.vTrackBar();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.vApplicationMenuItem2 = new VIBlend.WinForms.Controls.vApplicationMenuItem();
            this.vApplicationMenuItem3 = new VIBlend.WinForms.Controls.vApplicationMenuItem();
            this.vApplicationMenuItem4 = new VIBlend.WinForms.Controls.vApplicationMenuItem();
            this.vApplicationMenuItem5 = new VIBlend.WinForms.Controls.vApplicationMenuItem();
            this.vApplicationMenuItem6 = new VIBlend.WinForms.Controls.vApplicationMenuItem();
            this.vBubbleBar1 = new VIBlend.WinForms.Controls.vBubbleBar();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.vButton2 = new VIBlend.WinForms.Controls.vButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.PhoneStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.vLabel1 = new VIBlend.WinForms.Controls.vLabel();
            this.volume = new System.Windows.Forms.ToolTip(this.components);
            this.vToolTip1 = new VIBlend.WinForms.Controls.vToolTip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vGroupBox1 = new VIBlend.WinForms.Controls.vGroupBox();
            this.vCheckBox1 = new VIBlend.WinForms.Controls.vCheckBox();
            this.vTrackBar1 = new VIBlend.WinForms.Controls.vTrackBar();
            this.vTrackBar2 = new VIBlend.WinForms.Controls.vTrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.vGroupBox2 = new VIBlend.WinForms.Controls.vGroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.phoner8ClickMenu = new VIBlend.WinForms.Controls.vContextMenu();
            this.accountSettingToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.dNDToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.BreakToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.BreakRequestmenuItem = new System.Windows.Forms.MenuItem();
            this.CancelRequestmenuItem = new System.Windows.Forms.MenuItem();
            this.EndBreakmenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.RingtoneOnMenuItem = new System.Windows.Forms.MenuItem();
            this.RingtoneOffmenuItem = new System.Windows.Forms.MenuItem();
            this.ShowInTaskbarmenuItem = new System.Windows.Forms.MenuItem();
            this.pnlVolume.SuspendLayout();
            this.gbVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnVolumeClose)).BeginInit();
            this.vBubbleBar1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.vGroupBox1.SuspendLayout();
            this.vGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlVolume
            // 
            this.pnlVolume.Controls.Add(this.gbVolume);
            this.pnlVolume.Location = new System.Drawing.Point(178, 35);
            this.pnlVolume.Name = "pnlVolume";
            this.pnlVolume.Size = new System.Drawing.Size(192, 131);
            this.pnlVolume.TabIndex = 48;
            // 
            // gbVolume
            // 
            this.gbVolume.BackColor = System.Drawing.Color.Transparent;
            this.gbVolume.Controls.Add(this.vCircularProgressBar1);
            this.gbVolume.Controls.Add(this.btnVolumeClose);
            this.gbVolume.Controls.Add(this.chkSpeakerMute);
            this.gbVolume.Controls.Add(this.CheckBoxMute);
            this.gbVolume.Controls.Add(this.TrackBarMicrophone);
            this.gbVolume.Controls.Add(this.TrackBarSpeaker);
            this.gbVolume.Controls.Add(this.Label11);
            this.gbVolume.Controls.Add(this.Label10);
            this.gbVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVolume.Location = new System.Drawing.Point(0, 0);
            this.gbVolume.Name = "gbVolume";
            this.gbVolume.Size = new System.Drawing.Size(192, 131);
            this.gbVolume.TabIndex = 3;
            this.gbVolume.TabStop = false;
            this.gbVolume.Text = "Volume";
            this.gbVolume.UseThemeBorderColor = true;
            this.gbVolume.UseThemeTextColor = true;
            this.gbVolume.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vCircularProgressBar1
            // 
            this.vCircularProgressBar1.AllowAnimations = true;
            this.vCircularProgressBar1.BackColor = System.Drawing.Color.Black;
            this.vCircularProgressBar1.IndicatorsCount = 8;
            this.vCircularProgressBar1.Location = new System.Drawing.Point(167, 126);
            this.vCircularProgressBar1.Maximum = 100;
            this.vCircularProgressBar1.Minimum = 0;
            this.vCircularProgressBar1.Name = "vCircularProgressBar1";
            this.vCircularProgressBar1.Size = new System.Drawing.Size(98, 86);
            this.vCircularProgressBar1.TabIndex = 51;
            this.vCircularProgressBar1.Text = "vCircularProgressBar1";
            this.vCircularProgressBar1.UseThemeBackground = false;
            this.vCircularProgressBar1.UseWaitCursor = true;
            this.vCircularProgressBar1.Value = 0;
            this.vCircularProgressBar1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.OFFICEBLACK;
            // 
            // btnVolumeClose
            // 
            this.btnVolumeClose.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.close_button_red_1412058367_123_231_20_248;
            this.btnVolumeClose.Location = new System.Drawing.Point(167, 11);
            this.btnVolumeClose.Name = "btnVolumeClose";
            this.btnVolumeClose.Size = new System.Drawing.Size(15, 14);
            this.btnVolumeClose.TabIndex = 1;
            this.btnVolumeClose.TabStop = false;
            // 
            // chkSpeakerMute
            // 
            this.chkSpeakerMute.BackColor = System.Drawing.Color.Transparent;
            this.chkSpeakerMute.Location = new System.Drawing.Point(3, 95);
            this.chkSpeakerMute.Name = "chkSpeakerMute";
            this.chkSpeakerMute.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkSpeakerMute.Size = new System.Drawing.Size(113, 24);
            this.chkSpeakerMute.TabIndex = 10;
            this.chkSpeakerMute.Text = "Mute microphone";
            this.chkSpeakerMute.UseVisualStyleBackColor = false;
            this.chkSpeakerMute.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // CheckBoxMute
            // 
            this.CheckBoxMute.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxMute.Location = new System.Drawing.Point(3, 73);
            this.CheckBoxMute.Name = "CheckBoxMute";
            this.CheckBoxMute.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CheckBoxMute.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxMute.TabIndex = 9;
            this.CheckBoxMute.Text = "Mute microphone";
            this.CheckBoxMute.UseVisualStyleBackColor = false;
            this.CheckBoxMute.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // TrackBarMicrophone
            // 
            this.TrackBarMicrophone.BackColor = System.Drawing.Color.Transparent;
            this.TrackBarMicrophone.Location = new System.Drawing.Point(69, 47);
            this.TrackBarMicrophone.Maximum = 65535;
            this.TrackBarMicrophone.Name = "TrackBarMicrophone";
            this.TrackBarMicrophone.RoundedCornersMask = ((byte)(15));
            this.TrackBarMicrophone.RoundedCornersMaskThumb = ((byte)(15));
            this.TrackBarMicrophone.Size = new System.Drawing.Size(112, 23);
            this.TrackBarMicrophone.TabIndex = 8;
            this.TrackBarMicrophone.Text = "vTrackBar1";
            this.TrackBarMicrophone.Value = 0;
            this.TrackBarMicrophone.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // TrackBarSpeaker
            // 
            this.TrackBarSpeaker.BackColor = System.Drawing.Color.Transparent;
            this.TrackBarSpeaker.Location = new System.Drawing.Point(69, 23);
            this.TrackBarSpeaker.Maximum = 65535;
            this.TrackBarSpeaker.Name = "TrackBarSpeaker";
            this.TrackBarSpeaker.RoundedCornersMask = ((byte)(15));
            this.TrackBarSpeaker.RoundedCornersMaskThumb = ((byte)(15));
            this.TrackBarSpeaker.Size = new System.Drawing.Size(102, 23);
            this.TrackBarSpeaker.TabIndex = 7;
            this.TrackBarSpeaker.Value = 10000;
            this.TrackBarSpeaker.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            this.TrackBarSpeaker.ValueChanged += new System.EventHandler(this.TrackBarSpeaker_ValueChanged);
            this.TrackBarSpeaker.MouseEnter += new System.EventHandler(this.TrackBarSpeaker_MouseEnter);
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(8, 52);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(63, 13);
            this.Label11.TabIndex = 4;
            this.Label11.Text = "Microphone";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(8, 28);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(47, 13);
            this.Label10.TabIndex = 3;
            this.Label10.Text = "Speaker";
            // 
            // vApplicationMenuItem2
            // 
            this.vApplicationMenuItem2.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem2.ItemType = VIBlend.WinForms.Controls.vApplicationMenuItemType.MenuItem;
            this.vApplicationMenuItem2.Location = new System.Drawing.Point(0, 0);
            this.vApplicationMenuItem2.Name = "vApplicationMenuItem2";
            this.vApplicationMenuItem2.SelectedChildMenuItem = null;
            this.vApplicationMenuItem2.Size = new System.Drawing.Size(200, 20);
            this.vApplicationMenuItem2.TabIndex = 0;
            this.vApplicationMenuItem2.Text = "vApplicationMenuItem";
            this.vApplicationMenuItem2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vApplicationMenuItem3
            // 
            this.vApplicationMenuItem3.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem3.ItemType = VIBlend.WinForms.Controls.vApplicationMenuItemType.MenuItem;
            this.vApplicationMenuItem3.Location = new System.Drawing.Point(0, 20);
            this.vApplicationMenuItem3.Name = "vApplicationMenuItem3";
            this.vApplicationMenuItem3.SelectedChildMenuItem = null;
            this.vApplicationMenuItem3.Size = new System.Drawing.Size(200, 20);
            this.vApplicationMenuItem3.TabIndex = 1;
            this.vApplicationMenuItem3.Text = "vApplicationMenuItem";
            this.vApplicationMenuItem3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem3.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vApplicationMenuItem4
            // 
            this.vApplicationMenuItem4.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem4.ItemType = VIBlend.WinForms.Controls.vApplicationMenuItemType.MenuItem;
            this.vApplicationMenuItem4.Location = new System.Drawing.Point(0, 40);
            this.vApplicationMenuItem4.Name = "vApplicationMenuItem4";
            this.vApplicationMenuItem4.SelectedChildMenuItem = null;
            this.vApplicationMenuItem4.Size = new System.Drawing.Size(200, 20);
            this.vApplicationMenuItem4.TabIndex = 2;
            this.vApplicationMenuItem4.Text = "vApplicationMenuItem";
            this.vApplicationMenuItem4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem4.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vApplicationMenuItem5
            // 
            this.vApplicationMenuItem5.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem5.ItemType = VIBlend.WinForms.Controls.vApplicationMenuItemType.Separator;
            this.vApplicationMenuItem5.Location = new System.Drawing.Point(0, 60);
            this.vApplicationMenuItem5.Name = "vApplicationMenuItem5";
            this.vApplicationMenuItem5.SelectedChildMenuItem = null;
            this.vApplicationMenuItem5.Size = new System.Drawing.Size(200, 20);
            this.vApplicationMenuItem5.TabIndex = 3;
            this.vApplicationMenuItem5.Text = "vApplicationMenuItem";
            this.vApplicationMenuItem5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem5.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vApplicationMenuItem6
            // 
            this.vApplicationMenuItem6.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem6.ItemType = VIBlend.WinForms.Controls.vApplicationMenuItemType.MenuItem;
            this.vApplicationMenuItem6.Location = new System.Drawing.Point(0, 80);
            this.vApplicationMenuItem6.Name = "vApplicationMenuItem6";
            this.vApplicationMenuItem6.SelectedChildMenuItem = null;
            this.vApplicationMenuItem6.Size = new System.Drawing.Size(200, 20);
            this.vApplicationMenuItem6.TabIndex = 4;
            this.vApplicationMenuItem6.Text = "vApplicationMenuItem";
            this.vApplicationMenuItem6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.vApplicationMenuItem6.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vBubbleBar1
            // 
            this.vBubbleBar1.AllowAnimations = true;
            this.vBubbleBar1.AutoAdjustBounds = true;
            this.vBubbleBar1.BackColor = System.Drawing.Color.Transparent;
            this.vBubbleBar1.BackgroundFillEnabled = false;
            this.vBubbleBar1.Controls.Add(this.vButton1);
            this.vBubbleBar1.DisplayBorder = false;
            this.vBubbleBar1.ItemsPosition = VIBlend.WinForms.Controls.BubbleBarPosition.Near;
            this.vBubbleBar1.ItemsSize = 50;
            this.vBubbleBar1.Location = new System.Drawing.Point(31, 161);
            this.vBubbleBar1.Name = "vBubbleBar1";
            this.vBubbleBar1.Size = new System.Drawing.Size(52, 69);
            this.vBubbleBar1.TabIndex = 49;
            this.vBubbleBar1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Location = new System.Drawing.Point(0, 0);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(50, 50);
            this.vButton1.TabIndex = 0;
            this.vButton1.Text = "vButton1";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // vButton2
            // 
            this.vButton2.AllowAnimations = true;
            this.vButton2.BackColor = System.Drawing.Color.Transparent;
            this.vButton2.Location = new System.Drawing.Point(108, 180);
            this.vButton2.Name = "vButton2";
            this.vButton2.RoundedCornersMask = ((byte)(15));
            this.vButton2.Size = new System.Drawing.Size(100, 30);
            this.vButton2.TabIndex = 50;
            this.vButton2.Text = "vButton2";
            this.vButton2.UseVisualStyleBackColor = false;
            this.vButton2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            this.vButton2.Click += new System.EventHandler(this.vButton2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PhoneStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(702, 22);
            this.statusStrip1.TabIndex = 51;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // PhoneStatus
            // 
            this.PhoneStatus.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.offline;
            this.PhoneStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PhoneStatus.Name = "PhoneStatus";
            this.PhoneStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PhoneStatus.Size = new System.Drawing.Size(40, 17);
            this.PhoneStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PhoneStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // vLabel1
            // 
            this.vLabel1.BackColor = System.Drawing.Color.Transparent;
            this.vLabel1.DisplayStyle = VIBlend.WinForms.Controls.LabelItemStyle.TextOnly;
            this.vLabel1.Ellipsis = false;
            this.vLabel1.ImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.Location = new System.Drawing.Point(214, 161);
            this.vLabel1.Multiline = true;
            this.vLabel1.Name = "vLabel1";
            this.vLabel1.Size = new System.Drawing.Size(80, 25);
            this.vLabel1.TabIndex = 52;
            this.vLabel1.Text = "vLabel1";
            this.vLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.UseMnemonics = true;
            this.vLabel1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // volume
            // 
            this.volume.BackColor = System.Drawing.Color.Black;
            this.volume.ForeColor = System.Drawing.Color.White;
            this.volume.ShowAlways = true;
            // 
            // vToolTip1
            // 
            this.vToolTip1.AbsoluteShowPosition = new System.Drawing.Point(0, 0);
            this.vToolTip1.AllowAnimations = true;
            this.vToolTip1.AutoPopDelay = 2500;
            this.vToolTip1.ContentMargin = new System.Windows.Forms.Padding(2);
            this.vToolTip1.DescriptionRelativeSizeProportion = new System.Drawing.SizeF(0.7F, 0.6F);
            this.vToolTip1.DescriptionText = "Description Text";
            this.vToolTip1.DescriptionTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.vToolTip1.DescriptionTextColor = System.Drawing.SystemColors.ControlText;
            this.vToolTip1.DescriptionTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vToolTip1.DropDownShowDirection = System.Windows.Forms.ToolStripDropDownDirection.Default;
            this.vToolTip1.FooterRelativeSizeProportion = new System.Drawing.SizeF(1F, 0.2F);
            this.vToolTip1.FooterText = "";
            this.vToolTip1.FooterTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.vToolTip1.FooterTextColor = System.Drawing.SystemColors.ControlText;
            this.vToolTip1.FooterTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vToolTip1.GradientStyle = VIBlend.Utilities.GradientStyles.Linear;
            this.vToolTip1.HeaderRelativeSizeProportion = new System.Drawing.SizeF(1F, 0.2F);
            this.vToolTip1.HeaderText = "";
            this.vToolTip1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.vToolTip1.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this.vToolTip1.HeaderTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vToolTip1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.vToolTip1.ImageRelativeSizeProportion = new System.Drawing.SizeF(0.3F, 0.6F);
            this.vToolTip1.InitialDelay = 800;
            this.vToolTip1.Location = new System.Drawing.Point(-50, 213);
            this.vToolTip1.Name = "vToolTip1";
            this.vToolTip1.ShowBelowOwner = true;
            this.vToolTip1.Size = new System.Drawing.Size(75, 23);
            this.vToolTip1.TabIndex = 53;
            this.vToolTip1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.vGroupBox1);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(311, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 131);
            this.panel1.TabIndex = 54;
            this.panel1.Visible = false;
            // 
            // vGroupBox1
            // 
            this.vGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.vGroupBox1.Controls.Add(this.vCheckBox1);
            this.vGroupBox1.Controls.Add(this.vTrackBar1);
            this.vGroupBox1.Controls.Add(this.vTrackBar2);
            this.vGroupBox1.Controls.Add(this.label1);
            this.vGroupBox1.Controls.Add(this.label2);
            this.vGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGroupBox1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.vGroupBox1.Name = "vGroupBox1";
            this.vGroupBox1.Size = new System.Drawing.Size(192, 131);
            this.vGroupBox1.TabIndex = 0;
            this.vGroupBox1.TabStop = false;
            this.vGroupBox1.Text = "Volume Setting";
            this.vGroupBox1.UseThemeBorderColor = true;
            this.vGroupBox1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vCheckBox1
            // 
            this.vCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.vCheckBox1.FocusColor = System.Drawing.Color.White;
            this.vCheckBox1.Location = new System.Drawing.Point(26, 70);
            this.vCheckBox1.Name = "vCheckBox1";
            this.vCheckBox1.Size = new System.Drawing.Size(104, 24);
            this.vCheckBox1.TabIndex = 15;
            this.vCheckBox1.Text = "vCheckBox1";
            this.vCheckBox1.UseThemeTextColor = false;
            this.vCheckBox1.UseVisualStyleBackColor = false;
            this.vCheckBox1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vTrackBar1
            // 
            this.vTrackBar1.BackColor = System.Drawing.Color.Black;
            this.vTrackBar1.ForeColor = System.Drawing.Color.White;
            this.vTrackBar1.Location = new System.Drawing.Point(73, 41);
            this.vTrackBar1.Maximum = 65535;
            this.vTrackBar1.Name = "vTrackBar1";
            this.vTrackBar1.RoundedCornersMask = ((byte)(15));
            this.vTrackBar1.RoundedCornersMaskThumb = ((byte)(15));
            this.vTrackBar1.Size = new System.Drawing.Size(112, 23);
            this.vTrackBar1.TabIndex = 14;
            this.vTrackBar1.Text = "vTrackBar1";
            this.vTrackBar1.Value = 0;
            this.vTrackBar1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vTrackBar2
            // 
            this.vTrackBar2.BackColor = System.Drawing.Color.Black;
            this.vTrackBar2.ForeColor = System.Drawing.Color.White;
            this.vTrackBar2.Location = new System.Drawing.Point(73, 17);
            this.vTrackBar2.Maximum = 65535;
            this.vTrackBar2.Name = "vTrackBar2";
            this.vTrackBar2.RoundedCornersMask = ((byte)(15));
            this.vTrackBar2.RoundedCornersMaskThumb = ((byte)(15));
            this.vTrackBar2.Size = new System.Drawing.Size(112, 23);
            this.vTrackBar2.SmallChange = 100;
            this.vTrackBar2.TabIndex = 13;
            this.vTrackBar2.Text = "vTrackBar1";
            this.vTrackBar2.Value = 0;
            this.vTrackBar2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "Microphone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "Speaker";
            // 
            // vGroupBox2
            // 
            this.vGroupBox2.BackColor = System.Drawing.Color.Black;
            this.vGroupBox2.Controls.Add(this.checkBox1);
            this.vGroupBox2.Controls.Add(this.pictureBox1);
            this.vGroupBox2.ForeColor = System.Drawing.Color.White;
            this.vGroupBox2.Location = new System.Drawing.Point(489, 35);
            this.vGroupBox2.Name = "vGroupBox2";
            this.vGroupBox2.Size = new System.Drawing.Size(192, 131);
            this.vGroupBox2.TabIndex = 55;
            this.vGroupBox2.TabStop = false;
            this.vGroupBox2.Text = "Volume";
            this.vGroupBox2.TitleBackColor = System.Drawing.Color.Black;
            this.vGroupBox2.UseThemeBorderColor = true;
            this.vGroupBox2.UseThemeTextColor = true;
            this.vGroupBox2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(43, 63);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.close_button_red_1412058367_123_231_20_248;
            this.pictureBox1.Location = new System.Drawing.Point(170, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 14);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // phoner8ClickMenu
            // 
            this.phoner8ClickMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.accountSettingToolStripMenuItem,
            this.volumeToolStripMenuItem,
            this.dNDToolStripMenuItem,
            this.BreakToolStripMenuItem,
            this.menuItem1,
            this.ShowInTaskbarmenuItem});
            // 
            // accountSettingToolStripMenuItem
            // 
            this.accountSettingToolStripMenuItem.Index = 0;
            this.accountSettingToolStripMenuItem.Text = "Account Setting";
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.Index = 1;
            this.volumeToolStripMenuItem.Text = "Volume";
            // 
            // dNDToolStripMenuItem
            // 
            this.dNDToolStripMenuItem.Index = 2;
            this.dNDToolStripMenuItem.Text = "DND";
            // 
            // BreakToolStripMenuItem
            // 
            this.BreakToolStripMenuItem.Index = 3;
            this.BreakToolStripMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.BreakRequestmenuItem,
            this.CancelRequestmenuItem,
            this.EndBreakmenuItem});
            this.BreakToolStripMenuItem.Text = "Break ";
            // 
            // BreakRequestmenuItem
            // 
            this.BreakRequestmenuItem.Index = 0;
            this.BreakRequestmenuItem.Text = "Break Request";
            // 
            // CancelRequestmenuItem
            // 
            this.CancelRequestmenuItem.Index = 1;
            this.CancelRequestmenuItem.Text = "Cancel Request";
            // 
            // EndBreakmenuItem
            // 
            this.EndBreakmenuItem.Index = 2;
            this.EndBreakmenuItem.Text = "End Break";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 4;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.RingtoneOnMenuItem,
            this.RingtoneOffmenuItem});
            this.menuItem1.Text = "Ringtone";
            // 
            // RingtoneOnMenuItem
            // 
            this.RingtoneOnMenuItem.Index = 0;
            this.RingtoneOnMenuItem.Text = "On";
            // 
            // RingtoneOffmenuItem
            // 
            this.RingtoneOffmenuItem.Index = 1;
            this.RingtoneOffmenuItem.Text = "Off";
            // 
            // ShowInTaskbarmenuItem
            // 
            this.ShowInTaskbarmenuItem.Checked = true;
            this.ShowInTaskbarmenuItem.Index = 5;
            this.ShowInTaskbarmenuItem.Text = "ShowIn Taskbar";
            this.ShowInTaskbarmenuItem.Click += new System.EventHandler(this.ShowInTaskbarmenuItem_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(702, 261);
            this.Controls.Add(this.vGroupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.vToolTip1);
            this.Controls.Add(this.vLabel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.vButton2);
            this.Controls.Add(this.vBubbleBar1);
            this.Controls.Add(this.pnlVolume);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.pnlVolume.ResumeLayout(false);
            this.gbVolume.ResumeLayout(false);
            this.gbVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnVolumeClose)).EndInit();
            this.vBubbleBar1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.vGroupBox1.ResumeLayout(false);
            this.vGroupBox1.PerformLayout();
            this.vGroupBox2.ResumeLayout(false);
            this.vGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlVolume;
        private VIBlend.WinForms.Controls.vGroupBox gbVolume;
        private PictureBox btnVolumeClose;
        private VIBlend.WinForms.Controls.vCheckBox chkSpeakerMute;
        private VIBlend.WinForms.Controls.vCheckBox CheckBoxMute;
        private VIBlend.WinForms.Controls.vTrackBar TrackBarMicrophone;
        private VIBlend.WinForms.Controls.vTrackBar TrackBarSpeaker;
        internal Label Label11;
        internal Label Label10;
        private VIBlend.WinForms.Controls.vApplicationMenuItem vApplicationMenuItem2;
        private VIBlend.WinForms.Controls.vApplicationMenuItem vApplicationMenuItem3;
        private VIBlend.WinForms.Controls.vApplicationMenuItem vApplicationMenuItem4;
        private VIBlend.WinForms.Controls.vApplicationMenuItem vApplicationMenuItem5;
        private VIBlend.WinForms.Controls.vApplicationMenuItem vApplicationMenuItem6;
        private VIBlend.WinForms.Controls.vBubbleBar vBubbleBar1;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private VIBlend.WinForms.Controls.vButton vButton2;
        private VIBlend.WinForms.Controls.vCircularProgressBar vCircularProgressBar1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel PhoneStatus;
        private VIBlend.WinForms.Controls.vLabel vLabel1;
        private ToolTip volume;
        private VIBlend.WinForms.Controls.vToolTip vToolTip1;
        private Panel panel1;
        private VIBlend.WinForms.Controls.vGroupBox vGroupBox1;
        private VIBlend.WinForms.Controls.vCheckBox vCheckBox1;
        private VIBlend.WinForms.Controls.vTrackBar vTrackBar1;
        private VIBlend.WinForms.Controls.vTrackBar vTrackBar2;
        internal Label label1;
        internal Label label2;
        private VIBlend.WinForms.Controls.vGroupBox vGroupBox2;
        private CheckBox checkBox1;
        private PictureBox pictureBox1;
        private VIBlend.WinForms.Controls.vContextMenu phoner8ClickMenu;
        private MenuItem accountSettingToolStripMenuItem;
        private MenuItem volumeToolStripMenuItem;
        private MenuItem dNDToolStripMenuItem;
        private MenuItem BreakToolStripMenuItem;
        private MenuItem BreakRequestmenuItem;
        private MenuItem CancelRequestmenuItem;
        private MenuItem EndBreakmenuItem;
        private MenuItem menuItem1;
        private MenuItem RingtoneOnMenuItem;
        private MenuItem RingtoneOffmenuItem;
        private MenuItem ShowInTaskbarmenuItem;





    }
}