using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Scriban;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = PrimitiveTextGame.Telegram.Modules.Games.Models.User;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ITelegramBotClient telegramBotClient, ILogger<NotificationService> logger)
        {
            _botClient = telegramBotClient ?? throw new ArgumentNullException(nameof(telegramBotClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task SendNotification(long userTelegramId, string template, object parameters = default, InlineKeyboardMarkup markup = default)
        {
            try
            {
                var templateMessage = Template.ParseLiquid(template);
                var message = await templateMessage.RenderAsync(parameters);
                await _botClient.SendMessage(userTelegramId, message, replyMarkup: markup);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error was occured during sending notification, Error messsage : {e.Message}");
                throw;
            }
        }
        
        public async Task SendUserCreated(User user)
        {
            var inlineMarkup = GetMainMenuMarkup();
            await _botClient.SendMessage(user.UserTelegramId, $"Уважаемый {user.UserName}. Ваш персонаж {user.Character.Name} создан. " +
                $"Что хотите сделать?", replyMarkup: inlineMarkup);
        }

        public async Task SendGameInvitation(User user, User opponent)
        {
            await SendUserInvitation(user, opponent);
            await SendUserInvitation(opponent, user);
        }

        private async Task SendUserInvitation(User user, User opponent)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Да", $"accept_game_{user.UserTelegramId}_{opponent.UserTelegramId}")
                .AddNewRow()
                .AddButton("Нет", $"decline_game_{user.UserTelegramId}_{opponent.UserTelegramId}");
            await _botClient.SendMessage(user.UserTelegramId, $"Противник найден. Вы играете против {opponent.UserName}. Готовы к битве?",
                replyMarkup: inlineMarkup);
        }

        public async Task SendStartSearching(long userTelegramId)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Выйти из поиска", $"stop_searching_{userTelegramId}");
            await _botClient.SendMessage(userTelegramId, "Вы встали в поиск. Скоро противник найдется :)", replyMarkup: inlineMarkup);
        }

        private InlineKeyboardMarkup GetMainMenuMarkup()
        {
            return new InlineKeyboardMarkup()
                .AddButton("Найти соперника", "search")
                .AddNewRow()
                .AddButton("Поменять персонажа", "choose_player_character")
                .AddNewRow()
                .AddButton("Покинуть игру", "quit_game");
        }
        
        public async Task SendNewCharacterSelection(long userTelegramId)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Рыцарь", "create_player_Knight")
                .AddButton("Маг", "create_player_Mage")
                .AddButton("Лесоруб", "create_player_Lumberjack")
                .AddNewRow();
            await _botClient.SendMessage(userTelegramId, "Выбери героя!", replyMarkup: inlineMarkup);
        }

        public async Task SendChangeCharacterSelection(User user)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Рыцарь", "change_player_Knight")
                .AddButton("Маг", "change_player_Mage")
                .AddButton("Лесоруб", "change_player_Lumberjack")
                .AddNewRow();
            await _botClient.SendMessage(user.UserTelegramId, "Выбери героя!", replyMarkup: inlineMarkup);
        }

        public async Task SendChangedCharacter(User user)
        {
            var inlineMarkup = GetMainMenuMarkup();
            await _botClient.SendMessage(user.UserTelegramId, $"Вы поменяли героя на {user.Character.Name}. Что хотите сделать?", replyMarkup: inlineMarkup);
        }

        public async Task SendReturnMessage(User user)
        {
            var inlineMarkup = GetMainMenuMarkup();
            await _botClient.SendMessage(user.UserTelegramId, $"Добро пожаловать обратно {user.UserName}. Ваш персонаж {user.Character.Name}. " +
                $"Что хотите сделать?", replyMarkup: inlineMarkup);
        }

        public async Task SendStopSearching(long userTelegramId)
        {
            var inlineMarkupForUser = GetMainMenuMarkup();
            await _botClient.SendMessage(userTelegramId, "Вы остановили поиск. Что хотите сделать?",
                replyMarkup: inlineMarkupForUser);
        }

        public async Task SendDeclineGame(long userTelegramId)
        {
            var inlineMarkup = GetMainMenuMarkup();
            await _botClient.SendMessage(userTelegramId, "Вы отклонили игру. Что хотите сделать?",
                replyMarkup: inlineMarkup);
        }

        public async Task SendReturnToSearch(long userTelegramId)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Выйти из поиска", $"stop_searching");
            await _botClient.SendMessage(userTelegramId, "Ваш соперник отклонил игру. Возвращаемся к поиску.",
                replyMarkup: inlineMarkup);
        }

        public async Task SendFirstTurn(User firstPlayer, User secondPlayer)
        {
            var inlineMarkup = GetWeaponsMarkup(firstPlayer, secondPlayer);
            await _botClient.SendMessage(firstPlayer.UserTelegramId, "Ваш ход. Выберите оружие", replyMarkup: inlineMarkup);
            await _botClient.SendMessage(secondPlayer.UserTelegramId, "Ожидайте ход противника.");
        }

        public async Task SendNextTurn(User attacker, User defender, Weapon weapon)
        {
            var inlineMarkup = GetWeaponsMarkup(attacker, defender);
            await _botClient.SendMessage(attacker.UserTelegramId, $"Вы нанесли {weapon.Damage} урона. У противника осталось {defender.Health} здоровья.");
            await _botClient.SendMessage(defender.UserTelegramId, $"Вам нанесли {weapon.Damage} урона. У вас осталось {defender.Health} здоровья.");
            await _botClient.SendMessage(attacker.UserTelegramId, "Выберите свое оружие", replyMarkup: inlineMarkup);
        }

        private InlineKeyboardMarkup GetWeaponsMarkup(User user, User opponent)
        {
            var weapons = user.Weapons;
            var replyMarkup = new InlineKeyboardMarkup();

            foreach (var weapon in weapons)
            {
                replyMarkup.AddButton(weapon.Name, $"attack_{weapon.Name}_{user.UserTelegramId}_{opponent.UserTelegramId}");
            }
            return replyMarkup;
        }

        public async Task SendEndgame(User attacker, User defender)
        {
            var inlineMarkup = GetMainMenuMarkup();
            await _botClient.SendMessage(attacker.UserTelegramId, $"Поздравляем! Вы победили {defender.UserName}. Что хотите сделать?",
                replyMarkup: inlineMarkup);
            await _botClient.SendMessage(defender.UserTelegramId, $"Вы проиграли. Победитель: {attacker.UserName}. Что хотите сделать?",
                replyMarkup: inlineMarkup);
        }
    }
}
