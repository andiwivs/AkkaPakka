using Akka.Actor;
using AkkaPakka.Messages;
using System;

namespace AkkaPakka.Actors
{
    public class UserActor : ReceiveActor
    {
        private ILogger _logger;
        private string _currentlyWatching;

        public UserActor(ILogger logger)
        {
            _logger = logger;

            _logger.WriteDebug("Creating a UserActor");
            _logger.WriteDebug("Setting initial behaviour to Stopped");
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => _logger.WriteError("Cannot start playing another movie before stopping existing one"));
            Receive<StopMovieMessage>(message => StopPlayingMovie());

            _logger.WriteSuccess("UserActor has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            Receive<StopMovieMessage>(message => _logger.WriteError("Cannot stop if nothing is playing"));

            _logger.WriteSuccess("UserActor has now become Stopped");
        }

        private void StartPlayingMovie(string movieTitle)
        {
            _currentlyWatching = movieTitle;

            _logger.WriteSuccess($"User is currently watching {movieTitle}");

            Become(Playing);
        }

        private void StopPlayingMovie()
        {
            _logger.WriteSuccess($"User has stopped watching {_currentlyWatching}");

            _currentlyWatching = null;

            Become(Stopped);
        }

        protected override void PreStart()
        {
            _logger.WriteDebug("UserActor PreStart");

            base.PreStart();
        }

        protected override void PostStop()
        {
            _logger.WriteDebug("UserActor PostStop");

            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.WriteError($"UserActor PreRestart: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.WriteError($"UserActor PostRestart: {reason}");

            base.PostRestart(reason);
        }
    }
}
