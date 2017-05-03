
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
#endregion

using System.ServiceModel.Security;
using DuoSoftware.DuoSoftPhone.Controllers.Service;

#region References

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DuoSoftware.DuoLogger;


#endregion

#region Content

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    


    public class AsynchronousSocketListener
    {
        public delegate void SocketMessageRecive(CallFunctions callFunction,string message);

        public  event SocketMessageRecive OnSocketMessageRecive;

        #region Local Variables

        public  ManualResetEvent allDone = new ManualResetEvent(false);
        private string DuoKey;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        
        public AsynchronousSocketListener(string ipAddress, int port, string securityToken, int tenantId, int companyId)
        {
            DuoKey = CommonDataHandler.GetEventInfo(securityToken, tenantId, companyId).SocketMsgKey;
            StartListening(IPAddress.Parse(ipAddress),port);
        }

       
        #endregion

        #region Interface Implementations

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Private Methods

        private  void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "Send", exception, Logger.LogLevel.Error);
            }
        }

        private  void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, string.Format("SendCallback-Sent {0} bytes to client.", bytesSent), Logger.LogLevel.Info);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "SendCallback", exception, Logger.LogLevel.Error);
            }
        }

        private void MessageRevice(string msg)
        {
            try
            {
                /*
                 * Message Format => DuoKey|Command|phoneNo|<EOF>
                 * MakeCall => Duokey|MakeCall|0000000000|<EOF> or Duokey|0|0000000000|<EOF>
                 * EndCall => Duokey|EndCall|0000000000|<EOF> or Duokey|1|0000000000|<EOF>
                 * HoldCall => Duokey|HoldCall|0000000000|<EOF> or Duokey|2|0000000000|<EOF>
                 * TransferCall = > Duokey|TransferCall|0000000000|<EOF> or Duokey|3|0000000000|<EOF>
                */

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "MessageRevice : "+msg, Logger.LogLevel.Info);
                var callInfo = msg.Split('|');
                if(callInfo.Length!=4)
                    throw new FieldAccessException("Invalid Call Information's.");
                if(!DuoKey.Equals(callInfo[0]))
                    throw new ExpiredSecurityTokenException("Invalid SecurityToken or Expired.");

                var callFunction = (CallFunctions)Enum.Parse(typeof(CallFunctions), callInfo[1]);  

                if (OnSocketMessageRecive != null)
                    OnSocketMessageRecive(callFunction,callInfo[2]);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "MessageRevice", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region protected Methods

        #endregion

        #region Public Methods

        public  void StartListening(IPAddress ipAddress, int port)
        {
            try
            {
                // Data buffer for incoming data.
                byte[] bytes = new Byte[1024];

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
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "Waiting for a connection...", Logger.LogLevel.Info);
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "continue...", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "StartListening", exception, Logger.LogLevel.Error);
            }
        }

        public  void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "AcceptCallback", exception, Logger.LogLevel.Error);
            }
        }

        public  void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, string.Format("Read {0} bytes from socket. \n Data : {1}", content.Length, content), Logger.LogLevel.Info);
                        MessageRevice(content);
                        
                        //// Echo the data back to the client.
                        //Send(handler, content);
                    }
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LoggerFiles.DuoLogger3, "ReadCallback", exception, Logger.LogLevel.Error);
            }
        }
    

        #endregion

        #endregion
    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
#endregion