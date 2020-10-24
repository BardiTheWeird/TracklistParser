using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    interface ICommandBehavior
    {
        public void Execute(ICommand commandIn, Scope scope);
    }
}
