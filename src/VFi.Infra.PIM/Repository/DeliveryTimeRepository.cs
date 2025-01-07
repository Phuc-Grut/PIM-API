using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class DeliveryTimeRepository : IDeliveryTimeRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<DeliveryTime> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public DeliveryTimeRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<DeliveryTime>();
        }

        public void Add(DeliveryTime deliveryTime)
        {
            DbSet.Add(deliveryTime);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<DeliveryTime>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<DeliveryTime> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(DeliveryTime deliveryTime)
        {
            DbSet.Remove(deliveryTime);
        }

        public void Update(DeliveryTime deliveryTime)
        {
            DbSet.Update(deliveryTime);
        }

        public async Task<IEnumerable<DeliveryTime>> Filter(string? keyword, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<DeliveryTime>> GetListListBox(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public void Update(IEnumerable<DeliveryTime> deliveryTimes)
        {
            DbSet.UpdateRange(deliveryTimes);
        }

        public async Task<(IEnumerable<DeliveryTime>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToUpper();
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }

            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
    }
}
