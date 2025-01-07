using Consul.Filtering;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class ProductVariantAttributeValueRepository : IProductVariantAttributeValueRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductVariantAttributeValue> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductVariantAttributeValueRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductVariantAttributeValue>();
        }

        public void Add(ProductVariantAttributeValue specificationAttributeOption)
        {
            DbSet.Add(specificationAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductVariantAttributeValue>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productVariantAttributeId"))
                {
                    query = query.Where(x => x.ProductVariantAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) ||  x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productVariantAttributeId"))
                {
                    query = query.Where(x => x.ProductVariantAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductVariantAttributeValue>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductVariantAttributeValue> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductVariantAttributeValue specificationAttributeOption)
        {
            DbSet.Remove(specificationAttributeOption);
        }

        public void Update(ProductVariantAttributeValue specificationAttributeOption)
        {
            DbSet.Update(specificationAttributeOption);
        }
        public void Add(IEnumerable<ProductVariantAttributeValue> t)
        {
            DbSet.AddRange(t);
        }
        public void Remove(IEnumerable<ProductVariantAttributeValue> t)
        {
            DbSet.RemoveRange(t);
        }
        public void Update(IEnumerable<ProductVariantAttributeValue> t)
        {
            DbSet.UpdateRange(t);
        }
        public async Task<IEnumerable<ProductVariantAttributeValue>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productVariantAttributeId"))
                {
                    query = query.Where(x => x.ProductVariantAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<ProductVariantAttributeValue>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductVariantAttributeId == id).OrderByDescending(x=>x.DisplayOrder).ToListAsync();
        }
        public async Task<IEnumerable<ProductVariantAttributeValue>> GetByParentId(IEnumerable<ProductProductAttributeMapping> list)
        {
            List<Guid> ids = new List<Guid>();
            ids = list.Select(x => x.Id).ToList();
            return await DbSet.Where(x => ids.Contains((Guid)x.ProductVariantAttributeId)).ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
