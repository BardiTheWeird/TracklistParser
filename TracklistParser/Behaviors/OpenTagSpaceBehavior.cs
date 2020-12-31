using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class OpenTagSpaceBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        #endregion

        public void Execute(IParserCommand commandIn, Scope scope) =>
            _tagSpaceManager.OpenTagSpace();

        public OpenTagSpaceBehavior(TagSpaceManager tagSpaceManager)
        {
            _tagSpaceManager = tagSpaceManager;
        }
    }
}
