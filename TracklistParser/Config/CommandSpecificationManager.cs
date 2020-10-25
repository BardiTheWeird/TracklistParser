﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using TracklistParser.Parser;

namespace TracklistParser.Config
{
    sealed class CommandSpecification
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public bool IsControl { get; set; }
        public Type CommandType { get; set; }
        public List<string> Properties { get; set; }

        public CommandSpecification()
        {
            Properties = new List<string>();
        }

        public CommandSpecification(string name, bool isClosed, bool isControl, Type commandType, List<string> properties)
        {
            Name = name;
            IsClosed = isClosed;
            IsControl = isControl;
            CommandType = commandType;
            Properties = properties;
        }
    }

    class CommandSpecificationManager
    {
        #region Exceptions
        class CommandSpecificationReadingException : Exception
        {
            public CommandSpecificationReadingException()
            {

            }

            public CommandSpecificationReadingException(string message)
                : base($"CommandSpecificationReadingException: {message}")
            {

            }
        }

        class CommandSpecificationDiscrepancyException : Exception
        {
            public CommandSpecificationDiscrepancyException() { }

            public CommandSpecificationDiscrepancyException(string message)
                : base($"CommandSpecificationReadingException: {message}")
            {

            }
        }
        #endregion

        #region fields
        private readonly Dictionary<(string, bool), CommandSpecification> _specifiedCommands;
        #endregion

        #region GetCommandSpecifications
        // Also validates commands
        public List<CommandSpecification> GetCommandSpecifications(List<ParsedCommand> parsedCommands)
        {
            var specificationList = new List<CommandSpecification>(parsedCommands.Count);

            for (int i = 0; i < parsedCommands.Count; i++)
            {
                var command = parsedCommands[i];

                var key = (command.Name, command.IsClosed);

                // Check if specification exists
                if (!_specifiedCommands.TryGetValue(key, out var specification))
                {
                    if (!_specifiedCommands.TryGetValue((command.Name, command.IsClosed), out var specification1))
                    {
                        throw new CommandSpecificationDiscrepancyException($"No command with name {command.Name} found");
                    }
                    else
                    {
                        throw new CommandSpecificationDiscrepancyException($"Command with name {command.Name} has no specification with IsClosed={command.IsClosed}");
                    }
                }

                // Check if properties are ok
                if (command.Properties.Count != specification.Properties.Count)
                {
                    throw new CommandSpecificationDiscrepancyException($"Command has {command.Properties.Count} properties, " +
                        $"while specification entails {specification.Properties.Count}");
                }
                foreach (var property in command.Properties)
                {
                    if (!specification.Properties.Contains(property.Name))
                    {
                        throw new CommandSpecificationDiscrepancyException($"{command.Name} specification has no {property.Name} property");
                    }
                }

                specificationList.Add(specification);
            }

            // Check if control commands are closed
            var controlSpecifications = specificationList.Where(x => x.IsControl);
            var openControlCommands = new Stack<CommandSpecification>(controlSpecifications.Count() / 2 + 1);

            foreach (var control in controlSpecifications)
            {
                if (!control.IsClosed)
                    openControlCommands.Push(control);
                else
                {
                    if (openControlCommands.Count > 0 && openControlCommands.Peek().Name == control.Name)
                    {
                        openControlCommands.Pop();
                    }
                    else
                    {
                        throw new CommandSpecificationDiscrepancyException($"No opening {control.Name} command for the closing {control.Name} command respective scope");
                    }
                }
            }

            if (openControlCommands.Count > 0)
            {
                var commandNames = string.Join(", ", openControlCommands.Select(x => x.Name));
                throw new CommandSpecificationDiscrepancyException($"No closing commands for such control commands: {commandNames}");
            }

            return specificationList;
        }
        #endregion

        #region SetDictionary
        void SetDictionary(string csvFilePath)
        {
            var commandTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(csvFilePath)), true))
            {
                commandTable.Load(csvReader);
            }

            for (int i = 0; i < commandTable.Rows.Count; i++)
            {
                string  name =      commandTable.Rows[i][0].ToString().Trim();
                string  isControl = commandTable.Rows[i][1].ToString().Trim();
                string  isClosed =  commandTable.Rows[i][2].ToString().Trim();
                string  type =      commandTable.Rows[i][3].ToString().Trim();
                var properties = new List<string>();
                properties.Add(commandTable.Rows[i][4].ToString().Trim());
                properties.Add(commandTable.Rows[i][5].ToString().Trim());
                properties.Add(commandTable.Rows[i][6].ToString().Trim());

                var textCommand = new CommandSpecification();
                textCommand.Name = name;

                Func<string, bool> stringToBool = x =>
                {
                    if (x.ToLower() == "true")
                        return true;
                    if (x.ToLower() == "false")
                        return false;
                    throw new CommandSpecificationReadingException(
                        $"{x} can't be interpreted as neither true nor false;\n" +
                        $"\tAt filepath {csvFilePath}, Row {i}");
                };

                textCommand.IsControl = stringToBool(isControl);
                textCommand.IsClosed = stringToBool(isClosed);

                textCommand.CommandType = Type.GetType("TracklistParser.Commands." + type + ", TracklistParser");
                if (type != "-" && textCommand.CommandType == null) 
                {
                    throw new CommandSpecificationReadingException(
                        $"Couldn't parse the Type of command {type}\n" +
                        $"At filepath {csvFilePath}, Row {i}");
                }

                foreach(var property in properties)
                {
                    if (property != "-")
                        textCommand.Properties.Add(property);
                }

                _specifiedCommands.Add((textCommand.Name, textCommand.IsClosed), textCommand);
            }
        }
        #endregion

        #region ctor
        public CommandSpecificationManager(string csvFilePath)
        {
            _specifiedCommands = new Dictionary<(string, bool), CommandSpecification>();
            SetDictionary(csvFilePath);
        }
        #endregion
    }
}