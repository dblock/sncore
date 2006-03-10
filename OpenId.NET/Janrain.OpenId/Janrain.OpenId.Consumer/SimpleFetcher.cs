using System;
using System.IO;
using System.Net;


namespace Janrain.OpenId.Consumer
{
    public class SimpleFetcher : Fetcher
    {
            
#region Fetcher Members
	public override FetchResponse Get ( Uri uri, 
					    uint maxRead )
	{
	    string message;

	    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
	    request.KeepAlive = false;
	    request.Method = "GET";
	    request.MaximumAutomaticRedirections = 10;
	    HttpWebResponse response;
	    FetchResponse fresp = null;
	    try 
	    {
		response = (HttpWebResponse) request.GetResponse();
		try 
		{
		    fresp = GetResponse(response, maxRead);
		    if (response.StatusCode == HttpStatusCode.OK) 
			return fresp;

		    message = response.StatusCode.ToString();
		} 
		finally {
		    response.Close();
		}
	    }
	    catch (WebException e) {
		response = (HttpWebResponse) e.Response;
		if (response != null)
		{
		    try
		    {
			fresp = GetResponse(response, maxRead);
		    }
		    finally
		    {
			response.Close();
		    }
		}
		message = e.Message;
	    }
	    throw new FetchException(fresp, message);
	}

	public override FetchResponse Post( Uri uri, 
					    byte[] body, 
					    uint maxRead )
	{
	    string message;

	    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
	    request.ReadWriteTimeout = 20;
	    request.KeepAlive = false;
	    request.Method = "POST";
	    request.MaximumAutomaticRedirections = 10;
	    request.ContentLength = body.Length;
	    request.ContentType = "application/x-www-form-urlencoded";
	    HttpWebResponse response;
	    FetchResponse fresp = null;
	    try {
		Stream outStream = request.GetRequestStream();
		outStream.Write(body, 0, body.Length);
		outStream.Close();

		response = (HttpWebResponse) request.GetResponse();
		try 
		{
		    fresp = GetResponse(response, maxRead);
		    if (response.StatusCode == HttpStatusCode.OK) 
			return fresp;

		    message = response.StatusCode.ToString();
		} 
		finally {
		    response.Close();
		}
	    }
	    catch (WebException e) {
		response = (HttpWebResponse) e.Response;
		if (response != null)
		{
		    try
		    {
			fresp = GetResponse(response, maxRead);
		    }
		    finally
		    {
			response.Close();
		    }
		}
		message = e.Message;
	    }
	    throw new FetchException(fresp, message);
	}
#endregion

    }
}
