using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracklistParser
{
    class TracklistManager
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        #endregion

        public List<Track> Tracklist { get; set; }

        public void AddTrack(bool closeTagSpace = true)
        {
            var tags = _tagSpaceManager.TagSpaces.Peek().Tags;

            if (!tags.ContainsKey("StartTime"))
                throw new Exception("Tracks need to have a StartTime tag");

            Tracklist.Add(new Track(tags["StartTime"], 
                tags.Where(x => x.Key != "StartTime")
                .ToDictionary(x => x.Key, x => x.Value)));

            if (closeTagSpace)
                _tagSpaceManager.CloseTagSpace();
        }

        public void PrintTracklist()
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                var track = Tracklist[i];
                Console.WriteLine($"Track number: {i}");
                foreach (var tag in track.Tags)
                    Console.WriteLine($"\t{tag.Key}: {tag.Value}");
                Console.WriteLine();
            }
        }

        #region Constructor
        public TracklistManager(TagSpaceManager tagSpaceManager, RuntimeData runtimeData)
        {
            _tagSpaceManager = tagSpaceManager;
            Tracklist = runtimeData.Tracklist;
        }
        #endregion
    }
}
