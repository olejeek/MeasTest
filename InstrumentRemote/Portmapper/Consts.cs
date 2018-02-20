using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.Portmapper
{
    #region Enums
    /// <summary>
    /// Portmapper procedures
    /// </summary>
    enum PortmapperProcedure
    {
        /// <summary>
        /// No work procedure. (Do nothing?)
        /// </summary>
        NULL,
        /// <summary>
        /// Register program in portmapper.
        /// </summary>
        SET,
        /// <summary>
        /// Unregister program in portmapper.
        /// </summary>
        UNSET,
        /// <summary>
        /// Return the port number on which the program is awaiting call requests.
        /// </summary>
        GETPORT,
        /// <summary>
        /// Enumerates all entries in the portmappers`s database.
        /// </summary>
        DUMP,
        /// <summary>
        /// Call another remote procedure on the same 
        /// machine without knowing the remote procedure`s port number.
        /// </summary>
        CALLIT
    }

    /// <summary>
    /// Supported values for the "prot" field.
    /// </summary>
    enum TransportProtocol
    {
        /// <summary>
        /// TCP
        /// </summary>
        TCP = 6,
        /// <summary>
        /// UDP
        /// </summary>
        UDP = 17
    }
    #endregion

    static class Consts
    {
        #region Consts
        /// <summary>
        /// Portmapper program number in RPC Protocol
        /// </summary>
        const uint PortmapperId = 100000;

        /// <summary>
        /// Portmapper port number
        /// </summary>
        const uint PortmapperPort = 111;
        #endregion
    }
}
