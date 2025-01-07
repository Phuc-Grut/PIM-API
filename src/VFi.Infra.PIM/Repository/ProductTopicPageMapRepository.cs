using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class ProductTopicPageMapRepository : IProductTopicPageMapRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTopicPageMap> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductTopicPageMapRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTopicPageMap>();
        }

        public void Add(ProductTopicPageMap item)
        {
            DbSet.Add(item);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductTopicPageMap>> GetAll()
        {
            return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<ProductTopicPageMap> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        
        public void Remove(ProductTopicPageMap item)
        {
            DbSet.Remove(item);
        }

        public void Update(ProductTopicPageMap item)
        {
            DbSet.Update(item);
        }

        public Task<bool> CheckExistById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ProductTopic>> GetProductTopic(string pagecode)
        {
           var topic = await DbSet.Include(x => x.ProductTopicNavigation).Where(x => x.ProductTopicPage.Equals(pagecode)).OrderBy(x=>x.DisplayOrder).Select(x=>x.ProductTopicNavigation).ToListAsync();
           return topic;
        }
        public async Task<IEnumerable<ProductTopic>> GetProductTopicBySlugPage(string slug)
        {
            var topic = await DbSet.Include(x => x.ProductTopicNavigation).Where(x => x.ProductTopicPageNavigation.Slug.Equals(slug)).OrderBy(x => x.DisplayOrder).Select(x => x.ProductTopicNavigation).ToListAsync();
            return topic;
        }
        public async Task<IEnumerable<ProductTopic>> GetProductTopicByPageId(Guid pageId)
        {
            var topic = await DbSet.Include(x => x.ProductTopicNavigation).Where(x => x.ProductTopicPageId.Equals(pageId)).OrderBy(x => x.DisplayOrder).Select(x => x.ProductTopicNavigation).ToListAsync();
            return topic;
        }
        public async Task<IEnumerable<Guid>> GetListProductTopicIdByPage(string page)
        {
            var pageId = Guid.Empty;
            if (Guid.TryParse(page, out pageId))
            {
                var items = await DbSet.Where(x => x.ProductTopicPageId.Equals(pageId)).Select(x => x.ProductTopicId).ToListAsync();
                return items;
            }
            else
            {
                var items = await DbSet.Where(x => x.ProductTopicPage.Equals(page)).Select(x => x.ProductTopicId).ToListAsync();
                return items;
            }

          
        }
        public async Task<IEnumerable<Guid>> GetListProductTopicIdBySlugPage(string slug)
        {
            var items = await DbSet.Where(x => x.ProductTopicPageNavigation.Slug.Equals(slug)).Select(x => x.ProductTopicId).ToListAsync();
            return items;
        }

        public async Task<ProductTopicPageMap> GetProductTopicMapByProductTopicId(Guid productTopicId)
        {
            return await DbSet.Where(x => x.ProductTopicId == productTopicId).FirstAsync();
        }

        public void Add(IEnumerable<ProductTopicPageMap> data)
        {
            DbSet.AddRange(data);
        }

        public void Remove(IEnumerable<ProductTopicPageMap> data)
        {
            DbSet.RemoveRange(data);
        }

        public async Task<IEnumerable<ProductTopicPageMap>> Filter(Guid productTopicId, Guid productTopicPageId)
        {
            var query = DbSet.AsQueryable();

            query =  query.Where(x => x.ProductTopicId == productTopicId && x.ProductTopicPageId == productTopicPageId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProductTopicPageMap>> Filter(Guid productTopicId)
        {
            var query = DbSet.AsQueryable();

            query = query.Where(x => x.ProductTopicId == productTopicId);

            return await query.ToListAsync();
        }
    }
}
