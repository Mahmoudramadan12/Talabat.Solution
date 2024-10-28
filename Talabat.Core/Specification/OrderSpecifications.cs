using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specification
{
	public class OrderSpecifications : BaseSpecifications<Order>
	{
        // Get Orders  For User
        public OrderSpecifications(string Email) :base(o=>o.BuyerEmail == Email)
        {

            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDesceding(o => o.OrderDate);

        }
        // Used To Get Order For User
     public OrderSpecifications(string Email , int id) :base(o=>o.BuyerEmail == Email && o.Id == id)
        {

            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }
   
    
    
    
    }
}
