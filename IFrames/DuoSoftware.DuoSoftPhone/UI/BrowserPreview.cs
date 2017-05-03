//INSTANT C# NOTE: Formerly VB project-level imports:
using System.Drawing;
using System.Windows.Forms;

namespace WowserBrowser
{
    public class BrowserPreview : PictureBox
    {
        private bool myIsSelected = false;

        public bool Selected
        {
            get
            {
                return myIsSelected;
            }
            set
            {
                myIsSelected = value;
                this.Refresh();
            }
        }

        private void BrowserPreview_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (myIsSelected)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Goldenrod, 8F), this.DisplayRectangle);
            }
        }

        public BrowserPreview()
        {
            SubscribeToEvents();
        }

        //INSTANT C# NOTE: Converted event handler wireups:
        private bool EventsSubscribed = false;

        private void SubscribeToEvents()
        {
            if (EventsSubscribed)
                return;
            else
                EventsSubscribed = true;

            this.Paint += BrowserPreview_Paint;
        }
    }
}