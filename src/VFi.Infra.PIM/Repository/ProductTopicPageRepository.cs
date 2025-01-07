using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class ProductTopicPageRepository : IProductTopicPageRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTopicPage> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductTopicPageRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTopicPage>();
        }

        public void Add(ProductTopicPage item)
        {
            DbSet.Add(item);
        }

        public void Dispose()
        {
            Db.Dispose();
        }
        public async Task<IEnumerable<ProductTopicPage>> GetAll()
        { 
                return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public async Task<IEnumerable<ProductTopicPage>> GetAll(int? status)
        {
            if (status.HasValue)
            {
                return await DbSet.Where(x => x.Status.Equals(status.Value)).OrderBy(x => x.DisplayOrder).ToListAsync();
            }
            else
                return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<ProductTopicPage> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<ProductTopicPage> GetByCode(string code)
        {
            return await DbSet.Include(e => e.ProductTopicPageMap).Where(x => x.Code.Equals(code)).FirstAsync();
        }
        public async Task<ProductTopicPage> GetBySlug(string slug)
        {
            return await DbSet.Where(x => x.Slug.Equals(slug)).FirstAsync();
        }
        public void Remove(ProductTopicPage item)
        {
            DbSet.Remove(item);
        }

        public void Update(ProductTopicPage item)
        {
            DbSet.Update(item);
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
        public async Task<IEnumerable<ProductTopicPage>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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

        public async Task<IEnumerable<ProductTopicPage>> GetListListBox(int? status, string? keyword)
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
        public void Update(IEnumerable<ProductTopicPage> t)
        {
            DbSet.UpdateRange(t);
        }


        public async Task<IEnumerable<ProductTopicPageMap>> GetProductTopicPageMapByCode(string code)
        {
            return await DbSet.Include(e => e.ProductTopicPageMap).Where(x => x.Code.Equals(code)).FirstAsync().Select(x=>x.ProductTopicPageMap.OrderBy(x=>x.DisplayOrder));
        }

        public async Task<(IEnumerable<ProductTopicPage>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Code, $"%{keyword}%") || EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            
            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
    }
}
