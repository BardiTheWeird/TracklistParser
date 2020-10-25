using Autofac;
using Autofac.Features.ResolveAnything;
using System;
using System.Collections.Generic;
using System.IO;
using TracklistParser.Behaviors;
using TracklistParser.Commands;
using TracklistParser.Config;
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

            builder.RegisterInstance(new CommandSpecificationManager(@"..\..\..\Config\CommandSpecificationCommaDelim.csv"));
            builder.RegisterInstance(new RuntimeData());

            builder.RegisterModule<ManagersModule>();
            builder.RegisterModule<BehaviorsModule>();

            builder.RegisterType<CommandParser>().AsSelf().SingleInstance();

            return builder.Build();
        }

        static void Main(string[] args)
        {
            var container = CreateContainer();

            string touhouCafeTracklist = @"..\..\..\Input\Tracklists\TouhouCafeTracklist.txt";
            string touhouCafeCommands = @"..\..\..\Input\Commands\TouhouCafeCommands.txt";

            string inputText = File.ReadAllText(touhouCafeTracklist);
            inputText = inputText.Replace("\r\n", "\n");

            string commandInput = File.ReadAllText(touhouCafeCommands);


            var commandList = container.Resolve<CommandParser>().ParseCommandList(commandInput);

            var commandManager = container.Resolve<CommandManager>();
            commandManager.SetScope(new Scope(inputText));
            commandManager.Execute(commandList);

            var tracklistManager = container.Resolve<TracklistManager>();
            tracklistManager.PrintTracklist();

            Console.WriteLine("Now I'm scared");
        }
    }
}
