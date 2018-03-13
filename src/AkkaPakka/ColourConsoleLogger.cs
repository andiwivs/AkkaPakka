using System;

namespace AkkaPakka
{
    public class ColourConsoleLogger : ILogger
    {
        private static readonly object ConsoleWriterLock = new object();

        public void WriteVerbose(string message)
        {
            lock (ConsoleWriterLock)
            {
                Console.WriteLine(DecorateMessage(message, "Verbose"));
            }
        }

        public void WriteSuccess(string message)
        {
            lock (ConsoleWriterLock)
            {
                var beforeColour = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(DecorateMessage(message, "Success"));
                Console.ForegroundColor = beforeColour;
            }
        }

        public void WriteError(string message)
        {
            lock (ConsoleWriterLock)
            {
                var beforeColour = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DecorateMessage(message, "Error"));
                Console.ForegroundColor = beforeColour;
            }
        }

        public void WriteDebug(string message)
        {
            lock (ConsoleWriterLock)
            {
                var beforeColour = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(DecorateMessage(message, "Debug"));
                Console.ForegroundColor = beforeColour;
            }
        }

        private string DecorateMessage(string message, string level)
        {
            return $"{DateTime.UtcNow} [{level}]: {message}";
        }
    }
}
