using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services
{
	public interface IPaymentService
	{
		// Create Or Update PaymentIntentId

		Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);

		Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag);


	}
}
