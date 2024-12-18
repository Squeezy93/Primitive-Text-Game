using System.Linq.Expressions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public interface ISpecification<TEntity>
{
	Expression<Func<TEntity, bool>>? Criteria { get; }
	List<Expression<Func<TEntity, object>>> Includes { get; }
}