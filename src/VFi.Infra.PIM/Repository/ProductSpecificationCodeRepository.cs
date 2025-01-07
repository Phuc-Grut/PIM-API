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
    public partial class ProductSpecificationCodeRepository : IProductSpecificationCodeRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductSpecificationCode> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductSpecificationCodeRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductSpecificationCode>();
        }

        public void Add(ProductSpecificationCode productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductSpecificationCode>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
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
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductSpecificationCode>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductSpecificationCode> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductSpecificationCode productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductSpecificationCode productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductSpecificationCode>> GetListListBox(Dictionary<string, object> filter)
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
        public void Add(IEnumerable<ProductSpecificationCode> options)
        {
            DbSet.AddRange(options);
        }
        public void Update(IEnumerable<ProductSpecificationCode> options)
        {
            DbSet.UpdateRange(options);
        }

        public async Task<IEnumerable<ProductSpecificationCode>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId == Id).ToListAsync();
        }

        public void Remove(IEnumerable<ProductSpecificationCode> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<ProductSpecificationCode>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductId == id).ToListAsync();
        }
    }
}
