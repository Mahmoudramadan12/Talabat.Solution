using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIS.Controllers
{

	public class BasketController :BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository  basketRepository , IMapper mapper)
        {
			_basketRepository = basketRepository;
			this._mapper = mapper;
		}



		// Get	Or ReCreate Basket 
		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
		{
			var Basket = await _basketRepository.GetBasketAsync(BasketId);

			return Basket is null ? new CustomerBasket(BasketId) : Basket;

		}



		// Update Or Create New Basket


		[HttpPost]

		public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto Basket)
		{
			var MappedBasket = _mapper.Map<CustomerBasketDto , CustomerBasket>(Basket);
			var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);

			if (CreatedOrUpdatedBasket is null)
				return BadRequest(new ApiResponse(400));
			return Ok(CreatedOrUpdatedBasket);
		
		}



		//Delete Basket  

		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
		{

			return await _basketRepository.DeleteBasketAsync(BasketId);

		}

	}
}
