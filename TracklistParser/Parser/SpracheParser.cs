using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TracklistParser.Parser
{
    public class CommandProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public CommandProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Command
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public List<CommandProperty> Properties { get; set; }

        #region Constructors
        public Command(string name)
        {
            Name = name;
        }

        public Command(string name, bool isClosed) : this(name)
        {
            IsClosed = isClosed;
        }

        public Command(string name, List<CommandProperty> properties) : this(name)
        {
            Properties = properties;
        }

        public Command(string name, bool isClosed, List<CommandProperty> properties) : this(name, isClosed)
        {
            Properties = properties;
        }
        #endregion
    }

    public class CommandList
    {
        public List<Command> Commands { get; set; }

        public CommandList(List<Command> commands)
        {
            Commands = commands;
        }
    }

    public class SpracheParser
    {
        private readonly static List<char> _trailingChars = new List<char>
        {
            '\a', '\b', '\t', '\r', '\v', '\f', '\n'
        };

        private static bool IsTrailing(char c) =>
            char.IsWhiteSpace(c) || _trailingChars.Contains(c);

        private static readonly Parser<string> Trailing =
            from trailing in Parse.Char(c => IsTrailing(c), "trailing characters").Many()
            select string.Concat(trailing);

        public static readonly Parser<string> PropertyName = 
            Parse.Letter.AtLeastOnce().Text().Token();

        public static readonly Parser<string> PropertyValue =
            (from open in Parse.Char('"')
             from content in Parse.CharExcept('"').Many().Text()
             from close in Parse.Char('"')
             select content).Token();

        public static readonly Parser<CommandProperty> CommandProperty =
             from name in PropertyName
             from equality in Parse.Char('=')
             from value in PropertyValue
             select new CommandProperty(name, value);

        public static readonly Parser<Command> Command =
            from trailingIn in Trailing
            from open in Parse.Char('<').Once()
            from name in PropertyName.Once()
            from properties in CommandProperty.Many()
            from close in Parse.String("/>").Or(Parse.String(">")).Once()
            from trailingOut in Trailing
            select new Command(name.First(), string.Concat(close.First()) == "/>",
                properties.ToList());

        public static readonly Parser<CommandList> Commands =
            //from commands in Command.Until(Parse.String("#END#"))
            from commands in Command.Many()
            select new CommandList(commands.ToList());
    }
}
