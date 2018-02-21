using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote.RPCv2
{
    /// <summary>
    /// Authentication description class
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Max Size (Bytes) of Body
        /// </summary>
        const int BodyMaxSize = 400;

        /// <summary>
        ///Flavor of Authentication
        /// </summary>
        public AuthFlavor Flavor { get; private set; }

        /// <summary>
        /// Body of authentification
        /// </summary>
        byte[] Body;

        public int Size
        {
            get
            {
                return sizeof(int) * 2 + Body.Length;
            }
        }

        /// <summary>
        /// Base constructor, creating instance of a class without authentication
        /// </summary>
        public Authentication()
        {
            Flavor = AuthFlavor.AUTH_NONE;
            Body = new byte[4] { 0, 0, 0, 0 };
        }

        /// <summary>
        /// Create instance of a class with parameters
        /// </summary>
        /// <param name="flavor">Authentication Flavor</param>
        /// <param name="body">Opaque authentication information</param>
        public Authentication(AuthFlavor flavor, byte[] body)
        {
            Flavor = flavor;
            if (body.Length > 400)
                throw new ArgumentException("RPCv2. Authentication." + 
                    " Body of authentication information is too large.");
            else Body = body;
        }

        /// <summary>
        /// Restore class from byte array
        /// </summary>
        /// <param name="source">Source byte array</param>
        public Authentication(byte[] source, int offset)
        {
            if (source.Length > 2 * sizeof(int))
            {
                Flavor = (AuthFlavor)NetUtils.ToIntFromBigEndian(source, offset);
                int l = NetUtils.ToIntFromBigEndian(source, offset + sizeof(int));
                if (l > 399) throw new ArgumentException("Authentication. Error size of auth information.");
                Body = new byte[l];
                Buffer.BlockCopy(source, offset + sizeof(int) * 2, Body, 0, l);
            }
            
        }

        /// <summary>
        /// Convert authentication information to byte array
        /// </summary>
        /// <returns>Byte array to send</returns>
        public byte[] ToBytes()
        {
            List<byte> rez = new List<byte>(sizeof(int)*2 + Body.Length);
            //rez.AddRange(BitConverter.GetBytes((int)Flavor));
            rez.AddRange(NetUtils.ToBigEndianBytes((int)Flavor));
            rez.AddRange(NetUtils.ToBigEndianBytes(Body.Length));
            rez.AddRange(Body);
            return rez.ToArray();
        }
        public override string ToString()
        {
            return "Authentication: " + Flavor.ToString();
        }
    }
}
