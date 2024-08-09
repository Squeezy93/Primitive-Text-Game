using Microsoft.Data.Sqlite;
using PrimitiveTextGame.Armors;
using PrimitiveTextGame.Armors.ArmorsType;
using PrimitiveTextGame.Armors.Decorator;

namespace PrimitiveTextGame.Characters
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly string _path;

        public CharacterRepository(string path)
        {
            _path = path;
        }

        public void Add(Character character)
        {
            using (var connection = new SqliteConnection($"Data Source={_path}"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        AddPlayer(character, connection, transaction);
                        var playerId = GetLastInsertRowId(connection, transaction);

                        AddCharacter(character, connection, transaction, playerId);
                        var characterId = GetLastInsertRowId(connection, transaction);

                        foreach (var armor in character.Armors)
                        {
                            AddDecoratedArmor(connection, transaction, armor, characterId);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        private void AddCharacter(Character character, SqliteConnection connection, SqliteTransaction transaction, long playerId)
        {
            using (var addCharacterCommand = connection.CreateCommand())
            {
                addCharacterCommand.Transaction = transaction;
                addCharacterCommand.CommandText = "INSERT INTO Characters (PlayerId, Type, Health, Class) " +
                    "VALUES (@PlayerId, @Type, @Health, @Class)";
                addCharacterCommand.Parameters.AddWithValue("@PlayerId", playerId);
                addCharacterCommand.Parameters.AddWithValue("@Type", character.GetType().FullName);
                addCharacterCommand.Parameters.AddWithValue("@Health", character.Health);
                addCharacterCommand.Parameters.AddWithValue("@Class", character.Class);
                addCharacterCommand.ExecuteNonQuery();
            }
        }

        private void AddPlayer(Character character, SqliteConnection connection, SqliteTransaction transaction)
        {
            using (var addPlayerCommand = connection.CreateCommand())
            {
                addPlayerCommand.Transaction = transaction;
                addPlayerCommand.CommandText = "INSERT INTO Players (PlayerName) VALUES (@PlayerName)";
                addPlayerCommand.Parameters.AddWithValue("@PlayerName", character.Player.PlayerName);
                addPlayerCommand.ExecuteNonQuery();
            }
        }

        private void AddDecoratedArmor(SqliteConnection connection, SqliteTransaction transaction, BaseArmor armor, long characterId)
        {
            using (var addDecoratedArmorCommand = connection.CreateCommand())
            {
                addDecoratedArmorCommand.Transaction = transaction;
                addDecoratedArmorCommand.CommandText = "" +
                    "INSERT INTO DecoratedArmors (CharacterId, ArmorType, ArmorValue, ArmorName) " +
                    "VALUES (@CharacterId, @ArmorType, @ArmorValue, @ArmorName)";
                addDecoratedArmorCommand.Parameters.AddWithValue("@CharacterId", characterId);
                addDecoratedArmorCommand.Parameters.AddWithValue("@ArmorType", armor.GetType().FullName);
                addDecoratedArmorCommand.Parameters.AddWithValue("@ArmorValue", armor.Value);
                addDecoratedArmorCommand.Parameters.AddWithValue("@ArmorName", armor.Name);
                addDecoratedArmorCommand.ExecuteNonQuery();
            }

            var armorId = GetLastInsertRowId(connection, transaction);
            if (armor is ArmorDecorator decoratedArmor)
            {
                AddBaseArmor(connection, transaction, decoratedArmor, armorId);
            }
        }

        private void AddBaseArmor(SqliteConnection connection, SqliteTransaction transaction, ArmorDecorator decoratedArmor, long armorId)
        {
            var baseArmor = decoratedArmor.GetBaseArmor();
            using (var addBaseArmorCommand = connection.CreateCommand())
            {
                addBaseArmorCommand.Transaction = transaction;
                addBaseArmorCommand.CommandText = "INSERT INTO BaseArmors (DecoratedArmorId, ArmorType, ArmorValue, ArmorName) " +
                    "VALUES (@DecoratedArmorId, @ArmorType, @ArmorValue, @ArmorName)";
                addBaseArmorCommand.Parameters.AddWithValue("@DecoratedArmorId", armorId);
                addBaseArmorCommand.Parameters.AddWithValue("@ArmorType", baseArmor.GetType().FullName);
                addBaseArmorCommand.Parameters.AddWithValue("@ArmorValue", baseArmor.Value);
                addBaseArmorCommand.Parameters.AddWithValue("@ArmorName", baseArmor.Name);
                addBaseArmorCommand.ExecuteNonQuery();
            }
        }

        private long GetLastInsertRowId(SqliteConnection connection, SqliteTransaction transaction)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "SELECT last_insert_rowid()";
                return (long)command.ExecuteScalar();
            }
        }

        public void Update(Character character)
        {
            using (var connection = new SqliteConnection($"Data Source={_path}"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var playerId = GetPlayerId(character.Player.PlayerName, connection, transaction);
                        UpdateCharacter(character, connection, transaction, playerId);
                        var characterId = GetCharacterId(playerId, connection, transaction);
                        var decoratedArmorIds = GetDecoratedArmorIds(characterId, connection, transaction);
                        UpdateDecoratedArmor(connection, transaction, characterId, character.Armors, decoratedArmorIds);                        

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        private void UpdateDecoratedArmor(SqliteConnection connection, SqliteTransaction transaction, long characterId, List<BaseArmor> armors, List<long> decoratedArmorIds)
        {
            for (int i = 0; i < armors.Count; i++)
            {
                if (i > decoratedArmorIds.Count - 1) 
                {
                    AddDecoratedArmor(connection, transaction, armors[i], characterId);
                    continue;
                }
                using (var updateDecoratedArmorCommand = connection.CreateCommand())
                {
                    updateDecoratedArmorCommand.Transaction = transaction;
                    updateDecoratedArmorCommand.CommandText = "UPDATE DecoratedArmors SET " +
                                     "CharacterId = @CharacterId, " +
                                     "ArmorType = @NewArmorType, " +
                                     "ArmorValue = @NewArmorValue, " +
                                     "ArmorName = @NewArmorName " +
                                     "WHERE Id = @Id";
                    updateDecoratedArmorCommand.Parameters.AddWithValue("@CharacterId", characterId);
                    updateDecoratedArmorCommand.Parameters.AddWithValue("@NewArmorType", armors[i].GetType().FullName);
                    updateDecoratedArmorCommand.Parameters.AddWithValue("@NewArmorValue", armors[i].Value);
                    updateDecoratedArmorCommand.Parameters.AddWithValue("@NewArmorName", armors[i].Name);
                    updateDecoratedArmorCommand.Parameters.AddWithValue("@Id", decoratedArmorIds[i]);
                    var rowsAffecter = updateDecoratedArmorCommand.ExecuteNonQuery();
                    if (rowsAffecter > 0 && armors[i] is ArmorDecorator decorator)
                    {
                        UpdateBaseArmor(connection, transaction, decoratedArmorIds, i, decorator);
                    }
                }
            }           
        }

        private void UpdateBaseArmor(SqliteConnection connection, SqliteTransaction transaction, List<long> decoratedArmorIds, int i, ArmorDecorator decorator)
        {
            var baseArmor = decorator.GetBaseArmor();
            var baseArmorId = GetBaseArmorId(decoratedArmorIds[i], connection, transaction);
            using (var updateBaseArmorCommand = connection.CreateCommand())
            {

                updateBaseArmorCommand.Transaction = transaction;
                updateBaseArmorCommand.CommandText = "UPDATE BaseArmors SET " +
                         "DecoratedArmorId = @DecoratedArmorId, " +
                         "ArmorType = @NewArmorType, " +
                         "ArmorValue = @NewArmorValue, " +
                         "ArmorName = @NewArmorName " +
                         "WHERE Id = @Id";
                updateBaseArmorCommand.Parameters.AddWithValue("@DecoratedArmorId", decoratedArmorIds[i]);
                updateBaseArmorCommand.Parameters.AddWithValue("@NewArmorType", baseArmor.GetType().FullName);
                updateBaseArmorCommand.Parameters.AddWithValue("@NewArmorValue", baseArmor.Value);
                updateBaseArmorCommand.Parameters.AddWithValue("@NewArmorName", baseArmor.Name);
                updateBaseArmorCommand.Parameters.AddWithValue("@Id", baseArmorId);
                updateBaseArmorCommand.ExecuteNonQuery();
            }
        }

        private void UpdateCharacter(Character character, SqliteConnection connection, SqliteTransaction transaction, long playerId)
        {
            using (var characterUpdateCommand = connection.CreateCommand())
            {
                characterUpdateCommand.Transaction = transaction;
                characterUpdateCommand.CommandText = "UPDATE Characters SET " +
                    "Type = @NewType, " +
                    "Health = @NewHealth, " +
                    "Class = @NewClass " +
                    "WHERE PlayerId = " +
                    "@PlayerId";
                characterUpdateCommand.Parameters.AddWithValue("@PlayerId", playerId);
                characterUpdateCommand.Parameters.AddWithValue("@NewType", character.GetType().FullName);
                characterUpdateCommand.Parameters.AddWithValue("@NewHealth", character.Health);
                characterUpdateCommand.Parameters.AddWithValue("@NewClass", character.Class);
                characterUpdateCommand.ExecuteNonQuery();
            }
        }

        private long GetPlayerId(string nickname, SqliteConnection connection, SqliteTransaction transaction)
        {
            using (var selectCommand = connection.CreateCommand())
            {
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = "SELECT Id FROM Players WHERE PlayerName = @PlayerName";
                selectCommand.Parameters.AddWithValue("@PlayerName", nickname);
                var id = (long)selectCommand.ExecuteScalar();
                return id;
            }
        }

        private long GetCharacterId(long playerId, SqliteConnection connection, SqliteTransaction transaction)
        {
            using (var selectCommand = connection.CreateCommand())
            {
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = "SELECT Id FROM Characters WHERE PlayerId = @PlayerId";
                selectCommand.Parameters.AddWithValue("@PlayerId", playerId);
                var id = (long)selectCommand.ExecuteScalar();
                return id;
            }
        }

        private List<long> GetDecoratedArmorIds(long characterId, SqliteConnection connection, SqliteTransaction transaction)
        {
            var result = new List<long>();
            using (var selectCommand = connection.CreateCommand())
            {
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = "SELECT * FROM DecoratedArmors WHERE CharacterId = @CharacterId";
                selectCommand.Parameters.AddWithValue("@CharacterId", characterId);
                using (var reader = selectCommand.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        result.Add(reader.GetInt64(0));
                    }
                }
            }
            return result;
        }

        private long GetBaseArmorId(long decoratedArmorId, SqliteConnection connection, SqliteTransaction transaction)
        {
            using (var selectCommand = connection.CreateCommand())
            {
                selectCommand.Transaction = transaction;
                selectCommand.CommandText = "SELECT Id FROM BaseArmors WHERE DecoratedArmorId = @DecoratedArmorId";
                selectCommand.Parameters.AddWithValue("@DecoratedArmorId", decoratedArmorId);
                var id = (long)selectCommand.ExecuteScalar();
                return id;
            }
        }

        public void Delete(string nickname)
        {
            using (var connection = new SqliteConnection($"Data Source={_path}"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Players WHERE PlayerName = @PlayerName";
                    command.Parameters.AddWithValue("@PlayerName", nickname);
                    command.ExecuteNonQuery();
                }
            }
        }        

        public async Task<Character> Get(string nickname)
        {
            long playerId;
            string characterType = null;
            int characterHealth = 0;
            int characterId = 0;
            List<BaseArmor> armors = new();

            await using (var connection = new SqliteConnection($"Data Source={_path}"))
            {
                await connection.OpenAsync();

                playerId = await GetPlayerIdAsync(nickname, connection);
                (characterId, characterType, characterHealth) = await GetCharacterDetailsAsync(playerId, connection);
                armors = await GetCharacterArmorsAsync(characterId, connection);
            }

            var playerType = Type.GetType(characterType) ?? throw new InvalidOperationException($"Character type '{characterType}' not found");
            var character = (Character)Activator.CreateInstance(playerType) ?? throw new InvalidOperationException($"Cannot create instance of '{playerType}'");

            character.SetHealth(characterHealth);
            character.SetPlayer(new Player(nickname));
            character.Armors = armors;
            return character;
        }

        private async Task<long> GetPlayerIdAsync(string nickname, SqliteConnection connection)
        {
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id FROM Players WHERE PlayerName = @PlayerName";
                command.Parameters.AddWithValue("@PlayerName", nickname);
                var result = await command.ExecuteScalarAsync();
                if (result == null) throw new InvalidOperationException($"Player with nickname '{nickname}' not found");
                return (long)result;
            }
        }

        private async Task<(int characterId, string characterType, int characterHealth)> GetCharacterDetailsAsync(long playerId, SqliteConnection connection)
        {
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Type, Health FROM Characters WHERE PlayerId = @PlayerId";
                command.Parameters.AddWithValue("@PlayerId", playerId);

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                    }
                    throw new InvalidOperationException($"Character for player id '{playerId}' not found");
                }
            }
        }

        private async Task<List<BaseArmor>> GetCharacterArmorsAsync(int characterId, SqliteConnection connection)
        {
            var armors = new List<BaseArmor>();

            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ArmorType, ArmorValue FROM DecoratedArmors WHERE CharacterId = @CharacterId";
                command.Parameters.AddWithValue("@CharacterId", characterId);

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var type = reader.GetString(0);
                        var armorValue = reader.GetInt32(1);

                        var armorType = Type.GetType(type) ?? throw new InvalidOperationException($"Armor type '{type}' not found");
                        var baseArmor = CreateBaseArmor(armorValue);
                        var decoratedArmor = (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor) ?? throw new InvalidOperationException($"Cannot create instance of '{armorType}'");

                        armors.Add(decoratedArmor);
                    }
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

        public async Task<List<Character>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
