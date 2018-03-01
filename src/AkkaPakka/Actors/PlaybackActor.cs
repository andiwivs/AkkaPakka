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
            _logger.WriteDebug($"Received play request for movie title {message.MovieTitle}, user {message.UserId}");
        }
    }
}
