using PrimitiveTextGame.Statistics;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame
{
    public class GameManager
    {
        private readonly IGameStateManager _gameStateManager;

        public GameManager(IGameStateManager gameStateManager) 
        { 
            _gameStateManager = gameStateManager;
        }

        public void Initialize()
        {
            while (true)
            {
                if (!AskToContinue())
                {
                    if (AskToStartNewGame())
                    {
                        StartNewGame();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    LoadGame();
                }
            }
        }

        private bool AskToContinue()
        {
            Console.WriteLine("Would you like to continue? (yes/no)");
            var input = Console.ReadLine()?.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (input == "yes")
            {
                return true;
            }
            else if (input == "no")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                return AskToContinue();
            }
        }

        private bool AskToStartNewGame()
        {
            Console.WriteLine("Would you like to start a new game? (yes/no)");
            var input = Console.ReadLine()?.ToLower(System.Globalization.CultureInfo.CurrentCulture);
            if (input == "yes")
            {
                return true;
            }
            else if (input == "no")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                return AskToStartNewGame();
            }
        }

        private void StartNewGame()
        {
            _gameStateManager.ClearGameState();
            var player = CharacterHelper.CreatePlayerCharacter();
            var game = new Game(player, _gameStateManager);
            game.StartBattle();
        }

        private void LoadGame()
        {
            var state = _gameStateManager.LoadGameState();
            if (state == null)
            {
                Console.WriteLine("Cannot load game, game not found. Starting a new game.");
                StartNewGame();
                return;
            }
            var game = new Game(state.Player, _gameStateManager);
            game.StartBattle();
        }
    }
}

