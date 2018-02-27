using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using InstrumentRemote.Portmapper;
using InstrumentRemote.RPCv2;
using InstrumentRemote;

namespace ConsoleApplication2
{
    class Program
    {
        static Socket conn;
        
        static void Main(string[] args)
        {
            Console.Read();

            PortmapperClient pm = new PortmapperClient(
                new IPEndPoint(IPAddress.Parse("172.20.53.7"), 1991),
                new IPEndPoint(IPAddress.Broadcast, 111));
            Mapping map = new Mapping(RpcProgram.VXI_11_Core, 1, TransportProtocol.TCP);
            List<IPEndPoint> instruments = new List<IPEndPoint>();
            uint port = 0;
            //try
            //{
                instruments = pm.GetPortBroadcast(map);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            //port = pm.GetPort((int)RpcProgram.VXI_11_Core, 1, TransportProtocol.TCP);
            pm.Close();
            Console.WriteLine(port);
            Console.WriteLine("List:");
            foreach( var inst in instruments)
            {
                Console.WriteLine(inst);
            }
            Console.WriteLine("-----");
            //RpcClient client = new RpcClient(new IPEndPoint(IPAddress.Parse("172.20.53.7"), 1990),
            //    ProtocolType.Udp, new IPEndPoint(IPAddress.Broadcast, 111));
            //client.Connect();
            //RpcCallMessage call = new RpcCallMessage(RpcProgram.Portmapper, 2, 3, new byte[] { 0x00, 0x06, 0x07, 0xAF,  0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00});
            //client.Call(call);
            //client.Close();

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }

        static bool Connect ()
        {

            //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.2.2"), 1005);
            //conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //conn.Connect(ip);
            for (int port = 4000; port < 65255; port++)
            {
                Console.CursorLeft = 0;
                Console.Write(port);
                try
                {
                    IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.2.2"), port);
                    conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    conn.Connect(ip);
                }
                catch
                {
                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine("{0} port closed.", port);
                    //Console.ResetColor();
                }
                if (conn.Connected)
                {
                    conn.Disconnect(false);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0} port opened.", port);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                //if (port % 10 == 0) Console.ReadLine();
            }

            if (conn.Connected) return true;
            else return false;
        }

        static void SendIdn ()
        {
            conn.Send(Encoding.ASCII.GetBytes("*IDN?\n"));
        }

        static string RecieveIdn()
        {
            byte[] data = new byte[1024];
            int recLen = conn.Receive(data);
            return Encoding.ASCII.GetString(data, 0, recLen);
        }
    }
}
