using PrimitiveTextGame.StrategyPattern.Characters;

namespace PrimitiveTextGame.StrategyPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose your character: Mage, Knight, Lumberjack");
            var characterChoice = Console.ReadLine().ToLower(System.Globalization.CultureInfo.CurrentCulture);

            Character playerCharacter = characterChoice switch
            {
                "mage" => new Mage(),
                "knight" => new Knight(),
                "lumberjack" => new Lumberjack(),
                _ => throw new ArgumentException("Character is unknown")
            };

            var logger = new FileLogger("log.txt");
            var game = new Game(playerCharacter, logger);
            game.StartBattle();
        }
    }
}
