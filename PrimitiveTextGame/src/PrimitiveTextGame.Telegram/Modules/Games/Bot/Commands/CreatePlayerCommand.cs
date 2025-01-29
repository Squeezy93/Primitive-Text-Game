using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.ArmorSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class CreatePlayerCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public CreatePlayerCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix { get; } = "create_player";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var characterRepository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
            var weaponRepository = scope.ServiceProvider.GetRequiredService<IWeaponRepository>();
            var armorRepository = scope.ServiceProvider.GetRequiredService<IArmorRepository>();

            var characterName = update.CallbackQuery.Data.Split('_').Last();
            var character = await characterRepository.GetAsync(new GetByCharacterNameSpecification(characterName));

            var weapons = (await weaponRepository.ListAsync(new GetByCharacterTypeSpecification(character.CharacterType))).ToList();

            var random = new Random();
            var armors = (await armorRepository.ListAsync(new GetByArmorLevelSpecification(ArmorLevel.Light))).OrderBy(x => random.Next()).Take(3).ToList();

            Models.User user = new Models.User(update.CallbackQuery.From.Id, update.CallbackQuery.From.Username, character, weapons, armors);
            userRepository.Create(user);

            var inlineMarkup = new InlineKeyboardMarkup()
                    .AddButton("Найти соперника", "search")
                    .AddNewRow()
                    .AddButton("Поменять персонажа", "change_player_character")
                    .AddNewRow()
                    .AddButton("Покинуть игру", "quit_game");

            await botClient.SendMessage(update.CallbackQuery.Message.Chat.Id, $"Уважаемый {user.UserName}. Ваш персонаж {user.Character.Name} создан. " +
                $"Что хотите сделать?", replyMarkup: inlineMarkup);
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
