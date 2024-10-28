using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services
{
	public interface IOrderService
	{


		Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);

		Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string BuyerEmail);

		Task<Order?> GetOrderByIdForSpecificUser(string BuyerEmail, int OrderId);

	}
}
