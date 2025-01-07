using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Queries;
using FluentValidation.Results;
using MassTransit.RabbitMqTransport;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using static VFi.Application.PIM.Commands.ProductCommand;
using static VFi.Application.PIM.DTOs.AuctionDto;
using static VFi.Application.PIM.DTOs.MercariDto;
using static MassTransit.ValidationResultExtensions;
using static System.Net.Mime.MediaTypeNames;
using Product = VFi.Domain.PIM.Models.Product;

namespace VFi.Application.PIM.Commands
{
    internal class ProductExCommandHandler : CommandHandler,
                                                            IRequestHandler<ProductAddFromLinkCommand, ValidationResult> 
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IProductStoreMappingRepository _productStoreMappingRepository;
        private readonly IProductGroupCategoryMappingRepository _productGroupCategoryMappingRepository;
        private readonly IProductProductTagMappingRepository _productProductTagMappingRepository;
        private readonly IProductCategoryMappingRepository _productCategoryMappingRepository;
        private readonly IPIMContextProcedures _pimContextProcedures;
        private readonly IProductInventoryRepository _ProductInventoryRepository;
        private readonly IProductPackageRepository _ProductPackageRepository;
        private readonly IContextUser _context;
        private readonly IProductProductAttributeMappingRepository _productProductAttributeMappingRepository;
        private readonly IProductVariantAttributeValueRepository _productVariantAttributeValueRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IProductMediaRepository _productMediaRepository;
        private readonly IMercariRepository _mercariRepository;
        public ProductExCommandHandler(IProductRepository productRepository, IProductTagRepository productTagRepository,
            IProductStoreMappingRepository productStoreMappingRepository,
            IProductGroupCategoryMappingRepository productGroupCategoryMappingRepository,
            IProductCategoryMappingRepository productCategoryMappingRepository,
            IProductProductTagMappingRepository productProductTagMappingRepository,
            IPIMContextProcedures pimContextProcedures,
            IProductInventoryRepository productInventoryRepository,
            IProductPackageRepository productPackageRepository, 
            IContextUser context,
            IProductProductAttributeMappingRepository productProductAttributeMappingRepository,
            IProductVariantAttributeValueRepository productVariantAttributeValueRepository,
            IProductAttributeRepository productAttributeRepository,
            IAuctionRepository auctionRepository, IProductMediaRepository productMediaRepository, IMercariRepository mercariRepository
            )
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _productStoreMappingRepository = productStoreMappingRepository;
            _productGroupCategoryMappingRepository = productGroupCategoryMappingRepository;
            _productCategoryMappingRepository = productCategoryMappingRepository;
            _productProductTagMappingRepository = productProductTagMappingRepository;
            _pimContextProcedures = pimContextProcedures;
            _ProductInventoryRepository = productInventoryRepository;
            _ProductPackageRepository = productPackageRepository;
            _context = context;
            _productProductAttributeMappingRepository = productProductAttributeMappingRepository;
            _productVariantAttributeValueRepository = productVariantAttributeValueRepository;
            _productAttributeRepository= productAttributeRepository; 
            _auctionRepository = auctionRepository; 
            _productMediaRepository = productMediaRepository; 
            _mercariRepository = mercariRepository;
        }
        public void Dispose()
        {
            _productRepository.Dispose();
        }
        

        public async Task<ValidationResult> Handle(ProductAddFromLinkCommand request, CancellationToken cancellationToken)
        {
             
            if (!request.IsValid(_productRepository)) return request.ValidationResult;

            var prodFromLink = new ProductPublish();

            if (request.Link.Contains("auctions.yahoo.co.jp"))
            {
                var auctionItem = new AuctionProductPublish();
                var aucContent = await _auctionRepository.GetProductDetail(request.Link);
                auctionItem.Convert(aucContent);
                prodFromLink = auctionItem;
            }
            if (request.Link.Contains("jp.mercari.com"))
            {
                var mercariItem = new MercariProductPublish();
                var mercariContent = await _mercariRepository.GetItem(request.Link);
                mercariItem.Convert(mercariContent);
                prodFromLink = mercariItem;
            }
        
            var product = new Product
            {
                Id = request.Id,
                Code = request.Code,
                ProductTypeId = Guid.Parse("d5bcc621-2fc9-40da-80d6-6a8f45556d4f"),
                ProductType = "",
                ForBuy = true,
                ForSale = true,
                ForProduction = false,
                Condition = 20,
                UnitType = "QTY",
                UnitId = Guid.Parse("5cb13608-0c78-446f-b70e-1e800bd5be6a"),
                UnitCode = "CHIEC",
                Name = prodFromLink.Name,
                SourceLink = prodFromLink.SourceLink,
                SourceCode = prodFromLink.SourceCode,
                ShortDescription = prodFromLink.ShortDescription,
                FullDescription = prodFromLink.FullDescription,
                //ManufacturerNumber = productCrawler.ManufacturerNumber,
                Image = prodFromLink.Image,
                //Gtin = productCrawler.Gtin,
                ProductCost = prodFromLink.Price,
                CurrencyCost = "JPY",
                Price = prodFromLink.Price,
                HasTierPrices = false,
                Currency = "JPY",
                IsTaxExempt = prodFromLink.IsTaxExempt,
                //TaxCategoryId = productCrawler.TaxCategoryId,
                //IsEsd = productCrawler.IsEsd,
                OrderMinimumQuantity = 1,
                OrderMaximumQuantity = 1,
                QuantityStep = 1,
                //ManageInventoryMethodId = request.ManageInventoryMethodId,
                UseMultipleWarehouses =false,
                //WarehouseId = request.WarehouseId,
                Sku = prodFromLink.Code,
                StockQuantity = 0,
                ReservedQuantity = 0,
                PlannedQuantity = 0,
                Packages = 1,
                //Weight = request.Weight,
                //Length = request.Length,
               // Width = request.Width,
                //Height = request.Height,
                //DeliveryTimeId = request.DeliveryTimeId,
                IsShipEnabled = true,
                IsFreeShipping = false,
                //AdditionalShippingCharge = request.AdditionalShippingCharge,
                CanReturn = false,
                //CustomsTariffNumber = request.CustomsTariffNumber,
                Deleted = false,
                Status = 1,
                CreatedBy = request.CreatedBy,
                CreatedDate = DateTime.Now,
                //OriginId = request.OriginId,
                //BrandId = request.BrandId,
               // ManufacturerId = request.ManufacturerId,
               // CategoryRootId = request.CategoryRootId,
                //CategoryRoot = request.CategoryRoot,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //product.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productRepository.Add(product);
            foreach (var item in prodFromLink.Images)
            {
                var productMedia = new ProductMedia
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    DisplayOrder = 0,
                    ProductId = product.Id,
                    MediaType = Path.GetExtension(item),
                    Path = item,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = DateTime.Now,
                    CreatedByName = request.CreatedByName
                };

                _productMediaRepository.Add(productMedia);
                // await Commit(_productMediaRepository.UnitOfWork);
            }
            try
            {
                return await Commit(_productRepository.UnitOfWork);
            }
            catch (Exception)
            {

                return new ValidationResult();

            }
        }
         
    }
}