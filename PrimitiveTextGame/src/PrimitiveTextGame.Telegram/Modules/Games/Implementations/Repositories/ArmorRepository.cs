using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;

public class ArmorRepository(ApplicationDataContext context) : Repository<Armor, Guid>(context), IArmorRepository;