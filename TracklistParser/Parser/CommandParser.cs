using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
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
            var commands = SpracheParser.Commands.Parse(input).Commands;
            List<CommandSpecification> specifications;
            try
            {
                specifications = _commandSpecificationManager.GetCommandSpecifications(commands);
            }
            catch(Exception e)
            {
                Console.WriteLine("Command Specification Discrepancy. Error message:\n" + e.Message);
                return null;
            }

            return null;
        }

        public CommandParser(CommandSpecificationManager commandSpecificationManager)
        {
            _commandSpecificationManager = commandSpecificationManager;
        }
    }
}
