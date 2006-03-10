using System;
using System.Text;

using Janrain.TestHarness;
using Janrain.OpenId;

namespace OpenIdTest.OpenId 
{
    [TestSuite]
    public class HMACSHA1AssociationTestSuite
    {
	[Test]
	public void SerializeDeserialize()
	{
	    TimeSpan expiresIn = new TimeSpan(0, 0, 600);
	    string handle = "handle";
	    byte[] secret = ASCIIEncoding.ASCII.GetBytes("secret");
	    
	    Association assoc = new HMACSHA1Association(
	        handle, secret, expiresIn);

	    byte[] s = assoc.Serialize();
	    
	    Association assoc2 = Association.Deserialize(s);

	    TestTools.Assert(assoc.Equals(assoc2));
	    TestTools.Assert(assoc.GetHashCode() == assoc2.GetHashCode());
	}
    }
}
