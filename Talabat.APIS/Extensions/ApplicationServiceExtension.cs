using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Repository;
using Talabat.Service;

namespace Talabat.APIS.Extensions
{
	public static class ApplicationServiceExtension
	{
		public static IServiceCollection AddApplicationServices( this IServiceCollection Services)
		{

			//builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//builder.Services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();

			Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
	     	Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

		
			//builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfiles ()));

			Services.AddAutoMapper(typeof(MappingProfiles));

			#region Error Handling 
	        Services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					// ModelState => Dic [KeyValuePair]
					// Key => Name of Param
					// Value =>Error

					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
														 .SelectMany(p => p.Value.Errors)
														 .Select(e => e.ErrorMessage).ToArray();



					var ValidationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};


					return new BadRequestObjectResult(ValidationErrorResponse);


				};

			});


			#endregion

			Services.AddScoped<IUnitOfWork , UnitOfWork>();

			Services.AddScoped<IOrderService, OrderService>();

			Services.AddScoped<IPaymentService , PaymentService>();
			Services.AddSingleton<IResponseCacheService , ResponseCacheService>();
			return Services;
		}
	}
}
