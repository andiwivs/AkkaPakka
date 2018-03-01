using System;
using Akka.Actor;
using AkkaPakka.Messages;

namespace AkkaPakka.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private ILogger _logger;

        public PlaybackActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteVerbose("Creating a PlaybackActor");

            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            _logger.WriteSuccess($"Received play request for movie title {message.MovieTitle}, user {message.UserId}");
        }

        protected override void PreStart()
        {
            _logger.WriteDebug("PlaybackActor PreStart");

            base.PreStart();
        }

        protected override void PostStop()
        {
            _logger.WriteDebug("PlaybackActor PostStop");

            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.WriteError($"PlaybackActor PreRestart: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.WriteError($"PlaybackActor PostRestart: {reason}");

            base.PostRestart(reason);
        }
    }
}
