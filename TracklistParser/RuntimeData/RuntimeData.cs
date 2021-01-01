using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TracklistParser
{
    public class RuntimeData
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
