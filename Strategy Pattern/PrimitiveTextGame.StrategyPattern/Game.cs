using PrimitiveTextGame.Statistics;
using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame
{
    public class Game
    {
        private readonly Random _random = new();
        private readonly GameStateManager _gameStateManager = new();
        private Character _player;
        private Character _enemy;

        private int _playerTurn = 0;
        private int _enemyTurn = 0;
        private int _playerTurnStreak = 0;

        public Game(Character player)
        {
            _player = player;
        }

        public void StartBattle()
        {
            _enemy = CharacterHelper.GenerateRandomCharacter();

            Console.WriteLine($"Player chose {_player.Name}");
            Console.WriteLine($"Enemy chose {_enemy.Name}");

            if (_player.Armors.Count == 0)
            {
                ArmorHelper.GenerateArmor(_player, 3);
                foreach (var armor in _player.Armors)
                {
                    Console.WriteLine($"Player gained {armor.Name} with {armor.Value}% damage reduce.");
                }
            }
            else
            {
                foreach (var armor in _player.Armors)
                {
                    Console.WriteLine($"Player gained {armor.Name} with {armor.Value} damage reduce.");
                }
            }

            while (_player.Health > 0 && _enemy?.Health > 0)
            {
                bool playerTurn = _random.Next(2) == 0;

                ExecuteTurn(playerTurn);
            }

            if (_player.Health <= 0)
            {
                Console.WriteLine($"Enemy won with {_enemy?.Health} health remaining. Turns to win {_enemyTurn}.");
            }
            else
            {
                _gameStateManager.SaveGameState(_player);
                Console.WriteLine($"Player won with {_player.Health} health remaining. Turns to win {_playerTurn}.");
            }
        }

        private void ExecuteTurn(bool playerTurn)
        {
            if (playerTurn)
            {
                HandlePlayerTurn();
            }
            else
            {
                HandleEnemyTurn();
            }
        }

        private void HandleEnemyTurn()
        {
            _playerTurnStreak = 0;
            EnemyTurn();
        }

        private void HandlePlayerTurn()
        {

            _playerTurnStreak++;

            if (_playerTurnStreak == 2)
            {
                HandlePlayerStreak();
            }

            PlayerTurn();
        }

        private void HandlePlayerStreak()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Streak!");
            Console.ResetColor();

            if (_enemy.Weapon == null)
            {
                var weapon = WeaponHelper.GenerateRandomWeapon(_enemy);
                _enemy.SetWeapon(weapon);
            }

            ArmorHelper.UpgradeArmor(_enemy.Weapon, _player);
        }

        private void PlayerTurn()
        {
            Console.WriteLine("Choose your weapon:");
            WeaponHelper.DisplayAvailableWeapons(_player);

            var weaponChoice = Console.ReadLine();
            var weapon = WeaponHelper.CreateWeapon(weaponChoice);
            _player.SetWeapon(weapon);
            _player.Attack(_enemy);
            _playerTurn++;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Player attacked with {_player.Weapon.Name}, dealing {_player.Weapon.Damage} damage. Enemy's health is now {_enemy.Health}.");
            Console.ResetColor();
        }

        private void EnemyTurn()
        {
            var weapon = WeaponHelper.GenerateRandomWeapon(_enemy);
            _enemy.SetWeapon(weapon);
            _enemy.Attack(_player);
            _enemyTurn++;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enemy attacked with {_enemy.Weapon.Name}, dealing {_enemy.Weapon.Damage} damage. Player's health is now {_player.Health}.");
            Console.ResetColor();
        }
    }
}