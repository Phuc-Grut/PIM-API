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
    public partial class ProductPackageRepository : IProductPackageRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductPackage> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductPackageRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductPackage>();
        }

        public void Add(ProductPackage productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductPackage>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
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

        public async Task<int> FilterCount(Dictionary<string, object> filter)
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
        public async Task<IEnumerable<ProductPackage>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x=>x.ProductId == id).ToListAsync();
        }
        public async Task<IEnumerable<ProductPackage>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductPackage> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductPackage productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductPackage productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }
        public void Add(IEnumerable<ProductPackage> t)
        {
            DbSet.AddRange(t);
        }
        public void Remove(IEnumerable<ProductPackage> t)
        {
            DbSet.RemoveRange(t);
        }
        public void Update(IEnumerable<ProductPackage> t)
        {
            DbSet.UpdateRange(t);
        }
        public async Task<IEnumerable<ProductPackage>> GetListListBox(Dictionary<string, object> filter)
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
        public async Task<List<ProductPackage>> GetByProducts(string ids)
        {
            var query = await DbSet.Where(x => ids.Contains(x.ProductId.ToString())).ToListAsync();
            return query;
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
