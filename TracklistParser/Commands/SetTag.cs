using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Commands
{
    class SetTag : ICommand
    {
        public string TagName { get; set; }
        public string TagValue { get; set; }
        //public string CurString { get; set; }
        //public string TagValuePattern { get; set; }

        public SetTag(string tagName, string tagValue)
        {
            TagName = tagName;
            TagValue = tagValue;
        }
    }
}
