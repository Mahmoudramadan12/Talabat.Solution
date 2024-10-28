using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services;
using Talabat.Core.Specification;
using Product = Talabat.Core.Entites.Product;

namespace Talabat.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork  unitOfWork
			)
        {
			this._configuration = configuration;
			this._basketRepository = basketRepository;
			this._unitOfWork = unitOfWork;
		}
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
		{
			// 125
			StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

			// Get Basket
			var Basket = await _basketRepository.GetBasketAsync(BasketId);
			if (Basket == null) 
				return null;
			var ShippingPrice = 0m;
			// Amount = SubTotal + DM.Cost
			if (Basket.DeliveryMethodId.HasValue)
			{
				var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
				ShippingPrice = DeliveryMethod.Cost;

			}

			if (Basket.Items.Count>0)
			{
				foreach ( var item in Basket.Items)
				{
					var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					if (item.Price != Product.Price)
						item.Price = Product.Price;

				}

			}


			var SubTotal = Basket.Items.Sum(item => item.Quantity * item.Price);

			var Service = new PaymentIntentService();
			PaymentIntent paymentIntent;
			if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // Create
			{
				var Options = new PaymentIntentCreateOptions()
				{
					Amount = (long)((ShippingPrice * 100) + (SubTotal * 100)),
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }

				};
				paymentIntent = await Service.CreateAsync(Options);
				Basket.PaymentIntentId = paymentIntent.Id;
				Basket.ClientSecret = paymentIntent.ClientSecret;
			}

			else // Update
			{
				var Options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)((ShippingPrice * 100) + (SubTotal * 100))

				};

				paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId,Options);
				Basket.PaymentIntentId = paymentIntent.Id;
				Basket.ClientSecret = paymentIntent.ClientSecret;
			}
			await _basketRepository.UpdateBasketAsync(Basket);
			return Basket;
		}

		public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string PaymentIntentId, bool flag)
		{
			var Spec = new OrderWithPaymentIntentIdSpec(PaymentIntentId);
			var order = await _unitOfWork.Repository<Order>().GetByEntityWithSpecAsync(Spec);
			if (flag) 
				order.Status = OrderStatus.PaymentReceived;
			else
				order.Status = OrderStatus.PaymentFailed;

			_unitOfWork.Repository<Order>().Update(order);
			await _unitOfWork.CompleteAsync();
			return order;
		}
	}
}
