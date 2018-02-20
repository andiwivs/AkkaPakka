using System;
using Akka.Actor;

namespace AkkaPakka
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create(nameof(MovieStreamingActorSystem));

            Console.ReadLine();

            MovieStreamingActorSystem.Terminate();
        }
    }
}
