using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentRemote
{
    public static class NetUtils
    {
        /// <summary>
        /// Convert integer to byte array where high byte is first 
        /// </summary>
        /// <param name="integer">Converted integer</param>
        /// <returns></returns>
        public static byte[] ToBigEndianBytes(int integer)
        {
            byte[] res;
            if (BitConverter.IsLittleEndian)
            {
                int intsize = sizeof(int);
                res = new byte[intsize];
                for (int i = 0; i < intsize; i++)
                {
                    res[i] = (byte)(integer >> (8 * (intsize - 1 - i)));
                }
            }
            else
                res = BitConverter.GetBytes(integer);
            return res;
        }

        /// <summary>
        /// Convert unsigned integer to byte array where high byte is first 
        /// </summary>
        /// <param name="integer">Converted unsigned integer</param>
        /// <returns></returns>
        public static byte[] ToBigEndianBytes(uint integer)
        {
            byte[] res;
            if (BitConverter.IsLittleEndian)
            {
                int intsize = sizeof(uint);
                res = new byte[intsize];
                for (int i = 0; i < intsize; i++)
                {
                    res[i] = (byte)(integer >> (8 * (intsize - 1 - i)));
                }
            }
            else
                res = BitConverter.GetBytes(integer);
            return res;
        }

        ///// <summary>
        ///// Convert char to byte array where high byte is first
        ///// </summary>
        ///// <param name="ch">Converted char</param>
        ///// <returns></returns>
        //public static byte[] ToBytes(char ch )
        //{
        //    int charSize = sizeof(char);
        //    byte[] res = new byte[charSize];
        //    for (int i = 0; i < charSize; i++)
        //    {
        //        res[i] = (byte)(ch >> (8 * (charSize - 1 - i)));
        //    }
        //    return res;
        //}

        ///// <summary>
        ///// Convert string to byte array where high byte is first
        ///// </summary>
        ///// <param name="str">Converted string</param>
        ///// <returns></returns>
        //public static byte[] ToBytes(string str)
        //{
        //    int strSize = sizeof(char) * str.Length;
        //    byte[] res = new byte[strSize];
        //    int pos = 0;
        //    int charSize = sizeof(char);

        //    foreach (char ch in str)
        //    {
        //        for(int i = 0; i < charSize; i++)
        //        {
        //            res[pos] = (byte)(ch >> (8 * (charSize - 1 - i)));
        //            pos++;
        //        }
        //    }
        //    return res;
        //}

        /// <summary>
        /// Convert big endian byte array to integer
        /// </summary>
        /// <param name="src">Converted big endian byte array</param>
        /// <returns></returns>
        public static int ToIntFromBigEndian (byte[] src)
        {
            if (BitConverter.IsLittleEndian)
            {
                int intSize = sizeof(int);
                byte[] temp = new byte[intSize];
                int minLength = src.Length < intSize ? src.Length : intSize;
                for (int i = 0; i < minLength; i++)
                {
                    temp[i] = src[minLength - i];
                }
                return BitConverter.ToInt32(temp, 0);
            }
            else return BitConverter.ToInt32(src, 0);
        }

        /// <summary>
        /// Convert part of byte array to int
        /// </summary>
        /// <param name="src">Source byte array</param>
        /// <param name="offset">Offset in byte array</param>
        /// <returns></returns>
        public static int ToIntFromBigEndian(byte[] src, int offset)
        {
            if (offset >= src.Length)
                throw new ArgumentException("NetUtils.ToIntFromBigEndian. Offset more than array size.");
            if (BitConverter.IsLittleEndian)
            {
                int intSize = sizeof(int);
                byte[] temp = new byte[intSize];
                int l = src.Length - offset;
                int minLength = l < intSize ? l : intSize;
                l = offset + minLength-1;
                for (int i = 0; i < minLength; i++)
                {
                    temp[i] = src[l - i];
                }
                return BitConverter.ToInt32(temp, 0);
            }
            else return BitConverter.ToInt32(src, offset);
        }
    }
}
