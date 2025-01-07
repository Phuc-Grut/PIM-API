using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class ServiceAddRepository : IServiceAddRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ServiceAdd> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ServiceAddRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ServiceAdd>();
        }

        public void Add(ServiceAdd serviceAdd)
        {
            DbSet.Add(serviceAdd);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ServiceAdd>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ServiceAdd> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ServiceAdd serviceAdd)
        {
            DbSet.Remove(serviceAdd);
        }

        public void Update(ServiceAdd serviceAdd)
        {
            DbSet.Update(serviceAdd);
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
        public async Task<(IEnumerable<ServiceAdd>, int)> Filter(string? keyword, IFopRequest request)
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

        public async Task<int> FilterCount(string? keyword, int? status)
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
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ServiceAdd>> GetListListBox(string? keyword, int? status)
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
        public void Update(IEnumerable<ServiceAdd> serviceAdds)
        {
            DbSet.UpdateRange(serviceAdds);
        }
    }
}
