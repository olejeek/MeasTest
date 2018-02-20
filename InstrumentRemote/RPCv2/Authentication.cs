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
        AuthFlavor Flavor;

        /// <summary>
        /// Body of authentification
        /// </summary>
        byte[] Body;

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
        public Authentication(byte[] source)
        {
            if (source.Length > 2 * sizeof(int))
            {
                Flavor = (AuthFlavor)BitConverter.ToInt32(source, 0);
                Body = new byte[source.Length - sizeof(int)];
                source.CopyTo(Body, sizeof(int));
            }
            
        }

        /// <summary>
        /// Convert authentication information to byte array
        /// </summary>
        /// <returns>Byte array to send</returns>
        public byte[] ToBytes()
        {
            //byte[] rez = new byte[sizeof(int) + Body.Length];
            List<byte> rez = new List<byte>(sizeof(int) + Body.Length);
            //rez.AddRange(BitConverter.GetBytes((int)Flavor));
            rez.AddRange(NetUtils.ToBigEndianBytes((int)Flavor));
            rez.AddRange(Body);
            return rez.ToArray();
        }
    }
}
