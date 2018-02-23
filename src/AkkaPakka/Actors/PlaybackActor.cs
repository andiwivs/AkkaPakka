using Akka.Actor;
using AkkaPakka.Messages;

namespace AkkaPakka.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private ILogger _logger;

        public PlaybackActor()
        {
            _logger = new ColourConsole();

            _logger.WriteVerbose("Creating a PlaybackActor");

            Receive<PlayMovieMessage>(HandlePlayMovieMessage, message => message.UserId == 42);
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            _logger.WriteDebug($"Received play request for movie title {message.MovieTitle}, user {message.UserId}");
        }
    }
}
