using PrimitiveTextGame.StrategyPattern.Characters;
using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.StrategyPattern.Weapons.Mage;

namespace PrimitiveTextGame.StrategyPattern
{
    public class Game
    {
        private readonly Random _random = new();
        private readonly Character _player;
        private readonly Character _enemy;
        private readonly ILogger _logger;
        
        private int _playerTurn = 0;
        private int _enemyTurn = 0;

        public Game(Character player, ILogger logger)
        {        
            _logger = logger;
            _player = player;
            _enemy = GenerateRandomCharacter();           
        }

        private Character GenerateRandomCharacter()
        {
            var characters = new Character[] { new Mage(), new Knight(), new Lumberjack() };
            var character = characters[_random.Next(characters.Length)];
            character.Weapon = GenerateRandomWeapon(character);

            return character;
        }

        private IWeapon GenerateRandomWeapon(Character character)
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

        public void StartBattle()
        {
            _logger.StartNewLog();
            _logger.Log($"Player chose: {_player.Name}");
            _logger.Log($"Enemy chose: {_player.Name}");

            while (_player.Health > 0 && _enemy?.Health > 0)
            {
                bool playerTurn = _random.Next(2) == 0;
                if (playerTurn)
                {
                    PlayerTurn();
                }
                else
                {
                    EnemyTurn();
                }
            }

            if(_player.Health <= 0)
            {
                _logger.Log($"Enemy won with {_enemy.Health} health remaining. Turns to win {_enemyTurn}.");
            }
            else
            {
                _logger.Log($"Player won with {_enemy.Health} health remaining. Turns to win {_enemyTurn}.");
            }           
        }

        private void DisplayAvailableWeapons(Character character)
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

        private IWeapon CreateWeapon(string weaponChoice, Character character)
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
                _ => throw new ArgumentException("Invalid weapon choice"),
            };
        }

        private void PlayerTurn()
        {
            Console.WriteLine("Choose your weapon:");
            DisplayAvailableWeapons(_player);

            var weaponChoice = Console.ReadLine();
            _player.Weapon = CreateWeapon(weaponChoice, _player);
            _player.Attack(_enemy);
            _playerTurn++;

            _logger.Log($"Player attacked with {_player.Weapon.Name}, dealing {_player.Weapon.Damage} damage. Enemy's health is now {_enemy.Health}.");
        }

        private void EnemyTurn()
        {
            _enemy.Weapon = GenerateRandomWeapon(_enemy);
            _enemy.Attack(_player);
            _enemyTurn++;      
            
            _logger.Log($"Enemy attacked with {_enemy.Weapon.Name}, dealing {_enemy.Weapon.Damage} damage. Player's health is now {_player.Health}.");
        }
    }
}
