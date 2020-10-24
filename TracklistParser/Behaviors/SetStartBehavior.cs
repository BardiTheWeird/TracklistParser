﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class SetStartBehavior : ICommandBehavior
    {
        public void Execute(ICommand commandIn, Scope scope)
        {
            var command = commandIn as SetStart;

            var matchStart = Regex.Match(scope.CurString, command.Pattern).Index;
            scope.CurString = scope.CurString.Substring(matchStart);
        }
    }
}