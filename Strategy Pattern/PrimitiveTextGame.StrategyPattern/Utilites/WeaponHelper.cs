using PrimitiveTextGame.Characters.Classes;
using PrimitiveTextGame.Characters;
using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.StrategyPattern.Weapons.Mage;

namespace PrimitiveTextGame.Utilites
{
    public static class WeaponHelper
    {
        private static Random _random = new Random();

        public static bool IsWeaponType(IWeapon weapon, Type weaponType)
        {
            return weapon.GetType() == weaponType;
        }

        public static IWeapon GenerateRandomWeapon(Character character)
        {
            IWeapon[] weapons;

            if (character is Mage)
                weapons = [new Lightning(), new Fire(), new Dagger()];
            else if (character is Knight)
                weapons = [new Spear(), new Knife(), new Sword()];
            else
                weapons = [new Axe(), new Log(), new BareHands()];

            return weapons[_random.Next(weapons.Length)];
        }

        public static void DisplayAvailableWeapons(Character character)
        {
            IWeapon[] weapons;

            if (character is Mage)
                weapons = [new Lightning(), new Fire(), new Dagger()];
            else if (character is Knight)
                weapons = [new Spear(), new Knife(), new Sword()];
            else
                weapons = [new Axe(), new Log(), new BareHands()];

            foreach (var weapon in weapons)
            {
                Console.WriteLine($"{weapon.Name} (Damage: {weapon.Damage})");
            }
        }

        public static IWeapon CreateWeapon(string weaponChoice)
        {
            while (true) 
            {
                return weaponChoice.ToLower(System.Globalization.CultureInfo.CurrentCulture) switch
                {
                    "lightning" => new Lightning(),
                    "fire" => new Fire(),
                    "dagger" => new Dagger(),
                    "spear" => new Spear(),
                    "knife" => new Knife(),
                    "sword" => new Sword(),
                    "axe" => new Axe(),
                    "log" => new Log(),
                    "barehands" => new BareHands(),
                    _ => RetryWeaponSelection()
                };
            }
        }

        private static IWeapon RetryWeaponSelection()
        {
            Console.WriteLine("Invalid weapon choice, please choose again.");
            string weaponChoice = Console.ReadLine();
            return CreateWeapon(weaponChoice);
        }
    }
}
