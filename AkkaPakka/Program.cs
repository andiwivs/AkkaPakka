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
            
            playbackActorRef.Tell(new PlayMovieMessage("Akka Pakka: The Movie", 42));
            playbackActorRef.Tell(new PlayMovieMessage("Movie for user who should not be able to play", 666));

            Console.ReadLine();

            MovieStreamingActorSystem.Terminate();
        }
    }
}
