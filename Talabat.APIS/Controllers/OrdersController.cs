using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services;

namespace Talabat.APIS.Controllers
{
	
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController( IOrderService orderService , IMapper mapper , IUnitOfWork unitOfWork)
        {
			this._orderService = orderService;
			this._mapper = mapper;
			this._unitOfWork = unitOfWork;
		}


		#region Create Order
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		[Authorize]



		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
			var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

			if (Order is null)
				return BadRequest(new ApiResponse(400, "There is a Problem with Your Order"));

			return Ok(Order);

		}
		#endregion

		#region Get Orders For User
		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet]
		[Authorize]

		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var BayerEmail = User.FindFirstValue(ClaimTypes.Email);

			var Order = await _orderService.GetOrdersForSpecificUser(BayerEmail);
			if (Order is null)
				return NotFound(new ApiResponse(404, "There is no Ordres For This User "));

			var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Order);
			
			return Ok(MappedOrder);

		}

		#endregion




		#region Get Order By Id For User
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[Authorize]
		[HttpGet("{id}")]

		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var Order = await _orderService.GetOrderByIdForSpecificUser(BuyerEmail, id);

			if (Order is null)
				return NotFound(new ApiResponse(404, $"There is no Order with {id} For This User"));

			var MappedOrder = _mapper.Map<Order,OrderToReturnDto>(Order);

			return Ok(MappedOrder);


		}
		#endregion



		[HttpGet("DeliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
		
		
			return Ok (DeliveryMethod);

		
		}




	}
}
