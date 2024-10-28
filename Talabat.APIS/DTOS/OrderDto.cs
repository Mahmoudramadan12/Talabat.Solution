using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIS.DTOS
{

	public class OrderDto
	{
		[Required]
	   public string BasketId { get; set; }
		[Required]
		public int DeliveryMethodId { get; set; }
		[Required]
		public AddressDto ShippingAddress { get; set; }
	}
}
