using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.ArmorSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Services
{
    public class UserService : ServiceScopeFactoryBase, IUserService
    {
        public UserService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public async Task<Models.User> CreateNewUser(Update update)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var characterRepository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
            var weaponRepository = scope.ServiceProvider.GetRequiredService<IWeaponRepository>();
            var armorRepository = scope.ServiceProvider.GetRequiredService<IArmorRepository>();
            var characterName = ExtractCharacterName(update);
            var character = await characterRepository.GetAsync(new GetByCharacterNameSpecification(characterName));
            var weapons = GetWeaponsForCharacter(weaponRepository, character).Result;
            var armors = GetRandomLightArmors(armorRepository, 3).Result;
            Models.User user = new Models.User(update.CallbackQuery.From.Id, update.CallbackQuery.From.Username, character, weapons, armors);
            userRepository.Create(user);
            await userRepository.SaveChangesAsync();
            return user;
        }

        private string ExtractCharacterName(Update update) => update.CallbackQuery.Data.Split('_').Last();

        private async Task<List<Weapon>> GetWeaponsForCharacter(IWeaponRepository weaponRepository, Character character) =>
        (await weaponRepository.ListAsync(new GetByCharacterTypeSpecification(character.CharacterType))).ToList();

        private async Task<List<Armor>> GetRandomLightArmors(IArmorRepository armorRepository, int count)
        {
            var random = new Random();
            return (await armorRepository.ListAsync(new GetByArmorLevelSpecification(ArmorLevel.Light)))
                   .OrderBy(_ => random.Next())
                   .Take(count)
                   .ToList();
        }

        public async Task<Models.User> StartSearchingForGame(long userTelegramId)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(userTelegramId));
            user.IsSearchingForGame = true;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            return user;
        }

        public async Task<Models.User> FindOpponent(Models.User currentUser)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var random = new Random();
            var potentialOpponents = await userRepository.ListAsync(new FindUsersSearchingForGameSpecification(currentUser));
            return potentialOpponents.OrderBy(_ => random.Next()).FirstOrDefault(op => !op.IsPlayingGame);
        }

    }
}
