using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace FoodCandy.DomainMail
{
    [Guid("6782A8DE-234C-4dde-A53D-EF3B3C5DD314")]
    [ComVisible(true)]
    public class Sink : IMailTransportSubmission
    {
        private static bool s_Debug = false;
        private static string s_QueuePath = string.Empty;
        // private static FileSystemWatcher s_ConfigurationChangeWatcher = null;

        static Sink()
        {
            LoadConfiguration();
        }

        public Sink()
        {

        }

        public static bool Debug
        {
            get
            {
                return s_Debug;
            }
        }

        private static void Configure(string filename)
        {
            SnCore.DomainMail.Configuration cnf = new SnCore.DomainMail.Configuration(filename);
            LogDebug(string.Format("Loaded configuration file \"{0}\".", filename));

            bool.TryParse(cnf["Debug"], out s_Debug);

            s_QueuePath = cnf["QueuePath"];

            if (string.IsNullOrEmpty(s_QueuePath))
            {
                s_QueuePath = Path.Combine(Path.GetTempPath(), "Queue");
            }

            if (!Directory.Exists(s_QueuePath)) Directory.CreateDirectory(s_QueuePath);
            LogDebug(string.Format("Queuing messages in \"{0}\".", s_QueuePath));
        }

        private static void LoadConfiguration()
        {
            string filename = Assembly.GetExecutingAssembly().Location + ".config";
            try
            {
                LogDebug(string.Format("Loading configuration file \"{0}\".", filename));
                if (File.Exists(filename))
                {
                    Configure(filename);
                }

                // s_ConfigurationChangeWatcher = new FileSystemWatcher(Path.GetDirectoryName(filename), "*.config");
                // s_ConfigurationChangeWatcher.Created += new FileSystemEventHandler(s_ConfigurationChangeWatcher_Changed);
                // s_ConfigurationChangeWatcher.Changed += new FileSystemEventHandler(s_ConfigurationChangeWatcher_Changed);
            }
            catch (Exception ex)
            {
                LogError(string.Format("Error loading configuration file \"{0}\"\n{1}", filename, ex.Message));
            }
        }

        //static void s_ConfigurationChangeWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    Configure(e.FullPath);
        //}

        void IMailTransportSubmission.OnMessageSubmission(
             MailMsg mailmsg,
             IMailTransportNotify notify,
             IntPtr context)
        {
            LogDebug("Invoking mail sink message submission callback.");

            try
            {
                Message message = new Message(mailmsg);
                LogDebug(string.Format("Processing message \"{0}\" ({1} byte(s)).", message.Rfc822MsgSubject, message.GetContentSize()));
                byte[] content = message.ReadContent(0, message.GetContentSize());
                string filename = s_QueuePath + "\\" + message.Rfc822MsgId;
                filename = filename.Replace("<", "_").Replace(">", "_").Replace(" ", "_");
                LogDebug(string.Format("Dumping message \"{0}\" to {1}.", message.Rfc822MsgSubject, filename));
                File.WriteAllBytes(filename, content);
            }
            catch (Exception ex)
            {
                LogError(ex.Message + "\n" + ex.StackTrace.ToString());
            }
            finally
            {
                if (null != mailmsg)
                {
                    Marshal.ReleaseComObject(mailmsg);
                }
            }
        }

        private static void Log(string message)
        {
            Log(message, EventLogEntryType.Information);
        }

        private static void Log(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(Assembly.GetExecutingAssembly().FullName,
              message, type);
        }

        private static void LogDebug(string message)
        {
            if (Debug)
            {
                Log(message);
            }
        }

        private static void LogError(string message)
        {
            Log(message, EventLogEntryType.Error);
        }
    }
}
