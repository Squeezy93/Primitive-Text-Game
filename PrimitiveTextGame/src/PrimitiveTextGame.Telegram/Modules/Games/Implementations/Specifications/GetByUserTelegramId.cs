using System.Linq.Expressions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications;

public class GetByUserTelegramId : SpecificationBase<User>
{
	public GetByUserTelegramId(long userTelegramId)
	{
		Criteria = user => user.UserTelegramId == userTelegramId;
	}
}