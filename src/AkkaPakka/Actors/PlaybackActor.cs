﻿using Akka.Actor;
using Akka.DI.Core;
using System;

namespace AkkaPakka.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private ILogger _logger;

        public PlaybackActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteVerbose("Creating a PlaybackActor");
            
            var userCoordinatorActorProps = Context.DI().Props<UserCoordinatorActor>();
            var playbackStatisticsActorProps = Context.DI().Props<PlaybackStatisticsActor>();

            Context.ActorOf(userCoordinatorActorProps, "UserCoordinator");
            Context.ActorOf(playbackStatisticsActorProps, "PlaybackStatistics");
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
