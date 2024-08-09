using TextGame.Domain.Entities.Characters;

namespace TextGame.Application
{
    public class Game
    {
        private Random _random = new();
        private Character _player;
        private Character _enemy;
    }
}
