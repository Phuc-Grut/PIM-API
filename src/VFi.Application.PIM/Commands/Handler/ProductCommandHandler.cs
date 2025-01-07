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
using static MassTransit.ValidationResultExtensions;
using static System.Net.Mime.MediaTypeNames;
using Product = VFi.Domain.PIM.Models.Product;

namespace VFi.Application.PIM.Commands
{
    internal class ProductCommandHandler : CommandHandler, IRequestHandler<ProductAddCommand, ValidationResult>,
                                                            IRequestHandler<ProductVariantAddCommand, ValidationResult>,
                                                            IRequestHandler<ProductAddCompactCommand, ValidationResult>,
                                                            IRequestHandler<ProductVariantCreateAllCommand, ValidationResult>,
                                                            IRequestHandler<ProductDeleteCommand, ValidationResult>,
                                                            IRequestHandler<ProductEditCommand, ValidationResult>,
                                                            IRequestHandler<ProductSizeEditCommand, ValidationResult>,
                                                            IRequestHandler<ProductDuplicateCommand, ValidationResult>,
                                                            IRequestHandler<ProductCrossCommand, ValidationResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSpecificationCodeRepository _productSpecificationCodeRepository;
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
        private readonly IProductMediaRepository _productMediaRepository;
        private readonly IProductSpecificationAttributeMappingRepository _productSpecificationAttributeMappingRepository;
        private readonly ITierPriceRepository _tierPriceRepository;
        private readonly IProductServiceAddRepository _productServiceAddRepository;
        public ProductCommandHandler(IProductRepository productRepository, IProductTagRepository productTagRepository,
            IProductStoreMappingRepository productStoreMappingRepository,
            IProductSpecificationCodeRepository productSpecificationCodeRepository,
            IProductGroupCategoryMappingRepository productGroupCategoryMappingRepository,
            IProductCategoryMappingRepository productCategoryMappingRepository,
            IProductProductTagMappingRepository productProductTagMappingRepository,
            IPIMContextProcedures pimContextProcedures,
            IProductInventoryRepository productInventoryRepository,
            IProductPackageRepository productPackageRepository,
            IContextUser context,
            IProductProductAttributeMappingRepository productProductAttributeMappingRepository,
            IProductVariantAttributeValueRepository productVariantAttributeValueRepository,
            IProductAttributeRepository productAttributeRepository, IProductMediaRepository productMediaRepository,
            IProductSpecificationAttributeMappingRepository productSpecificationAttributeMappingRepository,
            ITierPriceRepository tierPriceRepository,
            IProductServiceAddRepository productServiceAddRepository
            )
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _productSpecificationCodeRepository = productSpecificationCodeRepository;
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
            _productAttributeRepository = productAttributeRepository;
            _productMediaRepository = productMediaRepository;
            _productSpecificationAttributeMappingRepository = productSpecificationAttributeMappingRepository;
            _tierPriceRepository = tierPriceRepository;
            _productServiceAddRepository = productServiceAddRepository;
        }
        public void Dispose()
        {
            _productRepository.Dispose();
        }
        public async Task<ValidationResult> Handle(ProductVariantAddCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null || request.Id == new Guid())
            {

                if (!request.IsValid(_productRepository)) return request.ValidationResult;
            }
            DataTable tableInventory = new DataTable();
            tableInventory.Columns.Add("Id");
            tableInventory.Columns.Add("WarehouseId");
            tableInventory.Columns.Add("StockQuantity");
            tableInventory.Columns.Add("ReservedQuantity");
            tableInventory.Columns.Add("PlannedQuantity");
            tableInventory.Columns.Add("SpecificationCode1");
            tableInventory.Columns.Add("SpecificationCode2");
            tableInventory.Columns.Add("SpecificationCode3");
            tableInventory.Columns.Add("SpecificationCode4");
            tableInventory.Columns.Add("SpecificationCode5");
            tableInventory.Columns.Add("SpecificationCode6");
            tableInventory.Columns.Add("SpecificationCode7");
            tableInventory.Columns.Add("SpecificationCode8");
            tableInventory.Columns.Add("SpecificationCode9");
            tableInventory.Columns.Add("SpecificationCode10");
            DataRow rowI = null;
            if (request?.ListInventory?.Count > 0)
            {
                foreach (var _item in request?.ListInventory)
                {
                    rowI = tableInventory.NewRow();
                    rowI["Id"] = _item.Id;
                    rowI["WarehouseId"] = _item.WarehouseId;
                    rowI["StockQuantity"] = _item.StockQuantity;
                    rowI["ReservedQuantity"] = _item.ReservedQuantity;
                    rowI["PlannedQuantity"] = _item.PlannedQuantity;
                    rowI["SpecificationCode1"] = _item.SpecificationCode1;
                    rowI["SpecificationCode2"] = _item.SpecificationCode2;
                    rowI["SpecificationCode3"] = _item.SpecificationCode3;
                    rowI["SpecificationCode4"] = _item.SpecificationCode4;
                    rowI["SpecificationCode5"] = _item.SpecificationCode5;
                    rowI["SpecificationCode6"] = _item.SpecificationCode6;
                    rowI["SpecificationCode7"] = _item.SpecificationCode7;
                    rowI["SpecificationCode8"] = _item.SpecificationCode8;
                    rowI["SpecificationCode9"] = _item.SpecificationCode9;
                    rowI["SpecificationCode10"] = _item.SpecificationCode10;
                    tableInventory.Rows.Add(rowI);
                }
            }

            DataTable tablePackage = new DataTable();
            tablePackage.Columns.Add("Id");
            tablePackage.Columns.Add("Name");
            tablePackage.Columns.Add("Weight");
            tablePackage.Columns.Add("Length");
            tablePackage.Columns.Add("Width");
            tablePackage.Columns.Add("Height");
            DataRow rowP = null;
            if (request?.ListPackage?.Count > 0)
            {
                foreach (var _item in request?.ListPackage)
                {
                    rowP = tablePackage.NewRow();
                    rowP["Id"] = _item.Id;
                    rowP["Name"] = _item.Name;
                    rowP["Weight"] = _item.Weight;
                    rowP["Length"] = _item.Length;
                    rowP["Width"] = _item.Width;
                    rowP["Height"] = _item.Height;
                    tablePackage.Rows.Add(rowP);
                }
            }
            DataTable tableMedia = new DataTable();
            tableMedia.Columns.Add("Id");
            tableMedia.Columns.Add("Name");
            tableMedia.Columns.Add("Path");
            tableMedia.Columns.Add("MediaType");
            tableMedia.Columns.Add("DisplayOrder");
            DataRow rowM = null;
            if (request?.ListMedia?.Count > 0)
            {
                foreach (var _item in request?.ListMedia)
                {
                    rowM = tableMedia.NewRow();
                    rowM["Id"] = _item.Id;
                    rowM["Name"] = _item.Name;
                    rowM["Path"] = _item.Path;
                    rowM["MediaType"] = _item.MediaType;
                    rowM["DisplayOrder"] = _item.DisplayOrder;
                    tableMedia.Rows.Add(rowM);
                }
            }
            // thêm dữ liệu vào bảng mã quy cách
            DataTable tableSpecCode = new DataTable();
            tableSpecCode.Columns.Add("Id");
            tableSpecCode.Columns.Add("Name");
            tableSpecCode.Columns.Add("DuplicateAllowed");
            tableSpecCode.Columns.Add("Status");
            tableSpecCode.Columns.Add("DataTypes");
            tableSpecCode.Columns.Add("DisplayOrder");
            DataRow rowSpecCode = null;
            if (request?.ListProductSpecificationCode?.Count > 0)
            {
                foreach (var _item in request?.ListProductSpecificationCode)
                {
                    rowSpecCode = tableSpecCode.NewRow();
                    rowSpecCode["Id"] = _item.Id;
                    rowSpecCode["Name"] = _item.Name;
                    rowSpecCode["DuplicateAllowed"] = _item.DuplicateAllowed;
                    rowSpecCode["Status"] = _item.Status;
                    rowSpecCode["DataTypes"] = _item.DataTypes;
                    rowSpecCode["DisplayOrder"] = _item.DisplayOrder;
                    tableSpecCode.Rows.Add(rowSpecCode);
                }
            }
            var rs = await _pimContextProcedures.SP_ADD_PRODUCT_VARIANTAsync(
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
                request.UnitCode,
                request.UnitType,
                request.ManageInventoryMethodId,
                request.MultiPacking,
                request.Packages,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                request.ParentId,
                request.ActionBy,
                request.ActionByName,
                tableInventory,
                tablePackage,
                tableMedia,
                tableSpecCode
            );
            if (rs != 0)
            {
                return new ValidationResult();
            }
            else
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("notice", "Add Failure") });
            }
        }

        public void BuildCombination(int level, List<VariantAttribute> output, List<VariantAttribute[]> productOptions, List<Dictionary<string, string>> Result)
        {
            if (level < productOptions.Count)
            {
                foreach (VariantAttribute value in productOptions[level])
                {
                    var resultList = new List<VariantAttribute>();
                    resultList.AddRange(output);
                    resultList.Add(value);
                    if (resultList.Count == productOptions.Count)
                    {
                        Result.Add(resultList.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value));
                    }
                    BuildCombination(level + 1, resultList, productOptions, Result);
                }
            }
        }
        public async Task<ValidationResult> Handle(ProductVariantCreateAllCommand request, CancellationToken cancellationToken)
        {
            var actionBy = _context.GetUserId();
            var actionName = _context.UserClaims.FullName;

            var atts = await _productProductAttributeMappingRepository.Filter((Guid)request.Id);
            var attOption = await _productVariantAttributeValueRepository.GetByParentId(atts);

            var prdAtt = await _productAttributeRepository.GetAll();
            List<VariantAttribute[]> list = new List<VariantAttribute[]>();
            foreach (var att in atts)
            {
                var option = attOption.Where(x => x.ProductVariantAttributeId == att.Id);
                var _alias = prdAtt.First(x => x.Id == att.ProductAttributeId)?.Alias;
                List<VariantAttribute> newlist = new List<VariantAttribute>();
                foreach (var opt in option)
                {
                    newlist.Add(new VariantAttribute(_alias, opt.Alias));
                }
                list.Add(newlist.ToArray());
            }
            List<VariantAttribute> output = new List<VariantAttribute>();
            List<Dictionary<string, string>> Result = new List<Dictionary<string, string>>();
            BuildCombination(0, output, list, Result);
            List<string> listVariant = new List<string>();
            foreach (var r in Result)
            {
                string _string = "";
                foreach (var i in r)
                {
                    var a = "{" + '"' + i.Key + '"' + ":" + '"' + i.Value + '"' + "}";
                    if (_string == "")
                    {
                        _string = string.Concat(a);
                    }
                    else
                    {
                        _string = string.Concat(_string, ",", a);
                    }
                }
                listVariant.Add("[" + _string + "]");
            }
            var rs = await _pimContextProcedures.SP_CREATE_ALL_VARIANTAsync(request.Id, string.Join("; ", listVariant), actionBy, actionName);
            if (rs != 0)
            {
                return new ValidationResult();
            }
            else
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("notice", "Add Failure") });
            }
        }
        public async Task<ValidationResult> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productRepository)) return request.ValidationResult;

            var product = new Product
            {
                Id = request.Id,
                Code = request.Code,
                ProductTypeId = request.ProductTypeId,
                ProductType = request.ProductType,
                ForBuy = request.ForBuy,
                ForSale = request.ForSale,
                ForProduction = request.ForProduction,
                Condition = request.Condition,
                UnitId = request.UnitId,
                UnitType = request.UnitType,
                UnitCode = request.UnitCode,
                Name = request.Name,
                SourceLink = request.SourceLink,
                SourceCode = request.SourceCode,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                ManufacturerNumber = request.ManufacturerNumber,
                Image = request.Image,
                Gtin = request.Gtin,
                ProductCost = request.ProductCost,
                CurrencyCost = request.CurrencyCost,
                Price = request.Price,
                HasTierPrices = request.HasTierPrices,
                Currency = request.Currency,
                IsTaxExempt = request.IsTaxExempt,
                TaxCategoryId = request.TaxCategoryId,
                IsEsd = request.IsEsd,
                OrderMinimumQuantity = request.OrderMinimumQuantity,
                OrderMaximumQuantity = request.OrderMaximumQuantity,
                QuantityStep = request.QuantityStep,
                ManageInventoryMethodId = request.ManageInventoryMethodId,
                UseMultipleWarehouses = request.UseMultipleWarehouses,
                WarehouseId = request.WarehouseId,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                ReservedQuantity = request.ReservedQuantity,
                PlannedQuantity = request.PlannedQuantity,
                Packages = request.Packages,
                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                DeliveryTimeId = request.DeliveryTimeId,
                IsShipEnabled = request.IsShipEnabled,
                IsFreeShipping = request.IsFreeShipping,
                AdditionalShippingCharge = request.AdditionalShippingCharge,
                CanReturn = request.CanReturn,
                CustomsTariffNumber = request.CustomsTariffNumber,
                Deleted = request.Deleted,
                Status = request.Status,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                OriginId = request.OriginId,
                BrandId = request.BrandId,
                ManufacturerId = request.ManufacturerId,
                CategoryRootId = request.CategoryRootId,
                CategoryRoot = request.CategoryRoot,
                CreatedByName = request.CreatedByName,
                LimitedToStores = request.LimitedToStores,
            };

            //add domain event
            //product.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productRepository.Add(product);
            var result = await Commit(_productRepository.UnitOfWork);
            if (!result.IsValid) return result;
            //store
            List<ProductStoreMapping> productStores = new List<ProductStoreMapping>();
            if (request.LimitedToStores != null && request.LimitedToStores != "")
            {
                var stores = request.LimitedToStores.Split(',').ToList();
                foreach (string item in stores)
                {
                    productStores.Add(new ProductStoreMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        StoreId = new Guid(item)
                    });
                }
                if (productStores.Count > 0)
                {
                    _productStoreMappingRepository.Add(productStores);
                    await CommitNoCheck(_productStoreMappingRepository.UnitOfWork);
                }
            }
            //product category
            List<ProductGroupCategoryMapping> productGroupCategories = new List<ProductGroupCategoryMapping>();
            if (request.IdGroupCategories != null && request.IdGroupCategories != "")
            {
                var groupCategories = request.IdGroupCategories.Split(',').ToList();
                foreach (string item in groupCategories)
                {
                    productGroupCategories.Add(new ProductGroupCategoryMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        GroupCategoryId = new Guid(item)
                    });
                }
                if (productGroupCategories.Count > 0)
                {
                    _productGroupCategoryMappingRepository.Add(productGroupCategories);
                    await CommitNoCheck(_productGroupCategoryMappingRepository.UnitOfWork);
                }
            }
            // category
            List<ProductCategoryMapping> productCategories = new List<ProductCategoryMapping>();
            if (request.Categories != null)
            {
                int i = 0;
                foreach (ProductCategoriesDto item in request.Categories)
                {
                    productCategories.Add(new ProductCategoryMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        CategoryId = item.CategoryId,
                        GroupCategoryId = item.GroupCategoryId,
                        DisplayOrder = i
                    });
                    i++;
                }
                if (productCategories.Count > 0)
                {
                    _productCategoryMappingRepository.Add(productCategories);
                    await CommitNoCheck(_productCategoryMappingRepository.UnitOfWork);
                }
            }
            //product Tag
            List<ProductProductTagMapping> productProductTags = new List<ProductProductTagMapping>();
            if (request.ProductTag != null && request.ProductTag != "")
            {
                var productTags = request.ProductTag.Split(',').ToList();
                IEnumerable<Guid> productTagList = await _productTagRepository.Filter(productTags, (Guid)request.CreatedBy);
                foreach (Guid item in productTagList)
                {
                    productProductTags.Add(new ProductProductTagMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        ProductTagId = item
                    });
                }
                if (productProductTags.Count > 0)
                {
                    _productProductTagMappingRepository.Add(productProductTags);
                    await CommitNoCheck(_productProductTagMappingRepository.UnitOfWork);
                }
            }

            return result;
        }
        public async Task<ValidationResult> Handle(ProductAddCompactCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productRepository)) return request.ValidationResult;
            var actionBy = _context.GetUserId();
            var actionDate = DateTime.Now;
            var actionName = _context.UserClaims.FullName;
            var product = new Product
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                ProductTypeId = request.ProductTypeId,
                ProductType = request.ProductType,
                CategoryRootId = request.CategoryRootId,
                CategoryRoot = request.CategoryRoot,
                ForBuy = request.ForBuy,
                ForSale = request.ForSale,
                ForProduction = request.ForProduction,
                Condition = request.Condition,
                UnitId = request.UnitId,
                UnitType = request.UnitType,
                UnitCode = request.UnitCode,
                OriginId = request.OriginId,
                Origin = request.Origin,
                Image = request.Image,
                Status = request.Status,
                Price = request.Price,
                Currency = request.Currency,
                ProductCost = request.ProductCost,
                CurrencyCost = request.CurrencyCost,
                CreatedBy = actionBy,
                CreatedDate = actionDate,
                CreatedByName = actionName
            };
            _productRepository.Add(product);
            return await CommitNoCheck(_productRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productRepository)) return request.ValidationResult;
            //add domain event
            //product.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));
            var product = await _productRepository.GetById(request.Id);
            product.Deleted = true;
            product.DeletedDate = DateTime.Now;
            _productRepository.Update(product);
            return await CommitNoCheck(_productRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductEditCommand request, CancellationToken cancellationToken)
        {
            var actionBy = _context.GetUserId();
            var actionDate = DateTime.Now;
            var actionName = _context.UserClaims.FullName;
            if (!request.IsValid(_productRepository)) return request.ValidationResult;
            var dataStore = await _productStoreMappingRepository.Filter(request.Id);
            var dataGroup = await _productGroupCategoryMappingRepository.Filter(request.Id);
            var dataCategory = await _productCategoryMappingRepository.Filter(request.Id);
            var dataProductTag = await _productProductTagMappingRepository.Filter(request.Id);

            _productStoreMappingRepository.Remove(dataStore);
            _productGroupCategoryMappingRepository.Remove(dataGroup);
            _productCategoryMappingRepository.Remove(dataCategory);
            _productProductTagMappingRepository.Remove(dataProductTag);


            var product = await _productRepository.GetById(request.Id);
            product.ProductType = request.ProductType;
            product.ForBuy = request.ForBuy;
            product.ForSale = request.ForSale;
            product.ForProduction = request.ForProduction;
            product.Condition = request.Condition;
            product.UnitId = request.UnitId;
            product.UnitCode = request.UnitCode;
            product.UnitType = request.UnitType;
            product.Name = request.Name;
            product.SourceLink = request.SourceLink;
            product.SourceCode = request.SourceCode;
            product.ShortDescription = request.ShortDescription;
            product.FullDescription = request.FullDescription;
            //product.Origin = data.Origin;
            //product.Brand = data.Brand;
            //product.Manufacturer = data.Manufacturer;
            product.ManufacturerNumber = request.ManufacturerNumber;
            product.Image = request.Image;
            product.Gtin = request.Gtin;
            product.ProductCost = request.ProductCost;
            product.CurrencyCost = request.CurrencyCost;
            product.Price = request.Price;
            product.HasTierPrices = request.HasTierPrices;
            product.Currency = request.Currency;
            product.IsTaxExempt = request.IsTaxExempt;
            product.TaxCategoryId = request.TaxCategoryId;
            product.IsEsd = request.IsEsd;
            product.OrderMinimumQuantity = request.OrderMinimumQuantity;
            product.OrderMaximumQuantity = request.OrderMaximumQuantity;
            product.QuantityStep = request.QuantityStep;
            product.ManageInventoryMethodId = request.ManageInventoryMethodId;
            product.UseMultipleWarehouses = request.UseMultipleWarehouses;
            product.WarehouseId = request.WarehouseId;
            product.Sku = request.Sku;
            product.StockQuantity = request.StockQuantity;
            product.ReservedQuantity = request.ReservedQuantity;
            product.PlannedQuantity = request.PlannedQuantity;
            product.MultiPacking = request.MultiPacking;
            product.Packages = request.Packages;
            product.Weight = request.Weight;
            product.Length = request.Length;
            product.Width = request.Width;
            product.Height = request.Height;
            product.DeliveryTimeId = request.DeliveryTimeId;
            product.IsShipEnabled = request.IsShipEnabled;
            product.IsFreeShipping = request.IsFreeShipping;
            product.AdditionalShippingCharge = request.AdditionalShippingCharge;
            product.CanReturn = request.CanReturn;
            product.CustomsTariffNumber = request.CustomsTariffNumber;
            product.Deleted = request.Deleted;
            product.Status = request.Status;
            //product.CreatedBy = data.CreatedBy;
            //product.CreatedByName = data.CreatedByName;
            product.UpdatedBy = request.UpdatedBy;
            product.UpdatedByName = request.UpdatedByName;
            //product.CreatedDate = data.CreatedDate;
            product.UpdatedDate = request.UpdatedDate;
            product.OriginId = request.OriginId;
            product.BrandId = request.BrandId;
            product.ManufacturerId = request.ManufacturerId;
            product.ProductTypeId = request.ProductTypeId;
            product.CategoryRootId = request.CategoryRootId;
            //product.CategoryRoot = data.CategoryRoot;
            product.LimitedToStores = request.LimitedToStores;

            _productRepository.Update(product);

            //store
            List<ProductStoreMapping> productStores = new List<ProductStoreMapping>();
            if (request.LimitedToStores != null && request.LimitedToStores != "")
            {
                var stores = request.LimitedToStores.Split(',').ToList();
                foreach (string item in stores)
                {
                    productStores.Add(new ProductStoreMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        StoreId = new Guid(item)
                    });
                }
                if (productStores.Count > 0)
                {
                    _productStoreMappingRepository.Add(productStores);
                }
                await CommitNoCheck(_productStoreMappingRepository.UnitOfWork);
            }
            //product category
            List<ProductGroupCategoryMapping> productGroupCategories = new List<ProductGroupCategoryMapping>();
            if (request.IdGroupCategories != null && request.IdGroupCategories != "")
            {
                var groupCategories = request.IdGroupCategories.Split(',').ToList();
                foreach (string item in groupCategories)
                {
                    productGroupCategories.Add(new ProductGroupCategoryMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        GroupCategoryId = new Guid(item)
                    });
                }
                if (productGroupCategories.Count > 0)
                {
                    _productGroupCategoryMappingRepository.Add(productGroupCategories);
                }
                await CommitNoCheck(_productGroupCategoryMappingRepository.UnitOfWork);
            }
            //// category
            List<ProductCategoryMapping> productCategories = new List<ProductCategoryMapping>();
            if (request.Categories != null)
            {
                int i = 0;
                foreach (ProductCategoriesDto item in request.Categories)
                {
                    productCategories.Add(new ProductCategoryMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        CategoryId = item.CategoryId,
                        GroupCategoryId = item.GroupCategoryId,
                        DisplayOrder = i
                    });
                    i++;
                }
                if (productCategories.Count > 0)
                {
                    _productCategoryMappingRepository.Add(productCategories);
                }
                await CommitNoCheck(_productCategoryMappingRepository.UnitOfWork);
            }
            ////product Tag
            List<ProductProductTagMapping> productProductTags = new List<ProductProductTagMapping>();
            if (request.ProductTag != null && request.ProductTag != "")
            {
                var productTags = request.ProductTag.Split(',').ToList();
                IEnumerable<Guid> productTagList = await _productTagRepository.Filter(productTags, (Guid)request.CreatedBy);
                foreach (Guid item in productTagList)
                {
                    productProductTags.Add(new ProductProductTagMapping()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = request.Id,
                        ProductTagId = item
                    });
                }
                if (productProductTags.Count > 0)
                {
                    _productProductTagMappingRepository.Add(productProductTags);
                }
                await CommitNoCheck(_productProductTagMappingRepository.UnitOfWork);
            }
            if (request.ListInventory?.Count > 0)
            {
                List<ProductInventory> addInv = new List<ProductInventory>();
                List<ProductInventory> editInv = new List<ProductInventory>();
                var listInv = await _ProductInventoryRepository.GetByParentId(request.Id);
                var listid = request?.ListInventory.Select(x => x.Id).ToArray();
                var delInv = listInv.Where(x => !listid.Contains(x.Id));
                foreach (var x in request?.ListInventory)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        var y = listInv.Where(a => a.Id == x.Id).FirstOrDefault();
                        editInv.Add(new ProductInventory()
                        {
                            Id = x.Id,
                            WarehouseId = x.WarehouseId,
                            ProductId = x.ProductId,
                            StockQuantity = x.StockQuantity,
                            ReservedQuantity = x.ReservedQuantity,
                            PlannedQuantity = x.PlannedQuantity,
                            CreatedBy = y.CreatedBy,
                            CreatedByName = y.CreatedByName,
                            CreatedDate = y.CreatedDate,
                            UpdatedBy = actionBy,
                            UpdatedByName = actionName,
                            UpdatedDate = actionDate,
                            SpecificationCode1 = x.SpecificationCode1,
                            SpecificationCode2 = x.SpecificationCode2,
                            SpecificationCode3 = x.SpecificationCode3,
                            SpecificationCode4 = x.SpecificationCode4,
                            SpecificationCode5 = x.SpecificationCode5,
                            SpecificationCode6 = x.SpecificationCode6,
                            SpecificationCode7 = x.SpecificationCode7,
                            SpecificationCode8 = x.SpecificationCode8,
                            SpecificationCode9 = x.SpecificationCode9,
                            SpecificationCode10 = x.SpecificationCode10,
                            UnitId = x.UnitId
                        });
                    }
                    else
                    {
                        addInv.Add(new ProductInventory()
                        {
                            Id = Guid.NewGuid(),
                            WarehouseId = x.WarehouseId,
                            ProductId = x.ProductId,
                            StockQuantity = x.StockQuantity,
                            ReservedQuantity = x.ReservedQuantity,
                            PlannedQuantity = x.PlannedQuantity,
                            CreatedBy = actionBy,
                            CreatedByName = actionName,
                            CreatedDate = actionDate,
                            UpdatedBy = null,
                            UpdatedByName = null,
                            UpdatedDate = null,
                            SpecificationCode1 = x.SpecificationCode1,
                            SpecificationCode2 = x.SpecificationCode2,
                            SpecificationCode3 = x.SpecificationCode3,
                            SpecificationCode4 = x.SpecificationCode4,
                            SpecificationCode5 = x.SpecificationCode5,
                            SpecificationCode6 = x.SpecificationCode6,
                            SpecificationCode7 = x.SpecificationCode7,
                            SpecificationCode8 = x.SpecificationCode8,
                            SpecificationCode9 = x.SpecificationCode9,
                            SpecificationCode10 = x.SpecificationCode10,
                            UnitId = x.UnitId
                        });
                    }
                }
                _ProductInventoryRepository.Add(addInv);
                _ProductInventoryRepository.Update(editInv);
                _ProductInventoryRepository.Remove(delInv);
                await CommitNoCheck(_ProductInventoryRepository.UnitOfWork);
            }
            else
            {
                var listInv = await _ProductInventoryRepository.GetByParentId(request.Id);
                _ProductInventoryRepository.Remove(listInv);
                await CommitNoCheck(_ProductInventoryRepository.UnitOfWork);
            }

            if (request.ListPackage?.Count > 0)
            {
                List<ProductPackage> addPack = new List<ProductPackage>();
                List<ProductPackage> editPack = new List<ProductPackage>();
                var listPack = await _ProductPackageRepository.GetByParentId(request.Id);
                var listidPack = request?.ListPackage.Select(x => x.Id).ToArray();
                var delPack = listPack.Where(x => !listidPack.Contains(x.Id));
                foreach (var x in request?.ListPackage)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        var y = listPack.Where(a => a.Id == x.Id).FirstOrDefault();
                        editPack.Add(new ProductPackage()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProductId = x.ProductId,
                            Weight = x.Weight,
                            Length = x.Length,
                            Width = x.Width,
                            Height = x.Height,
                            CreatedBy = y.CreatedBy,
                            CreatedByName = y.CreatedByName,
                            CreatedDate = y.CreatedDate,
                            UpdatedBy = actionBy,
                            UpdatedByName = actionName,
                            UpdatedDate = actionDate
                        });
                    }
                    else
                    {
                        addPack.Add(new ProductPackage()
                        {
                            Id = Guid.NewGuid(),
                            Name = x.Name,
                            ProductId = x.ProductId,
                            Weight = x.Weight,
                            Length = x.Length,
                            Width = x.Width,
                            Height = x.Height,
                            CreatedBy = actionBy,
                            CreatedByName = actionName,
                            CreatedDate = actionDate,
                            UpdatedBy = null,
                            UpdatedByName = null,
                            UpdatedDate = null
                        });
                    }
                }
                _ProductPackageRepository.Add(addPack);
                _ProductPackageRepository.Update(editPack);
                _ProductPackageRepository.Remove(delPack);
                await CommitNoCheck(_ProductPackageRepository.UnitOfWork);
            }
            else
            {
                var listPack = await _ProductPackageRepository.GetByParentId(request.Id);
                _ProductPackageRepository.Remove(listPack);
                await CommitNoCheck(_ProductPackageRepository.UnitOfWork);
            }
            ///////////////// tiecnq
            if (request.ListProductSpecificationCode?.Count > 0)
            {
                var addSpecCode = new List<ProductSpecificationCode>();
                var editSpecCode = new List<ProductSpecificationCode>();
                var listSpecCode = await _productSpecificationCodeRepository.GetByParentId(request.Id);
                var listidSpecCode = request?.ListProductSpecificationCode.Select(x => x.Id).ToArray();
                var delSpecCode = listSpecCode.Where(x => !listidSpecCode.Contains(x.Id));
                foreach (var x in request?.ListProductSpecificationCode)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        editSpecCode.Add(new ProductSpecificationCode()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProductId = request.Id,
                            DuplicateAllowed = x.DuplicateAllowed,
                            DataTypes = x.DataTypes,
                            DisplayOrder = x.DisplayOrder,
                            Status = x.Status
                        });
                    }
                    else
                    {
                        addSpecCode.Add(new ProductSpecificationCode()
                        {
                            Id = Guid.NewGuid(),
                            Name = x.Name,
                            ProductId = request.Id,
                            DuplicateAllowed = x.DuplicateAllowed,
                            DataTypes = x.DataTypes,
                            DisplayOrder = x.DisplayOrder,
                            Status = x.Status
                        });
                    }
                }
                _productSpecificationCodeRepository.Add(addSpecCode);
                _productSpecificationCodeRepository.Update(editSpecCode);
                _productSpecificationCodeRepository.Remove(delSpecCode);
                await CommitNoCheck(_productSpecificationCodeRepository.UnitOfWork);
            }
            else
            {
                //  var listPack = await _productSpecificationCodeRepository.GetByParentId(request.Id);
                //  _productSpecificationCodeRepository.Remove(listPack);
            }

            //////////////////////TuanAnh
            if (request.ListProductSpecificationAttributeMapping?.Count > 0)
            {
                var addSpecAttri = new List<ProductSpecificationAttributeMapping>();
                var editSpecAttri = new List<ProductSpecificationAttributeMapping>();
                var listSpecAttri = await _productSpecificationAttributeMappingRepository.GetByParentId(request.Id);
                var listidSpecAttri = request?.ListProductSpecificationAttributeMapping.Select(x => x.Id).ToArray();
                var delSpecAttri = listSpecAttri.Where(x => !listidSpecAttri.Contains(x.Id));
                foreach (var x in request?.ListProductSpecificationAttributeMapping)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        editSpecAttri.Add(new ProductSpecificationAttributeMapping()
                        {
                            Id = (Guid)x.Id,
                            ProductId = request.Id,
                            SpecificationAttributeId = x.SpecificationAttributeId,
                            SpecificationAttributeOptionId = (Guid)x.SpecificationAttributeOptionId,
                            DisplayOrder = x.DisplayOrder,
                        });
                    }
                    else
                    {
                        addSpecAttri.Add(new ProductSpecificationAttributeMapping()
                        {
                            Id = Guid.NewGuid(),
                            ProductId = request.Id,
                            SpecificationAttributeId = x.SpecificationAttributeId,
                            SpecificationAttributeOptionId = (Guid)x.SpecificationAttributeOptionId,
                            DisplayOrder = x.DisplayOrder,
                        });
                    }
                }
                _productSpecificationAttributeMappingRepository.Add(addSpecAttri);
                _productSpecificationAttributeMappingRepository.Update(editSpecAttri);
                _productSpecificationAttributeMappingRepository.Remove(delSpecAttri);
                await CommitNoCheck(_productSpecificationAttributeMappingRepository.UnitOfWork);
            }

            //////////////////////TuanAnh
            if (request.ListTierPrice?.Count > 0)
            {
                var addTierPrice = new List<TierPrice>();
                var editTierPrice = new List<TierPrice>();
                var listTierPrice = await _tierPriceRepository.GetByParentId(request.Id);
                var listidTierPrice = request?.ListTierPrice.Select(x => x.Id).ToArray();
                var delTierPrice = listTierPrice.Where(x => !listidTierPrice.Contains(x.Id));
                foreach (var x in request?.ListTierPrice)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        editTierPrice.Add(new TierPrice()
                        {
                            Id = (Guid)x.Id,
                            ProductId = request.Id,
                            StoreId = x.StoreId,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            CalculationMethod = x.CalculationMethod,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            CreatedBy = x.CreatedBy,
                            CreatedByName = x.CreatedByName,
                            CreatedDate = x.CreatedDate,
                            UpdatedBy = _context.GetUserId(),
                            UpdatedByName = _context.FullName,
                            UpdatedDate = DateTime.Now,
                        });
                    }
                    else
                    {
                        addTierPrice.Add(new TierPrice()
                        {
                            Id = Guid.NewGuid(),
                            ProductId = request.Id,
                            StoreId = x.StoreId,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            CalculationMethod = x.CalculationMethod,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            CreatedBy = _context.GetUserId(),
                            CreatedDate = DateTime.Now,
                            CreatedByName = _context.FullName
                        });
                    }
                }
                _tierPriceRepository.Add(addTierPrice);
                _tierPriceRepository.Update(editTierPrice);
                _tierPriceRepository.Remove(delTierPrice);
                await CommitNoCheck(_tierPriceRepository.UnitOfWork);
            }

            //////////////////////TuanAnh
            if (request.ListProductServiceAdd?.Count > 0)
            {
                var addProductServiceAdd = new List<ProductServiceAdd>();
                var editProductServiceAdd = new List<ProductServiceAdd>();
                var listProductServiceAdd = await _productServiceAddRepository.GetByParentId(request.Id);
                var listidProductServiceAdd = request?.ListProductServiceAdd.Select(x => x.Id).ToArray();
                var delProductServiceAdd = listProductServiceAdd.Where(x => !listidProductServiceAdd.Contains(x.Id));
                foreach (var x in request?.ListProductServiceAdd)
                {
                    if (x.Id != null && x.Id != new Guid())
                    {
                        editProductServiceAdd.Add(new ProductServiceAdd()
                        {
                            Id = (Guid)x.Id,
                            ProductId = request.Id,
                            ServiceAddId = x.ServiceAddId,
                            PayRequired = x.PayRequired,
                            MaxPrice = x.MaxPrice,
                            Price = x.Price,
                            CalculationMethod = x.CalculationMethod,
                            PriceSyntax = x.PriceSyntax,
                            MinPrice = x.MinPrice,
                            Currency = x.Currency,
                            Status = x.Status,
                            CreatedBy = x.CreatedBy,
                            CreatedByName = x.CreatedByName,
                            CreatedDate = x.CreatedDate,
                            UpdatedBy = _context.GetUserId(),
                            UpdatedByName = _context.FullName,
                            UpdatedDate = DateTime.Now,
                        });
                    }
                    else
                    {
                        addProductServiceAdd.Add(new ProductServiceAdd()
                        {
                            Id = Guid.NewGuid(),
                            ProductId = request.Id,
                            ServiceAddId = x.ServiceAddId,
                            PayRequired = x.PayRequired,
                            MaxPrice = x.MaxPrice,
                            Price = x.Price,
                            CalculationMethod = x.CalculationMethod,
                            PriceSyntax = x.PriceSyntax,
                            MinPrice = x.MinPrice,
                            Currency = x.Currency,
                            Status = x.Status,
                            CreatedBy = _context.GetUserId(),
                            CreatedDate = DateTime.Now,
                            CreatedByName = _context.FullName
                        });
                    }
                }
                _productServiceAddRepository.Add(addProductServiceAdd);
                _productServiceAddRepository.Update(editProductServiceAdd);
                _productServiceAddRepository.Remove(delProductServiceAdd);
                await CommitNoCheck(_tierPriceRepository.UnitOfWork);
            }
            return await CommitNoCheck(_productRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductSizeEditCommand request, CancellationToken cancellationToken)
        {
            var actionBy = _context.GetUserId();
            var actionDate = DateTime.Now;
            var actionName = _context.UserClaims.FullName;
            if (!request.IsValid(_productRepository)) return request.ValidationResult;
            var data = await _productRepository.GetById(request.Id);
            var product = new Product
            {
                Id = request.Id,
                Code = data.Code,
                ProductType = data.ProductType,
                ForBuy = data.ForBuy,
                ForSale = data.ForSale,
                ForProduction = data.ForProduction,
                Condition = data.Condition,
                UnitId = data.UnitId,
                UnitCode = data.UnitCode,
                UnitType = data.UnitType,
                Name = data.Name,
                SourceLink = data.SourceLink,
                SourceCode = data.SourceCode,
                ShortDescription = data.ShortDescription,
                FullDescription = data.FullDescription,
                Origin = data.Origin,
                Brand = data.Brand,
                Manufacturer = data.Manufacturer,
                ManufacturerNumber = data.ManufacturerNumber,
                Image = data.Image,
                Gtin = data.Gtin,
                ProductCost = data.ProductCost,
                CurrencyCost = data.CurrencyCost,
                Price = data.Price,
                HasTierPrices = data.HasTierPrices,
                Currency = data.Currency,
                IsTaxExempt = data.IsTaxExempt,
                TaxCategoryId = data.TaxCategoryId,
                IsEsd = data.IsEsd,
                OrderMinimumQuantity = data.OrderMinimumQuantity,
                OrderMaximumQuantity = data.OrderMaximumQuantity,
                QuantityStep = data.QuantityStep,
                ManageInventoryMethodId = data.ManageInventoryMethodId,
                UseMultipleWarehouses = data.UseMultipleWarehouses,
                WarehouseId = data.WarehouseId,
                Sku = data.Sku,
                StockQuantity = data.StockQuantity,
                ReservedQuantity = data.ReservedQuantity,
                PlannedQuantity = data.PlannedQuantity,
                MultiPacking = data.MultiPacking,
                Packages = data.Packages,
                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                DeliveryTimeId = data.DeliveryTimeId,
                IsShipEnabled = data.IsShipEnabled,
                IsFreeShipping = data.IsFreeShipping,
                AdditionalShippingCharge = data.AdditionalShippingCharge,
                CanReturn = data.CanReturn,
                CustomsTariffNumber = data.CustomsTariffNumber,
                Deleted = data.Deleted,
                Status = data.Status,
                CreatedBy = data.CreatedBy,
                CreatedByName = data.CreatedByName,
                UpdatedBy = actionBy,
                UpdatedByName = actionName,
                CreatedDate = data.CreatedDate,
                UpdatedDate = actionDate,
                OriginId = data.OriginId,
                BrandId = data.BrandId,
                ManufacturerId = data.ManufacturerId,
                ProductTypeId = data.ProductTypeId,
                CategoryRootId = data.CategoryRootId,
                CategoryRoot = data.CategoryRoot
            };
            _productRepository.Update(product);
            return await Commit(_productRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductDuplicateCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productRepository)) return request.ValidationResult;

            int rs = await _pimContextProcedures.SP_COPY_PRODUCTAsync(request.Id, request.Code, request.Name, request.Status, request.CreatedBy, request.CreatedByName);
            if (rs == 0)
            {
                return new ValidationResult(new List<ValidationFailure>()
                { new ValidationFailure("Duplicate", "Duplicate unsuccessful") });
            }
            else
            {
                return new ValidationResult();

            }
        }

        public async Task<ValidationResult> Handle(ProductCrossCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productRepository)) return request.ValidationResult;
            var rs = await _productRepository.AddProductSimple(new ProductSimple()
            {
                Id = request.Id,
                Code = request.Code,
                ProductType = request.ProductType,
                Condition = request.Condition,
                UnitType = request.UnitType,
                UnitCode = request.UnitCode,
                Name = request.Name,
                SourceLink = request.SourceLink,
                SourceCode = request.SourceCode,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                LimitedToStores = request.LimitedToStores,
                Channel = request.Channel,
                Channel_Category = request.Channel_Category,
                Origin = request.Origin,
                Brand = request.Brand,
                Manufacturer = request.Manufacturer,
                ManufacturerNumber = request.ManufacturerNumber,
                Image = request.Image,
                Images = request.Images,
                Gtin = request.Gtin,
                ProductCost = request.ProductCost,
                CurrencyCost = request.CurrencyCost,
                Price = request.Price,
                Currency = request.Currency,
                IsTaxExempt = request.IsTaxExempt,
                Tax = request.Tax,
                OrderMinimumQuantity = request.OrderMinimumQuantity,
                OrderMaximumQuantity = request.OrderMaximumQuantity,
                IsShipEnabled = request.IsShipEnabled,
                IsFreeShipping = request.IsFreeShipping,
                ProductTag = request.ProductTag
            });
            if (rs == 0)
            {
                return new ValidationResult(new List<ValidationFailure>()
                { new ValidationFailure("", "Create product cross failed") });
            }
            else
            {
                return new ValidationResult();

            }
        }
    }
}