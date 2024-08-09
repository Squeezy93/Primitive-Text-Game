using PrimitiveTextGame.Armors.ArmorsType;
using PrimitiveTextGame.Armors.Decorator;
using PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes;
using PrimitiveTextGame.Characters;
using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors
{
    public class ArmorManager
    {
        /*private readonly Random _random = new();

        public ArmorDecorator[] GenerateArmor(Character character, int count)
        {
            var armors = new ArmorDecorator[count];
            var usedTypes = new HashSet<Type>();

            for (int i = 0; i < count; i++)
            {
                var localArmorsType = new BaseArmor[] { new LightArmor(), new MediumArmor(), new HeavyArmor() };
                var localArmorType = localArmorsType[_random.Next(localArmorsType.Length)];

                ArmorDecorator localArmor = null;
                var localArmors = new ArmorDecorator[]
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
            }

            return armors;
        }

        private ArmorDecorator CreateArmor(IWeapon weapon)
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
                _ => throw new ArgumentException("Cannot create armor for the given weapon.")
            };
        }

        private ArmorDecorator UpgradeBaseArmor(ArmorDecorator armor)
        {
            var baseArmorName = armor.GetBaseArmor().Name.ToLower();

            return baseArmorName switch
            {
                "light armor" => (ArmorDecorator)armor.SetArmor(new MediumArmor()),
                "medium armor" => (ArmorDecorator)armor.SetArmor(new HeavyArmor()),
                _ => armor
            };
        }

        private ArmorDecorator FindArmorMatchingWeapon(IWeapon weapon, Character character)
        {
            foreach (var armor in character.DecoratorArmors)
            {
                if (armor.Name.ToLower().Contains(weapon.Name.ToLower()))
                {
                    return armor;
                }
            }

            return null;
        }

        public ArmorDecorator UpgradeArmor(IWeapon weapon, Character character)
        {
            ArmorDecorator existingArmor = FindArmorMatchingWeapon(weapon, character);

            if (existingArmor == null) 
            {
                ArmorDecorator newArmor = CreateArmor(weapon);
                Console.WriteLine($"You got {newArmor.Name} for you streak!");
                return newArmor;
            }
            else
            {
                var armor = UpgradeBaseArmor(existingArmor);
                Console.WriteLine($"You upgraded {armor.Name} for your streak to {armor.Value}%!");
                return armor;
            }
        }*/
    }
}
