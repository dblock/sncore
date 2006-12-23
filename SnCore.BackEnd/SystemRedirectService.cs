using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Threading;
using System.Net.Mail;
using System.Diagnostics;
using System.Text;
using System.Net;
using SnCore.Tools.Web;
using System.IO;

namespace SnCore.BackEndServices
{
    public class SystemRedirectService : SystemService
    {
        public SystemRedirectService()
        {

        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunGenerateRedirectIni));
        }

        public void RunGenerateRedirectIni(ISession session)
        {           
            StringBuilder sb = new StringBuilder();
            IList redirects = session.CreateCriteria(typeof(AccountRedirect))
                .List();

            //sb.AppendLine("RewriteLog  c:\\temp\\iirfLog.out");
            //sb.AppendLine("RewriteLogLevel 3");

            foreach (AccountRedirect redirect in redirects)
            {
                sb.AppendFormat("RewriteRule    ^/{0}([\\/]*)$    /{1}",
                    redirect.SourceUri, redirect.TargetUri);
                sb.AppendLine();
            }

            string inipath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\IsapiRewrite4.ini");
            Console.WriteLine(inipath);

            FileStream f = new FileStream(inipath, FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(f);
            sw.Write(sb);
            sw.Close();
            f.Close();

            Thread.Sleep(1000 * InterruptInterval);
        }
    }
}
