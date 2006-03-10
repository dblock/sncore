using System;
using System.Security.Cryptography;
using System.Text;

using Mono.Security.Cryptography;


namespace Janrain.OpenId
{
    public class CryptUtil
    {
        static RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
        static ToBase64Transform base64Transform = new ToBase64Transform();
        static SHA1 sha1 = new SHA1CryptoServiceProvider();

        public static string ToBase64String(byte[] inputBytes)
        {
            StringBuilder sb = new StringBuilder();
            byte[] outputBytes = new byte[base64Transform.OutputBlockSize];
            
            // Initializie the offset size.
            int inputOffset = 0;
            
            // Iterate through inputBytes transforming by blockSize.
            int inputBlockSize = base64Transform.InputBlockSize;
            
            while(inputBytes.Length - inputOffset > inputBlockSize)
            {
                base64Transform.TransformBlock(inputBytes, inputOffset, inputBlockSize, outputBytes, 0);
                inputOffset += base64Transform.InputBlockSize;
                sb.Append(Encoding.ASCII.GetString(outputBytes, 0, base64Transform.OutputBlockSize));
            }

            // Transform the final block of data.
            outputBytes = base64Transform.TransformFinalBlock(inputBytes, inputOffset, inputBytes.Length - inputOffset);
            sb.Append(Encoding.ASCII.GetString(outputBytes, 0, outputBytes.Length));
            
            return sb.ToString();
        }

	private static byte[] EnsurePositive(byte[] inputBytes)
	{
	    // XXX : if len < 1 : throw error
	    if (inputBytes[0] > 127)
	    {
		byte[] temp = new byte[inputBytes.Length + 1];
		temp[0] = 0x00;
		inputBytes.CopyTo(temp, 1);
		inputBytes = temp;
	    }
	    return inputBytes;
	}

	public static string UnsignedToBase64(byte[] inputBytes)
	{
	    return ToBase64String(EnsurePositive(inputBytes));
	}
        
        public static void RandomSelection(byte[] tofill, byte[] choices)
        {
            // XXX: assert choices.Length < 257
            byte[] rand = new byte[1];
            UInt32 r;
            for (uint i = 0; i < tofill.Length; i++)
            {
                generator.GetBytes(rand);
                r = Convert.ToUInt32(rand[0]);
                tofill[i] = choices[r % choices.Length];
            }
        }

        public static byte[] DEFAULT_GEN = { 0x02 }; 
        
        public static byte[] DEFAULT_MOD = { 
            0x00, 0xDC, 0xF9, 0x3A, 0x0B, 0x88, 0x39, 0x72, 0xEC, 0x0E, 0x19,
	    0x98, 0x9A, 0xC5, 0xA2, 0xCE, 0x31, 0x0E, 0x1D, 0x37, 0x71, 0x7E,
	    0x8D, 0x95, 0x71, 0xBB, 0x76, 0x23, 0x73, 0x18, 0x66, 0xE6, 0x1E,
	    0xF7, 0x5A, 0x2E, 0x27, 0x89, 0x8B, 0x05, 0x7F, 0x98, 0x91, 0xC2,
	    0xE2, 0x7A, 0x63, 0x9C, 0x3F, 0x29, 0xB6, 0x08, 0x14, 0x58, 0x1C,
	    0xD3, 0xB2, 0xCA, 0x39, 0x86, 0xD2, 0x68, 0x37, 0x05, 0x57, 0x7D,
	    0x45, 0xC2, 0xE7, 0xE5, 0x2D, 0xC8, 0x1C, 0x7A, 0x17, 0x18, 0x76,
	    0xE5, 0xCE, 0xA7, 0x4B, 0x14, 0x48, 0xBF, 0xDF, 0xAF, 0x18, 0x82,
	    0x8E, 0xFD, 0x25, 0x19, 0xF1, 0x4E, 0x45, 0xE3, 0x82, 0x66, 0x34,
	    0xAF, 0x19, 0x49, 0xE5, 0xB5, 0x35, 0xCC, 0x82, 0x9A, 0x48, 0x3B,
	    0x8A, 0x76, 0x22, 0x3E, 0x5D, 0x49, 0x0A, 0x25, 0x7F, 0x05, 0xBD,
	    0xFF, 0x16, 0xF2, 0xFB, 0x22, 0xC5, 0x83, 0xAB 
	};

        public static DiffieHellman CreateDiffieHellman()
        {
            return new DiffieHellmanManaged(DEFAULT_MOD, DEFAULT_GEN, 1024);
        }
        
        public static byte[] SHA1XorSecret(DiffieHellman dh, byte[] keyEx, byte[] encMacKey)
        {
            byte[] dhShared = dh.DecryptKeyExchange(keyEx);
	    byte[] sha1DhShared = sha1.ComputeHash(EnsurePositive(dhShared));

            if (sha1DhShared.Length != encMacKey.Length)
                throw new ArgumentOutOfRangeException(String.Format("encMacKey's length is not 20 bytes: [{0}]", ToBase64String(encMacKey)));

	    byte[] secret = new byte[encMacKey.Length];
            for (uint i = 0; i < encMacKey.Length; i++)
	    {
                secret[i] = (byte) (encMacKey[i] ^ sha1DhShared[i]);
	    }

            return secret;
        }
    }
}
