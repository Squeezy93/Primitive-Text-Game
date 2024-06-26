using PrimitiveTextGame.Characters;
using System.Text.Json.Serialization;

namespace PrimitiveTextGame.Characters.Classes
{
    public class Mage : Character
    {
        public Mage() : base("Mage") { }

        public Mage(string name) : base(name) { }

        public string TypeDiscriminator => "Mage";
    }
}
