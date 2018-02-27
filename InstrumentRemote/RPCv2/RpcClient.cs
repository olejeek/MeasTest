using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace InstrumentRemote.RPCv2
{
    public class RpcClient
    {
        Socket RpcSocket;
        IPEndPoint LocalEndPoint;
        IPEndPoint RemoteEndPoint;
        ProtocolType ConnectionType;
        RpcCallMessage CallMessage;
        Dictionary<EndPoint, RpcReplyMessage> ReplyMessages;
        uint xid;
        public int Timeout
        {
            get
            {
                return RpcSocket.ReceiveTimeout;
            }
            set
            {
                RpcSocket.ReceiveTimeout = value;
            }
        }
        public bool Connected
        {
            get
            {
                return RpcSocket.Connected;
            }
        }

        public RpcClient(IPEndPoint localEndPoint, ProtocolType connectionType, IPEndPoint remoteEndPoint)
        {
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;
            ReplyMessages = new Dictionary<EndPoint, RpcReplyMessage>();
            Random r = new Random(DateTime.Now.Millisecond);
            xid = (uint)r.Next(65536);
            ConnectionType = connectionType;
            if (RemoteEndPoint.Address == IPAddress.Broadcast && connectionType == ProtocolType.Tcp)
                throw new ArgumentException("Can not create Broadcast TCP connection.");
            switch (connectionType)
            {
                case ProtocolType.Tcp:
                    RpcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, connectionType);
                    break;
                case ProtocolType.Udp:
                    RpcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, connectionType);
                    break;
                default:
                    throw new ArgumentException(connectionType.ToString() + " not supported.");
            }
            if (RemoteEndPoint.Address == IPAddress.Broadcast)
            {
                RpcSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            }
            RpcSocket.ReceiveTimeout = 1000;
            RpcSocket.Bind(LocalEndPoint);
            if (ConnectionType == ProtocolType.Tcp) RpcSocket.Connect(RemoteEndPoint);
        }

        public void Close()
        {
            RpcSocket.Close();
        }

        public void CallWithputReply (RpcCallMessage callProcedure)
        {
            CallMessage = callProcedure;
            byte[] mes = callProcedure.ToBytes();
            byte[] finalmes = new byte[sizeof(uint) + mes.Length];
            Buffer.BlockCopy(NetUtils.ToBigEndianBytes(xid), 0, finalmes, 0, 4);
            Buffer.BlockCopy(mes, 0, finalmes, 4, mes.Length);
            switch (ConnectionType)
            {
                case ProtocolType.Tcp:
                    if (!RpcSocket.Connected) RpcSocket.Connect(RemoteEndPoint);
                    RpcSocket.Send(finalmes);
                    break;
                case ProtocolType.Udp:
                    RpcSocket.SendTo(finalmes, RemoteEndPoint);
                    break;
                default:
                    throw new Exception(ConnectionType.ToString() +
                        " protocol have not realization of function Call().");
            }
        }

        public Dictionary<EndPoint, RpcReplyMessage> Call(RpcCallMessage callProcedure)
        {
            Dictionary<EndPoint, RpcReplyMessage> replies = new Dictionary<EndPoint, RpcReplyMessage>();
            CallMessage = callProcedure;
            bool waitReplies = ConnectionType == ProtocolType.Tcp ? false : true;
            EndPoint ep = new IPEndPoint(IPAddress.Any, 111);
            byte[] mes = callProcedure.ToBytes();
            byte[] finalmes = new byte[sizeof(uint) + mes.Length];
            Buffer.BlockCopy(NetUtils.ToBigEndianBytes(xid), 0, finalmes, 0, 4);
            Buffer.BlockCopy(mes, 0, finalmes, 4, mes.Length);
            switch (ConnectionType)
            {
                case ProtocolType.Tcp:
                    if (!RpcSocket.Connected) RpcSocket.Connect(RemoteEndPoint);
                    RpcSocket.Send(finalmes);
                    break;
                case ProtocolType.Udp:
                    RpcSocket.SendTo(finalmes, RemoteEndPoint);
                    break;
                default:
                    throw new Exception(ConnectionType.ToString() +
                        " protocol have not realization of function Call().");
            }
            do
            {
                try
                {
                    byte[] buff = new byte[1024];
                    int recSize = RpcSocket.ReceiveFrom(buff, ref ep);
                    if (!CheckReply((IPEndPoint)ep, buff))
                        continue;
                    recSize -= sizeof(uint);
                    byte[] nbuff = new byte[recSize];
                    Buffer.BlockCopy(buff, sizeof(uint), nbuff, 0, recSize);
                    if (((IPEndPoint)ep).Port == RemoteEndPoint.Port)
                    replies.Add(ep, new RpcReplyMessage(nbuff));
                }
                catch (SocketException)
                {
                    waitReplies = false;
                }
            }
            while (waitReplies);
            return replies;
        }

        private bool CheckReply(IPEndPoint rep, byte[] src)
        {
            int id = NetUtils.ToIntFromBigEndian(src, 0);
            MessageType type = (MessageType)NetUtils.ToIntFromBigEndian(src, sizeof(int));
            if (xid == id && rep.Port == RemoteEndPoint.Port && type == MessageType.REPLY)
                return true;
            return false;
        }

    }
}





