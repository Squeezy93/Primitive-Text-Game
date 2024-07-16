using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrimitiveTextGame.Armors;
using PrimitiveTextGame.Armors.ArmorsType;
using PrimitiveTextGame.Armors.Decorator;
using PrimitiveTextGame.Characters;

public class CharacterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Character).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JObject jo = new JObject();
        if (value is Character character)
        {
            jo.Add("Type", character.GetType().FullName);
            jo.Add("Health", character.Health);
            jo.Add("Name", character.Name);

            JArray armorsArray = new JArray();
            foreach (var armor in character.Armors)
            {
                JObject armorObject = new JObject
                {
                    { "ArmorType", armor.GetType().FullName },
                    { "ArmorValue", armor.Value },
                    { "ArmorName", armor.Name }
                };

                if (armor is ArmorDecorator decorator)
                {
                    armorObject.Add("baseArmor", JToken.FromObject(decorator.GetBaseArmor(), serializer));
                }

                armorsArray.Add(armorObject);
            }
            jo.Add("Armors", armorsArray);
        }
        jo.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        string nameTypeString = jo["Type"].ToString();
        Type nameType = Type.GetType(nameTypeString) ?? throw new InvalidOperationException($"Unknown nameType: {nameTypeString}");

        Character character = (Character)Activator.CreateInstance(nameType);

        int health = (int)jo["Health"];
        character.SetHealth(health);

        if (jo["Armors"] != null)
        {
            JArray armorsArray = (JArray)jo["Armors"];
            foreach (var item in armorsArray)
            {
                BaseArmor baseArmor = CreateBaseArmor((int)item["ArmorValue"]);
                ArmorDecorator armor = CreateArmor(item, baseArmor);               

                character.Armors.Add(armor);
            }
        }
        return character;
    }

    private BaseArmor CreateBaseArmor(int armorValue)
    {
        return armorValue switch
        {
            10 => new LightArmor(),
            25 => new MediumArmor(),
            50 => new HeavyArmor(),
            _ => throw new InvalidOperationException($"Unknown armor value: {armorValue}")
        };
    }

    private ArmorDecorator CreateArmor(JToken item, BaseArmor baseArmor)
    {
        string armorTypeString = item["ArmorType"].ToString();
        Type armorType = Type.GetType(armorTypeString) ?? throw new InvalidOperationException($"Unknown type: {armorTypeString}");

        return (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor);
    }
}

