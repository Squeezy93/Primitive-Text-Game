using Microsoft.Data.Sqlite;
using PrimitiveTextGame.Armors.Decorator;
using PrimitiveTextGame.Armors;
using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Armors.ArmorsType;

namespace PrimitiveTextGame.Statistics
{
    public class DatabaseGameStateManager : IGameStateManager
    {
        private readonly string _databasePath = @"C:\Обучение\Homework\Strategy Pattern\PrimitiveTextGame.StrategyPattern\charactersDatabase.db";

        public void ClearGameState()
        {
            DeleteTable(_databasePath, "BaseArmors");
            DeleteTable(_databasePath, "DecoratedArmors");
            DeleteTable(_databasePath, "Characters");
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"DataSource={_databasePath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                var sqlExpression = @"
                CREATE TABLE IF NOT EXISTS Characters (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Type TEXT NOT NULL,
                    Health INTEGER NOT NULL,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS DecoratedArmors (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CharacterId INTEGER NOT NULL,
                    ArmorType TEXT NOT NULL,
                    ArmorValue INTEGER NOT NULL,
                    ArmorName TEXT NOT NULL,
                    FOREIGN KEY(CharacterId) REFERENCES Characters(Id)
                );
                CREATE TABLE IF NOT EXISTS BaseArmors (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DecoratedArmorId INTEGER NOT NULL,
                    ArmorType TEXT NOT NULL,
                    ArmorValue INTEGER NOT NULL,
                    ArmorName TEXT NOT NULL,
                    FOREIGN KEY(DecoratedArmorId) REFERENCES DecoratedArmors(Id)
                );";
                command.CommandText = sqlExpression;
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица создана");
            }
        }

        public GameState LoadGameState()
        {
            using (var connection = new SqliteConnection($"DataSource={_databasePath}"))
            {
                connection.Open();

                var isCreatedCommand = connection.CreateCommand();
                isCreatedCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Characters'";
                var result = isCreatedCommand.ExecuteScalar();

                if(result != null)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Characters LIMIT 1";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string type = reader["Type"].ToString();
                            int health = Convert.ToInt32(reader["Health"]);
                            string name = reader["Name"].ToString();

                            Type characterType = Type.GetType(type);
                            Character character = (Character)Activator.CreateInstance(characterType);
                            character.SetHealth(health);

                            character.Armors = LoadArmors(connection, (long)reader["Id"]);

                            var gameState = new GameState()
                            {
                                Player = character
                            };

                            return gameState;
                        }
                    }                    
                }
                else
                {
                    return null;
                }

            }
            return null;
        }

        private List<BaseArmor> LoadArmors(SqliteConnection connection, long characterId)
        {
            var armors = new List<BaseArmor>();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM DecoratedArmors WHERE CharacterId = @CharacterId";
            command.Parameters.AddWithValue("@CharacterId", characterId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string type = reader["ArmorType"].ToString();
                    int value = Convert.ToInt32(reader["ArmorValue"]);
                    string name = reader["ArmorName"].ToString();

                    Type armorType = Type.GetType(type);
                    BaseArmor baseArmor = CreateBaseArmor(value);
                    ArmorDecorator decoratedArmor = (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor);

                    armors.Add(decoratedArmor);
                }
            }
            return armors;
        }

        private BaseArmor CreateBaseArmor(int armorValue)
        {
            return armorValue switch
            {
                10 => new LightArmor(),
                25 => new MediumArmor(),
                50 => new HeavyArmor(),
                _ => throw new InvalidOperationException($"Unknown armor value: {armorValue}")
            };
        }

        public void SaveGameState(Character character)
        {
            InitializeDatabase();

            ClearTableAndResetIdentity(_databasePath, "BaseArmors");
            ClearTableAndResetIdentity(_databasePath, "DecoratedArmors");
            ClearTableAndResetIdentity(_databasePath, "Characters");

            var characterName = character.Name;
            var characterHealth = character.Health;
            var characterType = character.GetType().FullName;
            using (var connection = new SqliteConnection($"DataSource={_databasePath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                var sqlExpression = $"INSERT INTO Characters (Type, Health, Name) " +
                    $"VALUES ('{characterType}', '{characterHealth}', '{characterName}');" +
                    $"SELECT last_insert_rowid();";
                command.CommandText = sqlExpression;

                var characterId = (long)command.ExecuteScalar();
                Console.WriteLine("Изменения в героя добавлены");

                foreach (var armor in character.Armors)
                {
                    AddDecoratedArmor(_databasePath, armor, characterId);
                }
            }
        }

        private void AddDecoratedArmor(string path, BaseArmor armor, long id)
        {
            using (var connection = new SqliteConnection($"DataSource={path}"))
            {
                connection.Open();

                var addCommand = connection.CreateCommand();
                addCommand.CommandText = $"INSERT INTO DecoratedArmors (CharacterId, ArmorType, ArmorValue, ArmorName)" +
                    $" VALUES ('{id}', '{armor.GetType().FullName}', '{armor.Value}', '{armor.Name}');" +
                    $"SELECT last_insert_rowid();";
                Console.WriteLine($"Armor added. Name = {armor.Name}");
                var decoratedArmorId = (long)addCommand.ExecuteScalar();

                if (armor is ArmorDecorator decorator)
                {
                    addCommand = connection.CreateCommand();
                    addCommand.CommandText = $"INSERT INTO BaseArmors (DecoratedArmorId, ArmorType, ArmorValue, ArmorName)" +
                    $" VALUES ('{decoratedArmorId}', '{decorator.GetBaseArmor().GetType().FullName}', '{decorator.GetBaseArmor().Value}', '{decorator.GetBaseArmor().Name}');";
                    addCommand.ExecuteNonQuery();
                }
            }
        }

        private  void ClearTableAndResetIdentity(string path, string tableName)
        {
            using (var connection = new SqliteConnection($"DataSource={path}"))
            {
                connection.Open();

                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText = $"DELETE FROM {tableName}";
                deleteCommand.ExecuteNonQuery();

                var vacuumCommand = connection.CreateCommand();
                vacuumCommand.CommandText = "VACUUM";
                vacuumCommand.ExecuteNonQuery();

                var resetCommand = connection.CreateCommand();
                resetCommand.CommandText = $"DELETE FROM sqlite_sequence WHERE name='{tableName}'";
                resetCommand.ExecuteNonQuery();
                Console.WriteLine($"Данные удалены. Название таблицы = {tableName}");
            }
        }

        private void DeleteTable(string path, string table)
        {
            using (var connection = new SqliteConnection($"DataSource={path}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                var sqlExpression = $"DROP TABLE IF EXISTS {table};";
                command.CommandText = sqlExpression;
                command.ExecuteNonQuery();
                Console.WriteLine($"Таблица удалена. Название = {table}");
            }
        }
    }
}
