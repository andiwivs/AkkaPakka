using Akka.Actor;
using System;
using System.Collections.Generic;
using AkkaPakka.Messages;

namespace AkkaPakka.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private ILogger _logger;
        private readonly Dictionary<string, int> _moviePlayCounts;

        public MoviePlayCounterActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteVerbose("Creating a MoviePlayCounterActor");

            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message =>
            {
                HandleIncrementMessage(message);
            });
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts[message.MovieTitle]++;
            }
            else
            {
                _moviePlayCounts.Add(message.MovieTitle, 1);
            }

            _logger.WriteDebug($"{Self.Path.Name}: {message.MovieTitle} has been watched {_moviePlayCounts[message.MovieTitle]} time(s)");
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
