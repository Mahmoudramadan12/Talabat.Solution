using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specification
{
	public class ProductWithBrandAndTypeSpecifications :BaseSpecifications<Product>
	{
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params)
		: base(P =>
		(string.IsNullOrEmpty(Params.Search)||P.Name.ToLower().Contains(Params.Search))&&
	    (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) &&
	    (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId))
		{
			Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
					case "PriceAsc":
						AddOrderBy(p => p.Price);
						break;
					case "PriceDesc":
						AddOrderByDesceding(p => p.Price);
						break;
					default:
						AddOrderBy(p => p.Name);
						break;
							
							

				}
			}

			



			ApplyPagination(Params.PageSize * (Params.PageIndex -1),Params.PageSize);

        }
		
        public ProductWithBrandAndTypeSpecifications(int id) :base(p=>p.Id == id)
        {
			Includes.Add(p => p.ProductBrand);
			Includes.Add(p => p.ProductType);

		}
    }
}
