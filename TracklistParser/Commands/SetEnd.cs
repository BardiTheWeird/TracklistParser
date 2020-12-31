using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetEnd : IParserCommand
    {
        public string Pattern { get; set; }
        public bool IsInclusive { get; set; }

        public SetEnd(string pattern, bool isInclusive)
        {
            Pattern = pattern;
            IsInclusive = isInclusive;
        }

        public SetEnd()
        {
        }
    }
}
