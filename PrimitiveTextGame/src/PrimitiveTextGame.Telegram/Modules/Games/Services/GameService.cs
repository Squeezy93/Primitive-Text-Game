using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Services
{
    public class GameService : ServiceScopeFactoryBase, IGameService
    {
        private readonly ITelegramBotClient _botClient;

        public GameService(ITelegramBotClient telegramBotClient, IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _botClient = telegramBotClient;
        }
        public async Task<bool> StartGame(long userId, long opponentId)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            //получение юзеров из бд
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(userId));
            var opponent = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(opponentId));
            if (user is null || opponent is null) return false;
            //обновление состояния текущего юзера
            user.IsPlayingGame = true;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            //проверка состояния оппонента
            if (opponent.IsPlayingGame)
            {
                user.IsSearchingForGame = false;
                opponent.IsSearchingForGame = false;
                userRepository.Update(user);
                userRepository.Update(opponent);
                await userRepository.SaveChangesAsync();
                var game = new Game(new List<User> { user, opponent });
                gameStateManager.AddGame(userId, game);
                gameStateManager.AddGame(opponentId, game);
                await StartBattle(game);
            }
            else
            {
                await _botClient.SendMessage(user.UserTelegramId, "Вы подтвердили участие. Ожидаем подтверждения от вашего оппонента.");
            }
            return true;
        }


        private async Task<bool> StartBattle(Game game)
        {
            var players = game.Users;
            if (players is null || players.Count == 0) return false;
            await ChooseFirstPlayerTurn(players);
            return true;
        }

        private async Task ChooseFirstPlayerTurn(List<User> players)
        {
            var random = new Random();
            var firstPlayerIndex = random.Next(players.Count);
            var firstPlayer = players[firstPlayerIndex];
            var secondPlayer = players.FirstOrDefault(p => p != firstPlayer);
            await FirstTurnNotify(firstPlayer, secondPlayer);
            await SecondTurnNotify(secondPlayer);
        }

        private async Task FirstTurnNotify(User user, User opponent)
        {
            var weapons = user.Weapons;
            var replyMarkupForUser = new InlineKeyboardMarkup();
            foreach (var weapon in weapons)
            {
                replyMarkupForUser.AddButton(weapon.Name, $"attack_{weapon.Name}_" +
                    $"{user.UserTelegramId}_{opponent.UserTelegramId}");
            }
            await _botClient.SendMessage(user.UserTelegramId, "Вы ходите первым. Выберите свое оружие",
                replyMarkup: replyMarkupForUser);
        }

        private async Task SecondTurnNotify(User user)
        {
            await _botClient.SendMessage(user.UserTelegramId, "Оппонент ходит первым. Дождитесь своего хода.");
        }

        public async Task<bool> HandleAttackCommand(string weaponName, long attackerId, long defenderId)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            var game = gameStateManager.GetGame(attackerId);
            var attacker = game.Users.FirstOrDefault(p => p.UserTelegramId == attackerId);
            var defender = game.Users.FirstOrDefault(p => p.UserTelegramId == defenderId);
            var weapon = attacker?.Weapons.FirstOrDefault(w => w.Name == weaponName);
            defender.Health -= weapon.Damage;
            if (defender.Health <= 0)
            {
                await EndBattle(attacker, defender);
            }
            else
            {
                await _botClient.SendMessage(attackerId, $"Вы нанесли {weapon.Damage} урона. У врага осталось {defender.Health} здоровья. Следующий ход врага");
                await _botClient.SendMessage(defenderId, $"Вам нанесли {weapon.Damage} урона. У вас осталось {defender.Health} здоровья. Ваш ход.");
                await NextTurn(defender, attacker);
            }
            return true;
        }

        private async Task NextTurn(User attacker, User defender)
        {
            var weapons = attacker.Weapons;
            var replyMarkupForUser = new InlineKeyboardMarkup();
            foreach (var weapon in weapons)
            {
                replyMarkupForUser.AddButton(weapon.Name, $"attack_{weapon.Name}_" +
                    $"{attacker.UserTelegramId}_{defender.UserTelegramId}");
            }
            await _botClient.SendMessage(attacker.UserTelegramId, "Выберите свое оружие",
                replyMarkup: replyMarkupForUser);
        }

        private async Task EndBattle(User attacker, User defender)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            gameStateManager.RemoveGame(attacker.UserTelegramId);
            gameStateManager.RemoveGame(defender.UserTelegramId);
            attacker.IsPlayingGame = false;
            defender.IsPlayingGame = false;
            attacker.Health = 100;
            defender.Health = 100;
            userRepository.Update(attacker);
            userRepository.Update(defender);
            await userRepository.SaveChangesAsync();
            var inlineMarkup = new InlineKeyboardMarkup()
                    .AddButton("Найти соперника", "search")
                    .AddNewRow()
                    .AddButton("Поменять персонажа", "change_player_character")
                    .AddNewRow()
                    .AddButton("Покинуть игру", "quit_game");

            await _botClient.SendMessage(attacker.UserTelegramId, $"Поздравляем! Вы победили {defender.UserName}. Что хотите сделать?",
                replyMarkup: inlineMarkup);
            await _botClient.SendMessage(defender.UserTelegramId, $"Вы проиграли. Победитель: {attacker.UserName}. Что хотите сделать?",
                replyMarkup: inlineMarkup);
            await userRepository.SaveChangesAsync();
        }
    }
}
