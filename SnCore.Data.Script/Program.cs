using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace SnCore.Data.Script
{
    class Program
    {
        public static int CompareTablesByName(Table left, Table right)
        {
            return left.Name.CompareTo(right.Name);
        }

        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
                Database db = server.Databases["SnCore"];
                if (db == null) throw new Exception("Missing SnCore database");
                Scripter scripter = new Scripter(server);
                scripter.Options.Default = true;
                scripter.Options.DriAll = true;
                scripter.Options.AllowSystemObjects = true;
                scripter.Options.AnsiFile = true;
                scripter.Options.AppendToFile = false;
                scripter.Options.FileName = "SnCoreSqlServer.sql";
                scripter.Options.FullTextCatalogs = true;
                scripter.Options.FullTextIndexes = true;
                scripter.Options.IncludeDatabaseRoleMemberships = false;
                scripter.Options.IncludeHeaders = false;
                scripter.Options.Indexes = true;
                scripter.Options.NoIdentities = false;
                scripter.Options.NonClusteredIndexes = true;
                scripter.Options.ClusteredIndexes = true;
                scripter.Options.SchemaQualifyForeignKeysReferences = true;
                scripter.Options.Permissions = false;
                scripter.Options.ScriptDrops = false;
                scripter.Options.Statistics = false;
                scripter.Options.ToFileOnly = true;
                scripter.Options.Triggers = true;
                scripter.Options.IncludeIfNotExists = true;

                UrnCollection c = new UrnCollection();

                List<Table> tables = new List<Table>();
                IEnumerator e = db.Tables.GetEnumerator();
                while (e.MoveNext()) tables.Add((Table) e.Current);
                tables.Sort(CompareTablesByName);
                Console.WriteLine("Processing {0} tables ...", tables.Count);

                foreach (Table tb in tables)
                {
                    c.Add(tb.Urn);

                    foreach (Check check in tb.Checks)
                        c.Add(check.Urn);

                    foreach (Index index in tb.Indexes)
                        c.Add(index.Urn);

                    foreach (ForeignKey key in tb.ForeignKeys)
                        c.Add(key.Urn);

                    if (tb.FullTextIndex != null)
                        c.Add(tb.FullTextIndex.Urn);
                }
                scripter.ScriptingProgress += new ProgressReportEventHandler(scripter_ScriptingProgress);
                scripter.ScriptingError += new ScriptingErrorEventHandler(scripter_ScriptingError);
                scripter.Script(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        static void scripter_ScriptingError(object sender, ScriptingErrorEventArgs e)
        {
            Console.WriteLine("Error: {0}", e.Current);
        }

        static void scripter_ScriptingProgress(object sender, ProgressReportEventArgs e)
        {
            Console.WriteLine(e.Current);
        }
    }
}
