using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class ForEachSplitBy : IParserCommand
    {
        public string Pattern { get; set; }
        public List<IParserCommand> Commands { get; set; }

        public ForEachSplitBy(string pattern, List<IParserCommand> commands)
        {
            Pattern = pattern;
            Commands = commands;
        }

        public ForEachSplitBy()
        {
            Commands = new List<IParserCommand>();
        }
    }
}
