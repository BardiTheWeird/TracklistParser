using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Managers;

namespace TracklistParser.Modules
{
    class ManagersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TagSpaceManager>().AsSelf().SingleInstance();
            builder.RegisterType<TracklistManager>().AsSelf().SingleInstance();
            builder.RegisterType<CommandManager>().AsSelf().SingleInstance();
        }
    }
}
