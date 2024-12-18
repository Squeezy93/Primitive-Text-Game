using System.Linq.Expressions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications;

public class SpecificationBase<T> : ISpecification<T>
{
	public Expression<Func<T, bool>>? Criteria { get; set; }
	public List<Expression<Func<T, object>>> Includes { get; }

	public void AddInclude(Expression<Func<T, object>> includeExpression)
	{
		Includes.Add(includeExpression);
	}
}