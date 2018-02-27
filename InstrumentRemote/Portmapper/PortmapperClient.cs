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
            if (RemoteEndPoint.Port != Consts.PortmapperPort)
                throw new ArgumentException("Portmaper. Program using " + 
                    Consts.PortmapperPort + " port.");
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
            RpcCallMessage getport = new RpcCallMessage(RpcProgram.Portmapper, 
                Consts.PortmapperVersion, (uint)PortmapperProcedure.GETPORT, 
                arg.ToBytes());
            Dictionary<EndPoint, RpcReplyMessage> replies = Client.Call(getport);
            foreach (var reply in replies.Values)
            {
                if (reply.AcceptState == ReplyAcceptState.SUCCESS)
                {
                    uint port = (uint)NetUtils.ToIntFromBigEndian(reply.Result);
                    if (port == 0)
                        throw new Exception("Portmapper. Program " + arg.Program +
                            " version " + arg.Version + " not available to " +
                            arg.Protocol.ToString() + ".");
                    else return port;
                }
            }
            throw new Exception("Portmapper. No success answer or no answer.");

        }

        public uint GetPort (uint prog, uint progVers, TransportProtocol protocol)
        {
            return GetPort(new Mapping(prog, progVers, protocol));
        }

        public List<IPEndPoint> GetPortBroadcast(Mapping arg)
        {
            List<IPEndPoint> list = new List<IPEndPoint>();
            RpcCallMessage getport = new RpcCallMessage(RpcProgram.Portmapper, Consts.PortmapperVersion,
                (uint)PortmapperProcedure.GETPORT, arg.ToBytes());
            //if (!Client.Connected) Client.Connect();
            //Client.Call(getport);
            //Dictionary<EndPoint, RpcReplyMessage> reply = Client.RecieveBroadcast();
            Dictionary<EndPoint, RpcReplyMessage> reply = Client.Call(getport);
            foreach (var r in reply)
            {
                if (r.Value.AcceptState == ReplyAcceptState.SUCCESS)
                {
                    int port = NetUtils.ToIntFromBigEndian(r.Value.Result);
                    IPAddress ad = ((IPEndPoint)r.Key).Address;
                    if (port != 0) list.Add(new IPEndPoint(((IPEndPoint)r.Key).Address, port));
                }
            }
            
            return list;
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
