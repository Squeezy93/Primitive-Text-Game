using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Statistics
{
    public interface IGameStateManager
    {
        public void SaveGameState(Character character);
        public GameState LoadGameState();
        public GameState LoadGameState(string nickname);
        public void ClearGameState(string nickname);
    }
}
