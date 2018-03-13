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

        private static readonly string SystemName = "MovieStreamingActorSystem";
        private static readonly string PlaybackActorName = "Playback";
        private static readonly string UserCoordinatorActorName = "UserCoordinator";

        static void Main(string[] args)
        {
            ConfigureAutofac();
            ResolveLogger();
            
            using (var system = ActorSystem.Create(SystemName))
            {
                Logger.WriteVerbose("Actor system created");

                var resolver = new AutoFacDependencyResolver(Container, system);

                system.ActorOf(resolver.Create<PlaybackActor>(), PlaybackActorName);

                bool running = true;

                do
                {
                    Console.WriteLine("Enter a command and hit Enter");
                    Console.WriteLine("eg 'play <user id> <movie title>'");
                    Console.WriteLine("eg 'stop <user id>'");
                    Console.WriteLine("(enter 'exit' to quit)");

                    var command = Console.ReadLine();
                    
                    if (command.StartsWith("play", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var parts = command.Split(new[] {' '}, 3);
                        var userId = int.Parse(parts[1]);
                        var movieTitle = parts[2];

                        var message = new PlayMovieMessage(movieTitle, userId);
                        system.ActorSelection($"/user/{PlaybackActorName}/{UserCoordinatorActorName}").Tell(message);
                    }

                    if (command.StartsWith("stop", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var parts = command.Split(new[] { ' ' }, 2);
                        var userId = int.Parse(parts[1]);

                        var message = new StopMovieMessage(userId);
                        system.ActorSelection($"/user/{PlaybackActorName}/{UserCoordinatorActorName}").Tell(message);
                    }

                    if (command.StartsWith("exit", StringComparison.InvariantCultureIgnoreCase))
                    {
                        running = false;
                    }

                } while (running);

                system.Terminate().GetAwaiter().GetResult();
                Logger.WriteDebug("Actor system terminated");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ColourConsoleLogger>().As<ILogger>();
            builder.RegisterType<PlaybackActor>();
            builder.RegisterType<UserCoordinatorActor>();
            builder.RegisterType<UserActor>();
            builder.RegisterType<PlaybackStatisticsActor>();
            builder.RegisterType<MoviePlayCounterActor>();

            Container = builder.Build();
        }

        static void ResolveLogger()
        {
            Logger = Container.Resolve<ILogger>();
        }
    }
}
