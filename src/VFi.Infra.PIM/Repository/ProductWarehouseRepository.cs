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
    public partial class ProductWarehouseRepository : IProductWarehouseRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductWarehouse> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductWarehouseRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductWarehouse>();
        }

        public void Add(ProductWarehouse productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
        public void Add(IEnumerable<ProductWarehouse> productCategorys)
        {
            DbSet.AddRange(productCategorys);
        }
        public void Dispose()
        {
            Db.Dispose();
        }
      
        public async Task<IEnumerable<ProductWarehouse>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
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
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductWarehouse>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductWarehouse> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductWarehouse productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductWarehouse productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductWarehouse>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<ProductWarehouse>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductWarehouse> t)
        {
            DbSet.RemoveRange(t);
        }
    }
}
