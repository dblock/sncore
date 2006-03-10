using System;

using System.Collections;
using System.Collections.Specialized;
using System.Text;

using Janrain.TestHarness;
using Janrain.OpenId;

namespace OpenIdTest.OpenId 
{
    [TestSuite]
    public class LinkParserTestSuite
    {
        ArrayList failures;
        ArrayList exceptions;
	static string delim = "\n\n";
        
	static object[] SplitCase(string s)
	{
	    ArrayList parts = new ArrayList();
	    int start = 0;
	    int match;
	    
	    while (start < s.Length)
	    {
		match = s.IndexOf(delim, start);
		if (match < 0)
		    break;
		
		parts.Add(s.Substring(start, match - start));
		start = match + 2;
	    }
	    
	    return parts.ToArray();
	}

        class Failure 
        {
            public string cs;
            public NameValueCollection[] actual;
            public NameValueCollection[] expected;
            
            public Failure(string cs, NameValueCollection[] expected, NameValueCollection[] actual)
            {
                this.cs = cs;
                this.expected = expected;
                this.actual = actual;
            }
        }

        class Exc
        {
            public string cs;
            public NameValueCollection[] expected;
            public Exception exc;
            
            public Exc(string cs, NameValueCollection[] expected, Exception exc)
            {
                this.cs = cs;
                this.expected= expected;
                this.exc = exc;
            }
        }

        private bool CheckDicts(NameValueCollection d1, NameValueCollection d2)
        {
            foreach (string key in d1)
            {
                if (d2[key] == null)
                    return false;
                
                if (d2[key] != d2[key])
                    return false;
            }
            return true;
        }

        private void Run(string cs, NameValueCollection[] expected)
        {
            byte[] csData = Encoding.UTF8.GetBytes(cs);
            try 
            {
                NameValueCollection[] actual = LinkParser.ParseLinkAttrs(csData, csData.Length, "utf-8");

                if (actual.Length != expected.Length)
                {
                    this.failures.Add(new Failure(cs, expected, actual));
                    return;
                }

                for (uint i = 0; i < expected.Length; i++)
                {
                    if (! (CheckDicts(actual[i], expected[i]) && 
                                CheckDicts(expected[i], actual[i])))
                    {
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                exceptions.Add(new Exc(cs, expected, e));
            }
        }

        private string Pretty(NameValueCollection[] input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach(NameValueCollection attrs in input)
            {
                sb.Append("{\n");
                foreach(string key in attrs)
                    sb.AppendFormat("  {0}: {1}\n", key, attrs[key]);
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private void CheckErrors()
        {
            foreach (Failure failure in failures)
            {
                Console.WriteLine("".PadRight(40, '='));
                Console.WriteLine("FAILURE:");
                Console.WriteLine("Case:");
                Console.WriteLine("[{0}]", failure.cs);
                Console.WriteLine("Expected:");
                Console.WriteLine(Pretty(failure.expected));
                Console.WriteLine("Actual:");
                Console.WriteLine(Pretty(failure.actual));
                Console.WriteLine("");
            }

            foreach (Exc e in exceptions)
            {
                Console.WriteLine("".PadRight(50, '='));
                Console.WriteLine("FAILURE:");
                Console.WriteLine("Case:");
                Console.WriteLine("[{0}]", e.cs);
                Console.WriteLine("Expected:");
                Console.WriteLine(Pretty(e.expected));
                Console.WriteLine("Exception:");
                Console.WriteLine("[{0}]", e.exc.ToString());
                Console.WriteLine("");
            }

            if (failures.Count > 0 || exceptions.Count> 0)
                TestTools.Assert(false, "some of the parse tests failed");
        }

        
        static string no_match_cases_str = @"
            <link>

            <html>
            <link>

            <head>
            <link>

            <html>
            <head>
            </head>
            <link>

            <html>
            <link>
            <head>

            <link>
            <html>
            <head>

            <html>
            <head>
            </head>
            </html>
            <link>

            <html>
            <head>
            <html>
            <link>

            <head>
            <html>
            <link>

            <html>
            <head>
            <body>
            <link>

            <html>
            <head>
            <head>
            <link>

            <html>
            <head>
            <script>
            <link>
            </script>

            <html>
            <head>
            <!--
            <link>
            -->

            <html>
            <head>
            <![CDATA[
            <link>
            ]]>

            <html>
            <head>
            <![cDaTa[
            <link>
            ]]>

            <htmlx>
            <head>
            <link>

            <html:summer>
            <head>
            <link>

            <html>
            <head:zucchini>
            <link>

            <html/>
            <head>
            <link>

            <html/>
            <html>
            <head>
            <link>

            <html>
            <head/>
            <link>

            <html>
            <head/>
            <head>
            <link>";

        [Test]
        public void NoMatch()
        {
            failures = new ArrayList();
            exceptions= new ArrayList();
            uint i = 0;
            foreach (string cs in SplitCase(no_match_cases_str))
            {
                Run(cs, new NameValueCollection[]{});
                i++;
            }
            CheckErrors();
        }
        
        
        static string empty_link_cases_str = @"
            <!-- Plain vanilla -->
            <html>
            <head>
            <link>

            <!-- Ignore tags in the <script:... > namespace -->
            <html>
            <head>
            <script:paddypan>
            <link>
            </script:paddypan>

            <!-- Short link tag -->
            <html>
            <head>
            <link/>

            <!-- Spaces in the HTML tag -->
            <html >
            <head>
            <link>

            <!-- Spaces in the head tag -->
            <html>
            <head >
            <link>

            <html>
            <head>
            <link >

            <html><head><link>

            <html>
            <head>
            <link>
            </head>

            <html>
            <head>
            <link>
            </head>
            <link>

            <html>
            <head>
            <link>
            <body>
            <link>

            <html>
            <head>
            <link>
            </html>

            <html>
            <head>
            <link>
            </html>
            <link>

            <html>
            <delicata>
            <head>
            <title>
            <link>

            <HtMl>
            <hEaD>
            <LiNk>

            <butternut>
            <html>
            <summer>
            <head>
            <turban>
            <link>

            <html>
            <head>
            <script>
            <link>

            <html><head><script><link>

            <html>
            <head>
            <!--
            <link>

            <html>
            <head>
            <![CDATA[
            <link>

            <html>
            <head>
            <![ACORN[
            <link>
            ]]>

            <html>
            <head>
            <link>
            -->";

        [Test]
        public void EmptyLink()
        {
            failures = new ArrayList();
            exceptions= new ArrayList();
            NameValueCollection[] expected = new NameValueCollection[] {new NameValueCollection()};
            foreach (string cs in SplitCase(empty_link_cases_str))
                Run(cs, expected);

            CheckErrors();
        }

        static string two_link_cases_str = @"
            <html>
            <head>
            <link>
            <link>

            <html>
            <gold nugget>
            <head>
            <link>
            <link>

            <html>
            <head>
            <link>
            <LiNk>
            <body>
            <link>
            ";

        [Test]
        public void TwoLink()
        {
            failures = new ArrayList();
            exceptions= new ArrayList();
            NameValueCollection[] expected = new NameValueCollection[] {
		new NameValueCollection(), new NameValueCollection()};
            
            foreach (string cs in SplitCase(two_link_cases_str))
                Run(cs, expected);

            CheckErrors();
        }
        
        static string attr_cases_str = @"
            <html><head><link rel=openid.server>

            <html><head><link rel=openid.server/>

            <html><head><link rel=openid.server />

            <html><head><link hubbard rel=openid.server>

            <html><head><link hubbard rel=openid.server/>

            <html><head><link hubbard rel=openid.server />

            <html><head><link / rel=openid.server>

            <html><head><link rel=""openid.server"">

            <html><head><link rel='openid.server'>
            ";

        [Test]
        public void Attr()
        {
            failures = new ArrayList();
            exceptions= new ArrayList();
            NameValueCollection attrs = new NameValueCollection();
            attrs.Add("rel", "openid.server");
            
            foreach (string cs in SplitCase(attr_cases_str))
                Run(cs, new NameValueCollection[]{attrs});

            CheckErrors();
        }

        class Case
        {
            public string html;
            public NameValueCollection[] expected;
            public Case(string html, 
                    params NameValueCollection[] expected)
            {
                this.html = html;
                this.expected = expected;
            }
        }

        private static NameValueCollection NewDict(params string[] input)
        {
            NameValueCollection d = new NameValueCollection();
            for (uint i = 0; i < (input.Length/2); i++)
                d.Add(input[2*i], input[2*i+1]);
            return d;
        }

        static Case[] cases = new Case[] { new Case(
                "<html><head><link x=y><link a=b>", 
                NewDict("x", "y"),
                NewDict("a", "b")),
            new Case("<html><head><link x=&y>", 
                    NewDict("x", "&y")),
            new Case("<html><head><link x=\"&y\">", 
                    NewDict("x", "&y")),
            new Case("<html><head><link x=\"&amp;\">", 
                    NewDict("x", "&")),
            new Case("<html><head><link x=\"&#26;\">", 
                    NewDict("x", "&#26;")),
            new Case("<html><head><link x=\"&lt;\">", 
                    NewDict("x", "<")),
            new Case("<html><head><link x=\"&gt;\">", 
                    NewDict("x", ">")),
            new Case("<html><head><link x=\"&quot;\">", 
                    NewDict("x", "\"")),
            new Case("<html><head><link x=\"&amp;&quot;\">", 
                    NewDict("x", "&\"")),
            new Case("<html><head><link x=\"&amp;&quot;&hellip;&gt;\">",
                    NewDict("x", "&\"&hellip;>")),
            new Case("<html><head><link x=\"x&amp;&quot;&hellip;&gt;x\">",
                    NewDict("x", "x&\"&hellip;>x")),
            new Case("<html><head><link x=y<>", 
                    NewDict("x", "y")),
            new Case("<html><head><link x=y<link x=y />",
                    NewDict("x", "y"), 
                    NewDict("x", "y")),
            new Case("<html><head><link x=y<link x=y />",
                    NewDict("x", "y"), 
                    NewDict("x", "y")),
            new Case("<html><head><link x=y y=><link x=y />",
                    NewDict("x", "y"), 
                    NewDict("x", "y")),
            new Case("<html><head><link x=y", 
                    NewDict("x", "y")),
            new Case("<html><head><link x=\"<\">", 
                    NewDict("x", "<")),
            new Case("<html><head><link x=\">\">", 
                    NewDict("x", ">")),
            new Case("<html><head><link x=\"\u1234\">", 
                    NewDict("x", "\u1234")),
            new Case("<html><head><link x=\"\u1234&amp;\">",
                    NewDict("x", "\u1234&")),
            new Case("<html><head><link x=z x=y>",
                    NewDict("x", "y")),
            new Case("<html><head><link x=y x=y>",
                    NewDict("x", "y")),
            new Case("<html><head><link x=y y=z>",
                    NewDict("x", "y", "y", "z")),
            //The important examples:
            // Well-formed link rel="openid.server"
            new Case("<html><head><link rel=\"openid.server\" " +
                    "href=\"http://www.myopenid.com/server\" />" +
                    "</head></html>",
                    NewDict("rel", "openid.server", "href", 
                        "http://www.myopenid.com/server")),
            // Well-formed link rel="openid.server" and "openid.delegate"
            new Case("<html><head><link rel=\"openid.server\" " +
                    "href=\"http://www.myopenid.com/server\" />" +
                    "<link rel=\"openid.delegate\" href=\"http://example.myopenid.com/\" />" +
                    "</head></html>",
                    NewDict("rel", "openid.server", "href",
                        "http://www.myopenid.com/server"),
                    NewDict("rel", "openid.delegate", "href", 
                        "http://example.myopenid.com/")),
            // from brian's livejournal page
            new Case(@"<!DOCTYPE html
          PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN""
                 ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                 <html xmlns=""http://www.w3.org/1999/xhtml"">
                   <head>
                     <link rel=""stylesheet"" href=""http://www.livejournal.com/~serotta/res/319998/stylesheet?1130478711"" type=""text/css"" />
         <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
 <meta name=""foaf:maker"" content=""foaf:mbox_sha1sum '12f8abdacb5b1a806711e23249da592c0d316260'"" />
 <meta name=""robots"" content=""noindex, nofollow, noarchive"" />
 <meta name=""googlebot"" content=""nosnippet"" />
 <link rel=""openid.server"" href=""http://www.livejournal.com/openid/server.bml"" />
                   <title>Brian</title>
         </head>",
         NewDict("rel", "stylesheet", "href", "http://www.livejournal.com/~serotta/res/319998/stylesheet?1130478711", "type", "text/css"),
         NewDict("rel", "openid.server", "href", "http://www.livejournal.com/openid/server.bml"))
        };
        
        [Test]
        public void RealAttrs()
        {
            failures = new ArrayList();
            exceptions= new ArrayList();

            foreach (Case cs in cases)
                Run(cs.html, cs.expected);

            CheckErrors();
        }
    }
}
