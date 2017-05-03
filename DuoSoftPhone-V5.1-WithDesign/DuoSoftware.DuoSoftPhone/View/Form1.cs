using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuoSoftware.DuoLogger;
using Infragistics.Win.UltraWinGrid;

namespace DuoSoftware.DuoSoftPhone.View
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        DataTable CallLog = new DataTable("Table");
        private void Form1_Load(object sender, EventArgs e)
        {
            DuoPhoneTilePanel.Size = new Size(262, 394);
           


            //Create a table that will contain three columns.
            

            //Create three columns that will hold sample data.
           DataColumn Directions = new DataColumn("Directions", typeof(Bitmap));
            DataColumn Duration = new DataColumn("Duration", typeof(double));
            DataColumn Id = new DataColumn("Id", typeof(Guid));
            DataColumn Time = new DataColumn("Time", typeof(string));
            DataColumn Number = new DataColumn("Number", typeof(string));

            //Add the three columns to the table.
            CallLog.Columns.AddRange(new DataColumn[] { Directions, Duration, Id, Time, Number });

            var row = CallLog.NewRow();
           row["Directions"] = (Bitmap) Properties.Resources.online;
           row["Duration"] = Math.Round(10.25, 2);
            row["Id"] = Guid.NewGuid();
            row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
            row["Number"] = "0773756692";


            CallLog.Rows.Add(row);

            row = CallLog.NewRow();
            row["Directions"] = (Bitmap)Properties.Resources.IncommingCall;
            row["Duration"] = Math.Round(10.2, 2);
            row["Id"] = Guid.NewGuid();
            row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
            row["Number"] = "123";


            CallLog.Rows.Add(row);
            row = CallLog.NewRow();
            row["Directions"] = (Bitmap)Properties.Resources.online;
            row["Duration"] = Math.Round(10.29, 2);
            row["Id"] = Guid.NewGuid();
            row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
            row["Number"] = "123";


            CallLog.Rows.Add(row); 

            DataSet set=new DataSet();
            set.Tables.Add(CallLog);

            //Add the table to the dataset.
            //GVCallLogs.DataSource = set;
            //GVCallLogs.DataMember = "Table";
            //GVCallLogs.DataBind();
        }

        private Guid currentCallLogId;
        private void AddOutgoingCallToCallLogs(string no)
        {
            try
            {
                lock (CallLog)
                {
                    var row = CallLog.NewRow();
                    row["Directions"] = (Bitmap)Properties.Resources.calloutgoing;
                    row["Duration"] = 0;
                    row["Id"] = currentCallLogId;
                    row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
                    row["CallStartTime"] = DateTime.Now;
                    row["Number"] = no;

                    CallLog.Rows.Add(row);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddOutgoingCallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddIncommingToCallLogs(string no)
        {
            try
            {
                lock (CallLog)
                {

                    var log = CallLog.Select().ToList();
                    if (log.Exists(r => r["Id"].Equals(currentCallLogId)))
                    {
                        CallLog.Rows.Remove(log.Find(r => r["Id"].Equals(currentCallLogId)));
                    }
                    var row = CallLog.NewRow();
                    row["Directions"] = (Bitmap)Properties.Resources.IncommingCall;
                    row["Duration"] = 0;
                    row["Id"] = currentCallLogId;
                    row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
                    row["CallStartTime"] = DateTime.Now;
                    row["Number"] = no;

                    CallLog.Rows.Add(row);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddIncommingToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void AddCallDurations()
        {
            try
            {
                var log = CallLog.Select().ToList().Find(r => r["Id"].Equals(currentCallLogId));
                log["Duration"] = Math.Round(DateTime.Now.Subtract(Convert.ToDateTime(log["CallStartTime"])).TotalSeconds, 2);//
                lock (CallLog)
                {
                    CallLog.Rows.Remove(log);
                    CallLog.Rows.Add(log);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddCallDurations", exception, Logger.LogLevel.Error);
            }
        }

        private void AddMiscallToCallLogs()
        {
            try
            {
                var log = CallLog.Select().ToList().Find(r => r["Id"].Equals(currentCallLogId));
                log["Directions"] = (Bitmap)Properties.Resources.MissCallCallLog;
                lock (CallLog)
                {
                    CallLog.Rows.Remove(log);
                    CallLog.Rows.Add(log);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "AddMiscallToCallLogs", exception, Logger.LogLevel.Error);
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            var row = CallLog.NewRow();
            row = CallLog.NewRow();
            row["Directions"] = (Bitmap)Properties.Resources.online;
            row["Duration"] = Math.Round(10.29, 2);
            row["Id"] = Guid.NewGuid();
            row["Time"] = DateTime.Now.ToString("MM : dd HH:mm:ss");
            row["Number"] = "123";


            CallLog.Rows.Add(row); 
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
           currentCallLogId = Guid.NewGuid();
            AddIncommingToCallLogs("1234567890");
        }

        private void ultraButton3_Click(object sender, EventArgs e)
        {
            currentCallLogId = Guid.NewGuid();
            AddOutgoingCallToCallLogs("9876543210");
        }

        private void ultraButton4_Click(object sender, EventArgs e)
        {
            AddMiscallToCallLogs();
        }

        private void ultraButton5_Click(object sender, EventArgs e)
        {
            AddCallDurations();
        }

        private void btnFreez_Click(object sender, EventArgs e)
        {

        }
    }
}
