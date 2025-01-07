using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class WarehouseRepository : IWarehouseRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Warehouse> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public WarehouseRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<Warehouse>();
        }

        public void Add(Warehouse warehouse)
        {
            DbSet.Add(warehouse);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Warehouse>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Warehouse> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(Warehouse warehouse)
        {
            DbSet.Remove(warehouse);
        }

        public void Update(Warehouse warehouse)
        {
            DbSet.Update(warehouse);
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

        public async Task<(IEnumerable<Warehouse>, int)> Filter(string? keyword, IFopRequest request)
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

        public async Task<IEnumerable<Warehouse>> GetListListBox(string? keyword, int? status)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            if (status is not null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<Warehouse> warehouses)
        {
            DbSet.UpdateRange(warehouses);
        }
        public async Task<IEnumerable<Warehouse>> Filter(IEnumerable<ProductInventory> productInvetories)
        {
            var query = DbSet.AsQueryable();
            var data = productInvetories.Select(x => x.WarehouseId);
            return await query.Where(x => data.Contains(x.Id)).ToListAsync();
        }
    }
}
