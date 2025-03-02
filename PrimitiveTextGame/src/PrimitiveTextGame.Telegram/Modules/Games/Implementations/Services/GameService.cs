﻿using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot;

namespace PrimitiveTextGame.Telegram.Modules.Games.Services
{
    public class GameService : ServiceScopeFactoryBase, IGameService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, string> _messagesTemplateDictionary;

        public GameService(ITelegramBotClient telegramBotClient, IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache) : base(serviceScopeFactory)
        {
            _botClient = telegramBotClient;
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            if (_memoryCache.TryGetValue(TemplateConstants.MessagesTemplate, out var messagesTemplate))
            {
                var value = messagesTemplate as Dictionary<string, string>;
                if (value != null)
                {
                    _messagesTemplateDictionary = value;
                }
                //TODO: надо подумать на случай если нет в кэше что делать
            }
        }
        public async Task<bool> StartGame(long userId, long opponentId)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var turnService = scope.ServiceProvider.GetRequiredService<ITurnService>();
            
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(userId));
            var opponent = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(opponentId));
            if (user is null || opponent is null) return false;
            
            user.IsPlayingGame = true;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            
            if (opponent.IsPlayingGame)
            {
                user.IsSearchingForGame = false;
                opponent.IsSearchingForGame = false;
                userRepository.Update(user);
                userRepository.Update(opponent);
                await userRepository.SaveChangesAsync();
                var game = new Game([user, opponent]);
                gameStateManager.AddGame(userId, game);
                gameStateManager.AddGame(opponentId, game);
                await turnService.FirstTurn(game);
            }
            else
            {
                var template = _messagesTemplateDictionary[TemplateConstants.WaitingForOpponent];
                await notificationService.SendNotification(user.UserTelegramId, template);
            }
            return true;
        }       

        public async Task<bool> EndGame(User attacker, User defender)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            gameStateManager.RemoveGame(attacker.UserTelegramId);
            gameStateManager.RemoveGame(defender.UserTelegramId);
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(attacker.UserTelegramId));
            var opponent = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(defender.UserTelegramId));
            user.IsPlayingGame = false;
            opponent.IsPlayingGame = false;
            /*user1.Health = attacker.Health;*/
            userRepository.Update(user);
            userRepository.Update(opponent);
            await notificationService.SendEndgame(attacker, defender);
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
