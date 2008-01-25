using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    /// <summary>
    /// A collection of audit entries; reduces load on large volume of traces.
    /// </summary>
    public class ManagedAccountAuditEntryCollection
    {
        private string mMessageFormat;
        private string mDelimiter = ", ";
        private string mLastDelimiter = " and ";
        private static int s_mMaxMessageLength = 256;
        private List<string> mTraces = new List<string>();

        private int MaxDelimiterLength
        {
            get
            {
                return Math.Max(mDelimiter.Length, mLastDelimiter.Length);
            }
        }

        public static void SetMaxMessageLength()
        {
            // figure out the max size of the Description field in the AccountAuditEntry class
            DomainClass dc = Session.Model["AccountAuditEntry"];
            s_mMaxMessageLength = dc["Description"].MaxLengthInChars;
        }

        public static int MaxMessageLength
        {
            get
            {
                return s_mMaxMessageLength;
            }
        }

        private int FormatLength
        {
            get
            {
                if (string.IsNullOrEmpty(mMessageFormat)) return 0;
                return mMessageFormat.Length - 3; /* reserved for {0} format */
            }
        }

        public string Delimiter
        {
            get
            {
                return mDelimiter;
            }
            set
            {
                mDelimiter = value;
            }
        }

        public string LastDelimiter
        {
            get
            {
                return mLastDelimiter;
            }
            set
            {
                mLastDelimiter = value;
            }
        }

        public string MessageFormat
        {
            get
            {
                return mMessageFormat;
            }
            set
            {
                mMessageFormat = value;
            }
        }

        public ManagedAccountAuditEntryCollection()
        {

        }

        public void Add(string trace)
        {
            mTraces.Add(trace);
        }

        private int GetNextAppendLength(int current, int append, int next)
        {
            return next + GetAppendLength(current, append);
        }

        private int GetAppendLength(int current, int append)
        {
            return current + (current > 0 ? MaxDelimiterLength : 0) + FormatLength + append;
        }

        public IList<string> GetAccountAuditStrings()
        {
            List<string> result_strings = new List<string>();
            List<string>.Enumerator e = mTraces.GetEnumerator();
            if (!e.MoveNext()) return result_strings;
            StringBuilder sb = new StringBuilder();
            bool fLast = false;
            while (! fLast)
            {
                string current = e.Current;
                fLast = ! e.MoveNext();

                // ignore empty strings
                if (current.Length == 0) continue;
                // is the sum of the current string + max delimiter + current text > max field
                if (GetAppendLength(sb.Length, current.Length) >= s_mMaxMessageLength)
                {
                    if (sb.Length > 0)
                    {
                        // save the previous string as a result
                        string formatted = (string.IsNullOrEmpty(mMessageFormat) ? sb.ToString() : string.Format(mMessageFormat, sb));
                        result_strings.Add(formatted);
                        sb = new StringBuilder();
                    }
                }
                 
                // what do we append before the value
                string append = string.Empty;
                if ((fLast || (GetNextAppendLength(sb.Length, current.Length, e.Current.Length) >= s_mMaxMessageLength)) && (sb.Length > 0)) append = mLastDelimiter;
                else if (sb.Length > 0) append = mDelimiter; // any delimiter
                sb.Append(append);
                sb.Append(current);
            }

            if (sb.Length > 0)
            {
                // save the previous string as a result
                string formatted = (string.IsNullOrEmpty(mMessageFormat) ? sb.ToString() : string.Format(mMessageFormat, sb));
                result_strings.Add(formatted);
            }

            return result_strings;
        }


        public IEnumerable<AccountAuditEntry> GetAccountAuditEntries(ISession session, Account account, string url)
        {
            IList<string> result_strings = GetAccountAuditStrings();
            List<AccountAuditEntry> result = new List<AccountAuditEntry>(result_strings.Count);
            foreach (string s in result_strings)
            {
                AccountAuditEntry entry = ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(
                    session, account, s, url);
                result.Add(entry);
            }
            return result;
        }
    }
}
