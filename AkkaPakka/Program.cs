using System;
using Akka.Actor;
using AkkaPakka.Actors;
using AkkaPakka.Messages;

namespace AkkaPakka
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem));
            Console.WriteLine("Actor system created");
            
            var playbackActorProps = Props.Create<PlaybackActor>();
            
            var playbackActorRef = MovieStreamingActorSystem.ActorOf(playbackActorProps, "playbackActor");

            var message = new PlayMovieMessage("Akka Pakka: The Movie", 42);
            playbackActorRef.Tell(message);

            Console.ReadLine();

            MovieStreamingActorSystem.Terminate();
        }
    }
}
