using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.RPCv2
{
    /// <summary>
    /// RPC Call Message Class
    /// </summary>
    public class RpcCallMessage : RpcMessage
    {
        #region Structure
        /// <summary>
        /// Version of RPC Protocol
        /// </summary>
        uint RpcVersion;

        /// <summary>
        /// Number of Program (может быть имеет смысл заменить uint на enum)
        /// </summary>
        RpcProgram Program;

        /// <summary>
        /// Program Version
        /// </summary>
        uint ProgramVersion;

        /// <summary>
        /// Number of Procedure
        /// </summary>
        uint Procedure;

        /// <summary>
        /// Information about Credentials of Authentication
        /// </summary>
        Authentication Credentials;

        /// <summary>
        /// Information about Verifier of Authentication
        /// </summary>
        Authentication Verifier;

        /// <summary>
        /// Calling Procedure Parameters
        /// </summary>
        byte[] ProcedureParams;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize new exemplar of RPC Call Message class
        /// </summary>
        /// <param name="prog">Program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="cred">Credentials</param>
        /// <param name="verif">Verifiers</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage (RpcProgram prog, uint progVers, uint proc, 
            Authentication cred, Authentication verif, byte[] procParams)
        {
            Type = MessageType.CALL;
            RpcVersion = Consts.RpcVersion;
            Program = prog;
            ProgramVersion = progVers;
            Procedure = proc;
            Credentials = cred;
            Verifier = verif;
            ProcedureParams = procParams;
        }

        /// <summary>
        /// Initialize new exemplar of RPC Call Message class
        /// </summary>
        /// <param name="prog">Number of program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="cred">Credentials</param>
        /// <param name="verif">Verifiers</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage(uint prog, uint progVers, uint proc,
           Authentication cred, Authentication verif, byte[] procParams) :
            this ((RpcProgram) prog, progVers, proc, cred, verif, procParams)
        { }

        /// <summary>
        /// Initialize new exemplar of RPC Call Message class
        /// </summary>
        /// <param name="prog">Program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="auth">Authentication information (both to credentials and verifiers)</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage(RpcProgram prog, uint progVers, uint proc,
            Authentication auth, byte[] procParams) :
            this(prog, progVers, proc, auth, auth, procParams)
        { }

        /// <summary>
        /// Initialize new exemplar of RPC Call Message class
        /// </summary>
        /// <param name="prog">Number of program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="auth">Authentication information (both to credentials and verifiers)</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage (uint prog, uint progVers, uint proc, 
            Authentication auth, byte[] procParams): 
            this(prog, progVers, proc, auth, auth, procParams)
        { }

        /// <summary>
        /// Initialize new exemplar of RPC Call Message class without authentication.
        /// </summary>
        /// <param name="prog">Number of program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage(uint prog, uint progVers, uint proc, byte[] procParams) :
            this(prog, progVers, proc, new Authentication(), procParams)
        { }

        /// <summary>
        /// Initialize new exemplar of RPC Call Message class without authentication.
        /// </summary>
        /// <param name="prog">Program using RPC protocol</param>
        /// <param name="progVers">Verion of program using RPC protocol</param>
        /// <param name="proc">Procedure number of program</param>
        /// <param name="procParams">Procedure parameters</param>
        public RpcCallMessage(RpcProgram prog, uint progVers, uint proc, byte[] procParams) :
            this(prog, progVers, proc, new Authentication(), procParams)
        { }

        /// <summary>
        /// Restore message from receive message
        /// </summary>
        /// <param name="source">Receiving message</param>
        //public RpcCallMessage (byte[] source)
        //{
        //    this.Type = MessageType.CALL;
        //    throw new Exception("RpcCallMessage. Restore call message from byte[] not supported.");
        //}
        #endregion

        #region Methods
        public override byte[] ToBytes()
        {
            List<byte> mes = new List<byte>(1000);

            //mes.AddRange(BitConverter.GetBytes((int)Type));
            mes.AddRange(NetUtils.ToBigEndianBytes((int)Type));
            //mes.AddRange(BitConverter.GetBytes(Consts.RpcVersion));
            mes.AddRange(NetUtils.ToBigEndianBytes(Consts.RpcVersion));
            //mes.AddRange(BitConverter.GetBytes((int)Program));
            mes.AddRange(NetUtils.ToBigEndianBytes((int)Program));
            //mes.AddRange(BitConverter.GetBytes(ProgramVersion));
            mes.AddRange(NetUtils.ToBigEndianBytes(ProgramVersion));
            //mes.AddRange(BitConverter.GetBytes(Procedure));
            mes.AddRange(NetUtils.ToBigEndianBytes(Procedure));
            mes.AddRange(Credentials.ToBytes());
            mes.AddRange(Verifier.ToBytes());
            mes.AddRange(ProcedureParams);
            return mes.ToArray();
        }
        #endregion

    }
}
