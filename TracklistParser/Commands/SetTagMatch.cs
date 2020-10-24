using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetTagMatch : ICommand
    {
        public string TagName { get; set; }
        public string TagValuePattern { get; set; }

        public SetTagMatch(string tagName, string tagValuePattern)
        {
            TagName = tagName;
            TagValuePattern = tagValuePattern;
        }
    }
}
