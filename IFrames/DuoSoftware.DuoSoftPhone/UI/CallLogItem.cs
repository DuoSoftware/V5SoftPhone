using System;
using System.Windows.Forms;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.UI
{
    public partial class CallLogItem : UserControl
    {
        public CallLogItem(string phoneNo, DateTime time, double duration)
        {
            InitializeComponent();
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, String.Format("{0}:{1}:{2}",phoneNo,time,duration), Logger.LogLevel.Info);
        }

        //  http://help.infragistics.com/Doc/WinForms/2012.1/CLR2.0/?page=WinExplorerBar_Create_a_Control_Container_Group.html
    }
}