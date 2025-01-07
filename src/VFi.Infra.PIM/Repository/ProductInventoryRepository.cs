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
    public partial class ProductInventoryRepository : IProductInventoryRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductInventory> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductInventoryRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductInventory>();
        }

        public void Add(ProductInventory productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductInventory>> GetByParentId(Guid parentid)
        {
            return await DbSet.Where(x => x.ProductId == parentid).ToListAsync();
        }

        public async Task<IEnumerable<ProductInventory>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("warehouseId"))
                {
                    query = query.Where(x => x.WarehouseId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("warehouseId"))
                {
                    query = query.Where(x => x.WarehouseId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductInventory>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductInventory> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductInventory productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductInventory productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductInventory>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("warehouseId"))
                {
                    query = query.Where(x => x.WarehouseId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Include(x => x.Warehouse).Include(x=>x.Unit).ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<ProductInventory>> Filter(IEnumerable<Product> products)

        {
            var list = products.Select(x => x.Id).ToList();
            return await DbSet.Where(x => x.ProductId != null ? list.Contains((Guid)x.ProductId) : false).ToListAsync();
        }

        public async Task<IEnumerable<ProductInventory>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }
        public void Add(IEnumerable<ProductInventory> t)
        {
            DbSet.AddRange(t);
        }
        public void Remove(IEnumerable<ProductInventory> t)
        {
            DbSet.RemoveRange(t);
        }
        public void Update(IEnumerable<ProductInventory> t)
        {
            DbSet.UpdateRange(t);
        }
    }
}
