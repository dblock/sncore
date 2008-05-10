using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

// Modified version of http://geekswithblogs.net/imilovanovic/archive/2004/09/27/11783.aspx
namespace SnCore.Smtp
{
    /// <summary>
    /// Simple session
    /// </summary>
    public class SmtpSession
    {
        Socket _socket;
        private int _id = 0;

        public delegate void DataHandler(SmtpSession sender, string line);
        public event DataHandler Received;
        public event DataHandler Sent;

        public delegate void EndHandler(SmtpSession sender);
        public event EndHandler End;

        public delegate void ExceptionHandler(SmtpSession sender, Exception ex);
        public event ExceptionHandler Error;

        private void Write(StreamWriter sw, string line)
        {
            sw.WriteLine(line);
            if (Sent != null) Sent(this, line);
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// process session
        /// </summary>
        public void Process()
        {
            NetworkStream networkStream = new NetworkStream(_socket);
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            streamWriter.AutoFlush = true;

            try
            {
                streamWriter.WriteLine("220 coolcat.de SMTP Mock Server Ready");
                bool datasent = false;

                DateTime ts = DateTime.UtcNow;

                while (_socket.Connected)
                {
                    string line = streamReader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                    {
                        if (ts.AddMinutes(1) <= DateTime.UtcNow)
                            break;

                        continue;
                    }

                    ts = DateTime.UtcNow;

                    if (Received != null)
                    {
                        Received(this, line);
                    }

                    if (line.ToUpper().StartsWith("QUIT"))
                    {
                        Write(streamWriter, "221 coolcat.de Service closing transmission channel");
                        break;
                    }

                    if (line.ToUpper().StartsWith("DATA"))
                    {
                        datasent = true;
                        Write(streamWriter, "354 Immediate Reply");
                    }
                    else if (datasent && line.Trim() == ".")
                    {
                        datasent = false;
                        Write(streamWriter, "250 OK");
                    }
                    else if (!datasent)
                    {
                        Write(streamWriter, "250 OK");
                    }
                }

                if (End != null)
                {
                    End(this);
                }
            }
            catch (Exception ex)
            {
                if (Error != null)
                {
                    Error(this, ex);
                }
            }
            finally
            {
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                _socket.Close();
            }

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="socket"></param>
        public SmtpSession(Socket socket, int id)
        {
            _socket = socket;
            _id = id;
        }

    }
    /// <summary>
    /// Simple SMTP Mock server
    /// </summary>
    public class SmtpMock
    {
        private TcpListener _smtpListener = null;
        private Thread _smtpServerThread = null;

        /// <summary>
        /// start server
        /// </summary>
        public void Start()
        {
            _smtpServerThread = new Thread(new ThreadStart(Run));
            _smtpServerThread.Start();
        }

        public void Wait()
        {
            _smtpServerThread.Join();
        }

        /// <summary>
        /// stop server
        /// </summary>
        public void Stop()
        {
            if (_smtpListener != null)
                _smtpListener.Stop();
        }

        public delegate void ConnectionHandler(SmtpMock sender, Socket sock);
        public ConnectionHandler Connect = null;

        public delegate void ExceptionHandler(SmtpMock sender, Exception ex);
        public ExceptionHandler Error = null;

        public delegate void StartHandler(SmtpMock sender, TcpListener listener);
        public StartHandler Started = null;

        public event SmtpSession.DataHandler Received;
        public event SmtpSession.DataHandler Sent;
        public event SmtpSession.EndHandler End;

        /// <summary>
        /// run server
        /// </summary>
        private void Run()
        {
            try
            {
                _smtpListener = new TcpListener(IPAddress.Any, 25); // open listener for port 
                _smtpListener.Start();

                if (Started != null) Started(this, _smtpListener);

                int count = 1;

                try
                {
                    while (true)
                    {
                        Socket clientSocket = _smtpListener.AcceptSocket();
                        if (Connect != null) Connect(this, clientSocket);
                        SmtpSession session = new SmtpSession(clientSocket, count);
                        session.Error += new SmtpSession.ExceptionHandler(session_Error);
                        session.Sent += new SmtpSession.DataHandler(session_Sent);
                        session.Received += new SmtpSession.DataHandler(session_Received);
                        session.End += new SmtpSession.EndHandler(session_End);
                        Thread sessionThread = new Thread(new ThreadStart(session.Process));
                        sessionThread.Start();
                        count++;
                    }
                }
                catch (InvalidOperationException)
                {
                    // server stopped
                }
                finally
                {
                    _smtpListener.Stop();
                }
            }
            catch (Exception ex)
            {
                if (Error != null)
                {
                    Error(this, ex);
                }

                _smtpListener.Stop();
            }
        }

        void session_End(SmtpSession sender)
        {
            if (End != null) End(sender);
        }

        void session_Received(SmtpSession sender, string line)
        {
            if (Received != null) Received(sender, line);
        }

        void session_Sent(SmtpSession sender, string line)
        {
            if (Sent != null) Sent(sender, line);
        }

        void session_Error(SmtpSession sender, Exception ex)
        {
            if (Error != null) Error(this, ex);
        }
    }
}
