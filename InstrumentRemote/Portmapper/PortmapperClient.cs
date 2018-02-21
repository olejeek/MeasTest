using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstrumentRemote.RPCv2;
using System.Net;
using System.Net.Sockets;

namespace InstrumentRemote.Portmapper
{
    public class PortmapperClient
    {
        RpcClient Client;
        IPEndPoint LocalEndPoint;
        IPEndPoint RemoteEndPoint;
        //Что-то типо RpcClient

        public PortmapperClient(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;
            Client = new RpcClient(localEndPoint, ProtocolType.Udp, remoteEndPoint);
            //Создание RpcClient
        }

        public void Null ()
        { }

        public bool Set (Mapping arg)
        {
            return false;
        }

        public bool Unset(Mapping arg)
        {
            
            return false;
        }

        public uint GetPort (Mapping arg)
        {
            RpcCallMessage getport = new RpcCallMessage(RpcProgram.Portmapper, 2,
                (uint)PortmapperProcedure.GETPORT, arg.ToBytes());
            if (!Client.Connected) Client.Connect();
            Client.Call(getport);
            RpcReplyMessage reply = Client.Recieve();
            if (reply.AcceptState == ReplyAcceptState.SUCCESS)
            {
                uint port = (uint)NetUtils.ToIntFromBigEndian(reply.Result);
                if (port == 0)
                    throw new Exception("Portmapper. Program " + arg.Program +
                        " version " + arg.Version + " not available to " +
                        arg.Protocol.ToString() + ".");
                else return port;
            }
            else
                throw new Exception("Portmapper. Wrong answer. " + reply.ToString() + ".");
        }

        public uint GetPort (uint prog, uint progVers, TransportProtocol protocol)
        {
            Mapping map = new Mapping(prog, progVers, protocol);
            return GetPort(map);
        }

        public LinkedList<Mapping> Dump ()
        {
            return null;
        }

        public CallitResult CallIt (CallitArgs args)
        {
            return null;
        }

        public void Close()
        {
            Client.Close();
        }
    }
}
