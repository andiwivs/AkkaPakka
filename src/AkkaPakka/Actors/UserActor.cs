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

            _logger.WriteVerbose("Creating a PlaybackActor");

            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
            Receive<StopMovieMessage>(message => HandleStopMovieMessage());
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            _logger.WriteDebug($"Received play request for movie title {message.MovieTitle}, user {message.UserId}");

            // business logic rule: only one movie can be played at any one time
            if (_currentlyWatching != null)
            {
                _logger.WriteError("Cannot start playing another movie before stopping existing one");
            }
            else
            {
                StartPlayingMovie(message.MovieTitle);
            }
        }

        private void HandleStopMovieMessage()
        {
            _logger.WriteDebug("Received stop request");

            // business logic rule: can only stop a movie if one is playing
            if (_currentlyWatching == null)
            {
                _logger.WriteError("Cannot stop if nothing is playing");
            }
            else
            {
                StopPlayingMovie();
            }
        }

        private void StartPlayingMovie(string movieTitle)
        {
            _currentlyWatching = movieTitle;

            _logger.WriteSuccess($"User is currently watching {movieTitle}");
        }

        private void StopPlayingMovie()
        {
            _logger.WriteSuccess($"User has stopped watching {_currentlyWatching}");

            _currentlyWatching = null;
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
