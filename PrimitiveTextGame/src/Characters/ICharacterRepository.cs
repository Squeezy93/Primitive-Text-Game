namespace PrimitiveTextGame.Characters
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAll();
        Task<Character> Get(string nickname);
        void Add(Character character);
        void Update(Character character);
        void Delete(string nickname);
    }
}
