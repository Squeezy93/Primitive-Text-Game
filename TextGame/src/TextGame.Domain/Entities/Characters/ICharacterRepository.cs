namespace TextGame.Domain.Entities.Characters
{
    public interface ICharacterRepository
    {
        void Add(Character character);
        void Update(Character character);
        void Delete(Character character);
        Task<List<Character>> GetAll();
        Task<Character> Get(string nickname);
    }
}
