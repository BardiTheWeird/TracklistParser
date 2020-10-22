using Autofac;
using System;

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

            return builder.Build();
        }

        static void Main(string[] args)
        {
            
        }
    }
}
