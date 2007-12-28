using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Trace;

namespace SnCore.Web.Tests
{
    public abstract class SqlTrace
    {
        public static PerformanceData EndTrace(string uri, TraceServer traceServerReader)
        {
            PerformanceData perf = new PerformanceData(uri);
            traceServerReader.Stop();

            while (traceServerReader.Read())
            {
                string ApplicationName = traceServerReader.GetString(2);
                if (ApplicationName == null) ApplicationName = string.Empty;
                if (ApplicationName == "SQL Management") continue;
                if (ApplicationName == "Microsoft SQL Server Management Studio") continue;
                if (ApplicationName.StartsWith("SQL Server Profiler")) continue;

                //0: EventClass
                //1: TextData
                //2: ApplicationName
                //3: NTUserName
                //4: LoginName
                //5: CPU
                //6: Reads
                //7: Writes
                //8: Duration
                //9: ClientProcessID
                //10: SPID
                //11: StartTime
                //12: EndTime
                //13: BinaryData

                //string query = traceServerReader.GetString(1);
                //if (string.IsNullOrEmpty(query)) query = string.Empty;
                //if (query.StartsWith("exec sp_executesql N'")) query = query.Remove(0, 21);
                // if (query.Length > 64) query = query.Substring(0, 64);

                //Console.WriteLine("{0}: {1} -> {2} | CPU={3} | Duration={4}", nEventNum, 
                //    query,
                //    traceServerReader.GetString(2),
                //    traceServerReader.GetOrdinal("CPU"),
                //    traceServerReader.GetOrdinal("Duration"));

                perf.Queries++;
                object duration = traceServerReader.GetValue(8);
                perf.TotalDuration += (duration == null ? 0 : (long)duration);
            }

            return perf;
        }

        public static SqlConnectionInfo GetTraceSqlConnectionInfo()
        {
            SqlConnectionInfo sci = new SqlConnectionInfo();
            sci.UseIntegratedSecurity = true;
            return sci;
        }

        public static TraceServer BeginTrace(SqlConnectionInfo sci)
        {
            TraceServer traceServerReader = new TraceServer();
            traceServerReader.InitializeAsReader(sci, GetTraceTemplateName());
            return traceServerReader;
        }

        private static string GetTraceTemplateName()
        {
            string traceConfigFileName = @"Microsoft SQL Server\90\Tools\Profiler\Templates\Microsoft SQL Server\90\Standard.tdf";
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFilesPath, traceConfigFileName);
        }
    }
}
