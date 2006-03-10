using System;

namespace Janrain.OpenId.Consumer
{
    public class FetchException : ApplicationException
    {
	public readonly FetchResponse response;

	public FetchException ( FetchResponse response, 
				string message ) : 
	    base ( message )
	{
	    this.response = response;
	}
    }
}
