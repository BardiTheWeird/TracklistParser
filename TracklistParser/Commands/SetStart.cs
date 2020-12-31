using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetStart : IParserCommand
    {
        public string Pattern { get; set; }
        public bool IsInclusive { get; set; }

        public SetStart(string pattern, bool isInclusive)
        {
            Pattern = pattern;
            IsInclusive = isInclusive;
        }

        public SetStart()
        {
        }
    }
}
