using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class SetTagMatchBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        #endregion

        public void Execute(ICommand commandIn, Scope scope)
        {
            var command = commandIn as SetTagMatch;
            var curString = scope.CurString;
            var valuePattern = command.Pattern;

            var match = Regex.Match(curString, valuePattern).Value;
            _tagSpaceManager.SetTag(command.TagName, match);
        }

        public SetTagMatchBehavior(TagSpaceManager tagSpaceManager)
        {
            _tagSpaceManager = tagSpaceManager;
        }
    }
}
