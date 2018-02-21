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
            RpcSocket.Connect(RemoteEndPoint);
        }

        public void Connect()
        {
            RpcSocket.Connect(RemoteEndPoint);
        }
        public void Close()
        {
            RpcSocket.Close();
        }

        public void Call (RpcCallMessage callProcedure)
        {
            CallMessage = callProcedure;
            byte[] mes = callProcedure.ToBytes();
            byte[] finalmes = new byte[sizeof(uint) + mes.Length];
            Buffer.BlockCopy(NetUtils.ToBigEndianBytes(xid), 0, finalmes, 0, 4);
            Buffer.BlockCopy(mes, 0, finalmes, 4, mes.Length);
            switch (ConnectionType)
            {
                case ProtocolType.Tcp:
                    TcpCall(finalmes);
                    break;
                case ProtocolType.Udp:
                    UdpCall(finalmes);
                    break;
                default:
                    throw new Exception(ConnectionType.ToString() + 
                        " protocol have not realization of function Call().");
            }
        }
        private void UdpCall(byte[] mes)
        {
            RpcSocket.SendTo(mes, RemoteEndPoint);
        }
        private void TcpCall(byte[] mes)
        {
            if (RemoteEndPoint.Address == IPAddress.Broadcast)
                throw new Exception("RpcClient. TCP not supported broadcast messages.");
            if (!RpcSocket.Connected) RpcSocket.Connect(RemoteEndPoint);

            RpcSocket.Send(mes);
        }

        public RpcReplyMessage Recieve()
        {
            RpcReplyMessage rp;
            switch (ConnectionType)
            {
                case ProtocolType.Tcp:
                    rp = TcpReceive();
                    ReplyMessages.Add(RemoteEndPoint, rp);
                    return rp;
                case ProtocolType.Udp:
                    rp = UdpReceive();
                    ReplyMessages.Add(RemoteEndPoint, rp);
                    return rp;
                default:
                    throw new Exception(ConnectionType.ToString() +
                        " protocol have not realization of function Call().");
            }
        }
        private RpcReplyMessage TcpReceive()
        {
            if (RemoteEndPoint.Address == IPAddress.Broadcast)
                throw new Exception("RpcClient. TCP not supported broadcast messages.");
            if (!RpcSocket.Connected) RpcSocket.Connect(RemoteEndPoint);

            byte[] buff = new byte[1024];
            int recSize = RpcSocket.Receive(buff);
            if (!CheckXID(buff) && CheckReceive(buff)) return null;
            recSize -= sizeof(uint);
            byte[] nbuff = new byte[recSize];
            Buffer.BlockCopy(buff, sizeof(uint), nbuff, 0, recSize);
            return new RpcReplyMessage(nbuff);
        }
        private RpcReplyMessage UdpReceive()
        {
            if (RemoteEndPoint.Address == IPAddress.Broadcast)
                RpcSocket.Connect(new IPEndPoint(IPAddress.Any, 0));
            byte[] buff = new byte[1024];
            int recSize = RpcSocket.Receive(buff);
            if (!(CheckXID(buff) && CheckReceive(buff))) return null;
            recSize -= sizeof(uint);
            byte[] nbuff = new byte[recSize];
            Buffer.BlockCopy(buff, sizeof(uint), nbuff, 0, recSize);
            return new RpcReplyMessage(nbuff);
        }
        private bool CheckReceive(byte[] src)
        {
            MessageType type = (MessageType)NetUtils.ToIntFromBigEndian(src, sizeof(int));
            if (type == MessageType.REPLY) return true;
            else return false;
        }

        public Dictionary<EndPoint, RpcReplyMessage> RecieveBroadcast()
        {
            Dictionary<EndPoint, RpcReplyMessage> replies = new Dictionary<EndPoint, RpcReplyMessage>();
            bool waitReplies = true;
            EndPoint ep = new IPEndPoint(IPAddress.Any, RemoteEndPoint.Port);
            while (waitReplies)
            {
                try
                {
                    byte[] buff = new byte[1024];
                    int recSize = RpcSocket.ReceiveFrom(buff, ref ep);
                    if (!CheckXID(buff)) continue;
                    recSize -= sizeof(uint);
                    byte[] nbuff = new byte[recSize];
                    Buffer.BlockCopy(buff, sizeof(uint), nbuff, 0, recSize);
                    replies.Add(ep, new RpcReplyMessage(nbuff));
                }
                catch (SocketException)
                {
                    waitReplies = false;
                }
            }
            return replies;
        }

        private bool CheckXID(byte[] src)
        {
            int id = NetUtils.ToIntFromBigEndian(src, 0);
            if (xid == id) return true;
            return false;
        }

    }
}





