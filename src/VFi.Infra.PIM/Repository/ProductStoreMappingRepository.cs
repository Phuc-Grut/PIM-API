using Consul.Filtering;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Infra.PIM.Repository
{
    public partial class ProductStoreMappingRepository : IProductStoreMappingRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductStoreMapping> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductStoreMappingRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductStoreMapping>();
        }

        public void Add(ProductStoreMapping productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
        public void Add(IEnumerable<ProductStoreMapping> productStores)
        {
            DbSet.AddRange(productStores);
        }
        public void Dispose()
        {
            Db.Dispose();
        }
        public async Task<IEnumerable<ProductStoreMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
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
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductStoreMapping>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductStoreMapping> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductStoreMapping productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductStoreMapping productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductStoreMapping>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ProductStoreMapping>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductStoreMapping> t)
        {
             DbSet.RemoveRange(t);
        }
    }
}
