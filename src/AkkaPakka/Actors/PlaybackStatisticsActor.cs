using Akka.Actor;
using System;
using Akka.DI.Core;

namespace AkkaPakka.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        private ILogger _logger;

        public PlaybackStatisticsActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteVerbose("Creating a PlaybackStatisticsActor");
            
            var moviePlayCounterActorProps = Context.DI().Props<MoviePlayCounterActor>();

            Context.ActorOf(moviePlayCounterActorProps, "MoviePlayCounter");
        }

        #region lifecycle hooks

        protected override void PreStart()
        {
            _logger.WriteDebug($"{Self.Path.Name} PreStart");

            base.PreStart();
        }

        protected override void PostStop()
        {
            _logger.WriteDebug($"{Self.Path.Name} PostStop");

            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.WriteError($"{Self.Path.Name} PreRestart: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.WriteError($"{Self.Path.Name} PostRestart: {reason}");

            base.PostRestart(reason);
        }

        #endregion
    }
}
