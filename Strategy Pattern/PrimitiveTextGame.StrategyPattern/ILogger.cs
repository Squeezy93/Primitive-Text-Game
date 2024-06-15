namespace PrimitiveTextGame.StrategyPattern
{
    public interface ILogger
    {
        void StartNewLog();
        void Log(string message);
    }
}
