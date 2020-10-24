using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TracklistParser.Behaviors;
using TracklistParser.Commands;

namespace TracklistParser.Modules
{
    class BehaviorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddTrackBehavior>().Keyed<ICommandBehavior>(typeof(AddTrack));
            builder.RegisterType<CloseTagSpaceBehavior>().Keyed<ICommandBehavior>(typeof(CloseTagSpace));
            builder.RegisterType<ForEachSplitByBehavior>().Keyed<ICommandBehavior>(typeof(ForEachSplitBy));
            builder.RegisterType<OpenTagSpaceBehavior>().Keyed<ICommandBehavior>(typeof(OpenTagSpace));
            builder.RegisterType<SetEndBehavior>().Keyed<ICommandBehavior>(typeof(SetEnd));
            builder.RegisterType<SetStartBehavior>().Keyed<ICommandBehavior>(typeof(SetStart));
            builder.RegisterType<SetTagBehavior>().Keyed<ICommandBehavior>(typeof(SetTag));
            builder.RegisterType<SetTagMatchBehavior>().Keyed<ICommandBehavior>(typeof(SetTagMatch));
        }
    }
}
