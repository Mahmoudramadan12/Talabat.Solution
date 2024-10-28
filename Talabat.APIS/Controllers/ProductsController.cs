using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.Core.Entites;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification;

namespace Talabat.APIS.Controllers
{
	//
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductType> _typeRepo;

		public ProductsController(IGenericRepository<Product> productRepo , IMapper mapper , IGenericRepository<ProductBrand> brandRepo , IGenericRepository<ProductType> typeRepo)
        {
			_productRepo = productRepo;
			_mapper = mapper;
			_brandRepo = brandRepo;
		    _typeRepo = typeRepo;
		}


		#region Get Products
		// Get All Products
		//BaseUrl/api/Products -> Get


		[CachedAttribute(300)]
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
		{

			///var products = await _productRepo.GetAllAsync();

			///using  AddInclude and base class
			///var Spec = new BaseSpecifications<Product>();
			///Spec.AddInclude(p => p.ProductBrand);
			///Spec.AddInclude(p => p.ProductType);
			///var products = await _productRepo.GetAllWithSpecAsync(Spec);



			// using ProductWithBrandAndTypeSpecifications that add them in his
			// constructor
			var Spec = new ProductWithBrandAndTypeSpecifications(Params);
			var products = await _productRepo.GetAllWithSpecAsync(Spec);

			var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var CountSpec = new ProductWithFiltrationForCountAsync(Params);
			var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);

			//var ReturedObject = new Pagination<ProductToReturnDto>()
			//{
			//	PageIndex = Params.PageIndex,
			//	PageSize = Params.PageSize,
			//	Data = MappedProducts

			//};

			//return Ok(ReturedObject); // 200 OK


			return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize , MappedProducts , Count));

		}
		#endregion


		#region Get Product By Id
		// Get Product By Id 
		//BaseUrl/api/Products/1 -> Get
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]

		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			// with out Specifications
			//var product = await _productRepo.GetByIdAsync(id);

			////using  AddInclude and base class

			//var Spec = new BaseSpecifications<Product>(p => p.Id == id);
			//Spec.AddInclude(p => p.ProductType);
			//Spec.AddInclude(p => p.ProductBrand);

			var Spec = new ProductWithBrandAndTypeSpecifications(id);
			var product = await _productRepo.GetByEntityWithSpecAsync(Spec);

			var mappedProduct =_mapper.Map<Product, ProductToReturnDto>(product);
			if (product == null)
				return NotFound(new ApiResponse(404)); // 404
			return Ok(mappedProduct);
		}


		#endregion



		#region Get Brands
		// Get All Brands
		//BaseUrl/api/Products/Brands -> Get


		[HttpGet("Brands")]

		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var Brands = await _brandRepo.GetAllAsync();
			return Ok(Brands);

		}
		#endregion

		// Get All Brands
		//BaseUrl/api/Products/Types -> Get

		#region Get Types

		[HttpGet("Types")]

		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
		{
			var Types = await _typeRepo.GetAllAsync();
			return Ok(Types);
		}

		#endregion



		

	}
}
