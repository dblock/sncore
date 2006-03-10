using System;
using System.IO;
using System.Text;

using Mono.Security.Cryptography;

using Janrain.TestHarness;
using Janrain.OpenId;

namespace OpenIdTest.OpenId 
{
    [TestSuite]
    public class DiffieHellmanTestSuite
    {
	private static string Test1()
	{
	    DiffieHellman dh1 = CryptUtil.CreateDiffieHellman();
	    DiffieHellman dh2 = CryptUtil.CreateDiffieHellman();
	    string secret1 = CryptUtil.ToBase64String(
		dh1.DecryptKeyExchange(dh2.CreateKeyExchange()));
	    string secret2 = CryptUtil.ToBase64String(
		dh2.DecryptKeyExchange(dh1.CreateKeyExchange()));
	    TestTools.Assert(secret1 == secret2);
	    return secret1;
	}
	
	[Test]
	public void Test()
	{
	    string s1 = Test1();
	    string s2 = Test1();
	    TestTools.Assert(s1 != s2);
	}

	[Test]
	public void TestPublic()
	{
	    StreamReader sr = new StreamReader(@"Tests/Janrain.OpenId/dhpriv");
	    try
	    {
		String line;
		while ( (line = sr.ReadLine()) != null )
		{
		    string[] parts = line.Trim().Split(new char[]{' '});
		    byte[] x = Convert.FromBase64String(parts[0]);
		    DiffieHellman dh = new DiffieHellmanManaged(CryptUtil.DEFAULT_MOD, CryptUtil.DEFAULT_GEN, x);
		    string pub = CryptUtil.UnsignedToBase64(dh.CreateKeyExchange());
		    TestTools.Assert(pub == parts[1]);
		}
	    }
	    finally
	    {
		sr.Close();
	    }
	}
    }
}
