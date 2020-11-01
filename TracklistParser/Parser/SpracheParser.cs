using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TracklistParser.Parser
{
    public class ParsedCommandProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ParsedCommandProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ParsedCommand
    {
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public List<ParsedCommandProperty> Properties { get; set; }

        #region Constructors
        public ParsedCommand(string name)
        {
            Name = name;
        }

        public ParsedCommand(string name, bool isClosed) : this(name)
        {
            IsClosed = isClosed;
        }

        public ParsedCommand(string name, List<ParsedCommandProperty> properties) : this(name)
        {
            Properties = properties;
        }

        public ParsedCommand(string name, bool isClosed, List<ParsedCommandProperty> properties) : this(name, isClosed)
        {
            Properties = properties;
        }
        #endregion
    }

    public class ParsedCommandList
    {
        public List<ParsedCommand> Commands { get; set; }

        public ParsedCommandList(List<ParsedCommand> commands)
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

        public static readonly Parser<ParsedCommandProperty> CommandProperty =
             from name in PropertyName
             from equality in Parse.Char('=')
             from value in PropertyValue
             select new ParsedCommandProperty(name, value);

        public static readonly Parser<ParsedCommand> Command =
            from trailingIn in Trailing
            from open in Parse.Char('<').Once()
            from name in PropertyName.Once()
            from properties in CommandProperty.Many()
            from close in Parse.String("/>").Or(Parse.String(">")).Once()
            from trailingOut in Trailing
            select new ParsedCommand(name.First(), string.Concat(close.First()) == "/>",
                properties.ToList());

        public static readonly Parser<ParsedCommandList> Commands =
            from commands in Command.Many()
            select new ParsedCommandList(commands.ToList());
    }
}
