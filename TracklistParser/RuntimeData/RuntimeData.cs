using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TracklistParser
{
    public class RuntimeData
    {
        public Stack<TagSpace> TagSpaces { get; set; }
        public ObservableCollection<Track> Tracklist { get; set; }

        public RuntimeData()
        {
            TagSpaces = new Stack<TagSpace>();
            Tracklist = new ObservableCollection<Track>();
        }

        public RuntimeData(Stack<TagSpace> tagSpaces, ObservableCollection<Track> tracklist)
        {
            TagSpaces = tagSpaces;
            Tracklist = tracklist;
        }
    }
}
