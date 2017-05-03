using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WowserBrowser;

namespace DuoSoftware.DuoSoftPhone.UI
{
    /// <summary>
    /// para list
    /// </summary>
    public struct MyTabPara
    {
        public TabControl MainTabControl;
        public FlowLayoutPanel PreviewPanel;
        public ToolStripButton BackToolStripButton;
        public ToolStripButton ForwardToolStripButton;
        public ToolStripButton StopToolStripButton;
        public NumericUpDown PreviewSizeNumericUpDown;
        public ToolStripComboBox AddressToolStripComboBox;
        public ToolTip MainToolTip;
        public string UserName;
        public string PassWord;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            MyTabPara c = (MyTabPara)obj;
            return ((MainTabControl == c.MainTabControl) && (PreviewPanel == c.PreviewPanel) && (BackToolStripButton == c.BackToolStripButton));
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(MyTabPara left, MyTabPara right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(MyTabPara left, MyTabPara right)
        {
            return !left.Equals(right);
        }
    }

    public class MyTab
    {
        private StatusStrip _statusStripBottom;
        private ToolStripProgressBar _mainProgressBar;
        private ToolStripStatusLabel _mainToolStripStatusLabel;
        private ToolStripStatusLabel _secureToolStripStatusLabel;
        private string UserName;
        private string PassWord;
        ContextMenuStrip Phoner8ClickMenu;
        private Bitmap favicon(Uri url)
        {
            WebRequest request = (HttpWebRequest)WebRequest.Create("http://" + url.Host + "/favicon.ico");

            Bitmap bm = new Bitmap(string.Format("{0}\\HTMLPageHSBig.bmp", Application.StartupPath));

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var tmp = Image.FromStream(ms); // changed bitmap to image
                            bm = new Bitmap(tmp);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MyTab", exception,
                    Logger.LogLevel.Error);
            }

            return bm;
        }

        private void InitializeComponent(TabPage tab)
        {
            this._statusStripBottom = new System.Windows.Forms.StatusStrip();
            this._mainProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._secureToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusStripBottom.SuspendLayout();
            //this.SuspendLayout();
            //
            // statusStripBottom
            //
            this._statusStripBottom.BackColor = System.Drawing.Color.Transparent;
            this._statusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._secureToolStripStatusLabel,
            this._mainProgressBar,
            this._mainToolStripStatusLabel});
            this._statusStripBottom.Location = new System.Drawing.Point(0, 331);
            this._statusStripBottom.Name = "_statusStripBottom";
            this._statusStripBottom.Size = new System.Drawing.Size(899, 22);
            this._statusStripBottom.TabIndex = 0;
            this._statusStripBottom.Text = "statusStrip1";
            //
            // MainProgressBar
            //
            this._mainProgressBar.Name = "_mainProgressBar";
            this._mainProgressBar.Size = new System.Drawing.Size(100, 16);
            //
            // MainToolStripStatusLabel
            //
            this._mainToolStripStatusLabel.Name = "_mainToolStripStatusLabel";
            this._mainToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this._mainToolStripStatusLabel.Text = "Ready";
            //
            // SecureToolStripStatusLabel
            //
            this._secureToolStripStatusLabel.Image = global::DuoSoftware.DuoSoftPhone.Properties.Resources.ProtectFormHS;
            this._secureToolStripStatusLabel.Name = "_secureToolStripStatusLabel";
            this._secureToolStripStatusLabel.Size = new System.Drawing.Size(16, 17);
            //
            // Form1
            //

            this._statusStripBottom.ResumeLayout(false);
            this._statusStripBottom.PerformLayout();

            tab.Controls.Add(this._statusStripBottom);
        }

        private void SelectPic(BrowserPreview pic, FlowLayoutPanel PreviewPanel)
        {
            foreach (Control ctrl in PreviewPanel.Controls)
            {
                if (ctrl.GetType() == typeof(BrowserPreview))
                {
                    if (ctrl == pic)
                    {
                        ((BrowserPreview)ctrl).Selected = true;
                    }
                    else
                    {
                        ((BrowserPreview)ctrl).Selected = false;
                    }
                }
            }
        }

        private bool GetThumbCallback()
        {
            return false;
        }

        public TabPage AddTab(string url, ContextMenuStrip phoner8ClickMenu)
        {
            try
            {
                TabPage tab = new TabPage { BorderStyle = BorderStyle.Fixed3D, Text = "New", ImageIndex = 2 };
                InitializeComponent(tab);
                WebBrowser curBrowser = new WebBrowser();
                Phoner8ClickMenu = phoner8ClickMenu;
                if (!string.IsNullOrEmpty(url))
                {
                    curBrowser.Url = new Uri(url);
                }

                BrowserPreview pic = new BrowserPreview
                {
                    SizeMode = PictureBoxSizeMode.AutoSize,
                    BorderStyle = BorderStyle.Fixed3D
                };
                //pic.Image = new Bitmap(string.Format("{0}\\HTMLPageHSBig.bmp", Application.StartupPath));
                //.GetThumbnailImage(48, 48, New Image.GetThumbnailImageAbort(AddressOf GetThumbCallback), System.IntPtr.Zero)
                PreviewPanel.Controls.Add(pic);
                curBrowser.Tag = pic;

                SelectPic(pic, PreviewPanel);

                pic.Click += (o, e) => { MainTabControl.SelectedTab = (TabPage)((BrowserPreview)o).Tag; };

                curBrowser.Dock = DockStyle.Fill;
                curBrowser.CanGoBackChanged += (s, o) => { BackToolStripButton.Enabled = curBrowser.CanGoBack; };

                curBrowser.CanGoForwardChanged += (sender, e) =>
                {
                    ForwardToolStripButton.Enabled = curBrowser.CanGoForward;
                };

                curBrowser.DocumentCompleted += (sender, e) =>
                {
                    try
                    {
                        WebBrowser br = curBrowser;
                        
                        if (!br.IsBusy)
                        {
                            StopToolStripButton.Enabled = false;
                            _statusStripBottom.Visible = false;

                            Image img = new Bitmap(br.DisplayRectangle.Width, br.DisplayRectangle.Height);
                            Graphics gfxdst = Graphics.FromImage(img);
                            //Dim gfxsrc As Graphics = br.CreateGraphics

                            //BitBlt(gfxdst.GetHdc, 0, 0, br.DisplayRectangle.Width, br.DisplayRectangle.Height, gfxsrc.GetHdc, 0, 0, TernaryRasterOperations.SRCCOPY)

                            gfxdst.CopyFromScreen(
                                br.PointToScreen(new Point(br.DisplayRectangle.X, br.DisplayRectangle.Y)), new Point(0, 0),
                                br.DisplayRectangle.Size);

                            pic.Image = img.GetThumbnailImage(Convert.ToInt32(PreviewSizeNumericUpDown.Value),
                                Convert.ToInt32(PreviewSizeNumericUpDown.Value),
                                new Image.GetThumbnailImageAbort(GetThumbCallback), System.IntPtr.Zero);
                            pic.SizeMode = PictureBoxSizeMode.AutoSize;
                            pic.BorderStyle = BorderStyle.Fixed3D;
                            gfxdst.Dispose();
                            //gfxsrc.Dispose()
                            img.Dispose();
                        }

                        pic.Image = favicon(br.Url);
                        ImageList imgList = new ImageList();
                        imgList.Images.Add(pic.Image);
                        tab.ImageIndex = 0;
                        br.ContextMenuStrip = Phoner8ClickMenu;
                    }
                    catch (Exception ex)
                    {
                    }
                };
                curBrowser.DocumentTitleChanged += (sender, e) =>
                {
                    try
                    {
                        if (curBrowser.DocumentTitle.Length > 0)
                        {
                            MainTabControl.SelectedTab.Text = curBrowser.DocumentTitle;
                            //maintabcontrol.SelectedTab.ImageIndex =
                        }
                        else
                        {
                            if (AddressToolStripComboBox.Text.Length < 1)
                            {
                                MainTabControl.SelectedTab.Text = string.Format("Page {0}", MainTabControl.TabCount + 1);
                            }
                            else
                            {
                                MainTabControl.SelectedTab.Text = AddressToolStripComboBox.Text;
                            }
                        }
                        MainToolTip.SetToolTip(pic, MainTabControl.SelectedTab.Text);
                    }
                    catch (Exception ex)
                    {
                    }
                };
                curBrowser.EncryptionLevelChanged += (sender, e) =>
                {
                    switch (curBrowser.EncryptionLevel)
                    {
                        case WebBrowserEncryptionLevel.Bit128:
                        case WebBrowserEncryptionLevel.Bit40:
                        case WebBrowserEncryptionLevel.Bit56:
                        case WebBrowserEncryptionLevel.Fortezza:
                            _secureToolStripStatusLabel.Visible = true;
                            break;

                        default:
                            _secureToolStripStatusLabel.Visible = false;
                            break;
                    }
                };
                curBrowser.ProgressChanged += (sender, e) =>
                {
                    try
                    {
                        if (_mainProgressBar.Value <= e.MaximumProgress)
                        {
                            _mainProgressBar.Maximum = (System.Int32)e.MaximumProgress;
                            if (e.CurrentProgress >= _mainProgressBar.Minimum)
                            {
                                _mainProgressBar.Value = (System.Int32)e.CurrentProgress;
                            }
                            else
                            {
                                _mainProgressBar.Value = 0;
                            }
                        }
                        else
                        {
                            _mainProgressBar.Value = 0;
                            _mainProgressBar.Maximum = (System.Int32)e.MaximumProgress;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                };
                curBrowser.StatusTextChanged += (sender, e) =>
                {
                    if (!(curBrowser.StatusText == null))
                    {
                        _mainToolStripStatusLabel.Text = curBrowser.StatusText;
                    }
                    else
                    {
                        _mainToolStripStatusLabel.Text = "Done";
                    }
                };
                curBrowser.Navigating += (sender, e) =>
                {
                    StopToolStripButton.Enabled = true;
                    _statusStripBottom.Visible = true;
                };
                curBrowser.Navigated += (sender, e) =>
                {
                    if (curBrowser.Url != null)
                    {
                        AddressToolStripComboBox.Text = curBrowser.Url.ToString();
                    }
                };

                //tab.Text = String.Format("Page {0}", MainTabControl.TabCount)
                tab.Controls.Add(curBrowser);
                MainTabControl.TabPages.Insert(MainTabControl.TabCount - 1, tab);
                pic.Tag = tab;

                var authHdr = "Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName + ":" + PassWord)) + "\r\n";
                curBrowser.Navigate(url, null, null, authHdr);
                return tab;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MyTab", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        public MyTab(MyTabPara tabParams)
        {
            try
            {
                UserName = tabParams.UserName;
                PassWord = tabParams.PassWord;
                MainTabControl = tabParams.MainTabControl;
                PreviewPanel = tabParams.PreviewPanel;
                BackToolStripButton = tabParams.BackToolStripButton;

                ForwardToolStripButton = tabParams.ForwardToolStripButton;
                StopToolStripButton = tabParams.StopToolStripButton;
                PreviewSizeNumericUpDown = tabParams.PreviewSizeNumericUpDown;
                AddressToolStripComboBox = tabParams.AddressToolStripComboBox;
                MainToolTip = tabParams.MainToolTip;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MyTab", exception,
                    Logger.LogLevel.Error);
                throw;
            }
        }

        private TabControl MainTabControl;
        private FlowLayoutPanel PreviewPanel;
        private ToolStripButton BackToolStripButton;

        private ToolStripButton ForwardToolStripButton;
        private ToolStripButton StopToolStripButton;
        private NumericUpDown PreviewSizeNumericUpDown;
        private ToolStripComboBox AddressToolStripComboBox;
        private ToolTip MainToolTip;
    }
}