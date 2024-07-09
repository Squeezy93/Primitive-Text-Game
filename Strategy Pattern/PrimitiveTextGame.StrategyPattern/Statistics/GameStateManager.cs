using Newtonsoft.Json;
using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Statistics
{
    public class GameStateManager
    {
        private readonly string _filePath = "gameState.json";

        public void SaveGameState(Character character)
        {
            var gameState = new GameState()
            {
                Player = character,
            };

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new CharacterConverter() }
            };

            var jsonString = JsonConvert.SerializeObject(gameState, settings);
            File.WriteAllText(_filePath, jsonString);
            Console.WriteLine("Game state was saved with health: " + character.Health);
        }

        public GameState LoadGameState()
        {
            var jsonString = File.ReadAllText(_filePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new CharacterConverter() }
            };
            GameState gameState = JsonConvert.DeserializeObject<GameState>(jsonString, settings);
            if (gameState == null) 
            {
                return null;
            }
            Console.WriteLine("Game state was loaded with health: " + gameState.Player.Health);
            return gameState;
        }

        public void ClearGameState()
        {
            File.WriteAllText(_filePath, string.Empty);
            Console.WriteLine("Game state has been cleared.");
        }

    }
}
