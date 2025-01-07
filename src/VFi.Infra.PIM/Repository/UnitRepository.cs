using Consul.Filtering;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class UnitRepository : IUnitRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Unit> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public UnitRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<Unit>();
        }

        public void Add(Unit unit)
        {
            DbSet.Add(unit);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Unit>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Unit> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(Unit unit)
        {
            DbSet.Remove(unit);
        }

        public void Update(Unit unit)
        {
            DbSet.Update(unit);
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
        public async Task<(IEnumerable<Unit>, int)> Filter(string? keyword, IFopRequest request)
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

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToUpper();
                query = query.Where(x => x.Code.ToUpper().Contains(keyword) || x.Name.ToUpper().Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupUnitId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("groupCode"))
                {
                    query = query.Where(x => x.GroupUnit.Code.Equals(item.Value));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Unit>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupUnitId.Equals(item.Value != null ? new Guid(item.Value + "") : null));
                }
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();

        }
        public void Update(IEnumerable<Unit> units)
        {
            DbSet.UpdateRange(units);
        }
    }
}
