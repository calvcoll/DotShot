using System;
using System.Security.Cryptography;

namespace DotShot
{
    public class MathHelper
    {
        public int GetRandInt(int min, int max)
        {
            Byte[] rndBytes = new Byte[10];
            RNGCryptoServiceProvider rndC = new RNGCryptoServiceProvider();
            rndC.GetBytes(rndBytes);
            int seed = BitConverter.ToInt32(rndBytes, 0);
            Random rand = new Random(seed);
            return rand.Next(min, max);
        }
    }
}
