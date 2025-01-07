using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class SpecificationAttributeRepository : ISpecificationAttributeRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<SpecificationAttribute> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public SpecificationAttributeRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<SpecificationAttribute>();
        }

        public void Add(SpecificationAttribute specificationAttribute)
        {
            DbSet.Add(specificationAttribute);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<SpecificationAttribute>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<SpecificationAttribute> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(SpecificationAttribute specificationAttribute)
        {
            DbSet.Remove(specificationAttribute);
        }

        public void Update(SpecificationAttribute specificationAttribute)
        {
            DbSet.Update(specificationAttribute);
        }

        public async Task<IEnumerable<SpecificationAttribute>> Filter(string? keyword, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Alias.Contains(keyword) || x.Name.Contains(keyword) || x.Code.Contains(keyword));
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Alias.Contains(keyword) || x.Name.Contains(keyword) || x.Code.Contains(keyword));
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<SpecificationAttribute>> GetListListBox(string? keyword, int? status)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Alias.Contains(keyword) || x.Name.Contains(keyword) || x.Code.Contains(keyword));
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
        public void Update(IEnumerable<SpecificationAttribute> attributes)
        {
            DbSet.UpdateRange(attributes);
        }

        public async Task<(IEnumerable<SpecificationAttribute>, int)> Filter(string? keyword, IFopRequest request)
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
