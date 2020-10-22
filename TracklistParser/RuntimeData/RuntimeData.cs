using System;
using System.Collections.Generic;
using System.Text;

namespace TracklistParser
{
    class RuntimeData
    {
        public Stack<TagSpace> TagSpaces { get; set; }
        public List<Track> Tracklist { get; set; }

        public RuntimeData()
        {
            TagSpaces = new Stack<TagSpace>();
            Tracklist = new List<Track>();
        }

        public RuntimeData(Stack<TagSpace> tagSpaces, List<Track> tracklist)
        {
            TagSpaces = tagSpaces;
            Tracklist = tracklist;
        }
    }
}
