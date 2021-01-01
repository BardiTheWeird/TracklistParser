using Autofac;
using siof.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TracklistParser;
using TracklistParser.Behaviors;
using TracklistParser.Parser;
using TracklistParser.TrackView;

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
        public ObservableCollection<TrackObservable> ParsedTracks { get; set; }
        
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
                x => 
                {
                    try
                    {
                        TracklistString = TracklistString.Replace("\r\n", "\n");
                        ParserCode = ParserCode.Replace("\r\n", "\n");

                        _tracklistManager.Clear();
                        var commands = _commandParser.ParseCommandList(ParserCode);
                        _commandManager.SetScope(new Scope(TracklistString));
                        _commandManager.Execute(commands);

                        ParsedTracks = TrackObservable.CreateTrackObservables(_tracklistManager.Tracklist);
                    }
                    catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                        Debug.WriteLine("An exception has occured:");
                        Debug.WriteLine(e.Message);
                    }
                },
                x => !(string.IsNullOrEmpty(ParserCode) || string.IsNullOrEmpty(TracklistString)));

            SetInputAudioPath = new RelayCommand(x => Debug.WriteLine("Set input was used"));
            SetOutputPath = new RelayCommand(x => Debug.WriteLine("Set output was used"));

            // container init
            _container = Program.CreateContainer();
            _tracklistManager = _container.Resolve<TracklistManager>();
            _commandManager = _container.Resolve<TracklistParser.Managers.CommandManager>();
            _commandParser = _container.Resolve<CommandParser>();

            // fields
            ParsedTracks = new ObservableCollection<TrackObservable>();
        }
    }

    #region converters
    class HasAncestorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object parent = null;
            if (value != null && parameter != null &&
                parameter is Type && value is DependencyObject)
            {
                var control = value as DependencyObject;
                Type t = parameter as Type;
                parent = ParentFinder.FindParent(control, t);
            }
            return parent != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ParentFinder
    {
        public static object FindParent(DependencyObject child, Type parentType)
        {
            object parent = null;
            var logicalParent = LogicalTreeHelper.GetParent(child);
            var visualParent = VisualTreeHelper.GetParent(child);

            if (!(logicalParent == null && visualParent == null))
            {
                if (logicalParent != null && logicalParent.GetType() == parentType)
                    parent = logicalParent;
                else if (visualParent != null && visualParent.GetType() == parentType)
                    parent = visualParent;
                else
                {
                    if (visualParent != null)
                        parent = FindParent(visualParent, parentType);
                    if (parent == null && logicalParent != null)
                        parent = FindParent(logicalParent, parentType);
                }
            }
            return parent;
        }
    }
    #endregion
}
