using System;
using System.Security.Cryptography;

namespace Assets.Scripts.Common
{
	public static class CRandom
	{
	    private static readonly byte[] Buffer = new byte[1024];
	    private static int _bufferOffset = Buffer.Length;
	    private static readonly RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider();


		/// <summary>
	    /// Chance 0-100
	    /// </summary>
	    public static bool Chance(int chance)
	    {
	        return UnityEngine.Random.Range(0, 100) < chance;
	    }

        private static void FillBuffer()
        {
            CryptoProvider.GetBytes(Buffer);
            _bufferOffset = 0;
        }
	}
}
