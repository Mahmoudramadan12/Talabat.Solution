﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specification
{
	public class ProductWithFiltrationForCountAsync : BaseSpecifications<Product>
	{

        public ProductWithFiltrationForCountAsync(ProductSpecParams Params)
        :base(P=>
		(string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search)) &&
		(!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) &&
		(!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId))
		{

		}

    }
}