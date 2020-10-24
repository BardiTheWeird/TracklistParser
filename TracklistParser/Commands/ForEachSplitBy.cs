using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class ForEachSplitBy : ICommand
    {
        public string Pattern { get; set; }
        public List<ICommand> Commands { get; set; }

        public ForEachSplitBy(string pattern, List<ICommand> commands)
        {
            Pattern = pattern;
            Commands = commands;
        }
    }
}
