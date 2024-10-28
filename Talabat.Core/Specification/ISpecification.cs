using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specification
{
	public interface ISpecification <T> where T : BaseEntity
	{
		// _dbcontext.Set<Product>().Where(p => p.Id == id)
		// .Include(p => p.ProductBrand).Include(p => p.ProductType)
		// .FirstOrDefaultAsync() as T;

		// Sign For Property for Where Condition [Where(p => p.Id == id)]

		public Expression<Func<T,bool>> Criteria { get; set; }

		// Sign For Property for List Of Include [Include(p => p.ProductBrand).Include(p => p.ProductType)]

		public List<Expression<Func<T,object>>> Includes { get; set; }




		// Prop For OrderBy [OrderBy (P=>P.Name)]
		public Expression<Func<T, object>> OrderBy { get; set; }
		// Prop For OrderByDesc [OrderByDesc (P=>P.Name)]
		public Expression<Func<T, object>> OrderByDescending { get; set; }

	
		public int Take { get; set; }


		public int Skip { get; set; }

		public bool IsPaginationEnable { get; set; }
	
	}
}
