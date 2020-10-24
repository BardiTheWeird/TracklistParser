using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetStart : ICommand
    {
        public string Pattern { get; set; }

        public SetStart(string pattern)
        {
            Pattern = pattern;
        }
    }
}
