using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Statistics;

namespace PrimitiveTextGame
{
    internal class Program
    {
        static void Main(string[] args)
        {    
            var _path = @"C:\Обучение\Homework\PrimitiveTextGame\src\charactersDatabase.db";            
            var characterRepository = new CharacterRepository(_path);
            DatabaseGameStateManager gameStateManager = new DatabaseGameStateManager(characterRepository);
            gameStateManager.InitializeDatabase();
            var gameManager = new GameManager(gameStateManager);
            gameManager.Initialize();
        }
    }
}
