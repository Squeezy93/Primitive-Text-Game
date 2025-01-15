using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications
{
    public class FindUsersSearchingForGameSpecification : SpecificationBase<User>
    {
        public FindUsersSearchingForGameSpecification(User currentUser)
        {
            Criteria = user => user.UserTelegramId != currentUser.UserTelegramId && user.IsSearchingForGame;
            AddInclude(user => user.Weapons);
            AddInclude(user => user.Armors);
            AddInclude(user => user.Character);
        }
    }
}
