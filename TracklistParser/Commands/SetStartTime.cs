using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetStartTime : ICommand
    {
        public string Pattern { get; set; }
        public string TimePattern { get; set; }

        public SetStartTime(string pattern, string timePattern)
        {
            Pattern = pattern;
            TimePattern = timePattern;
        }

        public SetStartTime()
        {
        }
    }
}
