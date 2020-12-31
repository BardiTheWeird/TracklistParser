using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetStartTime : IParserCommand
    {
        public string Pattern { get; set; }
        public string TimeFormat { get; set; }

        public SetStartTime(string pattern, string timePattern)
        {
            Pattern = pattern;
            TimeFormat = timePattern;
        }

        public SetStartTime()
        {
        }
    }
}
