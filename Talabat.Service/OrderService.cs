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

namespace Talabat.Service
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		//private readonly IGenericRepository<Product> _productRepo;
		//private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		//private readonly IGenericRepository<Order> _orderRepo;

		public OrderService(IBasketRepository basketRepo , IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
			this._basketRepo = basketRepo;
			this._unitOfWork = unitOfWork;
			this._paymentService = paymentService;
			//_productRepo = ProductRepo;
			//_deliveryMethodRepo = DeliveryMethodRepo;
			//_orderRepo = OrderRepo;
		}
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
		{
			//1.Get Basket From Basket Repo

			var Basket = await _basketRepo.GetBasketAsync(BasketId);

			//2.Get Selected Items at Basket From Product Repo
			var OrderItems = new List<OrderItem>();
			if (Basket?.Items.Count > 0)
			{
				foreach (var item in Basket.Items)
				{
					var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);

					var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, Product.Price);
					OrderItems.Add(OrderItem);


				}

			}


			//3.Calculate SubTotal

			var SubTotal = OrderItems.Sum(item => item.Quantity * item.Price);
			//4.Get Delivery Method From DeliveryMethod Repo
			var DeliveryMethod = await  _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
			//5.Create Order
			var Spec = new OrderWithPaymentIntentIdSpec(Basket.PaymentIntentId);

			var ExOrder = await _unitOfWork.Repository<Order>().GetByEntityWithSpecAsync(Spec);

			if (ExOrder != null)
			{
				 _unitOfWork.Repository<Order>().Delete(ExOrder);
				await _paymentService.CreateOrUpdatePaymentIntent(BasketId);

			}

			var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal , Basket.PaymentIntentId);
			//6.Add Order Locally

			await _unitOfWork.Repository<Order>().AddAsync(Order);

			//7.Save Order To Database[ToDo]

			 var Result = await _unitOfWork.CompleteAsync();

			if (Result <= 0)
				return null;
			return Order;

		}

		public async Task<Order?> GetOrderByIdForSpecificUser(string BuyerEmail, int OrderId)
		{

			var Spec =  new OrderSpecifications(BuyerEmail, OrderId);
			var Order = await _unitOfWork.Repository<Order>().GetByEntityWithSpecAsync(Spec);

			return Order;
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUser(string BuyerEmail)
		{
			var Spec = new OrderSpecifications(BuyerEmail);
			var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);

			return Orders;

		}
	}
}
