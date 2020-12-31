using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TracklistParser.Commands;

namespace TracklistParser.Behaviors
{
    class SetStartTimeBehavior : ICommandBehavior
    {
        #region Dependencies
        private readonly TracklistManager _tracklistManager;
        #endregion

        static readonly string[] propertyNames = { "Hours", "Minutes", "Seconds", "Frames" };

        #region Parsing
        static string FormatCharToPropertyName(char c)
        {
            switch (c)
            {
                case 'h':
                    return propertyNames[0];
                case 'm':
                    return propertyNames[1];
                case 's':
                    return propertyNames[2];
                case 'f':
                    return propertyNames[3];
            }
            throw new ArgumentException($"{c} is not one of the format characters");
        }

        static string TimeFormatToRegex(string timeFormat)
        {
            var charsEncountered = new List<char>();
            var formatCharMatches = Regex.Matches(timeFormat, @"[hfms]+").Cast<Match>();

            // Validate timeFormat
            foreach (var match in formatCharMatches.Select(m => m.Value))
            {
                var newChar = match[0];
                if (charsEncountered.Contains(newChar))
                    throw new FormatException($"Start time format {timeFormat} contains more than one {newChar} zone");

                charsEncountered.Add(newChar);

                for (int i = 1; i < match.Length; i++)
                {
                    if (match[i] != newChar)
                        throw new FormatException($"Format zone {match} consists of more than one character, start time format {timeFormat}");
                }
            }

            // Create a regex with a capture group
            int indexOffset = 0;
            string regexString = timeFormat;

            foreach (var match in formatCharMatches)
            {
                string captureGroup = $"(?<{FormatCharToPropertyName(match.Value[0])}>[0-9]+)";
                regexString = regexString.Remove(match.Index + indexOffset, match.Length)
                    .Insert(match.Index + indexOffset, captureGroup);
                indexOffset += captureGroup.Length - match.Length;
            }
            return regexString;
        }

        static Index ParseIndex(string timeString, string timeFormat)
        {
            var index = new Index();
            foreach (var formatString in timeFormat.Split('|'))
            {
                var regex = TimeFormatToRegex(formatString);
                var match = Regex.Match(timeString, regex);
                if (match.Success)
                {
                    foreach (var groupName in propertyNames)
                    {
                        var group = match.Groups[groupName];
                        if (group.Success)
                        {
                            typeof(Index).GetProperty(groupName)
                                .SetValue(index, Convert.ToInt32(group.Value));
                        }
                    }
                    return index;
                }
            }
            throw new ArgumentException($"Time formats {timeFormat} does not contain an appropriate time format for {timeString}");
        }
        #endregion

        public void Execute(IParserCommand commandIn, Scope scope)
        {
            var command = commandIn as SetStartTime;

            var timeString = Regex.Match(scope.CurString, command.Pattern).Value;
            var index = ParseIndex(timeString, command.TimeFormat);
            _tracklistManager.SetCurIndex(index);
        }

        public SetStartTimeBehavior(TracklistManager tracklistManager)
        {
            _tracklistManager = tracklistManager;
        }
    }
}
