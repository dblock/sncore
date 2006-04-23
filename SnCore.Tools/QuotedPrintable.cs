using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;

namespace SnCore.Tools
{
    /// <summary>
    /// <para>
    /// Based on 
    /// Bill Gearhart
    /// http://www.aspemporium.com/classes.aspx?cid=4
    /// </para>
    public class QuotedPrintable
    {
        private QuotedPrintable()
        {
        }

        public static string Fold(string s)
        {
            s = s.Replace("\r\n", "\\n");
            return FormatEncodedString(s, RFC_1521_MAX_CHARS_PER_LINE, null);
        }

        /// <summary>
        /// Gets the maximum number of characters per quoted-printable
        /// line as defined in the RFC minus 1 to allow for the =
        /// character (soft line break).
        /// </summary>
        /// <remarks>
        /// (Soft Line Breaks): The Quoted-Printable encoding REQUIRES 
        /// that encoded lines be no more than 76 characters long. If 
        /// longer lines are to be encoded with the Quoted-Printable 
        /// encoding, 'soft' line breaks must be used. An equal sign 
        /// as the last character on a encoded line indicates such a 
        /// non-significant ('soft') line break in the encoded text.
        /// </remarks>
        public const int RFC_1521_MAX_CHARS_PER_LINE = 75;

        /// <summary>
        /// Encodes a very large string into the Quoted-Printable
        /// encoding for transmission via SMTP
        /// </summary>
        /// <param name="toencode">
        /// the very large string to encode
        /// </param>
        /// <returns>The Quoted-Printable encoded string</returns>
        /// <exception cref="ObjectDisposedException">
        /// A problem occurred while attempting to read the encoded 
        /// string.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        /// There is insufficient memory to allocate a buffer for the
        /// returned string. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// A string is passed in as a null reference.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs, such as the stream being closed.
        /// </exception>  
        /// <exception cref="ArgumentOutOfRangeException">
        /// The charsperline argument is less than or equal to 0.
        /// </exception>
        /// <remarks>
        /// This method encodes a large string into the quoted-printable
        /// encoding and then properly formats it into lines of 76 characters
        /// using the <see cref="FormatEncodedString"/> method.
        /// </remarks>
        public static string Encode(string toencode)
        {
            return Encode(toencode, RFC_1521_MAX_CHARS_PER_LINE);
        }

        /// <summary>
        /// Encodes a very large string into the Quoted-Printable
        /// encoding for transmission via SMTP
        /// </summary>
        /// <param name="toencode">
        /// the very large string to encode
        /// </param>
        /// <param name="charsperline">
        /// the number of chars per line after encoding
        /// </param>
        /// <returns>The Quoted-Printable encoded string</returns>
        /// <exception cref="ObjectDisposedException">
        /// A problem occurred while attempting to read the encoded 
        /// string.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        /// There is insufficient memory to allocate a buffer for the
        /// returned string. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// A string is passed in as a null reference.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs, such as the stream being closed.
        /// </exception>  
        /// <exception cref="ArgumentOutOfRangeException">
        /// The charsperline argument is less than or equal to 0.
        /// </exception>
        /// <remarks>
        /// This method encodes a large string into the quoted-printable
        /// encoding and then properly formats it into lines of 
        /// charsperline characters using the <see cref="FormatEncodedString"/> 
        /// method.
        /// </remarks>
        public static string Encode(string toencode, int charsperline)
        {
            if (toencode == null)
                throw new ArgumentNullException();

            if (charsperline <= 0)
                throw new ArgumentOutOfRangeException();

            return FormatEncodedString(EncodeSmall(toencode), charsperline);
        }

        /// <summary>
        /// Encodes a small string into the Quoted-Printable encoding
        /// for transmission via SMTP. The string is not split
        /// into lines of X characters like the string that the 
        /// Encode method returns.
        /// </summary>
        /// <param name="s">
        /// The string to encode.
        /// </param>
        /// <returns>The Quoted-Printable encoded string</returns>
        /// <exception cref="ArgumentNullException">
        /// A string is passed in as a null reference.
        /// </exception>
        /// <remarks>
        /// This method encodes a small string into the quoted-printable
        /// encoding. The resultant encoded string has NOT been separated
        /// into lined results using the <see cref="FormatEncodedString"/>
        /// method.
        /// </remarks>
        public static string EncodeSmall(string s)
        {
            if (s == null)
                throw new ArgumentNullException();

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                int code = s[i];

                if (((code >= 33) && (code <= 60)) || ((code >= 62) && (code <= 126)))
                {
                    result.Append((char)code);
                }
                else
                {
                    result.AppendFormat("={0}", code.ToString("X2"));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Formats a quoted-printable string into lines equal to maxcharlen,
        /// following all protocol rules such as byte stuffing. This method is
        /// called automatically by the Encode method and the EncodeFile method.
        /// </summary>
        /// <param name="qpstr">
        /// the quoted-printable encoded string.
        /// </param>
        /// <param name="maxcharlen">
        /// the number of chars per line after encoding.
        /// </param>
        /// <returns>
        /// The properly formatted Quoted-Printable encoded string in lines of
        /// 76 characters as defined by the RFC.</returns>
        /// <exception cref="ArgumentNullException">
        /// A string is passed in as a null reference.
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurs, such as the stream being closed.
        /// </exception>  
        /// <remarks>
        /// Formats a quoted-printable encoded string into lines of
        /// maxcharlen characters for transmission via SMTP.
        /// </remarks>
        public static string FormatEncodedString(string s, int maxcharlen)
        {
            return FormatEncodedString(s, maxcharlen, "=");
        }

        public static string FormatEncodedString(string s, int maxcharlen, string linesep)
        {
            if (s == null)
                throw new ArgumentNullException();

            StringBuilder result = new StringBuilder();
            
            int i = 0;
            int j = 0;
            while (i < s.Length)
            {
                if (j == maxcharlen)
                {
                    result.AppendLine(linesep);
                    j = 0;
                }

                result.Append(s[i]);

                j++;
                i++;
            }

            return result.ToString();
        }

        static string HexDecoderEvaluator(Match m)
        {
            string hex = m.Groups[2].Value;
            int iHex = Convert.ToInt32(hex, 16);
            char c = (char)iHex;
            return c.ToString();
        }

        static string HexDecoder(string line)
        {
            if (line == null)
                throw new ArgumentNullException();

            //parse looking for =XX where XX is hexadecimal
            Regex re = new Regex(
             "(\\=([0-9A-F][0-9A-F]))",
             RegexOptions.IgnoreCase
            );
            return re.Replace(line, new MatchEvaluator(HexDecoderEvaluator));
        }

        /// <summary>
        /// Decodes a Quoted-Printable string of any size into 
        /// it's original text.
        /// </summary>
        /// <param name="encoded">
        /// The encoded string to decode.
        /// </param>
        /// <returns>The decoded string.</returns>
        /// <exception cref="ArgumentNullException">
        /// A string is passed in as a null reference.
        /// </exception>
        /// <remarks>
        /// Decodes a quoted-printable encoded string into a string
        /// of unencoded text of any size.
        /// </remarks>
        public static string Decode(string encoded)
        {
            if (encoded == null)
                throw new ArgumentNullException();

            string line;
            StringWriter sw = new StringWriter();
            StringReader sr = new StringReader(encoded);
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.EndsWith("="))
                    {
                        sw.Write(HexDecoder(line.Substring(0, line.Length - 1)));
                    }
                    else
                    {
                        sw.Write(HexDecoder(line));
                    }

                    sw.Flush();
                }
                return sw.ToString();
            }
            finally
            {
                sw.Close();
                sr.Close();
                sw = null;
                sr = null;
            }
        }
    }
}