namespace Talabat.APIS.Extensions
{
	public static class AddSwaggerExtension
	{
		public static WebApplication UseSwaggerMiddleware(this WebApplication app)

		{

			app.UseSwagger();
			app.UseSwaggerUI();


			return app;
		}
	}
}
