using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIS.Helpers
{
	public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderPictureUrlResolver(IConfiguration configuration)
        {
			this._configuration = configuration;
		}
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
				return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
			return string.Empty;
		}
	}
}
