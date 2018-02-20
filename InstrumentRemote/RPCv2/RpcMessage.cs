﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.RPCv2
{
    /// <summary>
    /// Abstract Class of RPC Message
    /// </summary>
    public abstract class RpcMessage
    {
        /// <summary>
        /// Type of RPC Message
        /// </summary>
        public MessageType Type;

        /// <summary>
        /// Convert RPC Message to array of byte
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToBytes();

    }
}
