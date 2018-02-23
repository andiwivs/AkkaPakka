using Akka.Actor;
using AkkaPakka.Messages;
using System;

namespace AkkaPakka.Actors
{
    public class PlaybackActor : UntypedActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");
        }

        protected override void OnReceive(object message)
        {
            if (message is PlayMovieMessage playMessage)
            {
                Console.WriteLine($"Received play request for movie title {playMessage.MovieTitle}, user {playMessage.UserId}");
            }
            else
            {
                // we should let the actor system know when a message cannot be processed
                Unhandled(message);
            }
        }
    }
}
