namespace AkkaPakka
{
    public interface ILogger
    {
        void WriteVerbose(string message);
        void WriteSuccess(string message);
        void WriteError(string message);
        void WriteDebug(string message);
    }
}