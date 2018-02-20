using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.RPCv2
{

    #region Enums
    /// <summary>
    /// Authentication Flavor. 
    /// </summary>
    public enum AuthFlavor
    {
        /// <summary>
        /// Authentication disabled
        /// </summary>
        AUTH_NONE = 0,
        AUTH_SYS = 1,
        AUTH_SHORT = 2,
        AUTH_DH = 3,
        RPCSEC_GSS = 6
        //and more to be defined
    }

    /// <summary>
    /// Type of message RPC
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Call remote procedure
        /// </summary>
        CALL = 0,
        /// <summary>
        /// Reply remote procedure
        /// </summary>
        REPLY = 1
    };

    /// <summary>
    /// Status of reply message RPC
    /// </summary>
    public enum ReplyStatus
    {
        /// <summary>
        /// Message accepted
        /// </summary>
        MSG_ACCEPTED = 0,
        /// <summary>
        /// Message denied
        /// </summary>
        MSG_DENIED = 1
    };

    /// <summary>
    /// Status of accepted reply message RPC
    /// </summary>
    public enum ReplyAcceptState
    {
        /// <summary>
        /// RPC executed successfully
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// Remote hasn`t exported program (Program not found)
        /// </summary>
        PROG_UNAVAIL = 1,
        /// <summary>
        /// Remote can`t support version # (Program version not supported)
        /// </summary>
        PROG_MISMATCH = 2,
        /// <summary>
        /// Program can`t (not) support procedure
        /// </summary>
        PROC_UNAVAIL = 3,
        /// <summary>
        /// Procedure can`t decode params
        /// </summary>
        GARBAGE_ARGS = 4,
        /// <summary>
        /// System error
        /// </summary>
        SYSTEM_ERR = 5
    }

    /// <summary>
    /// Status of denied reply message RPC
    /// </summary>
    public enum ReplyRejectState
    {
        /// <summary>
        /// Not supported RPC version (not RPCv2)
        /// </summary>
        RPC_MISMATCH = 0,
        /// <summary>
        /// remote can`t authenticate caller (authentication error)
        /// </summary>
        AUTH_ERROR = 1
    }

    /// <summary>
    /// Status of authentication message RPC
    /// </summary>
    public enum AuthenticationState
    {
        /// <summary>
        /// Authentication success
        /// </summary>
        AUTH_OK = 0,
        /// <summary>
        /// Bad Credential (seal broken)
        /// </summary>
        AUTH_BADCRED = 1,
        /// <summary>
        /// Client must begin new session
        /// </summary>
        AUTH_REJECTEDCRED = 2,
        /// <summary>
        /// Bad verifier (seal broken)
        /// </summary>
        AUTH_BADVERF = 3,
        /// <summary>
        /// Verifier expired or replayed
        /// </summary>
        AUTH_REJECTEDVERF = 4,
        /// <summary>
        /// Rejected for security reasons
        /// </summary>
        AUTH_TOOWEAK = 5,
        /// <summary>
        /// Bogus response verifier
        /// </summary>
        AUTH_INVALIDRESP = 6,
        /// <summary>
        /// Reason unknown
        /// </summary>
        AUTH_FAILED = 7,
        /// <summary>
        /// Kerberos generic error
        /// </summary>
        AUTH_KERB_GENERIC = 8,
        /// <summary>
        /// Time of credential expired
        /// </summary>
        AUTH_TIMEEXPIRE = 9,
        /// <summary>
        /// Problem with ticket file
        /// </summary>
        AUTH_TKT_FILE = 10,
        /// <summary>
        /// Can`t decode authenticator
        /// </summary>
        AUTH_DECODE = 11,
        /// <summary>
        /// Wrong net address in ticket
        /// </summary>
        AUTH_NET_ADDR = 12,
        /// <summary>
        /// No credentials for user
        /// </summary>
        RPCSEC_GSS_CREDPROBLEM = 13,
        /// <summary>
        /// Problem with context
        /// </summary>
        RPCSEC_GSS_CTXPROGLEM = 14
    }

    /// <summary>
    /// List of Program using RPC Protocol
    /// </summary>
    public enum RpcProgram
    {
        /// <summary>
        /// Portmapper
        /// </summary>
        Portmapper  = 100000,
        /// <summary>
        /// VXI-11 Core
        /// </summary>
        VXI11Core   = 395183
    }
    #endregion

    static class Consts
    {
        /// <summary>
        /// Current version of RPC Protocol
        /// </summary>
        public const uint RpcVersion = 2;
    }
}
