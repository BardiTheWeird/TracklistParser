using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser.Behaviors
{
    class Scope
    {
        public string CurString { get; set; }

        public Scope(string curString)
        {
            CurString = curString;
        }

        public Scope(Scope scope)
        {
            CurString = scope.CurString;
        }
    }
}
