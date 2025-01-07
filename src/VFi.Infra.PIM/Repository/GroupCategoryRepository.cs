using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class GroupCategoryRepository : IGroupCategoryRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<GroupCategory> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public GroupCategoryRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<GroupCategory>();
        }

        public void Add(GroupCategory groupCategory)
        {
            DbSet.Add(groupCategory);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<GroupCategory>> GetAll()
        {
            return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<GroupCategory> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<GroupCategory> GetByCode(string code)
        {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstAsync();
        }
        public void Remove(GroupCategory groupCategory)
        {
            DbSet.Remove(groupCategory);
        }

        public void Update(GroupCategory groupCategory)
        {
            DbSet.Update(groupCategory);
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
        public async Task<IEnumerable<GroupCategory>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
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

        public async Task<IEnumerable<GroupCategory>> GetListListBox(int? status, string? keyword)
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
        public void Update(IEnumerable<GroupCategory> t)
        {
            DbSet.UpdateRange(t);
        }

        public async Task<(IEnumerable<GroupCategory>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
           
            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
    }
}
