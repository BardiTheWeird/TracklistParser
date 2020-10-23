using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Parser
{
    interface IParser
    {
        public void ParseText(string filepath);
    }
}
