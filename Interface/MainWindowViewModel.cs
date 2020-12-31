using Autofac;
using siof.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using TracklistParser;
using TracklistParser.Parser;

namespace Interface
{
    class MainWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region properties
        public string TracklistString { get; set; }
        public string ParserCode { get; set; }
        public string ErrorMessage { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public ObservableCollection<Track> ParsedTracks { get; set; }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region commands
        public ICommand ParseTracklist { get; set; }
        public ICommand SetInputAudioPath { get; set; }
        public ICommand SetOutputPath { get; set; }
        #endregion

        #region fields
        private readonly IContainer _container;
        private readonly TracklistManager _tracklistManager;
        private readonly TracklistParser.Managers.CommandManager _commandManager;
        private readonly CommandParser _commandParser;
        #endregion

        public MainWindowViewModel()
        {
            // commands
            ParseTracklist = new RelayCommand(
                x => {
                    Debug.WriteLine("Kinda works");
                    Debug.WriteLine(TracklistString + ParserCode);
                    },
                x => !(string.IsNullOrEmpty(ParserCode) || string.IsNullOrEmpty(TracklistString)));
            SetInputAudioPath = new RelayCommand(x => Debug.WriteLine("Set input was used"));
            SetOutputPath = new RelayCommand(x => Debug.WriteLine("Set output was used"));

            // container init
            _container = Program.CreateContainer();
            _tracklistManager = _container.Resolve<TracklistManager>();
            _commandManager = _container.Resolve<TracklistParser.Managers.CommandManager>();
            _commandParser = _container.Resolve<CommandParser>();

            ParsedTracks = _tracklistManager.Tracklist;
        }
    }
}
