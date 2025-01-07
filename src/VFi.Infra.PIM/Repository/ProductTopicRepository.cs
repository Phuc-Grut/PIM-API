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
    public partial class ProductTopicRepository : IProductTopicRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTopic> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductTopicRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTopic>();
        }

        public void Add(ProductTopic groupCategory)
        {
            DbSet.Add(groupCategory);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductTopic>> GetAll()
        {
            return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<ProductTopic> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<ProductTopic> GetByCode(string code)
        {
            return await DbSet.Include(e => e.ProductTopicQuery).Where(x => x.Code.Equals(code)).FirstAsync();
        }
        public async Task<ProductTopic> GetBySlug(string slug)
        {
            return await DbSet.Where(x => x.Slug.Equals(slug)).FirstAsync();
        }
        public void Remove(ProductTopic groupCategory)
        {
            DbSet.Remove(groupCategory);
        }

        public void Update(ProductTopic groupCategory)
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
        public async Task<IEnumerable<ProductTopic>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
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

        public async Task<IEnumerable<ProductTopic>> GetListListBox(int? status, string? keyword, Guid? productTopicPageId)
        {
            var query = DbSet.Include(x => x.ProductTopicPageMap).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            if (productTopicPageId != null)
            {
                query = query.Where(x => x.ProductTopicPageMap.Any(map => map.ProductTopicPageId == productTopicPageId));
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<ProductTopic> t)
        {
            DbSet.UpdateRange(t);
        }

        public async Task<(IEnumerable<ProductTopic>, int)> Filter(string? keyword, Guid? productTopicPageId, IFopRequest request)
        {
            var query = DbSet.Include(x => x.ProductTopicPageMap).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Code, $"%{keyword}%") || EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            if (productTopicPageId != null)
            {
                query = query.Where(x => x.ProductTopicPageMap.Any(map => map.ProductTopicPageId == productTopicPageId));
            }
            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
    }
}
