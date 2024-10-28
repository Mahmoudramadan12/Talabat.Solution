using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIS.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _expireTimeInSeconds;

		public CachedAttribute( int ExpireTimeInSeconds)
        {
			_expireTimeInSeconds = ExpireTimeInSeconds;
		}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

			var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
			var CachedReposne = await CacheService.GetCachedResponseAsync(CacheKey);
			if (!string.IsNullOrEmpty(CachedReposne))
			{
				var contentResult = new ContentResult()
				{
					Content = CachedReposne,
					ContentType = "application/json",
					StatusCode = 200,

				};
				context.Result = contentResult;
				return;
			}

			var ExecutedEndPointContext = await next.Invoke(); // Will  Execute EndPoint
			if (ExecutedEndPointContext.Result is OkObjectResult result)
			{

				await CacheService.CacheResponseAsync(CacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSeconds));


			}
		}

		private string GenerateCacheKeyFromRequest( HttpRequest request)
		{
			var keyBuilder = new StringBuilder();
			keyBuilder.Append(request.Path); //api/Products

			foreach (var (key,value) in request.Query.OrderBy(X=>X.Key))
			{
				// Sort = Name
				// Page Index = 1
				// Page Size = 5

				keyBuilder.Append($"|{key}-{value}"); // api/Products|Sort-Name|PageIndex-1|PageSize-5
			}
			return keyBuilder.ToString();

		}
	}
}
