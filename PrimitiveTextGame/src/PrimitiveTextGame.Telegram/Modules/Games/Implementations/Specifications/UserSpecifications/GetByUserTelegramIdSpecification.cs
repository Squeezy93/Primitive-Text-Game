using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;

public class GetByUserTelegramIdSpecification : SpecificationBase<User>
{
    public GetByUserTelegramIdSpecification(long userTelegramId)
    {
        Criteria = user => user.UserTelegramId == userTelegramId;
        AddInclude(user => user.Weapons);
        AddInclude(user => user.Armors);
        AddInclude(user => user.Character);
    }
}