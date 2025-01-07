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
    public partial class ProductReviewHelpfulnessRepository : IProductReviewHelpfulnessRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductReviewHelpfulness> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductReviewHelpfulnessRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductReviewHelpfulness>();
        }

        public void Add(ProductReviewHelpfulness productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductReviewHelpfulness>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
          
            foreach (var item in filter)
            {
                if (item.Key.Equals("productReviewId"))
                {
                    query = query.Where(x => x.ProductReviewId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productReviewId"))
                {
                    query = query.Where(x => x.ProductReviewId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductReviewHelpfulness>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductReviewHelpfulness> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductReviewHelpfulness productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(ProductReviewHelpfulness productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<ProductReviewHelpfulness>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productReviewId"))
                {
                    query = query.Where(x => x.ProductReviewId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
