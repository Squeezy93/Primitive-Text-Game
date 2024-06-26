namespace PrimitiveTextGame.Statistics
{
    public interface ILogger
    {
        void StartNewLog();
        void Log(string message);
    }
}
