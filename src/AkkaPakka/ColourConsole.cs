using System;

namespace AkkaPakka
{
    public interface ILogger
    {
        void WriteVerbose(string message);
        void WriteSuccess(string message);
        void WriteError(string message);
        void WriteDebug(string message);
    }

    public class ColourConsole : ILogger
    {
        public void WriteVerbose(string message)
        {
            Console.WriteLine(DecorateMessage(message, "Verbose"));
        }

        public void WriteSuccess(string message)
        {
            var beforeColour = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(DecorateMessage(message, "Success"));
            Console.ForegroundColor = beforeColour;
        }

        public void WriteError(string message)
        {
            var beforeColour = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DecorateMessage(message, "Error"));
            Console.ForegroundColor = beforeColour;
        }

        public void WriteDebug(string message)
        {
            var beforeColour = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(DecorateMessage(message, "Debug"));
            Console.ForegroundColor = beforeColour;
        }

        private string DecorateMessage(string message, string level)
        {
            return $"{DateTime.UtcNow} [{level}]: {message}";
        }
    }
}
