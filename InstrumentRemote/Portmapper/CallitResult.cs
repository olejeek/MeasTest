using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.Portmapper
{
    /// <summary>
    /// Struct call_result in rfc 1833.
    /// Return value of procedure CALLIT
    /// </summary>
    public class CallitResult
    {
        uint Port;
        byte[] Answer;
    }
}
