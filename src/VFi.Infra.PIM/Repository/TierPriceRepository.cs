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
    public partial class TierPriceRepository : ITierPriceRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<TierPrice> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public TierPriceRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<TierPrice>();
        }

        public void Add(TierPrice productAttributeOption)
        {
            DbSet.Add(productAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<TierPrice>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
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
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<TierPrice>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TierPrice> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(TierPrice productAttributeOption)
        {
            DbSet.Remove(productAttributeOption);
        }

        public void Update(TierPrice productAttributeOption)
        {
            DbSet.Update(productAttributeOption);
        }

        public async Task<IEnumerable<TierPrice>> GetListListBox(Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            foreach (var item in filter)
            {
                if (item.Key.Equals("productId"))
                {
                    query = query.Where(x => x.ProductId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("storeId"))
                {
                    query = query.Where(x => x.StoreId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public void Add(IEnumerable<TierPrice> t)
        {
            DbSet.AddRange(t);
        }
        public void Update(IEnumerable<TierPrice> t)
        {
            DbSet.UpdateRange(t);
        }
        public void Remove(IEnumerable<TierPrice> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<TierPrice>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.ProductId == id).ToListAsync();
        }
    }
}
