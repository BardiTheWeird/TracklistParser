using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TracklistParser.Parser
{
    class TempParser : IParser
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        private readonly TracklistManager _tracklistManager;
        #endregion

        public void ParseText(string filepath)
        {
            var text = File.ReadAllText(filepath).Trim();
 
            foreach (var command in Regex.Split(text, "\n")) 
            {
                string type = Regex.Match(command, "[-+#]").Value;
                if (type == "+")
                {
                    _tagSpaceManager.OpenTagSpace();
                    foreach (var tag in Regex.Split(command.Substring(1).Trim(), ";"))
                    {
                        if (tag.Length > 0)
                        {
                            var tagName = Regex.Match(tag, @"([^:]+)(?=:)").Value;
                            var tagValue = Regex.Match(tag, @"(?<=:)([^:]+)").Value;

                            _tagSpaceManager.SetTag(tagName, tagValue);
                        }
                    }
                }
                else if (type == "-")
                    _tagSpaceManager.CloseTagSpace();
                else if (type == "#")
                    _tracklistManager.AddTrack();
                else
                    throw new Exception("something went very wrong, lol");
            }
        }

        #region Constructor
        public TempParser(TagSpaceManager tagSpaceManager, TracklistManager tracklistManager)
        {
            _tagSpaceManager = tagSpaceManager;
            _tracklistManager = tracklistManager;
        }
        #endregion
    }
}
