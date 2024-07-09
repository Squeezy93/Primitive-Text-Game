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
        Type nameType = Type.GetType(nameTypeString);        

        if (nameType == null)
        {
            throw new InvalidOperationException($"Unknown nameType: {nameTypeString}");
        }
        

        Character character = (Character)Activator.CreateInstance(nameType);

        int health = (int)jo["Health"];
        character.SetHealth(health);

        if (jo["Armors"] != null) 
        { 
            JArray armorsArray = (JArray)jo["Armors"];
            foreach (var item in armorsArray)
            {
                string armorTypeString = item["ArmorType"].ToString();
                Type armorType = Type.GetType(armorTypeString);
                var armorValue = (int)item["ArmorValue"];

                if (armorType == null)
                {
                    throw new InvalidOperationException($"Unknown type: {armorTypeString}");
                }

                BaseArmor baseArmor;
                ArmorDecorator armor;

                if (armorValue == 10)
                {
                    baseArmor = new LightArmor();
                    armor = (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor);
                }
                else if (armorValue == 25)
                {
                    baseArmor = new MediumArmor();
                    armor = (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor);
                }
                else if (armorValue == 50)
                {
                    baseArmor = new HeavyArmor();
                    armor = (ArmorDecorator)Activator.CreateInstance(armorType, baseArmor);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown type: {armorType}");
                }

                character.Armors.Add(armor);
            }
        }

        return character;
    }
}

