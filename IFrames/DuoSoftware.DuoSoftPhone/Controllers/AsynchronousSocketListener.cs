#region About

/*
Object				:
Purpose             :
Developed By		: Rajinda waruna
Developed On		: Oct 29, 2010
Modified By		    :
Last Updated 		:
Notes				:
*/

#endregion About

using System.ServiceModel.Security;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;

#region References

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DuoSoftware.DuoTools.DuoLogger;

#endregion References

#region Content

namespace DuoSoftware.DuoSoftPhone.Controllers
{

    public class AsynchronousSocketListener
    {


        public event SocketMessage OnRecive;

        #region Local Variables

        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        private readonly string _duoKey;

        #endregion Local Variables

        #region Properties

        #endregion Properties

        #region Constructors

        public AsynchronousSocketListener(string ipAddress, int port, string securityToken, int tenantId, int companyId)
        {
            _duoKey = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId).SocketMsgKey;
            IPAddress ip;
            IPAddress.TryParse(ipAddress, out ip);
            StartListening(ip, port);
        }

        #endregion Constructors

        #region Interface Implementations

        #endregion Interface Implementations

        #region Events

        #endregion Events

        #region Methods

        #region Private Methods

        private void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    SendCallback, handler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "Send", exception, Logger.LogLevel.Error);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            var logger = Logger.Instance;
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                logger.LogMessage(Logger.LogAppender.DuoLogger3, string.Format("SendCallback-Sent {0} bytes to client.", bytesSent), Logger.LogLevel.Info);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                //test
                Send(null,null);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger3, "SendCallback", exception, Logger.LogLevel.Error);
            }
        }

        private void MessageRevice(string msg)
        {
            var logger = Logger.Instance;
            try
            {
                /*
                 * Message Format => DuoKey|Command|phoneNo|<EOF>
                 * MakeCall => Duokey|MakeCall|0000000000|<EOF> or Duokey|0|0000000000|<EOF>
                 * EndCall => Duokey|EndCall|0000000000|<EOF> or Duokey|1|0000000000|<EOF>
                 * HoldCall => Duokey|HoldCall|0000000000|<EOF> or Duokey|2|0000000000|<EOF>
                 * TransferCall = > Duokey|TransferCall|0000000000|<EOF> or Duokey|3|0000000000|<EOF>
                */

                logger.LogMessage(Logger.LogAppender.DuoLogger3, "MessageRevice : " + msg, Logger.LogLevel.Info);
                var callInfo = msg.Split('|');
                if (callInfo.Length != 4)
                    throw new FieldAccessException("Invalid Call Information's.");
                if (!_duoKey.Equals(callInfo[0]))
                    throw new ExpiredSecurityTokenException("Invalid SecurityToken or Expired.");

                var callFunction = (CallFunction)Enum.Parse(typeof(CallFunction), callInfo[1]);

                if (OnRecive != null)
                    OnRecive(callFunction, callInfo[2]);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger3, "MessageRevice", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Private Methods

        #region protected Methods

        #endregion protected Methods

        #region Public Methods

        private void StartListening(IPAddress ipAddress, int port)
        {
            var logger = Logger.Instance;
            try
            {
                // Data buffer for incoming data.

                // Establish the local endpoint for the socket.
                // The DNS name of the computer
                // running the listener is "host.contoso.com".
                //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);

                    while (true)
                    {
                        // Set the event to nonsignaled state.
                        _allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        logger.LogMessage(Logger.LogAppender.DuoLogger3, "Waiting for a connection...", Logger.LogLevel.Info);
                        listener.BeginAccept(
                            AcceptCallback,
                            listener);

                        // Wait until a connection is made before continuing.
                        _allDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    logger.LogMessage(Logger.LogAppender.DuoLogger3, "StartListening1", e, Logger.LogLevel.Error);
                }

                logger.LogMessage(Logger.LogAppender.DuoLogger3, "continue...", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger3, "StartListening", exception, Logger.LogLevel.Error);
                throw;
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.
                if (_allDone != null) _allDone.Set();

                // Get the socket that handles the client request.
                if (ar == null) return;
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject {WorkSocket = handler};
                handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,ReadCallback, state);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "AcceptCallback", exception, Logger.LogLevel.Error);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var logger = Logger.Instance;
            try
            {
                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                if (ar == null) return;
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.WorkSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.Sb.Append(Encoding.ASCII.GetString(
                        state.Buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.
                    var content = state.Sb.ToString();
                    if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.
                        logger.LogMessage(Logger.LogAppender.DuoLogger3, string.Format("Read {0} bytes from socket. \n Data : {1}", content.Length, content), Logger.LogLevel.Info);
                        MessageRevice(content);

                        //// Echo the data back to the client.
                        //Send(handler, content);
                    }
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                            ReadCallback, state);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogMessage(Logger.LogAppender.DuoLogger3, "ReadCallback", exception, Logger.LogLevel.Error);
            }
        }

        #endregion Public Methods

        #endregion Methods
    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket WorkSocket;

        // Size of receive buffer.
        static public readonly int BufferSize = 1024;

        // Receive buffer.
        public readonly byte[] Buffer = new byte[BufferSize];

        // Received data string.
        public readonly StringBuilder Sb = new StringBuilder();
    }
}

#endregion Content