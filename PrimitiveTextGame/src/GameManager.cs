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
            Console.WriteLine("Please, enter your nickname");
            var nickname = Console.ReadLine();
            LoadGame(nickname);
            while (true)
            {
                if (!AskToContinue())
                {
                    if (AskToStartNewGame())
                    {
                        StartNewGame(nickname);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    LoadGame(nickname);
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
                Console.WriteLine("Invalid nickname. Please enter 'yes' or 'no'.");
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
                Console.WriteLine("Invalid nickname. Please enter 'yes' or 'no'.");
                return AskToStartNewGame();
            }
        }

        private void StartNewGame(string name)
        {
            var player = CharacterHelper.CreatePlayerCharacter();
            player.SetPlayer(new Player(name));
            var game = new Game(player, _gameStateManager);
            game.StartBattle();
        }

        private void LoadGame(string nickname)
        {
            var state = _gameStateManager.LoadGameState(nickname);
            if (state == null)
            {
                Console.WriteLine("Cannot load game, game not found. Starting a new game.");
                StartNewGame(nickname);
                return;
            }
            var game = new Game(state.Player, _gameStateManager);
            game.StartBattle();
        }
    }
}

