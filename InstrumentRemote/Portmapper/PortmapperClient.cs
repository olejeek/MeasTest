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
    class PortmapperClient
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
            if (!Client.Connected) Client.Connect();
            Client.Call(new RpcCallMessage(RpcProgram.Portmapper, 2, 
                (uint)PortmapperProcedure.GETPORT, arg.ToBytes()));
            RpcReplyMessage reply = Client.Recieve();
            return 0;
        }

        public uint GetPort (uint prog, uint progVers, TransportProtocol protocol)
        {
            Mapping map = new Mapping();
            return 0;
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
