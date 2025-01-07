using Consul.Filtering;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class ProductSpecificationAttributeMappingRepository : IProductSpecificationAttributeMappingRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductSpecificationAttributeMapping> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductSpecificationAttributeMappingRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductSpecificationAttributeMapping>();
        }

        public void Add(ProductSpecificationAttributeMapping productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("specificationAttributeOptionId"))
                {
                    query = query.Where(x => x.SpecificationAttributeOptionId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<int> FilterCount( Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("specificationAttributeOptionId"))
                {
                    query = query.Where(x => x.SpecificationAttributeOptionId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductSpecificationAttributeMapping> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductSpecificationAttributeMapping productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductSpecificationAttributeMapping productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("specificationAttributeOptionId"))
                {
                    query = query.Where(x => x.SpecificationAttributeOptionId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public void Add(IEnumerable<ProductSpecificationAttributeMapping> options)
        {
            DbSet.AddRange(options);
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductSpecificationAttributeMapping> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();

            foreach (var item in filter)
            {
                if (item.Key.Equals("specificationAttributeId"))
                {
                    query = query.Where(x => x.SpecificationAttributeId == (Guid)item.Value);
                }
            }
            return await query.ToListAsync();
        }

        public void Update(IEnumerable<ProductSpecificationAttributeMapping> t)
        {
            DbSet.UpdateRange(t);
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMapping>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductId == id).ToListAsync();
        }
    }
}
