using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using TracklistParser.Behaviors;
using TracklistParser.Commands;
using TracklistParser.Managers;
using TracklistParser.Modules;
using TracklistParser.Parser;

namespace TracklistParser
{
    class Program
    {
        static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new RuntimeData());

            builder.RegisterModule<ManagersModule>();
            builder.RegisterModule<BehaviorsModule>();

            return builder.Build();
        }

        static void Main(string[] args)
        {
            var container = CreateContainer();

            string filepath1 = @"..\..\..\Input\testInput1.txt";
            string filepath2 = @"..\..\..\Input\testInput2.txt";

            string inputText = File.ReadAllText(filepath2);
            inputText = inputText.Replace("\r\n", "\n");

            var commandList = new List<ICommand>
            {
                new OpenTagSpace(),
                new SetTag("Album", "Bro Heiter!"),


                new SetStart(@"(?<=\n)\w"),
                new SetEnd(@"(?<=t)\n\n"),

                new ForEachSplitBy(@"\n", new List<ICommand>
                {
                    new OpenTagSpace(),
                    new SetTagMatch("Artist", @"(.+)(?=(\s\-\s))"),
                    new SetTagMatch("Title", @"(?<=(\s\-\s))(.+)"),
                    new AddTrack()
                }),

                new CloseTagSpace()
            };

            container.Resolve<CommandManager>().SetScope(new Scope(inputText));
            container.Resolve<CommandManager>().Execute(commandList);

            container.Resolve<TracklistManager>().PrintTracklist();
        }
    }
}
