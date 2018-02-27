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
        /// State of Reply Message
        /// </summary>
        private ReplyState state = (ReplyState)(- 1);
        /// <summary>
        /// State of Reply Message
        /// </summary>
        public ReplyState State { get { return state; } }

        #region if (State == ReplyStatus.MSG_SUCCESS)

        /// <summary>
        /// Accept State of Reply Message. Using only if Status = MSG_ACCEPTED
        /// </summary>
        private ReplyAcceptState acceptState = (ReplyAcceptState)(-1);
        /// <summary>
        /// Accept State of Reply Message. Using only if Status = MSG_ACCEPTED
        /// </summary>
        public ReplyAcceptState AcceptState { get { return acceptState; } }

        #region if (AcceptState == ReplyAcceptState.SUCCESS)

        /// <summary>
        /// Procedure-specific results
        /// </summary>
        public byte[] Result { get; private set; }
        #endregion

        #region if (AcceptState == ReplyAcceptState.PROG_MISMATCH)

        /// <summary>
        /// Lowest supported version of Program. Using only if AcceptState = PROG_MISMATCH
        /// </summary>
        public uint PROGvLow { get; private set; }

        /// <summary>
        /// Highest supported version of Program. Using only if AcceptState = PROG_MISMATCH
        /// </summary>
        public uint PROGvHigh { get; private set; }
        #endregion

        #region if (AcceptState != (ReplyAcceptState.SUCCESS | ReplyAcceptState.PROG_MISMATCH))
        #endregion

        #endregion

        #region if (Status == ReplyStatus.MSG_DENIED)

        /// <summary>
        /// Reject State of Reply Message. Using only if Status = MSG_Denied
        /// </summary>
        private ReplyRejectState rejectState = (ReplyRejectState)(-1);
        /// <summary>
        /// Reject State of Reply Message. Using only if Status = MSG_Denied
        /// </summary>
        public ReplyRejectState RejectState { get { return rejectState; } }

        #region if (RejectState == ReplyRejectState.RPC_MISMATCH)

        /// <summary>
        /// Lowest supported version of RPC. Using only if RejectState = RPC_MISMATCH
        /// </summary>
        public uint RPCvLow { get; private set; }
        /// <summary>
        /// Highest supported version of RPC. Using only if RejectState = RPC_MISMATCH
        /// </summary>
        public uint RPCvHigh { get; private set; }
        #endregion

        #region if (RejectState == ReplyRejectState.AUTH_ERROR)

        /// <summary>
        /// Authentication state. Using only if RejectState = AUTH_ERROR
        /// </summary>
        AuthenticationState authState = (AuthenticationState)(-1);
        /// <summary>
        /// Authentication state. Using only if RejectState = AUTH_ERROR
        /// </summary>
        public AuthenticationState AuthState { get { return authState; } }
        #endregion
        #endregion
        #endregion


        public RpcReplyMessage(byte[] recieved)
        {
            Type = MessageType.REPLY;
            int pos = sizeof(int);
            state = (ReplyState)NetUtils.ToIntFromBigEndian(recieved, sizeof(int));
            pos += sizeof(int);
            switch (state)
            {
                case ReplyState.MSG_ACCEPTED:
                    #region Accepted
                    ServerVerifier = new Authentication(recieved,pos);
                    pos += ServerVerifier.Size;
                    if (ServerVerifier.Flavor == AuthFlavor.AUTH_NONE)
                    {
                        acceptState = (ReplyAcceptState)NetUtils.ToIntFromBigEndian(recieved, pos);
                        pos += sizeof(int);
                        switch (acceptState)
                        {
                            case ReplyAcceptState.SUCCESS:
                                Result = new byte[recieved.Length - pos];
                                Buffer.BlockCopy(recieved, pos, Result, 0, recieved.Length - pos);
                                break;
                            case ReplyAcceptState.PROG_MISMATCH:
                                PROGvLow = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                                pos += sizeof(int);
                                PROGvHigh = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                                break;
                            case ReplyAcceptState.PROG_UNAVAIL:
                            case ReplyAcceptState.PROC_UNAVAIL:
                            case ReplyAcceptState.GARBAGE_ARGS:
                            case ReplyAcceptState.SYSTEM_ERR:
                                break;
                            default:
                                throw new ArgumentException("RpcReplyMessage. Wrong Accept State.");
                        }
                    }
                    else
                        throw new ArgumentException("RPCv2. Library not support authentication.");
                    #endregion
                    break;
                case ReplyState.MSG_DENIED:
                    #region Denied
                    rejectState = (ReplyRejectState)NetUtils.ToIntFromBigEndian(recieved, pos);
                    pos += sizeof(int);
                    switch (rejectState)
                    {
                        case ReplyRejectState.RPC_MISMATCH:
                            RPCvLow = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                            pos += sizeof(int);
                            RPCvHigh = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                            break;
                        case ReplyRejectState.AUTH_ERROR:
                            authState = (AuthenticationState)NetUtils.ToIntFromBigEndian(recieved, pos);
                            break;
                        default:
                            throw new ArgumentException("RpcReplyMessage. Wrong Reject State.");
                    }
                    #endregion
                    break;
                default:
                    throw new ArgumentException("RpcReplyMessage. Wrong Message Status.");
            }
        }

        public override byte[] ToBytes()
        {
            throw new Exception("RpcReplyMessage. Function not supported.");
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(100);
            str.AppendFormat(base.ToString());
            str.AppendFormat(" State: " + state.ToString() + " - ");
            if (state == ReplyState.MSG_ACCEPTED)
            {
                if (acceptState == ReplyAcceptState.PROG_MISMATCH)
                    str.AppendFormat(acceptState.ToString() +
                            ". Min support program version: " +
                            PROGvLow + ". Max support program version: " + PROGvHigh + ". ");
                else
                    str.AppendFormat(acceptState.ToString() + ". ");
                str.AppendFormat(ServerVerifier.ToString());
            }
            else
            {
                if (rejectState == ReplyRejectState.RPC_MISMATCH)
                    str.AppendFormat(rejectState.ToString() + ". Min support RPC version:" +
                        RPCvLow + ". Max support RPC version: " + RPCvHigh + ".");
                else
                    str.AppendFormat(rejectState.ToString() + " - " + AuthState.ToString() + ".");
            }
            return str.ToString();
        }
    }
}
