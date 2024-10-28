using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specification
{
	public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderByDescending { get; set; }
		public int Take { get; set; }
		public int Skip { get; set; }
		public bool IsPaginationEnable { get; set; }
		public BaseSpecifications()
        {
            
            //Includes = new List<Expression<Func<T, object>>>();
        }

        public BaseSpecifications( Expression<Func<T, bool>> crieriaExpression)
        {
            //Includes = new List<Expression<Func<T, object>>>();

            Criteria = crieriaExpression;
        }

		public void AddInclude(Expression<Func<T, object>> includeExpression)
		{
			Includes.Add(includeExpression);
		}
		public void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
		{
			OrderBy = OrderByExpression;
		}
		public void AddOrderByDesceding(Expression<Func<T, object>> OrderByDescExpression)
		{
			OrderByDescending = OrderByDescExpression;
		}


		public void ApplyPagination(int skip , int take)
		{
			IsPaginationEnable = true;
			Skip = skip;
			Take = take;

		}



	}


}
