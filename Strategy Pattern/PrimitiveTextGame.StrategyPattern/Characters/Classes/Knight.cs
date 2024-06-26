using PrimitiveTextGame.Characters;
using System.Text.Json.Serialization;

public class Knight : Character
{
    public Knight() : base("Knight") { }

    public Knight(string name) : base(name) { }

    public string TypeDiscriminator => "Knight";
}
