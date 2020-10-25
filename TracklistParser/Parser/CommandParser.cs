using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TracklistParser.Commands;
using TracklistParser.Config;

namespace TracklistParser.Parser
{
    class CommandParser
    {
        #region Dependencies
        private readonly CommandSpecificationManager _commandSpecificationManager;
        #endregion

        public List<ICommand> ParseCommandList(string input)
        {
            var parsedCommands = SpracheParser.Commands.Parse(input).Commands;
            List<CommandSpecification> specifications;
            try
            {
                specifications = _commandSpecificationManager.GetCommandSpecifications(parsedCommands);
            }
            catch(Exception e)
            {
                Console.WriteLine("Command Specification Discrepancy. Error message:\n" + e.Message);
                return null;
            }

            return GetCommands(parsedCommands, specifications);
        }

        class ScopeIntWrapper
        {
            public int i { get; set; }

            public ScopeIntWrapper(int i)
            {
                this.i = i;
            }
        }

        List<ICommand> GetCommands(List<ParsedCommand> parsedCommands, List<CommandSpecification> specifications, ScopeIntWrapper scopeWrapper = null, int ? lo = null, int? hi = null)
        {
            scopeWrapper = scopeWrapper ?? new ScopeIntWrapper(0);
            lo = lo ?? 0;
            hi = hi ?? parsedCommands.Count;

            var commands = new List<ICommand>();

            if (hi == lo)
                return commands;

            for (scopeWrapper.i = (int)lo; scopeWrapper.i < hi; scopeWrapper.i++)
            {
                var type = specifications[scopeWrapper.i].CommandType;
                var command = Activator.CreateInstance(type);
                foreach (var property in parsedCommands[scopeWrapper.i].Properties)
                {
                    var propertyInfo = type.GetProperty(property.Name);
                    if (property.Value.Length >= 2 && property.Name.Substring(0, 2) == "Is")
                    {
                        switch (property.Value.ToLower())
                        {
                            case "true":
                                propertyInfo.SetValue(command, true);
                                break;
                            case "false":
                                propertyInfo.SetValue(command, false);
                                break;
                            default:
                                throw new ArgumentException($"Couldn't convert {property.Value} to bool.\n" +
                                    $"Command number {scopeWrapper.i + 1}, Name: {parsedCommands[scopeWrapper.i].Name}, " +
                                    $"PropertyName: {property.Name}, PropertyValue: {property.Value}");
                        }
                    }
                    else
                        propertyInfo.SetValue(command, property.Value);
                }

                if (specifications[scopeWrapper.i].IsControl)
                {
                    var closingIndex = specifications.FindIndex(scopeWrapper.i, x => x.Name == specifications[scopeWrapper.i].Name && x.IsClosed);
                    var propertyInfo = type.GetProperty("Commands");
                    //scopeWrapper.i++;
                    propertyInfo.SetValue(command, GetCommands(parsedCommands, specifications, scopeWrapper, scopeWrapper.i + 1, closingIndex));
                }
                commands.Add((ICommand)command);
            }

            return commands;
        }

        public CommandParser(CommandSpecificationManager commandSpecificationManager)
        {
            _commandSpecificationManager = commandSpecificationManager;
        }
    }
}
