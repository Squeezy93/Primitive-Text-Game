using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;

public class WeaponRepository(ApplicationDataContext context) : Repository<Weapon, Guid>(context), IWeaponRepository;

