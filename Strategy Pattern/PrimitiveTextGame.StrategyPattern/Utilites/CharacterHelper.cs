using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Characters.Classes;

namespace PrimitiveTextGame.Utilites
{
    public static class CharacterHelper
    {
        private static readonly Random _random = new Random();

        public static Character CreatePlayerCharacter()
        {
            Console.WriteLine("Choose your character: Mage, Knight, Lumberjack");
            var characterChoice = Console.ReadLine()?.ToLower(System.Globalization.CultureInfo.CurrentCulture);

            Character player = characterChoice switch
            {
                "mage" => new Mage(),
                "knight" => new Knight(),
                "lumberjack" => new Lumberjack(),
                _ => throw new ArgumentException("Character is unknown")
            };
            return player;
        }

        public static Character GenerateRandomCharacter()
        {
            var characters = new Character[] { new Mage(), new Knight(), new Lumberjack() };
            var character = characters[_random.Next(characters.Length)];

            return character;
        }
    }
}
