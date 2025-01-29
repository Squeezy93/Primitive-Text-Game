using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications
{
    public class GetByNameSpecification : SpecificationBase<Weapon>
    {
        public GetByNameSpecification(string weaponName)
        {
            Criteria = weapon => weapon.Name == weaponName;
        }
    }
}
