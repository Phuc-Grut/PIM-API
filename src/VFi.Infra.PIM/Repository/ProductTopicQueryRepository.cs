using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public class ProductTopicQueryRepository : IProductTopicQueryRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTopicQuery> DbSet;
        public IUnitOfWork UnitOfWork => Db;

        public ProductTopicQueryRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTopicQuery>();
        }

        public void Add(ProductTopicQuery t)
        {
            DbSet.Add(t);
        }

        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductTopicQuery>> Filter(int? status, string? keyword, Guid? productTopicId)
        {
            var query = DbSet.Include(x => x.ProductTopic).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            if (productTopicId != null)
            {
                query = query.Where(x => x.ProductTopic.Id == productTopicId);
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<IEnumerable<ProductTopicQuery>> GetAll()
        {
            return await DbSet.OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<ProductTopicQuery> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductTopicQuery t)
        {
            DbSet.Remove(t);
        }

        public void Update(ProductTopicQuery t)
        {
            DbSet.Update(t);
        }

        public async Task<(IEnumerable<ProductTopicQuery>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }

        public void Update(IEnumerable<ProductTopicQuery> t)
        {
            DbSet.UpdateRange(t);
        }
    }
}
