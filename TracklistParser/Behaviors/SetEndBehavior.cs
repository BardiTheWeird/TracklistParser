using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class SetEndBehavior : ICommandBehavior
    {
        public void Execute(ICommand commandIn, Scope scope)
        {
            var command = commandIn as SetEnd;

            var matchEnd = Regex.Match(scope.CurString, command.Pattern);
            var length = matchEnd.Index;
            if (command.IsInclusive)
                length += matchEnd.Value.Length;

            scope.CurString = scope.CurString.Substring(0, length);
        }
    }
}
