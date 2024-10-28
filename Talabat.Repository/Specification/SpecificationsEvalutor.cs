using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Talabat.Repository.Specification
{
	public static class SpecificationsEvalutor<T> where T : BaseEntity
	{

		// _dbcontext.Set<Product>().Where(p => p.Id == id)
		// .Include(p => p.ProductBrand).Include(p => p.ProductType)
		// .FirstOrDefaultAsync() as T;


		public static IQueryable<T> GetQuery (IQueryable<T> inputQuery , ISpecification<T> Spec)
		{

			var Query = inputQuery; // _dbcontext.Set<Product>()

			if (Spec.Criteria is not null)
			{
				Query = Query.Where(Spec.Criteria); // _dbcontext.Set<Product>().Where(p => p.Id == id)
			}

			if (Spec.OrderBy is not null)
			{
				Query = Query.OrderBy(Spec.OrderBy); //  _dbcontext.Set<Product>().OrderBy(p=>p.Name)

			}
			else if (Spec.OrderByDescending is not null)
			{
				Query = Query.OrderByDescending(Spec.OrderByDescending);

			}


			if (Spec.IsPaginationEnable)
			{
				Query = Query.Skip(Spec.Skip).Take(Spec.Take);

			}


			Query = Spec.Includes.Aggregate(Query, (CurrentQuery,IncludeExpression) => CurrentQuery.Include(IncludeExpression));

			Console.WriteLine("mmmmmtes",Query.ToQueryString());  // Log generated SQL

			return Query;

		}




	}
}
