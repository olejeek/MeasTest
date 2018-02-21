using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstrumentRemote.RPCv2;

namespace InstrumentRemote.Portmapper
{
    /// <summary>
    /// Struct mapping in rfc 1833.
    /// Argument to procedure SET, UNSET and GETPORT
    /// </summary>
    public class Mapping
    {
        public RpcProgram Program { get; private set; }
        public uint Version { get; private set; }
        public TransportProtocol Protocol { get; private set; }
        public uint Port { get; private set; }

        public Mapping (RpcProgram program, uint version, TransportProtocol protocol)
        {
            Program = program;
            Version = version;
            Protocol = protocol;
            Port = 0;
        }

        public Mapping(uint program, uint version, TransportProtocol protocol) :
            this((RpcProgram)program, version, protocol)
        { }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>(4 * sizeof(int));
            bytes.AddRange(NetUtils.ToBigEndianBytes((uint)Program));
            bytes.AddRange(NetUtils.ToBigEndianBytes(Version));
            bytes.AddRange(NetUtils.ToBigEndianBytes((int)Protocol));
            bytes.AddRange(NetUtils.ToBigEndianBytes(Port));
            return bytes.ToArray();
        }
    }
}
