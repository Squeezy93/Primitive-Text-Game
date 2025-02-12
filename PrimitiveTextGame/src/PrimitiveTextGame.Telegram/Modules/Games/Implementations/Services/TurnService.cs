using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Services
{
    public class TurnService : ServiceScopeFactoryBase, ITurnService
    {
        public TurnService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public async Task<bool> FirstTurn(Game game)
        {
            var players = game.Users;
            if (players is null || players.Count == 0) return false;
            var random = new Random();
            var firstPlayer = players[random.Next(players.Count)];
            var secondPlayer = players.FirstOrDefault(p => p != firstPlayer);
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var parametersTemplateService = scope.ServiceProvider.GetRequiredService<IParametersTemplateService>();
            var markupTemplateService = scope.ServiceProvider.GetRequiredService<IMarkupTemplateService>();
            var parameters = await parametersTemplateService.GetParametersAsync(TemplateConstants.FirstTurn, firstPlayer, secondPlayer);
            var markup = await markupTemplateService.GetMarkupAsync(TemplateConstants.FirstTurn, parameters);
            await notificationService.SendFirstTurn(firstPlayer, secondPlayer);
            return true;
        }

        public async Task<bool> NextTurn(string weaponName, long attackerId, long defenderId)
        {
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var gameStateManager = scope.ServiceProvider.GetRequiredService<IGameStateService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
            var game = gameStateManager.GetGame(attackerId);
            var attacker = game.Users.FirstOrDefault(p => p.UserTelegramId == attackerId);
            var defender = game.Users.FirstOrDefault(p => p.UserTelegramId == defenderId);
            var weapon = attacker?.Weapons.FirstOrDefault(w => w.Name == weaponName);
            defender.Health -= weapon.Damage;
            if (defender.Health <= 0)
            {
                await gameService.EndGame(attacker, defender);
            }
            else
            {
                await notificationService.SendNextTurn(defender, attacker, weapon);
            }
            return true;
        }
    }
}
