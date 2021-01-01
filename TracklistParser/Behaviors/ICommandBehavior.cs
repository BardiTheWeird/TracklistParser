using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    public interface ICommandBehavior
    {
        public void Execute(IParserCommand commandIn, Scope scope);
    }
}
