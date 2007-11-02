using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SnCore.Tools.Web
{
    public interface IMarkupRendererHandler
    {
        string Handle(string tag, string tagname, string tagvalue);
    }

    public class MarkupRendererClearHandler : IMarkupRendererHandler
    {
        public string Handle(string tag, string tagname, string tagvalue)
        {
            return tagvalue;
        }
    }

    public class MarkupRenderer<Handler> 
        where Handler : IMarkupRendererHandler
    {
        private Handler mHandler;

        public MarkupRenderer(Handler handler)
        {
            mHandler = handler;
        }

        static Regex MarkupExpression = new Regex(@"(?<tag>[\[]+)(?<name>[\w\s]*):(?<value>[\w\s\'\-!]*)[\]]+",
            RegexOptions.IgnoreCase);

        private string ReferenceHandler(Match ParameterMatch)
        {
            string tag = ParameterMatch.Groups["tag"].Value;
            string tagname = ParameterMatch.Groups["name"].Value.Trim();
            string tagvalue = ParameterMatch.Groups["value"].Value.Trim();

            switch (tag)
            {
                case "[[":
                    return string.Format("[{0}:{1}]", tagname, tagvalue);
                default:
                    return mHandler.Handle(tag, tagname, tagvalue);
            }
        }

        public string Render(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            MatchEvaluator mhd = new MatchEvaluator(ReferenceHandler);
            return MarkupExpression.Replace(s, mhd);
        }
    }
}
