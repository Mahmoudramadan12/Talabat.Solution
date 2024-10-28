
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helpers;
using Talabat.APIS.Middlewares;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repository;

namespace Talabat.APIS
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			#region Configure Services Add services to the container.
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddDbContext<AppIdentityDbcontext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			



			builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
			{

				var Connection = builder.Configuration.GetConnectionString("RedisConnection");

				return ConnectionMultiplexer.Connect(Connection);
			});

			builder.Services.AddCors(Options =>
			{
				Options.AddPolicy("MyPolicy", options =>
				{
					options.AllowAnyHeader();
					options.AllowAnyMethod();
					options.WithOrigins(builder.Configuration["FrontBaseUrl"]);

				});

			});


			#region with out using the serviers in class 
			//builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//builder.Services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();

			//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			//builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfiles ()));

			//builder.Services.AddAutoMapper(typeof(MappingProfiles));

			#region Error Handling 
			//builder.Services.Configure<ApiBehaviorOptions>(Options =>
			//	{
			//		Options.InvalidModelStateResponseFactory = (actionContext) =>
			//		{
			//			// ModelState => Dic [KeyValuePair]
			//			// Key => Name of Param
			//			// Value =>Error

			//			var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
			//												 .SelectMany(p => p.Value.Errors)
			//												 .Select(e => e.ErrorMessage).ToArray();



			//			var ValidationErrorResponse = new ApiValidationErrorResponse()
			//			{
			//				Errors = errors
			//			};


			//			return new BadRequestObjectResult(ValidationErrorResponse);


			//		};

			//	});
			#endregion

			#endregion

			// using the  Application Service in  External Class
			builder.Services.AddApplicationServices();


			builder.Services.AddIdentityService(builder.Configuration);
			#endregion

			var app = builder.Build();
			
			#region Update Database

			 //StoreContext dbcontext = new StoreContext();
			 //await dbcontext.Database.Migrate(); // Invaild


			// Group of Services lifeTime Scooped
			using var scope = app.Services.CreateScope(); // AddScoped
			// Services Its Self 
			var services = scope.ServiceProvider;

			var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				//Ask CLR Explicitly for Creating Object From DbContext Explicitly
				var _dbcontext = services.GetRequiredService<StoreContext>();
				await _dbcontext.Database.MigrateAsync(); // Updata Database

				var _Indentitydbcontext = services.GetRequiredService<AppIdentityDbcontext>();
				await _Indentitydbcontext.Database.MigrateAsync();

				var UserManager = services.GetRequiredService<UserManager<AppUser>>();
			   await AppIdentityDbcontextSeed.SeedUserAsync(UserManager);
				await StoreContextSeed.SeedAsync(_dbcontext); // Data Seeding
			
			
			
			}
			catch (Exception ex)
			{

				var Logger = LoggerFactory.CreateLogger<Program>();
				Logger.LogError(ex, "An Error Occurred During Migration");
			} 

			#endregion

			#region Configure -  Configure the HTTP request pipeline.
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
		    app.UseMiddleware<ExceptionMiddleWare>();
				//app.UseSwagger();
				//app.UseSwaggerUI();

				app.UseSwaggerMiddleware();
			}


			app.UseStatusCodePagesWithReExecute("/errors/{0}");
			app.UseHttpsRedirection();
			app.UseStaticFiles();


			app.UseCors("MyPolicy");

			app.UseAuthentication();
			app.UseAuthorization();



			app.MapControllers();

			#endregion
			
			
			
			app.Run();
		}
	}
}
