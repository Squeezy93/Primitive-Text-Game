using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications
{
    public class GetByGuidSpecification : SpecificationBase<Weapon>
    {
        public GetByGuidSpecification(Guid id)
        {
            Criteria = weapon => weapon.Id == id;
        }
    }
}
