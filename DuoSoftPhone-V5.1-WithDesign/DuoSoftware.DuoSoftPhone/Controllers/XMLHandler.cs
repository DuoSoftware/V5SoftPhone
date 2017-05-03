using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using DuoSoftware.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public class XMLHandler
    {
        private string filePath;

        public XMLHandler()
        {
            filePath = string.Format(@"C:\Users\{0}\AppData\Local\DuoSoftware", Environment.UserName);
        }

        public  Dictionary<string, object> ReadXML()
        {
            var initDictionary = new Dictionary<string, object>();

            try
            {
                
                if (!File.Exists(filePath))
                {
                   return null;
                }
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNodeList xnList = xmlDoc.SelectNodes("/SipSetting/Speakers");
                foreach (XmlNode xn in xnList)
                {
                    string SpkDeviceID = xn["SpkDeviceID"].InnerText;
                    string SpkDeviceName = xn["SpkDeviceName"].InnerText;
                    string SpkVolume = xn["SpkVolume"].InnerText;
                   
                    initDictionary.Add("SpkDeviceID", SpkDeviceID);
                    initDictionary.Add("SpkDeviceName", SpkDeviceName);
                    initDictionary.Add("SpkVolume", SpkVolume);
                }

                xnList = xmlDoc.SelectNodes("/SipSetting/Microphone");
                foreach (XmlNode xn in xnList)
                {
                    string MicDeviceID = xn["MicDeviceID"].InnerText;
                    string MicVolume = xn["MicVolume"].InnerText;
                    string MicDeviceName = xn["MicDeviceName"].InnerText;

                    initDictionary.Add("MicDeviceID", Int32.Parse(MicDeviceID));
                    initDictionary.Add("MicDeviceName", MicDeviceName);
                    initDictionary.Add("MicVolume", Int32.Parse(MicVolume));
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "ReadXML", exception, Logger.LogLevel.Error);
            }
            return initDictionary;
        }

        public void WriteXML(int micDeviceId, int micVolume, int spkDeviceId, string spkDeviceName, int spkVolume, string micDeviceName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("SipSetting");
                xmlDoc.AppendChild(rootNode);


                // sip 

                XmlNode PhoneNode = xmlDoc.CreateElement("Microphone");
                XmlNode localPortNode = xmlDoc.CreateElement("MicDeviceID");
                localPortNode.InnerText = micDeviceId.ToString();

                XmlNode licenceKeyNode = xmlDoc.CreateElement("MicDeviceName");
                licenceKeyNode.InnerText = micDeviceName;

                XmlNode serverPortNode = xmlDoc.CreateElement("MicVolume");
                serverPortNode.InnerText = micVolume.ToString();

                PhoneNode.AppendChild(localPortNode);
                PhoneNode.AppendChild(licenceKeyNode);
                PhoneNode.AppendChild(serverPortNode);
                
                rootNode.AppendChild(PhoneNode);

                // Speakers Info
                XmlNode userNode = xmlDoc.CreateElement("Speakers");
                XmlNode userNameNode = xmlDoc.CreateElement("SpkDeviceID");
                userNameNode.InnerText = spkDeviceId.ToString();

                XmlNode passwordNode = xmlDoc.CreateElement("SpkDeviceName");
                passwordNode.InnerText = spkDeviceName;

                XmlNode displayNameNode = xmlDoc.CreateElement("SpkVolume");
                displayNameNode.InnerText = spkVolume.ToString();

                userNode.AppendChild(userNameNode);
                userNode.AppendChild(passwordNode);
                userNode.AppendChild(displayNameNode);
               
                rootNode.AppendChild(userNode);
                
                SetPermissions(filePath);

                //FileStream F = new FileStream("test.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                xmlDoc.Save(filePath);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoDefault, "WriteXML", exception, Logger.LogLevel.Error);
            }
        }

        private static void SetPermissions(string dirPath)
        {
            DirectoryInfo info = new DirectoryInfo(dirPath);
            WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
            DirectorySecurity ds = info.GetAccessControl();
            ds.AddAccessRule(new FileSystemAccessRule(self.Name, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
            info.SetAccessControl(ds);
        }
    }


}
