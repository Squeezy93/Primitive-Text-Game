using Newtonsoft.Json;
using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Statistics
{
    public class JsonGameStateManager : IGameStateManager
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Game state was loaded. Character class is {gameState.Player.Class}. Hp = {gameState.Player.Health}");
            Console.ResetColor();

            return gameState;
        }

        public void ClearGameState(string n)
        {
            File.WriteAllText(_filePath, string.Empty);
        }

        public GameState LoadGameState(string nickname)
        {
            throw new NotImplementedException();
        }
    }
}
