using Akka.Actor;
using Akka.DI.Core;
using System;
using AkkaPakka.Exceptions;

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

        protected override SupervisorStrategy SupervisorStrategy()
        {
            var customStrategy = new OneForOneStrategy(exception =>
            {
                // for (simulated) currupt state, we need to force a restart to correct the state issue...
                if (exception is SimulatedCorruptStateException)
                {
                    _logger.WriteDebug("Supervisor strategy: Restart as (simulated) corrupt state detected");
                    return Directive.Restart;
                }

                // for a less significant issue, we can allow the actor to resume...
                if (exception is SimulatedTerribleMovieException)
                {
                    _logger.WriteDebug("Supervisor strategy: Resume as (simulated) low importance issue detected");
                    return Directive.Resume;
                }

                // otherwise, revert to default behaviour (ie to restart the actor)
                _logger.WriteDebug("Supervisor strategy: Restart as fallback behaviour");
                return Directive.Restart;
            });

            return customStrategy;
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
