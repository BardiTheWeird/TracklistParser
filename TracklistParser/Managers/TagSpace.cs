using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser
{
    public class TagSpace
    {
        public Dictionary<string, string> Tags { get; set; }

        public TagSpace(Dictionary<string, string> tags)
        {
            Tags = tags;
        }
    }
}
