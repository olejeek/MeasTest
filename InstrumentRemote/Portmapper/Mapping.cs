using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.Portmapper
{
    /// <summary>
    /// Struct mapping in rfc 1833.
    /// Argument to procedure SET, UNSET and GETPORT
    /// </summary>
    class Mapping
    {
        uint Program;
        uint Version;
        TransportProtocol Protocol;
        uint Port;
    }
}
