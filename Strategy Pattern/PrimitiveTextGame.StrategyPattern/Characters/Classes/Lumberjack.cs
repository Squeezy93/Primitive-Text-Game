using PrimitiveTextGame.Characters;
using System.Text.Json.Serialization;

namespace PrimitiveTextGame.Characters.Classes
{
    public class Lumberjack : Character
    {
        public Lumberjack() : base("Lumberjack") { }

        public Lumberjack(string name) : base(name) { }

        public string TypeDiscriminator => "Lumberjack";
    }
}
