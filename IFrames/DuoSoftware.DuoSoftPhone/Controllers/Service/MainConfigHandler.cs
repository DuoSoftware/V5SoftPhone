using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using DuoSoftware.DuoSoftPhone.RefDuoMainConfigService;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    public struct IvrInfo
    {
        public string IvrName;
        public string IvrExtension;
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var c = (IvrInfo)obj;
            return ((IvrName == c.IvrName) && (IvrExtension == c.IvrExtension) );
        }
        public override int GetHashCode()
        {
            return 0;
        }
        public static bool operator ==(IvrInfo left, IvrInfo right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(IvrInfo left, IvrInfo right)
        {
            return !left.Equals(right);
        }
    }

    public delegate void MainConfigHandlerEvent(List<IvrInfo> ivrList);

    public class MainConfigHandler
    {
        public event MainConfigHandlerEvent OnGetIvrListCompleted;

        public void GetIvRlist(string securityToken)
        {
            try
            {
                var client = new SIPRegistrationClient();
                client.GetUserExtensionsCompleted += (o, e) =>
                {
                    try
                    {
                        var ivrList = e.Result.Select(a => new IvrInfo { IvrName = a.Name, IvrExtension = a.Extension }).ToList();
                        if (OnGetIvrListCompleted != null)
                            OnGetIvrListCompleted(ivrList);
                    }
                    catch (Exception exception)
                    {
                         Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetIVRlist-GetUserExtensionsCompleted", exception, Logger.LogLevel.Error);
                    }
                };
                client.GetUserExtensionsAsync(securityToken, ExtType.IVR);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetIVRlist", exception, Logger.LogLevel.Error);
                throw;
            }
        }
    }
}