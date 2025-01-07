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
    public partial class ProductGroupCategoryMappingRepository : IProductGroupCategoryMappingRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductGroupCategoryMapping> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductGroupCategoryMappingRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductGroupCategoryMapping>();
        }

        public void Add(ProductGroupCategoryMapping productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
        public void Add(IEnumerable<ProductGroupCategoryMapping> productGroupCategorys)
        {
             DbSet.AddRange(productGroupCategorys);
        }
        public void Dispose()
        {
            Db.Dispose();
        }
        public async Task<IEnumerable<ProductGroupCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("groupCategoryId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
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
                if (item.Key.Equals("groupCategoryId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductGroupCategoryMapping>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductGroupCategoryMapping> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductGroupCategoryMapping productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductGroupCategoryMapping productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductGroupCategoryMapping>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("groupCategoryId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ProductGroupCategoryMapping>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductGroupCategoryMapping> t)
        {
            DbSet.RemoveRange(t);
        }
    }
}
