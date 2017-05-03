using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoLogger;
using VIBlend.Utilities;

namespace DuoSoftware.DuoSoftPhone.Ui
{
    public partial class Form1 : Form
    {
        private DataTable _numberList;

        // Create the list to use as the custom source.  
        AutoCompleteStringCollection source = new AutoCompleteStringCollection();

        public Form1()
        {
            source.AddRange(new string[]
                    {
                        "January",
                        "February",
                        "March",
                        "April",
                        "May",
                        "June",
                        "July",
                        "August",
                        "September",
                        "October",
                        "November",
                        "December"
                    });
            InitializeComponent();
        }

        private TextBox textBox;
        private int X;
        private int Y;

        private void Form1_Load(object sender, EventArgs e)
        {


            applyStyle();
            // Create and initialize the text box. 
            //textBox = new TextBox
            //{
            //    AutoCompleteCustomSource = source,
            //    AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            //    AutoCompleteSource = AutoCompleteSource.CustomSource,
            //    Location = new Point(20, 20),
            //    Width = ClientRectangle.Width - 40,
            //    Visible = true
            //};

            //Controls.Add(textBox);


        }

        private void applyStyle()
        {

            //using vblend styles downloaded from http://www.viblend.com/products/net/windows-forms/controls/free-winforms-controls.aspx

            FillStyleGradientEx highlightGradient = new FillStyleGradientEx(Color.LightGreen, Color.GreenYellow, Color.Green, Color.DarkGreen, 90f, 0.2f, 0.3f);
            FillStyleGradientEx defaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx pressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx disabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme theme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            theme.StyleHighlight.FillStyle = highlightGradient;
            theme.StyleDisabled.FillStyle = disabledGradient;
            theme.StylePressed.FillStyle = pressedGradient;
            theme.StyleNormal.FillStyle = defaultGradient;
            //this.buttonAnswer.StyleKey = "answerStyle";
            //this.buttonAnswer.Theme = theme;
            //this.buttonAnswer.UseThemeTextColor = false;
            //this.buttonAnswer.HighlightTextColor = Color.White;
            //this.buttonAnswer.ForeColor = Color.White;
            //this.buttonAnswer.PressedTextColor = Color.White;

            vCircularProgressBar1.UseThemeBackground = true;
            
            vCircularProgressBar1.Theme = theme;
            //vButton1.Theme = theme;
            
            FillStyleGradientEx rejecthighlightGradient = new FillStyleGradientEx(Color.OrangeRed, Color.OrangeRed, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx rejectdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx rejectpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx rejectdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme rejecttheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            rejecttheme.StyleHighlight.FillStyle = rejecthighlightGradient;
            rejecttheme.StyleDisabled.FillStyle = rejectdisabledGradient;
            rejecttheme.StylePressed.FillStyle = rejectpressedGradient;
            rejecttheme.StyleNormal.FillStyle = rejectdefaultGradient;

            //vButton2.Theme = rejecttheme;

            FillStyleGradientEx numhighlightGradient = new FillStyleGradientEx(Color.Blue, Color.Blue, Color.Blue, Color.Blue, 90f, 0.2f, 0.3f);
            FillStyleGradientEx numdefaultGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx numpressedGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx numdisabledGradient = new FillStyleGradientEx(Color.Silver, Color.Silver, Color.Silver, Color.Silver, 90f, 0.4f, 0.5f);
            ControlTheme numtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            numtheme.StyleHighlight.FillStyle = numhighlightGradient;
            numtheme.StyleDisabled.FillStyle = numdisabledGradient;
            numtheme.StylePressed.FillStyle = numpressedGradient;
            numtheme.StyleNormal.FillStyle = numdefaultGradient;

            

            FillStyleGradientEx holdhighlightGradient = new FillStyleGradientEx(Color.OrangeRed, Color.OrangeRed, Color.DarkRed, Color.DarkRed, 90f, 0.2f, 0.3f);
            FillStyleGradientEx holddefaultGradient = new FillStyleGradientEx(Color.Gray, Color.LightGray, Color.Black, Color.Black, 90f, 0.3f, 0.5f);
            FillStyleGradientEx holdpressedGradient = new FillStyleGradientEx(Color.Gray, Color.LightGray, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            FillStyleGradientEx holddisabledGradient = new FillStyleGradientEx(Color.Black, Color.Black, Color.Black, Color.Black, 90f, 0.4f, 0.5f);
            ControlTheme holdtheme = ControlTheme.GetDefaultTheme(VIBLEND_THEME.STEEL);
            holdtheme.StyleHighlight.FillStyle = holdhighlightGradient;
            holdtheme.StyleDisabled.FillStyle = holddisabledGradient;
            holdtheme.StylePressed.FillStyle = holdpressedGradient;
            holdtheme.StyleNormal.FillStyle = holddefaultGradient;
            //this.buttonHold.StyleKey = "holdStyle";
            //this.buttonHold.Theme = holdtheme;
            //this.buttonHold.UseThemeTextColor = false;
            //this.buttonHold.HighlightTextColor = Color.White;
            //this.buttonHold.ForeColor = Color.White;
            //this.buttonHold.PressedTextColor = Color.White;

            //vButton4.Theme = holdtheme;
            //this.vButton4.StyleKey = "theme";
            //this.vButton4.Theme = theme;
            //this.vButton4.UseThemeTextColor = false;
            //this.vButton4.HighlightTextColor = Color.White;
            //this.vButton4.ForeColor = Color.White;
            //this.vButton4.PressedTextColor = Color.White;

            gbVolume.Theme = holdtheme;
            gbVolume.UseThemeBorderColor = true;
            gbVolume.UseThemeBorderColor = true;
            gbVolume.UseThemeTextColor = true;
            gbVolume.UseTitleBackColor = true;
            gbVolume.VIBlendTheme=VIBLEND_THEME.NERO;
            
            

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            applyStyle();
            vCircularProgressBar1.Hide();
            this.ContextMenu = phoner8ClickMenu;
            //vToolTip1.OwnerControl = TrackBarSpeaker;
            //vToolTip1.Text = string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value));
            
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            
            //vCircularProgressBar1.Show();
            vCircularProgressBar1.Start();

            PhoneStatus.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.online;
        }

        private SoundPlayer _wavPlayer;
        private void PlayRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "Start to play ring tone", Logger.LogLevel.Info);
                _wavPlayer = new SoundPlayer
                {
                    SoundLocation = @"I:\Duo Projects\Team Foundation[TFS]\DuoSoftware.DuoSoftPhone\bin\Debug\Ringtone.wav",
                    // @"C:\Users\Public\Music\Sample Music\ALBSlide.wav"
                };
                _wavPlayer.LoadCompleted += wavPlayer_LoadCompleted;
                _wavPlayer.LoadAsync();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone > End", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "PlayRingTone", exception, Logger.LogLevel.Error);

            }
        }

        private void StopRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Error);
                if (_wavPlayer == null) return;
                _wavPlayer.Stop();
                _wavPlayer.Dispose();
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone>End", Logger.LogLevel.Error);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "StopRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "playing ring tone", Logger.LogLevel.Error);
                ((SoundPlayer)sender).PlayLooping();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "wavPlayer_LoadCompleted", exception, Logger.LogLevel.Error);
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            vCircularProgressBar1.Stop();
            vCircularProgressBar1.Hide();

            PhoneStatus.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.offline;
        }

        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
           // vToolTip1.Text = string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value));
            volume.SetToolTip(TrackBarSpeaker, string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value)));
            //t.
            //volume.ToolTipTitle = string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value));
            //vLabel1.Text = string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value));
        }

        private int DisplayValue(int val)
        {
            var aa = (decimal.Divide(val, 65535) * 100);
            return (int)aa;
        }

        private void TrackBarSpeaker_MouseEnter(object sender, EventArgs e)
        {
            //volume.SetToolTip(TrackBarSpeaker, string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value)));
            vToolTip1.HeaderText = string.Format("{0}%", DisplayValue(TrackBarSpeaker.Value));
            vToolTip1.ShowTooltip(TrackBarSpeaker.Location,TrackBarSpeaker);
        }

        private void ShowInTaskbarmenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = !ShowInTaskbarmenuItem.Checked;
            ShowInTaskbarmenuItem.Checked = this.ShowInTaskbar;
        }
    }
}
