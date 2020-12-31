using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class SetTagBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        #endregion

        public void Execute(IParserCommand commandIn, Scope scope) 
        {
            var command = commandIn as SetTag;

            _tagSpaceManager.SetTag(command.TagName, command.TagValue);
        }

        public SetTagBehavior(TagSpaceManager tagSpaceManager)
        {
            _tagSpaceManager = tagSpaceManager;
        }
    }
}
