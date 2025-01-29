using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.GameSpecifications
{
    public class GetByGameIdSpecification : SpecificationBase<Game>
    {
        public GetByGameIdSpecification(Guid id)
        {
            Criteria = game => game.Id == id;
        }
    }
}
