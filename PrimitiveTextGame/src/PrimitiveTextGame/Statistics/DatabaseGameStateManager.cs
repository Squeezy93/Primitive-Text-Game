using Microsoft.Data.Sqlite;
using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Statistics
{
    public class DatabaseGameStateManager : IGameStateManager
    {
        private readonly string _databasePath = @"C:\Обучение\Homework\PrimitiveTextGame\src\charactersDatabase.db";
        private readonly ICharacterRepository _characterRepository;

        public DatabaseGameStateManager(ICharacterRepository characterRepository)
        {
            characterRepository = new CharacterRepository(_databasePath);
            _characterRepository = characterRepository;
        }

        public void ClearGameState(string nickname)
        {
            _characterRepository.Delete(nickname);
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"DataSource={_databasePath}"))
            {
                connection.Open();

                string[] sqlExpressions =
                {
                    @"
                    CREATE TABLE IF NOT EXISTS Players (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PlayerName TEXT NOT NULL UNIQUE
                    );",

                    @"
                    CREATE TABLE IF NOT EXISTS Characters (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PlayerId INTEGER NOT NULL,
                        Type TEXT NOT NULL,
                        Health INTEGER NOT NULL,
                        Class TEXT NOT NULL,
                        FOREIGN KEY(PlayerId) REFERENCES Players(Id) ON DELETE CASCADE
                    );",

                    @"
                    CREATE TABLE IF NOT EXISTS DecoratedArmors (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CharacterId INTEGER NOT NULL,
                        ArmorType TEXT NOT NULL,
                        ArmorValue INTEGER NOT NULL,
                        ArmorName TEXT NOT NULL,
                        FOREIGN KEY(CharacterId) REFERENCES Characters(Id) ON DELETE CASCADE
                    );",

                    @"
                    CREATE TABLE IF NOT EXISTS BaseArmors (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DecoratedArmorId INTEGER NOT NULL,
                        ArmorType TEXT NOT NULL,
                        ArmorValue INTEGER NOT NULL,
                        ArmorName TEXT NOT NULL,
                        FOREIGN KEY(DecoratedArmorId) REFERENCES DecoratedArmors(Id) ON DELETE CASCADE
                     );"
                };

                foreach (string sqlExpression in sqlExpressions)
                {
                    using var command = new SqliteCommand(sqlExpression, connection);
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            Console.WriteLine("Таблицы созданы");
        }

        public GameState LoadGameState(string nickname)
        {            
            if(!IsCharacterExisted(nickname))
            {
                Console.WriteLine($"Cannot find player with name {nickname}");
                return null;
            }
            else
            {
                var character = _characterRepository.Get(nickname).Result;
                return new GameState
                {
                    Player = character
                };
            }
        }       

        public void SaveGameState(Character character)
        {
            if(IsCharacterExisted(character.Player.PlayerName))
            {
                _characterRepository.Update(character);
            }
            else
            {
                _characterRepository.Add(character);
            }
        }

        public void ClearTableAndResetIdentity(string path, string tableName)
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

        public void DeleteTable(string path, string tableName)
        {
            using (var connection = new SqliteConnection($"DataSource={path}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                var sqlExpression = $"DROP TABLE IF EXISTS {tableName};";
                command.CommandText = sqlExpression;
                command.ExecuteNonQuery();
                Console.WriteLine($"Таблица удалена. Название = {tableName}");
            }
        }

        public bool IsCharacterExisted(string name)
        {
            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                using (var findCharacterCommand = connection.CreateCommand())
                {
                    findCharacterCommand.CommandText = "SELECT COUNT(*) FROM Players WHERE LOWER(PlayerName) = LOWER(@PlayerName)";
                    findCharacterCommand.Parameters.AddWithValue("@PlayerName", name.ToLower());

                    long count = (long)findCharacterCommand.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public GameState LoadGameState()
        {
            throw new NotImplementedException();
        }
    }
}
