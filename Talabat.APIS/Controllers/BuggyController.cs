using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIS.Controllers
{
	
	public class BuggyController : BaseApiController
	{
		private readonly StoreContext _dbcontext;

		public BuggyController(StoreContext dbcontext)
        {
			this._dbcontext = dbcontext;
		}



		[HttpGet("NotFound")]

		public ActionResult GetNotFoundRequest()
		{

			var Product = _dbcontext.Products.Find(100);
			if (Product is null)
				return NotFound();
			return Ok(Product);
		}
		
		
		
		[HttpGet("ServerError")]
		public async Task<ActionResult> GetServerError()
		{
			var product =  _dbcontext.Products.Find(100);
			var productToReturn = product.ToString();
			return Ok(productToReturn);

		}
		 


		[HttpGet("BadRequset")]
		public ActionResult GetBadRequset()
		{
			return BadRequest(new ApiResponse(400));
		}
		[HttpGet("BadRequset/{id}")]
		public ActionResult GetBadRequset(int id)
		{
			return Ok();
		}
	}
}
