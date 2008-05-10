using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Smtp;
using System.Net;
using System.Net.Sockets;

namespace SnCore.SmtpMockServer
{
    class Program
    {
        static void Exception(SmtpMock mock, Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        static void Main(string[] args)
        {
            SmtpMock mock = new SmtpMock();
            mock.Started += new SmtpMock.StartHandler(mock_Start);
            mock.Error += new SmtpMock.ExceptionHandler(Exception);
            mock.Sent += new SmtpSession.DataHandler(mock_Sent);
            mock.Received += new SmtpSession.DataHandler(mock_Received);
            mock.End += new SmtpSession.EndHandler(mock_End);
            mock.Start();
            mock.Wait();
        }

        static void mock_End(SmtpSession sender)
        {
            Console.WriteLine("{0}= Session Ended", sender.Id);
        }

        static void mock_Start(SmtpMock sender, TcpListener listener)
        {
            Console.WriteLine("== Listening on {0}", listener.LocalEndpoint);
        }

        static void mock_Received(SmtpSession sender, string line)
        {
            Console.WriteLine("{0}< {1}", sender.Id, line);
        }

        static void mock_Sent(SmtpSession sender, string line)
        {
            Console.WriteLine("{0}> {1}", sender.Id, line);
        }
    }
}
