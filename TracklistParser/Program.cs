using Autofac;
using System;
using System.IO;
using TracklistParser.Parser;

namespace TracklistParser
{
    class Program
    {
        static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new RuntimeData());

            builder.RegisterType<TagSpaceManager>().AsSelf().SingleInstance();
            builder.RegisterType<TracklistManager>().AsSelf().SingleInstance();
            builder.RegisterType<TempParser>().As<IParser>().SingleInstance();

            return builder.Build();
        }

        static void Main(string[] args)
        {
            var container = CreateContainer();

            string filepath = @"..\..\..\Input\testInput1.txt";
            container.Resolve<IParser>().ParseText(filepath);
            container.Resolve<TracklistManager>().PrintTracklist();
        }
    }
}
