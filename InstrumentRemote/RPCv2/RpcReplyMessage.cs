using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.RPCv2
{
    /// <summary>
    /// RPC Reply Message Class
    /// </summary>
    public class RpcReplyMessage : RpcMessage
    {
        #region Structure
        /// <summary>
        /// Authentication information to verify server
        /// </summary>
        Authentication ServerVerifier;

        /// <summary>
        /// Status of Reply Message
        /// </summary>
        ReplyStatus Status;
        #region if (Status == ReplyStatus.MSG_SUCCESS)
        /// <summary>
        /// Accept State of Reply Message. Using only if Status = MSG_ACCEPTED
        /// </summary>
        ReplyAcceptState AcceptState;

        #region if (AcceptState == ReplyAcceptState.SUCCESS)

        /// <summary>
        /// Procedure-specific results
        /// </summary>
        byte[] result;
        #endregion

        #region if (AcceptState == ReplyAcceptState.PROG_MISMATCH)

        /// <summary>
        /// Lowest supported version of Program. Using only if AcceptState = PROG_MISMATCH
        /// </summary>
        uint PROGvLow;

        /// <summary>
        /// Highest supported version of Program. Using only if AcceptState = PROG_MISMATCH
        /// </summary>
        uint PROGvHigh;
        #endregion

        #region if (AcceptState != (ReplyAcceptState.SUCCESS | ReplyAcceptState.PROG_MISMATCH))
        #endregion

        #endregion

        #region if (Status == ReplyStatus.MSG_DENIED)

        /// <summary>
        /// Reject State of Reply Message. Using only if Status = MSG_Denied
        /// </summary>
        ReplyRejectState RejectState;

        #region if (RejectState == ReplyRejectState.RPC_MISMATCH)

        /// <summary>
        /// Lowest supported version of RPC. Using only if RejectState = RPC_MISMATCH
        /// </summary>
        uint RPCvLow;
        /// <summary>
        /// Highest supported version of RPC. Using only if RejectState = RPC_MISMATCH
        /// </summary>
        uint RPCvHigh;
        #endregion

        #region if (RejectState == ReplyRejectState.AUTH_ERROR)

        /// <summary>
        /// Authentication state. Using only if RejectState = AUTH_ERROR
        /// </summary>
        AuthenticationState AuthState;
        #endregion
        #endregion
        #endregion

        public RpcReplyMessage(byte[] recieved)
        {
            Type = (MessageType)0;
            result = recieved;
        }

        public override byte[] ToBytes()
        {
            return new byte[1];
        }
    }
}
