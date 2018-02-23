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
            ILogger logger = new ColourConsole();

            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem));
            logger.WriteVerbose("Actor system created");
            
            var playbackActorProps = Props.Create<PlaybackActor>();
            var playbackActorRef = MovieStreamingActorSystem.ActorOf(playbackActorProps, "playbackActor");
            
            playbackActorRef.Tell(new PlayMovieMessage("Akka Pakka: The Movie", 42));
            playbackActorRef.Tell(new PlayMovieMessage("Partial Recall", 99));
            playbackActorRef.Tell(new PlayMovieMessage("Boolean Lies", 77));
            playbackActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 1));

            // press any key to shut down the system...
            Console.ReadKey();

            MovieStreamingActorSystem.Terminate();
            logger.WriteVerbose("Actor system terminated");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
