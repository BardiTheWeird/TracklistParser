using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetTagMatch : IParserCommand
    {
        public string TagName { get; set; }
        public string Pattern { get; set; }

        public SetTagMatch(string tagName, string pattern)
        {
            TagName = tagName;
            Pattern = pattern;
        }

        public SetTagMatch()
        {
        }
    }
}
