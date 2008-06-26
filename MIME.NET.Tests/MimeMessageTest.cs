using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Collections;
using MIME;
using System.IO;
using System.Reflection;

namespace MIME.NET.Tests
{
    [TestFixture]
    public class MimeMessageTest
    {
        [Test]
        public void TestMimeMessage()
        {
            string[] tests = { MimeMessages.SimpleNDR, MimeMessages.BinaryNDR, MimeMessages.BrokenNDR };

            /// http://www.codeproject.com/useritems/MIME_De_Encode_in_C_.asp
            foreach (string test in tests)
            {
                MimeMessage msg = new MimeMessage();
                msg.LoadBody(test);

                ArrayList bodylist = new ArrayList();
                msg.GetBodyPartList(bodylist);

                for (int i = 0; i < bodylist.Count; i++)
                {
                    MIME.MimeBody ab = (MimeBody)bodylist[i];
                    Console.WriteLine(ab.GetType().Name);
                    Console.WriteLine(" {0}", ab.GetContentType());
                    switch (ab.GetContentType())
                    {
                        case "message/delivery-status":
                            /// TODO: move to Mime processor
                            MimeDSN dsn = new MimeDSN();
                            dsn.LoadBody(ab.GetText());
                            Console.WriteLine("ReportingMTA: {0}", dsn.ReportingMTA);
                            Console.WriteLine("ReceivedFromMTA: {0}", dsn.ReceivedFromMTA);
                            Console.WriteLine("OriginalEnvelopeId: {0}", dsn.OriginalEnvelopeId);
                            foreach (MimeDSNRecipient r in dsn.Recipients)
                            {
                                Console.WriteLine("{0}: {1}", r.Action, r.FinalRecipientEmailAddress);
                            }
                            break;
                    }
                }
            }
        }

        [Test]
        public void TestEmbeddedResources()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();

            /// http://www.codeproject.com/useritems/MIME_De_Encode_in_C_.asp
            foreach (string resource in resources)
            {
                bool expected_found = false;

                if (resource.StartsWith("MIME.NET.Tests.Tests._"))
                {
                    expected_found = true;
                }
                else if (resource.StartsWith("MIME.NET.Tests.Failures._"))
                {
                    expected_found = false;
                }
                else
                {
                    continue;
                }

                Console.WriteLine(resource);

                StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resource));

                string filename = Path.GetTempFileName();
                StreamWriter sw = new StreamWriter(filename);
                sw.Write(sr.ReadToEnd());
                sw.Close();

                MimeMessage msg = new MimeMessage();
                StreamReader content = new StreamReader(filename);
                msg.LoadBody(content.ReadToEnd());
                content.Close();

                bool found = false;

                ArrayList bodylist = new ArrayList();
                msg.GetBodyPartList(bodylist);

                for (int i = 0; i < bodylist.Count; i++)
                {
                    MIME.MimeBody ab = (MimeBody)bodylist[i];
                    Console.WriteLine(ab.GetType().Name);
                    Console.WriteLine(" {0}", ab.GetContentType());
                    switch (ab.GetContentType())
                    {
                        case "message/delivery-status":
                            /// TODO: move to Mime processor
                            MimeDSN dsn = new MimeDSN();
                            dsn.LoadBody(ab.GetText());
                            Console.WriteLine("ReportingMTA: {0}", dsn.ReportingMTA);
                            Console.WriteLine("ReceivedFromMTA: {0}", dsn.ReceivedFromMTA);
                            Console.WriteLine("OriginalEnvelopeId: {0}", dsn.OriginalEnvelopeId);
                            foreach (MimeDSNRecipient r in dsn.Recipients)
                            {
                                found = true;
                                Console.WriteLine("{0}: {1}", r.Action, r.FinalRecipientEmailAddress);
                            }
                            break;
                    }
                }

                File.Delete(filename);
                Console.WriteLine("{0}: {1}", resource, found);
                Console.WriteLine("-------------------------------------------------------------------------");
                Assert.AreEqual(expected_found, found);
            }
        }
    }
}
