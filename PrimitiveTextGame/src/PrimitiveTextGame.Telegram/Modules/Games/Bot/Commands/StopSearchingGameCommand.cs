﻿using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class StopSearchingGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public StopSearchingGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "stop_searching";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            //получение юзера из бд
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.CallbackQuery.From.Id));
            //обновление статуса юзера
            user.IsSearchingForGame = false;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            //оповещение юзера            
            await notificationService.SendStopSearching(user.UserTelegramId);
            return true;
        }
    }
}
