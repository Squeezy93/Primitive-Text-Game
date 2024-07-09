using PrimitiveTextGame.Armors.ArmorsType;
using PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes;
using PrimitiveTextGame.Armors;
using PrimitiveTextGame.Armors.Decorator;
using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Utilites
{
    public static class ArmorHelper
    {
        private static readonly Random _random = new Random();

        public static BaseArmor[] GenerateArmor(Character character, int count)
        {
            var armors = new BaseArmor[count];
            var usedTypes = new HashSet<Type>();

            for (int i = 0; i < count; i++)
            {
                var localArmorsType = new BaseArmor[] { new LightArmor(), new MediumArmor(), new HeavyArmor() };
                var localArmorType = localArmorsType[_random.Next(localArmorsType.Length)];

                BaseArmor localArmor = null;

                var localArmors = new BaseArmor[]
                    {
                        new AxeArmor(localArmorType),
                        new BarehandsArmor(localArmorType),
                        new DaggerArmor(localArmorType),
                        new FireArmor(localArmorType),
                        new KnifeArmor(localArmorType),
                        new LightningArmor(localArmorType),
                        new LogArmor(localArmorType),
                        new SpearArmor(localArmorType),
                        new SwordArmor(localArmorType)
                    };
                do
                {
                    localArmor = localArmors[_random.Next(localArmors.Length)];
                } 
                while (!usedTypes.Add(localArmor.GetType()));

                armors[i] = localArmor;
                character.Armors.Add(localArmor);
            }
            
            return armors;
        }

        private static BaseArmor CreateArmor(IWeapon weapon)
        {
            var baseArmor = new LightArmor();

            return weapon.Name.ToLower() switch
            {
                "axe" => new AxeArmor(baseArmor),
                "barehands" => new BarehandsArmor(baseArmor),
                "dagger" => new DaggerArmor(baseArmor),
                "fire" => new FireArmor(baseArmor),
                "knife" => new KnifeArmor(baseArmor),
                "lightning" => new LightningArmor(baseArmor),
                "log" => new LogArmor(baseArmor),
                "spear" => new SpearArmor(baseArmor),
                "sword" => new SwordArmor(baseArmor),
                _ => throw new ArgumentException("Cannot create armor for the given weapon")
            };
        }

        private static BaseArmor UpgradeBaseArmor(BaseArmor armor)
        {
            if (armor is ArmorDecorator decorator)
            {
                var baseArmorName = decorator.GetBaseArmor().Name.ToLower();

                switch (baseArmorName)
                {
                    case "light armor":
                        return decorator.SetArmor(new MediumArmor());
                    case "medium armor":
                        return decorator.SetArmor(new HeavyArmor());
                    default:
                        Console.WriteLine($"Cannot upgrade armor: {armor.Name}. Maximum armor level reached.");
                        break;
                }
            }
            return armor;
        }

        public static void UpgradeArmor(IWeapon weapon, Character character)
        {
            BaseArmor existingArmor = FindArmorMatchingWeapon(weapon, character);

            if (existingArmor == null)
            {
                BaseArmor newBaseArmor = CreateArmor(weapon);
                character.Armors.Add(newBaseArmor);
                Console.WriteLine($"You got {newBaseArmor.Name} for your streak!");
            }
            else
            {
                var armor = UpgradeBaseArmor(existingArmor);
                character.Armors.Remove(existingArmor);
                character.Armors.Add(armor);
                Console.WriteLine($"You upgraded {armor.Name} for your streak to {armor.Value}%!");
            }
        }

        private static BaseArmor FindArmorMatchingWeapon(IWeapon weapon, Character character)
        {
            foreach (var armor in character.Armors)
            {
                if (armor.Name.ToLower().Contains(weapon.Name.ToLower()))
                {
                    return armor;
                }
            }

            return null;
        }
    }
}
