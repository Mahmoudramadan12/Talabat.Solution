using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System.Net.Http;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Talabat.APIS.Controllers
{

	public class PaymentsController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		// This is your Stripe CLI webhook secret for testing your endpoint locally.
		//const string endpointSecret = "whsec_c0082f1a1f7b1fcc0c6551fddb7059db46e314ba92a8a7aa1668518364a2abc1";
		const string endpointSecret = "whsec_e1ece0d2bdd9f64603beb57bdfdd2adb50b47b689d22c9d6b75dad6af27d8656";
		public PaymentsController(IPaymentService paymentService)
		{
			this._paymentService = paymentService;
		}
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var Basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (Basket is null)
				return BadRequest(new ApiResponse(400, "There is A Problem with Your Basket"));

			return Ok(Basket);

		}


		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebHook()
		{

			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			var signatureHeader = Request.Headers["Stripe-Signature"];
			var stripeEvent = EventUtility.ParseEvent(json);


			try
			{
				stripeEvent = EventUtility.ConstructEvent(json,
								   signatureHeader, endpointSecret);
					var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

				// Handle the event
				if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
				{
					await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id , true);

				}
				else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
				{
					await _paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, false);
				}
				
				return Ok();
			}

			catch (StripeException e)
			{
				Console.WriteLine("Error: {0}", e.Message);
				return BadRequest();
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}



		}
	}

}
