using System;
using System.Reflection;

namespace Janrain.TestHarness
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TestSuiteAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute { }

    public class AssertionException : ApplicationException 
    { 
	public AssertionException(string message) : base(message) { }
    }

    public class TestTools
    {
	private TestTools () {}

	public static void Assert(bool assertion)
	{
	    Assert(assertion, "Assertion Error");
	}

	public static void Assert(bool assertion, string message)
	{
	    if ( !assertion )
		throw new AssertionException(message);
	}
    }
    
}
