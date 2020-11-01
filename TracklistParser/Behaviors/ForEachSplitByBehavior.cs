using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Commands;
using TracklistParser.Managers;

namespace TracklistParser.Behaviors
{
    class ForEachSplitByBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly CommandManager _commandManager;
        #endregion

        // Maybe use the command manager thing, lol
        public void Execute(ICommand commandIn, Scope scope)
        {
            var forEachCommand = commandIn as ForEachSplitBy;

            foreach (var str in Regex.Split(scope.CurString, forEachCommand.Pattern))
            {
                var newScope = new Scope(str);
                foreach(var command in forEachCommand.Commands)
                    _commandManager.Execute(command, newScope);
            }
        }

        #region Constructor
        public ForEachSplitByBehavior(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }
        #endregion
    }
}
