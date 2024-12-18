using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations;

public class Repository<TEntity, TId>(ApplicationDataContext applicationDataContext)
	: IRepository<TEntity, TId>
	where TEntity : BaseEntity<TId>
	where TId : IEquatable<TId>
{
	public async Task<TEntity?> GetAsync(ISpecification<TEntity> specification)
	{
		IQueryable<TEntity> query = applicationDataContext.Set<TEntity>();

		foreach (var include in specification.Includes)
		{
			query = query.Include(include);
		}

		if (specification.Criteria is not null)
		{
			query = query.Where(specification.Criteria);
		}

		return await query.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification)
	{
		IQueryable<TEntity> query = applicationDataContext.Set<TEntity>();

		foreach (var include in specification.Includes)
		{
			query = query.Include(include);
		}

		if (specification.Criteria is not null)
		{
			query = query.Where(specification.Criteria);
		}

		return await query.ToListAsync();
	}

	public void Update(TEntity entity) => applicationDataContext.Set<TEntity>().Update(entity);

	public void Create(TEntity entity) => applicationDataContext.Set<TEntity>().Add(entity);

	public void Delete(TEntity entity) => applicationDataContext.Set<TEntity>().Remove(entity);

	public async Task<bool> IsExists(ISpecification<TEntity> specification)
	{
		IQueryable<TEntity> query = applicationDataContext.Set<TEntity>();

		if (specification.Criteria is not null)
		{
			query = query.Where(specification.Criteria);
		}

		return await query.AnyAsync(); 
	}

	public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) =>
		await applicationDataContext.SaveChangesAsync(cancellationToken) >= 0;
}