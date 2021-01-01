using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TracklistParser
{
    public class TracklistManager
    {
        #region Dependencies
        private readonly TagSpaceManager _tagSpaceManager;
        #endregion

        #region Exceptions
        class TrackIndexException : Exception
        {
            public TrackIndexException()
            {

            }

            public TrackIndexException(string message)
                : base($"TrackIndexException: {message}")
            {

            }
        }
        #endregion

        public List<Track> Tracklist { get; set; }
        private Index curTrackIndex { get; set; }

        public void SetCurIndex(Index index) =>
            curTrackIndex = index;

        public void AddTrack(bool closeTagSpace = true)
        {
            if (curTrackIndex == null)
                throw new TrackIndexException("Can't add track without StartTime");

            var tags = _tagSpaceManager.TagSpaces.Peek().Tags;

            Tracklist.Add(new Track(curTrackIndex, tags));

            curTrackIndex = null;

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

        public void Clear()
        {
            Tracklist.Clear();
            _tagSpaceManager.Clear();
        }

        #region Constructor
        public TracklistManager(TagSpaceManager tagSpaceManager, RuntimeData runtimeData)
        {
            _tagSpaceManager = tagSpaceManager;
            Tracklist = runtimeData.Tracklist;
            curTrackIndex = null;
        }
        #endregion
    }
}
