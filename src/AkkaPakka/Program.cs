using Akka.Actor;
using Akka.DI.AutoFac;
using AkkaPakka.Actors;
using AkkaPakka.Messages;
using Autofac;
using System;
using Akka.Util.Internal;

namespace AkkaPakka
{
    class Program
    {
        private static IContainer Container;
        private static ILogger Logger;

        static void Main(string[] args)
        {
            ConfigureAutofac();
            ResolveLogger();
            
            using (var system = ActorSystem.Create("MovieStreamingActorSystem"))
            {
                Logger.WriteVerbose("Actor system created");

                var resolver = new AutoFacDependencyResolver(Container, system);

                var playbackActorRef = system.ActorOf(resolver.Create<PlaybackActor>(), "playbackActor");

                Logger.WriteVerbose("Finished registering actors");

                playbackActorRef.Tell(new PlayMovieMessage("Akka Pakka: The Movie", 42));
                playbackActorRef.Tell(new PlayMovieMessage("Partial Recall", 99));
                playbackActorRef.Tell(new PlayMovieMessage("Boolean Lies", 77));
                playbackActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 1));

                playbackActorRef.Tell(PoisonPill.Instance); // use a poison pill to force an actor instance to stop
                
                // press any key to shut down the system...
                Console.ReadKey();

                system.Terminate().GetAwaiter().GetResult();

                Logger.WriteVerbose("Actor system terminated");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ColourConsole>().As<ILogger>();
            builder.RegisterType<PlaybackActor>();

            Container = builder.Build();
        }

        static void ResolveLogger()
        {
            Logger = Container.Resolve<ILogger>();
        }
    }
}
