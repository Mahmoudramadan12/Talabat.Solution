using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Talabat.Core.Specification
{
	public class OrderWithPaymentIntentIdSpec : BaseSpecifications<Order>
	{
        public OrderWithPaymentIntentIdSpec(string  PaymentIntentId) : base(o=>o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
