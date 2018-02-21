using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.Portmapper
{
    /// <summary>
    /// Struct call_args in rfc 1833.
    /// Argument to procedure CALLIT
    /// </summary>
    public class CallitArgs
    {
        uint Program;
        uint Version;
        uint Procedure;
        byte[] ProcedureParams;
    }
}
