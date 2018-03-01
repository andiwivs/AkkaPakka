using Akka.Actor;
using Akka.DI.AutoFac;
using AkkaPakka.Actors;
using AkkaPakka.Messages;
using Autofac;
using System;

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

                var userActorRef = system.ActorOf(resolver.Create<UserActor>(), "userActor");

                Console.WriteLine("Press a key to progress through the process...");

                Console.ReadKey();
                Logger.WriteVerbose("Sending a PlayMovieMessage (Akka Pakka: The Movie)");
                userActorRef.Tell(new PlayMovieMessage("Akka Pakka: The Movie", 42));

                Console.ReadKey();
                Logger.WriteVerbose("Sending a PlayMovieMessage (Partial Recall)");
                userActorRef.Tell(new PlayMovieMessage("Partial Recall", 99));

                Console.ReadKey();
                Logger.WriteVerbose("Sending a StopMovieMessage");
                userActorRef.Tell(new StopMovieMessage());

                Console.ReadKey();
                Logger.WriteVerbose("Sending another StopMovieMessage");
                userActorRef.Tell(new StopMovieMessage());
                
                Console.ReadKey();
                system.Terminate().GetAwaiter().GetResult();
                Logger.WriteDebug("Actor system terminated");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ColourConsole>().As<ILogger>();
            builder.RegisterType<UserActor>();
            builder.RegisterType<PlaybackActor>();

            Container = builder.Build();
        }

        static void ResolveLogger()
        {
            Logger = Container.Resolve<ILogger>();
        }
    }
}
