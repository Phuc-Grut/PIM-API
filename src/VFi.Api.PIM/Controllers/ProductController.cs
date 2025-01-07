using Consul;
using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using PuppeteerSharp;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static VFi.Application.PIM.Commands.ProductCommand;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProductController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductController> _logger;
        private readonly CodeSyntaxConfig _codeSyntax;
        public ProductController(IMediatorHandler mediator, IContextUser context, CodeSyntaxConfig codeSyntax, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger; _codeSyntax = codeSyntax;
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new ProductQueryById(id));
            return Ok(rs);
        }
        [HttpGet("get-by-categoryrootid/{id}")]
        public async Task<IActionResult> GetByCRId(Guid id)
        {
            var rs = await _mediator.Send(new ProductQueryByCategoryRootId(id));
            return Ok(rs);
        }

        [HttpGet("get-by-code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var rs = await _mediator.Send(new ProductQueryByProductCode(code));
            return Ok(rs);
        }

        [HttpGet("get-code")]
        public async Task<IActionResult> GetCode(string syntax, int use)
        {
            var rs = await _mediator.Send(new GetCodeQuery(syntax, use));
            return Ok(rs);
        }
        [HttpGet("use-code")]
        public async Task<IActionResult> UseCode(string syntax)
        {
            return Ok();
        }
        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductRequest request)
        {
            var result = await _mediator.Send(new ProductQueryListBox(request.ToBaseQuery(), request.Keyword));
            return Ok(result);
        }

        [HttpGet("get-cbx-by-categories")]
        public async Task<IActionResult> GetListBoxBycategory([FromQuery] PagingProductCategoryMappingRequest request)
        {
            var result = await _mediator.Send(new ProductCategoryMappingQueryListBox(request.ToBaseQuery()));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Pagedresult([FromQuery] FilterQuery request)
        {
            var query = new ProductPagingFilterQuery(request?.Keyword, request?.Filter, request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("get-by-list-code")]
        public async Task<IActionResult> GetByListCode([FromBody] ProductQueryByListCodeRequest request)
        {
            var rs = await _mediator.Send(new ProductQueryByListCode(request.Codes.Split(",").ToList()));
            return Ok(rs);
        }

        [HttpGet("inventory")]
        public async Task<IActionResult> Inventory([FromQuery] FilterProductInventoryQuery request
            )
        {
            List<string> data = request.ListProduct.Split(',').ToList();
            var rs = await _mediator.Send(new ProductInventoryQueryByCode(data));
            return Ok(rs);
        }
        [HttpGet("inventory-by-list-id")]
        public async Task<IActionResult> InventoryListById([FromQuery] FilterProductInventoryQuery request
            )
        {
            var rs = await _mediator.Send(new ProductInventoryQueryByListId(request.ListProduct));
            return Ok(rs);
        }
        [HttpGet("inventory-warehouse-by-id/{id}")]
        public async Task<IActionResult> InventoryWarehouseById(Guid id)
        {
            var rs = await _mediator.Send(new ProductListWarehouseQueryById(id));
            return Ok(rs);
        }

        [HttpGet("get-price")]
        public async Task<IActionResult> GetPrice([FromQuery] FilterProductQueryPrice request)
        {
            var rs = await _mediator.Send(new ProductQueryPrice(request.ListProduct));
            return Ok(rs);
        }

        [HttpGet("inventory-detail-by-list-id")]
        public async Task<IActionResult> GetInventoryDetailBylistId([FromQuery] FilterProductQueryInventoryDetailBylistId request)
        {
            var rs = await _mediator.Send(new ProductQueryInventoryDetailByListId(request.ListProduct));
            return Ok(rs);
        }

        [HttpGet("get-inventory-by-product-id/{id}")]
        public async Task<IActionResult> InventoryByProductId(Guid id)
        {
            var rs = await _mediator.Send(new ProductStockQueryById(id));
            return Ok(rs);
        }

        [HttpGet("create-all-variant/{id}")]
        public async Task<IActionResult> CreateAllVariant(Guid id)
        {
            var productAddCommand = new ProductVariantCreateAllCommand(id);
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }
        
        [HttpPost("add-variant")]
        public async Task<IActionResult> Add([FromBody] AddProductVariantRequest request)
        {
            var productAddCommand = new ProductVariantAddCommand(
                request.Id,
                request.Code,
                request.Name,
                request.Status,
                request.AttributesJson,
                request.Sku,
                request.ManufacturerNumber,
                request.Gtin,
                request.Price,
                request.Currency,
                request.DeliveryTimeId,
                request.UnitId,
                request.UnitType,
                request.UnitCode,
                request.ManageInventoryMethodId,
                request.MultiPacking,
                request.Packages,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                _context.GetUserId(),
                _context.UserName,
                request.ParentId,
                request.ListMedia,
                request.ListInventory,
                request.ListPackage,
                request.ListProductSpecificationCode
            );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }
        [HttpPut("edit-size")]
        public async Task<IActionResult> EditSize([FromBody] SizeProductRequest request)
        {
            var productCommand = new ProductSizeEditCommand(
                request.Id,
                request.Weight,
                request.Length,
                request.Width,
                request.Height
            );
            var result = await _mediator.SendCommand(productCommand);
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductRequest request)
        {
            var ProductId = Guid.NewGuid();
            var Code = request.Code;
            int UsedStatus = 1;
            if (request.IsAuto == 1)
            {
                Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
            }
            else
            {
                var useCodeCommand = new UseCodeCommand(
                    request.ModuleCode,
                    Code
                    );
                _mediator.SendCommand(useCodeCommand);
            }
            var categories = request.Categories?.Select(x => new ProductCategoriesDto()
            {
                CategoryId = x.CategoryId,
                GroupCategoryId = x.GroupCategoryId,
            }).ToList();
            var productAddCommand = new ProductAddCommand(
                ProductId,
                Code,
                request.ProductType,
                request.ForBuy,
                request.ForSale,
                request.ForProduction,
                request.Condition,
                request.UnitId,
                request.UnitType,
                request.UnitCode,
                request.Name,
                request.SourceLink,
                request.ShortDescription,
                request.FullDescription,
                request.LimitedToStores,
                request.IdGroupCategories,
                categories,
                request.IdCategories,
                request.Origin,
                request.Brand,
                request.Manufacturer,
                request.ManufacturerNumber,
                request.Image,
                request.Gtin,
                request.ProductCost,
                request.CurrencyCost,
                request.Price,
                request.HasTierPrices,
                request.Currency,
                request.IsTaxExempt,
                request.TaxCategoryId,
                request.IsEsd,
                request.OrderMinimumQuantity,
                request.OrderMaximumQuantity,
                request.QuantityStep,
                request.ManageInventoryMethodId,
                request.UseMultipleWarehouses,
                request.WarehouseId,
                request.Sku,
                request.StockQuantity,
                request.ReservedQuantity,
                request.PlannedQuantity,
                request.Packages,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                request.DeliveryTimeId,
                request.IsShipEnabled,
                request.IsFreeShipping,
                request.AdditionalShippingCharge,
                request.CanReturn,
                request.CustomsTariffNumber,
                request.Deleted,
                request.Status,
                _context.GetUserId(),
                DateTime.Now,
                request.ProductTag,
                request.OriginId,
                request.BrandId,
                request.ManufacturerId,
                request.ProductTypeId,
                request.CategoryRootId,
                request.CategoryRoot,
                _context.UserName
          );
            productAddCommand.SourceCode = request.SourceCode;
            var result = await _mediator.SendCommand(productAddCommand);
            if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ProductCode AlreadyExists")
            {
                int loopTime = 5;
                for (int i = 0; i < loopTime; i++)
                {
                    productAddCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                    var res = await _mediator.SendCommand(productAddCommand);
                    if (res.IsValid == true)
                    {
                        return await HandleResult(res, ProductId);
                    }
                }
            }
            return await HandleResult(result, ProductId);
        }
        [HttpPost("add-compact")]
        public async Task<IActionResult> AddCompact([FromBody] AddProductRequest request)
        {
            var id = Guid.NewGuid();
            var Code = request.Code;
            int UsedStatus = 1;
            if (request.IsAuto == 1)
            {
                Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
            }
            else
            {
                var useCodeCommand = new UseCodeCommand(
                    request.ModuleCode,
                    Code
                    );
                _mediator.SendCommand(useCodeCommand);
            }
            var productAddCommand = new ProductAddCompactCommand(
                id,
                Code,
                request.Name,
                request.ProductTypeId,
                request.ProductType,
                request.CategoryRootId,
                request.CategoryRoot,
                request.ForBuy,
                request.ForSale,
                request.ForProduction,
                request.Condition,
                request.UnitId,
                request.UnitType,
                request.UnitCode,
                request.OriginId,
                request.Origin,
                request.Image,
                request.Status,
                request.Price,
                request.Currency,
                request.ProductCost,
                request.CurrencyCost
               
          );
            var result = await _mediator.SendCommand(productAddCommand);
            if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ProductCode AlreadyExists")
            {
                int loopTime = 5;
                for (int i = 0; i < loopTime; i++)
                {
                    productAddCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                    var res = await _mediator.SendCommand(productAddCommand);
                    if (res.IsValid == true)
                    {
                        res.RuleSetsExecuted = new[] { id.ToString() };
                        return Ok(res);
                    }
                }
            }
            result.RuleSetsExecuted = new[] { id.ToString() };
            return Ok(result);
        }
        private async Task<IActionResult> HandleResult(FluentValidation.Results.ValidationResult result, Guid ProductId)
        {
            if (result.IsValid == true)
            {
                return Ok(
                    new ProductResult()
                    {
                        Id = ProductId,
                        IsValid = result.IsValid,
                    }
                    );
            }
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductRequest request)
        {
            var categories = request?.Categories?.Select(x => new ProductCategoriesDto()
            {
                CategoryId = x.CategoryId,
                GroupCategoryId = x.GroupCategoryId,
            }).ToList();
            int stt = 1;
            foreach( var item in request.ListProductSpecificationCode)
            {
                item.DisplayOrder = stt++;
            }
            var productEditCommand = new ProductEditCommand(
                new Guid(request.Id),
                request.Code,
                request.ProductType,
                request.ForBuy,
                request.ForSale,
                request.ForProduction,
                request.Condition,
                request.UnitId,
                request.UnitType,
                request.UnitCode,
                request.Name,
                request.SourceLink,
                request.ShortDescription,
                request.FullDescription,
                request.LimitedToStores,
                request.IdGroupCategories,
                categories,
                request.IdCategories,
                request.Origin,
                request.Brand,
                request.Manufacturer,
                request.ManufacturerNumber,
                request.Image,
                request.Gtin,
                request.ProductCost,
                request.CurrencyCost,
                request.Price,
                request.HasTierPrices,
                request.Currency,
                request.IsTaxExempt,
                request.TaxCategoryId,
                request.IsEsd,
                request.OrderMinimumQuantity,
                request.OrderMaximumQuantity,
                request.QuantityStep,
                request.ManageInventoryMethodId,
                request.UseMultipleWarehouses,
                request.WarehouseId,
                request.Sku,
                request.StockQuantity,
                request.ReservedQuantity,
                request.PlannedQuantity,
                request.MultiPacking,
                request.Packages,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                request.DeliveryTimeId,
                request.IsShipEnabled,
                request.IsFreeShipping,
                request.AdditionalShippingCharge,
                request.CanReturn,
                request.CustomsTariffNumber,
                request.Deleted,
                request.Status,
                request.CreatedBy,
                _context.GetUserId(),
                request.CreatedDate,
                DateTime.Now,
                request.ProductTag,
                request.OriginId,
                request.BrandId,
                request.ManufacturerId,
                request.ProductTypeId,
                request.CategoryRootId,
                request.CategoryRoot,
                request.SpecificationAttributeOptions,
                request.ProductAttributes,
                request.RelatedProducts,
                _context.UserName,
                request.ListInventory,
                request.ListPackage,
                request.ListProductSpecificationCode,
                request.ListProductSpecificationAttributeMapping?.Select(x => new ProductSpecificationAttributeMappingDto()
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    SpecificationAttributeId = x.SpecificationAttributeId,
                    ProductId = x.ProductId,
                    SpecificationAttributeOptionId = x.SpecificationAttributeOptionId
                }).ToList(),
                request.ListTierPrice,
                request.ListProductServiceAdd
           );
            productEditCommand.SourceCode = request.SourceCode;
            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductDeleteCommand(new Guid(id)));

            return Ok(result);
        }
        [HttpPost("duplicate")]
        public async Task<IActionResult> Duplicate([FromBody] DuplicateProductRequest request)
        {
            var ProductId = request.Id;
            var Code = request.Code;
            int UsedStatus = 1;
            if (request.IsAuto == 1)
            {
                Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
            }
            else
            {
                var useCodeCommand = new UseCodeCommand(
                    request.ModuleCode,
                    Code
                    );
                _mediator.SendCommand(useCodeCommand);
            }
            var productDuplicateCommand = new ProductDuplicateCommand(
                ProductId,
                Code,
                request.Name,
                UsedStatus,
                 _context.GetUserId(),
                 _context.UserName
           );
            var result = await _mediator.SendCommand(productDuplicateCommand);
            
            var productId = await _mediator.Send(new ProductQueryByCode(Code));

            if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ProductCode AlreadyExists")
            {
                int loopTime = 5;
                for (int i = 0; i < loopTime; i++)
                {
                    productDuplicateCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                    var res = await _mediator.SendCommand(productDuplicateCommand);
                    if (res.IsValid == true)
                    {
                        return await HandleResult(res, ProductId);
                    }
                }
            }
            return await HandleResult(result, productId.Id);
        } 
        
    }
}