using Consul.Filtering;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class ProductProductTagMappingRepository : IProductProductTagMappingRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductProductTagMapping> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductProductTagMappingRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductProductTagMapping>();
        }

        public void Add(ProductProductTagMapping productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
        public void Add(IEnumerable<ProductProductTagMapping> productStores)
        {
            DbSet.AddRange(productStores);
        }
        public void Dispose()
        {
            Db.Dispose();
        }
        public async Task<IEnumerable<ProductProductTagMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("productTagId"))
                {
                    query = query.Where(x => x.ProductTagId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
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
                if (item.Key.Equals("productTagId"))
                {
                    query = query.Where(x => x.ProductTagId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductProductTagMapping>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductProductTagMapping> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductProductTagMapping productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductProductTagMapping productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductProductTagMapping>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("productTagId"))
                {
                    query = query.Where(x => x.ProductTagId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<ProductProductTagMapping>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductProductTagMapping> t)
        {
            DbSet.RemoveRange(t);
        }
    }
}
