using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TracklistParser.TrackView
{
    public static class DictionaryExtensions
    {
        public static T2 GetValueOrDefault<T1, T2>(this Dictionary<T1, T2> dict, T1 key)
        {
            if (dict.TryGetValue(key, out var val))
                return val;
            return default(T2);
        }
    }

    public class TrackObservable : INotifyPropertyChanged
    {
        public static ObservableCollection<TrackObservable> CreateTrackObservables(List<Track> tracklist)
        {
            var observables = new ObservableCollection<TrackObservable>();
            for (int i = 0; i < tracklist.Count; i++)
            {
                var track = tracklist[i];
                var tags = track.Tags;

                var obsTrack = new TrackObservable();
                obsTrack.StartTime = track.StartTime;
                obsTrack.TrackNumber = i + 1;

                obsTrack.Title = tags.GetValueOrDefault(nameof(Title));
                obsTrack.Artist = tags.GetValueOrDefault(nameof(Artist));
                obsTrack.Album = tags.GetValueOrDefault(nameof(Album));
                obsTrack.Genre = tags.GetValueOrDefault(nameof(Genre));

                if (int.TryParse(tags.GetValueOrDefault(nameof(Year)), out var res))
                    obsTrack.Year = res;

                var otherTags = tags.Where(
                    x => x.Key != "Artist"
                    && x.Key != "Album"
                    && x.Key != "Year"
                    && x.Key != "Genre"
                    && x.Key != "Title");

                var sb = new StringBuilder();
                foreach (var tag in otherTags)
                    sb.Append($"{tag.Key} = \"{tag.Value}\"; ");

                obsTrack.OtherTags = sb.ToString().Trim();

                observables.Add(obsTrack);
            }
            return observables;
        }

        public Index StartTime { get; set; }
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string OtherTags { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
