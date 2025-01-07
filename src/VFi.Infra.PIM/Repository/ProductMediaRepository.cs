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
    public partial class ProductMediaRepository : IProductMediaRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductMedia> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductMediaRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductMedia>();
        }

        public void Add(ProductMedia productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductMedia>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) ||  x.MediaType.Contains(keyword) || x.Path.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.MediaType.Contains(keyword) || x.Path.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductMedia>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductMedia> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductMedia productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductMedia productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductMedia>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.MediaType.Contains(keyword) || x.Path.Contains(keyword));
            }
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
        public void Add(IEnumerable<ProductMedia> t)
        {
            DbSet.AddRange(t);
        }
    }
}
