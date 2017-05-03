using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class ScrollingTextControl : UserControl
    {
        private Timer MarqueeTimer = new Timer();
        private String CurrentText = "Scrolling text";
        private String OurText;

        public ScrollingTextControl()
        {
            InitializeComponent();
        }

        private string SetText(string val)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(val);
            for (int i = 0; i < _textLength; i++)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }

        //public int Interval
        //{
        //    get { return MarqueeTimer.Interval; }
        //    set { MarqueeTimer.Interval = value; }
        //}

        //public override String Text
        //{
        //    get { return OurText; }
        //    set
        //    {
        //        OurText = SetText(value);
        //        //OurText = value;
        //         CurrentText = OurText;
        //    }
        //}

        //public override String Text
        //{
        //    get { return OurText; }
        //    set
        //    {
        //        OurText = SetText(value);
        //        //OurText = value;
        //        CurrentText = OurText;
        //    }
        //}

        private int marqueeTimer = 100;

        [Description("Marquee text"), Category("Data")]
        public int MarqueeTimerInterval
        {
            get { return marqueeTimer; }
            set
            {
                marqueeTimer = value;
                MarqueeTimer.Interval = marqueeTimer;
                MarqueeTimer.Enabled = true;
                MarqueeTimer.Tick += new EventHandler(MarqueeUpdate);
            }
        }

        private int _textLength = 0;

        [Description("MarqueeTextLength"), Category("Data")]
        public int MarqueeTextLength
        {
            get { return _textLength; }
            set
            {
                _textLength = value;
                OurText = SetText(marqueeText);
                CurrentText = OurText;
            }
        }

        private string marqueeText = string.Empty;

        [Description("Marquee text"), Category("Data")]
        public string MarqueeText
        {
            get { return OurText; }
            set { if (value != null) marqueeText = value.Trim(); }
        }

        [Description("Marquee text"), Category("Data")]
        public Image MarqueeImage
        {
            get
            {
                return pictureBox1 != null ? pictureBox1.Image : null;
            }
            set { if (pictureBox1 != null) pictureBox1.Image = value; }
        }

        private void ScrollingTextControl_Paint(object sender, PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, CurrentText, Font, ClientRectangle, ForeColor, BackColor);
        }

        private void MarqueeUpdate(object sender, EventArgs e)
        {
            CurrentText = CurrentText.Substring(1) + CurrentText[0];
            Invalidate();
        }

        private void ScrollingTextControl_Load(object sender, EventArgs e)
        {
        }
    }
}