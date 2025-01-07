using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class GroupUnitRepository : IGroupUnitRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<GroupUnit> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public GroupUnitRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<GroupUnit>();
        }

        public void Add(GroupUnit groupUnit)
        {
            DbSet.Add(groupUnit);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<GroupUnit>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<GroupUnit> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(GroupUnit groupUnit)
        {
            DbSet.Remove(groupUnit);
        }

        public void Update(GroupUnit groupUnit)
        {
            DbSet.Update(groupUnit);
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
        public async Task<(IEnumerable<GroupUnit>, int)> Filter(string? keyword, IFopRequest request)
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
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<GroupUnit>> GetListListBox(int? status, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<GroupUnit> groupUnits)
        {
            DbSet.UpdateRange(groupUnits);
        }
    }
}
