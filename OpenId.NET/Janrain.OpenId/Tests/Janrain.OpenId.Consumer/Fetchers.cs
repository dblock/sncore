using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Janrain.TestHarness;
using Janrain.OpenId.Consumer;

namespace OpenIdTest.OpenId
{
    class TestHttpHandler
    {
	static string CRLF = "\r\n";
	static string SERVER_LINE = "Server: OpenId TestHttpServer\r\n";
	//    Socket sock;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	enum Method {
	    GET,
	    POST
	}

	class Case
	{
	    public readonly int code;
	    public readonly string redir;

	    public Case( int code,
			 string redir )
	    {
		this.code = code;
		this.redir = redir;
	    }
	}

	static Hashtable cases;

	static TestHttpHandler()
	{
	    cases = new Hashtable();
	    cases.Add("/success", new Case(200, null));
	    cases.Add("/301redirect", new Case(301, "/success"));
	    cases.Add("/302redirect", new Case(302, "/success"));
	    cases.Add("/303redirect", new Case(303, "/success"));
	    cases.Add("/307redirect", new Case(307, "/success"));
	    cases.Add("/notfound", new Case(404, null));
	    cases.Add("/badreq", new Case(400, null));
	    cases.Add("/forbidden", new Case(403, null));
	    cases.Add("/error", new Case(500, null));
	    cases.Add("/server_error", new Case(503, null));
	}
    

	public TestHttpHandler ( Socket sock )
	{
	    //this.sock = sock;
	    this.stream = new NetworkStream(sock);
	    this.writer = new StreamWriter(this.stream, Encoding.UTF8);
	    this.reader = new StreamReader(this.stream, Encoding.ASCII);
	}

	public void ProcessRequest ()
	{
	    string headerLine = this.reader.ReadLine();
	    string[] parts = headerLine.Split();
	    string methodStr = parts[0];
	    
	    Method method;
	    switch (methodStr)
	    {
	    case "GET":
		method = Method.GET;
		break;
	    case "POST":
		method = Method.POST;
		break;
	    default:
		Error(501, new NameValueCollection());
		return;
	    }

	    string path = parts[1];
	    NameValueCollection headers = new NameValueCollection();
	    // Consume rest of headers
	    while(true)
	    {
		headerLine = this.reader.ReadLine();
		if(headerLine.Equals(CRLF) || headerLine.Equals(""))
		    break;
		parts = headerLine.Split(":".ToCharArray());
		headers[parts[0].Trim()] = parts[1].Trim();
	    }

	    if (method == Method.POST)
	    {
		string postBody = this.reader.ReadToEnd();
	    }
	    else
	    {
		Case cs;
		if ((cs = (Case) cases[path]) != null)
		{
		    NameValueCollection extraHeaders = null;
		    if (cs.redir != null)
		    {
			extraHeaders = new NameValueCollection();
			string location = String.Format("http://localhost:9111{0}", cs.redir);
			extraHeaders.Add("Location", location);
		    }
	  
		    Respond(cs.code, "text/plain", path, extraHeaders);
		}
		else
		{
		    BadPath(path, methodStr, "Bad path");
		}
	    }
	}

	private void BadPath(string path, string method, string message)
	{
	    StringBuilder msg = new StringBuilder();
	    msg.Append("Bad request:\r\n");
	    msg.AppendFormat("HTTP method: {0}\r\n", method);
	    msg.AppendFormat("path: {0}\r\n", path);
	    if (message != null)
		msg.AppendFormat("message: {0}\r\n", message);
      
	    Respond(400, "text/plain", msg.ToString(), null);
	}

	private void Error(int code, NameValueCollection headers)
	{
	    string contentType = "text/html" ;
	    string bodyFmt = "<HTML>" + 
		"<HEAD><TITLE>{0} {1}</TITLE></HEAD>" +
		"<BODY>{0} {1}</BODY></HTML>";
	    string body = String.Format(bodyFmt, code, 
					((HttpStatusCode) code).ToString());
	    Respond(code, contentType, body, headers);
	}

	private void Respond ( int code, string contentType, string content, NameValueCollection headers )
	{
	    string statusLine = String.Format("HTTP/1.1 {0} {1}{2}", code, ((HttpStatusCode) code).ToString(), CRLF);
	    string contentTypeLine = "Content-type: " + contentType + CRLF;
	    string contentLengthLine = String.Format("Content-Length: {0}{1}", content.Length, CRLF); 

	    writer.Write(statusLine);
	    writer.Write(SERVER_LINE);
	    writer.Write(contentTypeLine);
	    writer.Write(contentLengthLine);

	    if (headers != null)
	    {
		foreach (string key in headers)
		    writer.Write(String.Format("{0}: {1}{2}", key, headers[key], CRLF));
	    }

	    writer.Write(CRLF);
	    writer.Write(content);
	    writer.Flush();
	}
    }

    class TestHttpServer
    {
	const int port = 9111;
	TcpListener myListener;

	public TestHttpServer ()
	{
	    myListener = new TcpListener(port);
	    myListener.Start();
	    Console.WriteLine("Web Server Running");
	}

	public void ServeForever ()
	{
	    TestHttpHandler handler;
	    while(true)
	    {
		//Accept a new connection
		Socket mySocket = myListener.AcceptSocket();
		handler = new TestHttpHandler(mySocket);
		try
		{
		    handler.ProcessRequest();
		}
		catch (Exception e)
		{
		    Console.Write(e.ToString());
		}
		finally
		{
		    mySocket.Close();
		}
	    }
	}
    }

    [TestSuite]
    public class FetcherTestSuite
    {
	struct Expected
	{
	    public readonly HttpStatusCode code;
	    public readonly Uri final;
	    public readonly string body;

	    public Expected ( int code,
			      Uri final,
			      string body )
	    {
		this.code = (HttpStatusCode) code;
		this.final = final;
		this.body = body;
	    }
	    
	    public void Test(FetchResponse actual)
	    {
		TestTools.Assert(actual.code == this.code);
		TestTools.Assert(actual.finalUri.Equals(this.final));
		TestTools.Assert(
		    Encoding.GetEncoding(actual.charset).GetString(
			actual.data) == this.body);
	    }
	}
	    
	struct Case
	{
	    public readonly Uri fetchUri;
	    public readonly Expected expected;
	    
	    public Case ( string path,
			  Expected expected )
	    {
		this.fetchUri = GetUrl(path);
		this.expected = expected;
	    }
	}
	
	static Uri GetUrl(string path)
	{
	    return new Uri(String.Format("http://localhost:9111{0}", path));
	}
	
	static Case Plain(string path, int code)
	{
	    path = "/" + path;
	    return new Case(path, new Expected(code, GetUrl(path), path));
	}
	
	static ArrayList cases;

	static FetcherTestSuite ()
	{
	    cases = new ArrayList();
            cases.Add(Plain("success", 200));
            cases.Add(new Case("/301redirect", new Expected(
				   200, GetUrl("/success"), "/success")));
            cases.Add(new Case("/302redirect", new Expected(
				   200, GetUrl("/success"), "/success")));
            cases.Add(new Case("/303redirect", new Expected(
				   200, GetUrl("/success"), "/success")));
            cases.Add(new Case("/307redirect", new Expected(
				   200, GetUrl("/success"), "/success")));
	    cases.Add(Plain("notfound", 404));
	    cases.Add(Plain("badreq", 400));
	    cases.Add(Plain("forbidden", 403));
	    cases.Add(Plain("error", 500));
	    cases.Add(Plain("server_error", 503));
	}

	private static void WebserverThread()
	{
	    TestHttpServer server = new TestHttpServer();
	    server.ServeForever();
	}

	private static void TestFetcher(Fetcher f)
	{
	    f.Get(new Uri("http://localhost:9111/success"));
	    
	    FetchResponse actual;
	    foreach (Case c in cases)
	    {
		Console.Write("Fetching {0}...", c.fetchUri.ToString());
		try
		{
		    try
		    {
			actual = f.Get(c.fetchUri);
			c.expected.Test(actual);
		    }
		    catch (FetchException e) 
		    {
			c.expected.Test(e.response);
		    }
		    Console.Write("OK");
		}
		finally
		{
		    Console.WriteLine("");
		}
	    }
	}

	[Test]
	public void Fetch ()
	{
	    Thread t = new Thread(new ThreadStart(WebserverThread));
	    t.Start();
	    try
	    {
		Thread.Sleep(0);
		Fetcher f = new SimpleFetcher();
		TestFetcher(f);
	    }
	    finally
	    {
		t.Abort();
	    }
	}
    }

}
