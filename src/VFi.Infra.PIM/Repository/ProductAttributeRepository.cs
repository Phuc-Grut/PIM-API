using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class ProductAttributeRepository : IProductAttributeRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductAttribute> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductAttributeRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductAttribute>();
        }

        public void Add(ProductAttribute productAttribute)
        {
            DbSet.Add(productAttribute);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductAttribute>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductAttribute> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductAttribute productAttribute)
        {
            DbSet.Remove(productAttribute);
        }

        public void Update(ProductAttribute productAttribute)
        {
            DbSet.Update(productAttribute);
        }

        public async Task<IEnumerable<ProductAttribute>> Filter(string? keyword, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Alias.Contains(keyword));
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Alias.Contains(keyword));
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductAttribute>> GetListListBox(string? keyword, int? status)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Alias.Contains(keyword));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public void Update(IEnumerable<ProductAttribute> attributes)
        {
            DbSet.UpdateRange(attributes);
        }

        public async Task<(IEnumerable<ProductAttribute>, int)> Filter(string? keyword, IFopRequest request)
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
