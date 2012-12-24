using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using libWyvernzora.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace libWyvernzora.Network
{
    /// <summary>
    /// Extended TCP Client Wrapper Class
    /// </summary>
    public class ExTcpClient : IDisposable
    {
        // Constants
        private const UInt16 SENDING_DATA = 0xD501;
        private const UInt16 SENDING_REPLY = 0xD502;
        private const UInt16 SWITCH_ROLES = 0xD503;
        private const UInt16 ERROR = 0xD5FF;


        // Internal Data Types
        /// <summary>
        /// Represents mode of the ExTcpClient
        /// </summary>
        public enum ClientMode
        {
            /// <summary>
            /// ExTcpClient in Active Mode actively sends data and waits for reply.
            /// Usually used on the client side of Server/Client architecture.
            /// </summary>
            Active,
            /// <summary>
            /// ExTcpClient waits for incoming data without actively sending data.
            /// Usually used on the server side of the Server/Client architecture.
            /// </summary>
            Passive
        }

        public sealed class DataReceivedEventArgs : EventArgs
        {
            public Byte[] Data { get; set; }
        }

        // Fields
        private TcpClient client;
        private Thread passiveWorker;
        private NetworkStream networkStream;
        private MemoryStream sendObjectBuffer;
        private ClientMode clientMode = ClientMode.Active;
        private Boolean initialized = false;

        // Properties
        public TcpClient Client
        { get { return client; } }
        
        public ClientMode Mode
        { get { return clientMode; } }

        public Int32 SendTimeout
        { get { return client.SendTimeout; } }

        public Int32 ReceiveTimeout
        { get { return client.ReceiveTimeout; } }

        // Constructors
        public ExTcpClient() { client = new TcpClient(); }
        public ExTcpClient(TcpClient client)
        {
            this.client = client;
            Initialize();
        }
        public ExTcpClient(AddressFamily addressFamily)
        {
            client = new TcpClient(addressFamily); 
            Initialize();
        }
        public ExTcpClient(IPEndPoint endpoint)
        {
            client = new TcpClient(endpoint);
            Initialize();
        }
        public ExTcpClient(String host, Int32 port)
        {
            client = new TcpClient(host, port);
            Initialize();
        }

        // Initialization & Connection
        /// <summary>
        /// Initializes ExTcpClient object after it is connected to remote host
        /// </summary>
        private void Initialize()
        {
            if (initialized) return;
            sendObjectBuffer = new MemoryStream();
            networkStream = client.GetStream();
            initialized = true;
        }
        /// <summary>
        /// Connects the client to a remote TCP host using the specified remote network endpoint.
        /// </summary>
        /// <param name="endpoint">The IPEndPoint to which you intend to connect.</param>
        public void Connect(IPEndPoint endpoint)
        {
            client.Connect(endpoint);
            Initialize();
        }
        /// <summary>
        /// Connects the client to a remote TCP host using the specified IP address and port number.
        /// </summary>
        /// <param name="address">The IPAddress of the host to which you intend to connect.</param>
        /// <param name="port">The port number to which you intend to connect.</param>
        public void Connect(IPAddress address, Int32 port)
        {
            client.Connect(address, port);
            Initialize();
        }
        /// <summary>
        /// Connects the client to a remote TCP host using the specified IP addresses and port number.
        /// </summary>
        /// <param name="addresses">The IPAddress array of the host to which you intend to connect.</param>
        /// <param name="port">The port number to which you intend to connect.</param>
        public void Connect(IPAddress[] addresses, Int32 port)
        {
            client.Connect(addresses, port);
            Initialize();
        }
        /// <summary>
        /// Connects the client to the specified port on the specified host.
        /// </summary>
        /// <param name="host">The DNS name of the remote host to which you intend to connect.</param>
        /// <param name="port">The port number of the remote host to which you intend to connect.</param>
        public void Connect(String host, Int32 port)
        {
            client.Connect(host, port);
            Initialize();
        }


        // Send Data
        public void Send(Byte[] payload)
        {
            networkStream.Write(SENDING_DATA);
            networkStream.Write(payload.Length);
            networkStream.Write(payload, 0, payload.Length);
        }
        // Send String
        public void Send(String payload, Encoding enc)
        {
            Byte[] data = enc.GetBytes(payload);
            Send(data);
        }
        // Send Object
        public void Send<T>(T payload, BinaryFormatter formatter)
        {
            sendObjectBuffer.SetLength(0);
            formatter.Serialize(sendObjectBuffer, payload);
            Send(sendObjectBuffer.ToArray());
        }

        // Active Mode Constructs
        /// <summary>
        /// Sends binary data expecting remote end to reply data.
        /// </summary>
        /// <param name="payload">Data to send</param>
        /// <returns>Reply</returns>
        public Byte[] SendExpectData(Byte[] payload)
        {
            Send(payload);

            // Wait for reply
            UInt16 ctrlCode = networkStream.ReadUInt16();
            if (ctrlCode != SENDING_REPLY)
                throw new Exception("ExTcpClient received an unexpected control code!");
            Int32 replySize = networkStream.ReadInt32();
            while (client.Available < replySize) ; // Wait for reply data
            Byte[] replyPayload = networkStream.ReadBytes(replySize);

            return replyPayload;
        }
        /// <summary>
        /// Sends binary data expecting remote end to reply success/failure.
        /// </summary>
        /// <param name="payload">Data to send</param>
        public void SendExpectSuccess(Byte[] payload)
        {
            // Send data
            Send(payload);

            // Wait for reply
            UInt16 ctrlCode = networkStream.ReadUInt16();
            if (ctrlCode != SENDING_REPLY)
                throw new Exception("ExTcpClient did not receive success reply!");
        }

        // Passive Mode Constructs
        private EventHandler<DataReceivedEventArgs> _onReceiveData;
        public event EventHandler<DataReceivedEventArgs> PassiveReceivedData
        { add { _onReceiveData += value; } remove { _onReceiveData -= value; } }
        private void OnReceivedData(Byte[] data)
        {
            if (_onReceiveData != null)
            {
                _onReceiveData(this, new DataReceivedEventArgs() { Data = data });
            }
        }

        private void PassiveModeWorkerLoop()
        {
            while (clientMode == ClientMode.Passive)
            {
                // wait for data to become available
                while (client.Available < 2) ;

                // control code
                UInt16 ctrl = networkStream.ReadUInt16();
                switch (ctrl)
                {
                    case SENDING_DATA:
                        while (client.Available < 4) ;
                        Int32 replySize = networkStream.ReadInt32();
                        while (client.Available < replySize) ; // Wait for reply data
                        Byte[] replyPayload = networkStream.ReadBytes(replySize);
                        OnReceivedData(replyPayload);
                        break;
                    case SWITCH_ROLES:
                        clientMode = ClientMode.Active;
                        return;
                }
            }
        }

        public void EnterPassiveMode()
        {
            clientMode = ClientMode.Passive;
            ThreadStart ts = new ThreadStart(PassiveModeWorkerLoop);
            passiveWorker = new Thread(ts);
            passiveWorker.Start();
        }

        #region IDisposable Members

        public void Dispose()
        {
            sendObjectBuffer.Dispose();
            client.Close();
        }

        #endregion

    }
}
