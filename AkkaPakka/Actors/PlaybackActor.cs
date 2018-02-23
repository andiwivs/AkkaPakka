using Akka.Actor;
using AkkaPakka.Messages;
using System;

namespace AkkaPakka.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");

            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            Console.WriteLine($"Received play request for movie title {message.MovieTitle}, user {message.UserId}");
        }
    }
}
