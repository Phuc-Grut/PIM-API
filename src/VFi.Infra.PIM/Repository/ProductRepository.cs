using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Infra.PIM.Repository
{
    public partial class ProductRepository : IProductRepository
    {
        protected readonly IPIMContextProcedures _store;
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Product> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductRepository(IServiceProvider serviceProvider, IPIMContextProcedures store)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<Product>();
            _store = store;
        }

        public void Add(Product product)
        {
            DbSet.Add(product);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        public async Task<Product> GetByCode(string code)
        {
            return await DbSet.Include(x => x.Unit).FirstOrDefaultAsync(x => x.Code.Equals(code));

        }
        public async Task<string> GetByCategoryRootId(Guid id)
        {
            var list = await DbSet.Where(x => x.CategoryRootId == id).Select(x => new { x.Id, x.Name }).ToListAsync();
            return string.Concat(string.Join(",", list.Select(x => x.Id)), ";", string.Join(",", list.Select(x => x.Name)));
        }
        public async Task<Product> GetById(Guid id)
        {
            //return await DbSet.Include(e => e.ProductTags).FirstOrDefaultAsync(e => e.Id == id);
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Remove(Product product)
        {
            DbSet.Remove(product);
        }

        public void Update(Product product)
        {
            DbSet.Update(product);
        }
        public async Task<(IEnumerable<Product>, int)> Filter(string? keyword, IFopRequest request)
        {

            var query = DbSet
                .Include(x => x.ProductInventory)
                .Include(x => x.ProductSpecificationCode)
                .Include(x => x.Unit)
                .Include(x => x.OriginNavigation)
                .AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.KeywordStandardized();
                query = query.Where(x => EF.Functions.Contains(x.Name, $"{keyword}")
                || (!string.IsNullOrEmpty(x.SourceLink) && x.SourceLink.Contains(keyword))
                || (!string.IsNullOrEmpty(x.SourceCode) && x.SourceCode.Contains(keyword))
                || (!string.IsNullOrEmpty(x.Code) && x.Code.Contains(keyword))
                || (!string.IsNullOrEmpty(x.Gtin) && x.Gtin.Contains(keyword)));
            }
            var (filtered, totalCount) = query.ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
        public async Task<IEnumerable<Product>> Filter(IEnumerable<RelatedProduct> relatedProducts)
        {
            var query = DbSet.AsQueryable();
            return await query.Where(x => relatedProducts.ToList().Select(x => x.ProductId2).Contains(x.Id)).ToListAsync();
        }
        public async Task<IEnumerable<Product>> Filter(List<string> products)
        {
            var query = DbSet.Include(x => x.ProductInventory).Include(x => x.ProductSpecificationCode).Include(x => x.Unit).AsQueryable();
            return await query.Where(x => products.Contains(x.Code)).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productType") && !string.IsNullOrEmpty(item.Value + ""))
                {
                    query = query.Where(x => x.UnitId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("unit") && !string.IsNullOrEmpty(item.Value + ""))
                {
                    query = query.Where(x => x.UnitId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("taxCategory") && !string.IsNullOrEmpty(item.Value + ""))
                {
                    query = query.Where(x => x.TaxCategoryId.Equals(new Guid(item.Value + "")
                        ));
                }
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("productCode"))
                {
                    query = query.Where(x => ((List<string>)item.Value).Contains(x.Code));
                }
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => ((List<Guid>)item.Value).Contains(x.Id));
                }
            }
            return await query.Where(x => x.Deleted == false).ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<Product> CheckExistAttrJson(string? attrJson, Guid parentId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.ParentId == parentId && attrJson == x.AttributesJson);
        }

        public async Task<int> AddProductSimple(ProductSimple item)
        {
            var result = await _store.SP_ADD_SIMPLE_PRODUCTAsync(item.Id, item.Code, item.ProductType, item.Condition, item.UnitType, item.UnitCode, item.Name, item.SourceLink, item.ShortDescription, item.FullDescription, item.LimitedToStores,
                item.Channel, item.Channel_Category, item.Origin, item.Brand, item.Manufacturer, item.ManufacturerNumber, item.Image, item.Images, item.Gtin, item.ProductCost, item.CurrencyCost, item.Price, item.Currency, item.IsTaxExempt, item.Tax,
                item.OrderMinimumQuantity, item.OrderMaximumQuantity, item.IsShipEnabled, item.IsFreeShipping, item.ProductTag);
            return result;
        }

        public async Task<IEnumerable<Product>> Filter(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();

            foreach (var item in filter)
            {
                if (item.Key.Equals("productTypeId"))
                {
                    query = query.Where(x => x.ProductTypeId == (Guid)item.Value);
                }
                if (item.Key.Equals("categoryRootId"))
                {
                    query = query.Where(x => x.CategoryRootId == (Guid)item.Value);
                }
                if (item.Key.Equals("taxCategoryId"))
                {
                    query = query.Where(x => x.TaxCategoryId == (Guid)item.Value);
                }
                if (item.Key.Equals("brandId"))
                {
                    query = query.Where(x => x.BrandId == (Guid)item.Value);
                }
                if (item.Key.Equals("originId"))
                {
                    query = query.Where(x => x.OriginId == (Guid)item.Value);
                }
                if (item.Key.Equals("manufacturerId"))
                {
                    query = query.Where(x => x.ManufacturerId == (Guid)item.Value);
                }
                if (item.Key.Equals("unitId"))
                {
                    query = query.Where(x => x.UnitId == (Guid)item.Value);
                }
                if (item.Key.Equals("deliveryTimeId"))
                {
                    query = query.Where(x => x.DeliveryTimeId == (Guid)item.Value);
                }
            }
            return await query.ToListAsync();
        }
    }


}
