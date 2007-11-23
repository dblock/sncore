using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Collections;
using MIME;

namespace MIME.NET.Tests
{
    [TestFixture]
    public class MimeMessageTest
    {
        [Test]
        public void TestMimeMessage()
        {
            /// http://www.codeproject.com/useritems/MIME_De_Encode_in_C_.asp
            
            MimeMessage msg = new MimeMessage();
            msg.LoadBody(MimeMessages.SimpleNDR);

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
}
