using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetEnd : ICommand
    {
        public string Pattern { get; set; }

        public SetEnd(string pattern)
        {
            Pattern = pattern;
        }
    }
}
