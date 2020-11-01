using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser
{
    class Track
    {
        public Index StartTime { get; set; }
        // public string StartTime { get; set; }
        public Dictionary<string, string> Tags { get; set; }

        public Track(Index startTime, Dictionary<string, string> tags)
        {
            StartTime = startTime;
            Tags = tags;
        }
    }

    class Index
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Frames { get; set; }

        public Index(int hours, int minutes, int seconds, int frames)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Frames = frames;
        }

        public Index()
        {
        }

        public override string ToString() =>
            $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}:{Frames:D2}";
    }
}
