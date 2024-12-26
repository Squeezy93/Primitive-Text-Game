namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public interface IRepository<TEntity, in TId>
	where TEntity : BaseEntity<TId>
	where TId : IEquatable<TId>
{
	Task<TEntity?> GetAsync(ISpecification<TEntity> specification);
	Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification);
	void Update(TEntity entity);
	void Create(TEntity entity);
	void Create(IEnumerable<TEntity> entities);
	void Delete(TEntity entity);
	Task<bool> IsExists(ISpecification<TEntity> specification);
	Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}