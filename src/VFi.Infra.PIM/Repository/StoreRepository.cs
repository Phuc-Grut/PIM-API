using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class StoreRepository : IStoreRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Store> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public StoreRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<Store>();
        }

        public void Add(Store store)
        {
            DbSet.Add(store);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Store>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Store> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<IEnumerable<Store>> GetById(IEnumerable<ProductStoreMapping> productStore)
        {
            var query = DbSet.AsQueryable();
            var productStoreList = productStore.ToList().Select(x => x.StoreId).ToList();
            return await query.Where(x => productStoreList.Contains(x.Id)).ToListAsync();
        }
        public void Remove(Store store)
        {
            DbSet.Remove(store);
        }

        public void Update(Store store)
        {
            DbSet.Update(store);
        }
        public async Task<bool> CheckExist(string? code, Guid? id)
        {
            if (id == null)
            {
                if (String.IsNullOrEmpty(code))
                {
                    return false;
                }
                return await DbSet.AnyAsync(x => x.Code.Equals(code));
            }
            return await DbSet.AnyAsync(x => x.Code.Equals(code) && x.Id != id);
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<(IEnumerable<Store>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToUpper();
                query = query.Where(x => x.Code.ToUpper().Contains(keyword) || EF.Functions.Like(x.Name.ToUpper(), $"%{keyword}%"));
            }
            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }

        public async Task<int> FilterCount(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Store>> GetListListBox(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<Store> stores)
        {
            DbSet.UpdateRange(stores);
        }
    }
}
