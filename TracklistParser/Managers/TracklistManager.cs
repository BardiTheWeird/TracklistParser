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

            try
            {
                if (!tags.ContainsKey("StartTime"))
                    throw new Exception("Tracks need to have a StartTime tag");
            }
            catch
            {
                Console.WriteLine("###Tracks DO need to have a StartTime tag, but I let you pass for now (:");
            }

            tags.TryGetValue("StartTime", out var startTime);
            startTime = startTime ?? "Time undefined";

            Tracklist.Add(new Track(startTime, 
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
                Console.WriteLine($"Track number: {i + 1}");
                foreach (var tag in track.Tags)
                    Console.WriteLine($"\t{tag.Key}: {tag.Value}");
                Console.WriteLine($"\tStartTime: {track.StartTime}");
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
