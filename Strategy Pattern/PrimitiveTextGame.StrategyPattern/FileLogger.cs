namespace PrimitiveTextGame.StrategyPattern
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(message);
            }
            Console.WriteLine(message);
        }

        public void StartNewLog()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
