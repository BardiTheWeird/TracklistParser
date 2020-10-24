using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Behaviors;
using TracklistParser.Commands;

namespace TracklistParser.Managers
{
    class CommandManager
    {
        #region fields
        private readonly IIndex<Type, ICommandBehavior> _behaviorIndex;
        #endregion

        #region Properties
        public Scope GlobalScope { get; set; }
        #endregion

        #region Property Injection
        public void SetScope(Scope scope) =>
            GlobalScope = scope;
        #endregion

        #region Execute Command(s)
        public void Execute(List<ICommand> commands, Scope scope = null)
        {
            foreach (var command in commands)
                Execute(command, scope);
        }

        public void Execute(ICommand command, Scope scope = null)
        {
            scope = scope ?? GlobalScope;

            if (_behaviorIndex.TryGetValue(command.GetType(), out var behavior))
            {
                behavior.Execute(command, scope);
            }
            else
                throw new KeyNotFoundException($"No {command.GetType()} registered in the CommandDict");
        }
        #endregion

        #region Constructor
        public CommandManager(IIndex<Type, ICommandBehavior> index)
        {
            _behaviorIndex = index;
        }
        #endregion
    }
}
