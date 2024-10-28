using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification;
using Talabat.Repository.Data;
using Talabat.Repository.Specification;

namespace Talabat.Repository.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbcontext;

		public GenericRepository(StoreContext dbcontext)
        {
			_dbcontext = dbcontext;
		}
		#region With out Specification
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			//if (typeof(T) == typeof(Product))
			//	return await _dbcontext.Set<Product>().Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync() as IReadOnlyList<T>;

			return await _dbcontext.Set<T>().ToListAsync();

		}

		public async Task<T> GetByIdAsync(int id)
		{
			//if (typeof(T) == typeof(Product))
			//	return await _dbcontext.Set<Product>().Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync() as T;

			return await _dbcontext.Set<T>().FindAsync(id);
		}

		public async Task AddAsync(T item)
		=> await _dbcontext.Set<T>().AddAsync(item);

		public void Update(T item)
		=> _dbcontext.Set<T>().Update(item);

		public void Delete(T item)
		=> _dbcontext.Set<T>().Remove(item);

		#endregion
		#region With Specification
		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> sepc)
		{
			return await ApplySpecification(sepc).ToListAsync();
		}


		public async Task<T> GetByEntityWithSpecAsync(ISpecification<T> sepc)
		{
			return await ApplySpecification(sepc).FirstOrDefaultAsync();
		}


		private IQueryable<T> ApplySpecification(ISpecification<T>Spec)
		{
			return SpecificationsEvalutor<T>.GetQuery(_dbcontext.Set<T>(),Spec);
		}

		public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
		{

			return await ApplySpecification(spec).CountAsync();
		}

		



		#endregion
	}
}
