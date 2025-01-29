using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Game : EntityBase<Guid>
{
    public List<User> Users { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public User Winner { get; set; }
    public Guid? WinnerId { get; set; }
    public List<History> Histories { get; set; }

    public Game(List<User> players)
    {
        StartDate = DateTime.UtcNow;
        EndDate = DateTime.UtcNow;
        Users = players;
        Histories = new List<History>();        
    }

    private Game()
    {

    }
}