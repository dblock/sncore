using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Janrain.OpenId
{
    class ValueError : ApplicationException 
    {
        public ValueError ( string msg ) : 
	    base(msg) 
	{ }
    }


    public class KVUtil
    {
        private KVUtil () { }

        private static void Error(string msg, bool strict)
        {
            if (strict)
                throw new ValueError(msg);
            // XXX: else: log msg
        }

        public static byte[] SeqToKV(NameValueCollection seq, bool strict)
        {
	    MemoryStream ms = new MemoryStream();
	    byte[] line;
            foreach (string key in seq)
            {
		string val = seq[key];
                if (key.IndexOf('\n') >= 0)
                    throw new ValueError("Invalid input for SeqToKV: key contains newline");

                if (key.Trim().Length != key.Length)
                    Error(String.Format("Key has whitespace at beginning or end: {0}", key), strict);

                if (val.IndexOf('\n') >= 0)
                    throw new ValueError("Invalid input for SeqToKV: value contains newline");

                if (val.Trim().Length != val.Length)
                    Error(String.Format("Value has whitespace at beginning or end: {0}", val), strict);

		line = Encoding.UTF8.GetBytes(String.Format("{0}:{1}\n", key, val));
                ms.Write(line, 0, line.Length);
            }
            return ms.ToArray();
        }

        public static byte[] DictToKV(NameValueCollection data)
        {
            return SeqToKV(data, false);
        }

        public static NameValueCollection KVToDict(byte[] data)
        {
	    StringReader reader = new StringReader(UTF8Encoding.UTF8.GetString(data));

	    string line;
	    NameValueCollection nvc = new NameValueCollection();
            int line_num = 0;
	    while((line = reader.ReadLine()) != null)
	    {
		line_num += 1;
		if (line.Trim().Length > 0)
		{
		    string[] parts = line.Split(new char[] { ':', }, 2);
		    if (parts.Length != 2)
			Error(String.Format("Line {0} does not contain a colon", line_num.ToString()), false);
		    else
			nvc[parts[0]] = parts[1];
		}
            }

            return nvc;
        }
    }
}
