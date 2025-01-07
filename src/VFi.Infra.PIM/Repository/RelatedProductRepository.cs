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
    public partial class RelatedProductRepository : IRelatedProductRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<RelatedProduct> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public RelatedProductRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<RelatedProduct>();
        }

        public void Add(RelatedProduct productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }
        public void Add(IEnumerable<RelatedProduct> productGroupCategorys)
        {
             DbSet.AddRange(productGroupCategorys);
        }
        public void Dispose()
        {
            Db.Dispose();
        }
        public async Task<IEnumerable<RelatedProduct>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId1"))
                {
                    query = query.Where(x => x.ProductId1.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("productId2"))
                {
                    query = query.Where(x => x.ProductId2.Equals(new Guid(item.Value + "")));
                }  
               
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId1"))
                {
                    query = query.Where(x => x.ProductId1.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("productId2"))
                {
                    query = query.Where(x => x.ProductId2.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<RelatedProduct>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<RelatedProduct> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(RelatedProduct productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(RelatedProduct productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<RelatedProduct>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId1"))
                {
                    query = query.Where(x => x.ProductId1.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("productId2"))
                {
                    query = query.Where(x => x.ProductId2.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<RelatedProduct>> Filter(Guid Id)
        {
            return await DbSet.Where(x => x.ProductId1 == Id).ToListAsync();
        }

        public void Remove(IEnumerable<RelatedProduct> t)
        {
            DbSet.RemoveRange(t);
        }
    }
}
