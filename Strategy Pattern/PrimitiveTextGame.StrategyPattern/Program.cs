using PrimitiveTextGame.Statistics;

namespace PrimitiveTextGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IGameStateManager gameStateManager = new DatabaseGameStateManager();
            var gameManager = new GameManager(gameStateManager);
            gameManager.Initialize();
        }
    }
}
