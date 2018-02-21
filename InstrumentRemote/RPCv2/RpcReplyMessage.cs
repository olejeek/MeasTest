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
        ReplyStatus Status = (ReplyStatus)(- 1);
        #region if (Status == ReplyStatus.MSG_SUCCESS)
        /// <summary>
        /// Accept State of Reply Message. Using only if Status = MSG_ACCEPTED
        /// </summary>
        ReplyAcceptState AcceptState = (ReplyAcceptState)(-1);

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
        ReplyRejectState RejectState = (ReplyRejectState)(-1);

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
        AuthenticationState AuthState = (AuthenticationState)(-1);
        #endregion
        #endregion
        #endregion


        public RpcReplyMessage(byte[] recieved)
        {
            Type = MessageType.REPLY;
            Status = (ReplyStatus)NetUtils.ToIntFromBigEndian(recieved);
            int pos = sizeof(int);
            switch (Status)
            {
                case ReplyStatus.MSG_ACCEPTED:
                    #region Accepted
                    ServerVerifier = new Authentication(recieved,pos);
                    pos += ServerVerifier.Size;
                    if (ServerVerifier.Flavor == AuthFlavor.AUTH_NONE)
                    {
                        AcceptState = (ReplyAcceptState)NetUtils.ToIntFromBigEndian(recieved, pos);
                        pos += sizeof(int);
                        switch (AcceptState)
                        {
                            case ReplyAcceptState.SUCCESS:
                                Buffer.BlockCopy(recieved, pos, result, 0, recieved.Length - pos);
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
                case ReplyStatus.MSG_DENIED:
                    #region Denied
                    RejectState = (ReplyRejectState)NetUtils.ToIntFromBigEndian(recieved, pos);
                    pos += sizeof(int);
                    switch (RejectState)
                    {
                        case ReplyRejectState.RPC_MISMATCH:
                            RPCvLow = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                            pos += sizeof(int);
                            RPCvHigh = (uint)NetUtils.ToIntFromBigEndian(recieved, pos);
                            break;
                        case ReplyRejectState.AUTH_ERROR:
                            AuthState = (AuthenticationState)NetUtils.ToIntFromBigEndian(recieved, pos);
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
    }
}
