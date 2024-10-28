using AutoMapper;
using Talabat.APIS.DTOS;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIS.Helpers
{
	public class MappingProfiles :Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductBrand, o=>o.MapFrom(m=>m.ProductBrand.Name))
                .ForMember(d=>d.ProductType,o=>o.MapFrom(m=>m.ProductType.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>())
                ;

            CreateMap<Core.Entites.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto , CustomerBasket>();
            CreateMap<BasketItemDto , BasketItem>();

            CreateMap<AddressDto , Core.Entites.Order_Aggregate.Address>();

			CreateMap<Order, OrderToReturnDto>()
					.ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
					.ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductId,o=>o.MapFrom(d=>d.Product.ProductId))
                .ForMember(d=>d.ProductName,o=>o.MapFrom(d=>d.Product.ProductName))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom(d=>d.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderPictureUrlResolver>());
                 



		}
	}
}
