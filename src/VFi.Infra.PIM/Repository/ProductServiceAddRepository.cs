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
    public partial class ProductServiceAddRepository : IProductServiceAddRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductServiceAdd> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductServiceAddRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductServiceAdd>();
        }

        public void Add(ProductServiceAdd productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
      
        public void Dispose()
        {
            Db.Dispose();
        }
      
        public async Task<IEnumerable<ProductServiceAdd>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
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

        public async Task<IEnumerable<ProductServiceAdd>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductServiceAdd> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductServiceAdd productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductServiceAdd productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductServiceAdd>> GetListListBox(Dictionary<string, object> filter)
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
        public async Task<IEnumerable<ProductServiceAdd>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Add(IEnumerable<ProductServiceAdd> t)
        {
            DbSet.AddRange(t);
        }
        public void Update(IEnumerable<ProductServiceAdd> t)
        {
            DbSet.UpdateRange(t);
        }
        public void Remove(IEnumerable<ProductServiceAdd> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<ProductServiceAdd>> Filter(Dictionary<string, object> filter)
        {
            var query = DbSet.Select(x => x);

            foreach (var item in filter)
            {
                if (item.Key.Equals("serviceAddId"))
                {
                    query = query.Where(x => x.ServiceAddId == (Guid)item.Value);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProductServiceAdd>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductId == id).ToListAsync();
        }
    }
}
