using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public static class StoreContextSeed
	{

		public static async Task SeedAsync(StoreContext _dbcontext)
		{
			if (!_dbcontext.ProductBrands.Any())
			{
			var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
			var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

				//brands = brands.Select(b=> new ProductBrand() 
				//{
				//	Name = b.Name,
				
				//}).ToList();

				if ( brands?.Count > 0)

				{
					foreach (var brand in brands)
						_dbcontext.Set<ProductBrand>().Add(brand);

					await _dbcontext.SaveChangesAsync();
				}
			}


			if (!_dbcontext.ProductTypes.Any())
			{
			var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/types.json");
			var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

				//types = brands.Select(b => new ProductBrand()
				//{
				//	Name = b.Name,

				//}).ToList();

				if (types?.Count > 0)

				{
					foreach (var type in types)
						_dbcontext.Set<ProductType>().Add(type);

					await _dbcontext.SaveChangesAsync();
				}
			}
			
			if (!_dbcontext.Products.Any())
			{
			var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
			var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

				//types = brands.Select(b => new ProductBrand()
				//{
				//	Name = b.Name,

				//}).ToList();

				if (Products?.Count > 0)

				{
					foreach (var product in Products)
						_dbcontext.Set<Product>().Add(product);

					await _dbcontext.SaveChangesAsync();
				}
			}
			
			if (!_dbcontext.DeliveryMethods.Any())
			{
			var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
			var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);


				if (DeliveryMethods?.Count > 0)

				{
					foreach (var DeliveryMethod in DeliveryMethods)
						_dbcontext.Set<DeliveryMethod>().Add(DeliveryMethod);

					await _dbcontext.SaveChangesAsync();
				}
			}
		
		
		
		
		}
	}
		
		
}
