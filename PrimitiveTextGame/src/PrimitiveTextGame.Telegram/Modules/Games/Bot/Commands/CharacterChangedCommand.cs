using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class CharacterChangedCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public CharacterChangedCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix { get; } = "change_player";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var characterRepository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
            var weaponRepository = scope.ServiceProvider.GetRequiredService<IWeaponRepository>();

            var characterName = update.CallbackQuery.Data.Split('_').Last();
            var character = await characterRepository.GetAsync(new GetByCharacterNameSpecification(characterName));
            var weapons = (await weaponRepository.ListAsync(new GetByCharacterTypeSpecification(character.CharacterType))).ToList();
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.CallbackQuery.From.Id));

            user.Character = character;
            user.Weapons = weapons;
            userRepository.Update(user);
            await notificationService.SendChangedCharacter(user);
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
