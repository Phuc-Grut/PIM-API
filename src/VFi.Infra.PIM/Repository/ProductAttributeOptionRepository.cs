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
    public partial class ProductAttributeOptionRepository : IProductAttributeOptionRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductAttributeOption> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductAttributeOptionRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductAttributeOption>();
        }

        public void Add(ProductAttributeOption productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductAttributeOption>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) ||  x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productAttributeId"))
                {
                    query = query.Where(x => x.ProductAttributeId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("linkedProductId"))
                {
                    query = query.Where(x => x.LinkedProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productAttributeId"))
                {
                    query = query.Where(x => x.ProductAttributeId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("linkedProductId"))
                {
                    query = query.Where(x => x.LinkedProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductAttributeOption>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductAttributeOption> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductAttributeOption productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductAttributeOption productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductAttributeOption>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Alias.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("productAttributeId"))
                {
                    query = query.Where(x => x.ProductAttributeId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("linkedProductId"))
                {
                    query = query.Where(x => x.LinkedProductId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ProductAttributeOption>> Filter(Guid id)
        {
            return await DbSet.Where(x => x.ProductAttributeId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<ProductAttributeOption> options)
        {
            DbSet.UpdateRange(options);
        }
        public void Add(IEnumerable<ProductAttributeOption> options)
        {
            DbSet.AddRange(options);
        }
        public void Remove(IEnumerable<ProductAttributeOption> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<ProductAttributeOption>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductAttributeId == id).ToListAsync();
        }
    }
}
