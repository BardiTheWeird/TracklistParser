using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class AddTrackBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly TracklistManager _tracklistManager;
        #endregion

        public void Execute(IParserCommand commandIn, Scope scope) =>
            _tracklistManager.AddTrack();

        public AddTrackBehavior(TracklistManager tracklistManager)
        {
            _tracklistManager = tracklistManager;
        }
    }
}
