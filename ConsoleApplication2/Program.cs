using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

            Random r = new Random();
            int i = r.Next();
            Console.WriteLine(i);
            Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(i)));
            Console.WriteLine(BitConverter.ToString(NetUtils.ToBigEndianBytes(i)));
            Console.WriteLine("LittleEndian: " + BitConverter.IsLittleEndian.ToString());
            uint j = (uint)r.Next();
            Console.WriteLine(j);
            Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(j)));
            Console.WriteLine(BitConverter.ToString(NetUtils.ToBigEndianBytes(j)));


            RpcClient client = new RpcClient(new IPEndPoint(IPAddress.Parse("192.168.1.10"), 1990),
                ProtocolType.Udp, new IPEndPoint(IPAddress.Broadcast, 111));
            client.Connect();
            RpcCallMessage call = new RpcCallMessage(RpcProgram.Portmapper, 2, 3, new byte[] { 0x00, 0x06, 0x07, 0xAF,  0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00});
            client.Call(call);
            client.Close();

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
