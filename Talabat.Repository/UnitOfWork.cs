using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Repository.Repository;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbcontext;
		private Hashtable _repositoires;

        public UnitOfWork( StoreContext dbcontext)
        {
            _repositoires = new Hashtable();
			this._dbcontext = dbcontext;
		}
        public async Task<int> CompleteAsync()
		=> await _dbcontext.SaveChangesAsync();

		public ValueTask DisposeAsync()
		=> _dbcontext.DisposeAsync();


		//Repository <Product>
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var type = typeof(TEntity).Name; // Product 
			
{

}
			if (!_repositoires.ContainsKey(type)) // First Time
			{
				var Repository = new GenericRepository<TEntity>(_dbcontext);
			
				_repositoires.Add(type, Repository);

			}

			return _repositoires[type] as IGenericRepository<TEntity>;

		}
	}
}
