using PrimitiveTextGame.Characters;
using System.Text.Json;

namespace PrimitiveTextGame.Statistics
{
    public class GameStateManager
    {
        private readonly string _saveFilePath = "gameState.json";        

        public void SaveGameState(Character player)
        {
            var gameState = new GameState()
            {
                Player = player,
            };

            var jsonString = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_saveFilePath, jsonString);

        }

        public GameState LoadGameState()
        {
            if (!File.Exists(_saveFilePath))    
            {
                Console.WriteLine("No saved game found.");
                return null;
            }

            var jsonString = File.ReadAllText(_saveFilePath);
            return JsonSerializer.Deserialize<GameState>(jsonString);
        }

        public void ClearGameState()
        {
            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
            }
        }
    }
}
