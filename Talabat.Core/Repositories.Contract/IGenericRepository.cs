using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specification;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenericRepository <T> where T : BaseEntity
	{


		#region With out Specification
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T> GetByIdAsync(int id);


		Task AddAsync(T item);

		void Update(T item);

		void Delete(T item);

		#endregion



		#region with Specification

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> sepc);
		Task<T> GetByEntityWithSpecAsync(ISpecification<T> sepc);



		Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

		#endregion

	}
}
