using Consul.Filtering;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class SpecificationAttributeOptionRepository : ISpecificationAttributeOptionRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<SpecificationAttributeOption> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public SpecificationAttributeOptionRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<SpecificationAttributeOption>();
        }

        public void Add(SpecificationAttributeOption specificationAttributeOption)
        {
            DbSet.Add(specificationAttributeOption);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<SpecificationAttributeOption>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("specificationAttributeId"))
                {
                    query = query.Where(x => x.SpecificationAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("specificationAttributeId"))
                {
                    query = query.OrderBy(x => x.DisplayOrder).Where(x => x.SpecificationAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<SpecificationAttributeOption>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<SpecificationAttributeOption> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(SpecificationAttributeOption specificationAttributeOption)
        {
            DbSet.Remove(specificationAttributeOption);
        }

        public void Update(SpecificationAttributeOption specificationAttributeOption)
        {
            DbSet.Update(specificationAttributeOption);
        }

        public async Task<IEnumerable<SpecificationAttributeOption>> GetListListBox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Color.Contains(keyword) || x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("specificationAttributeId"))
                {
                    query = query.Where(x => x.SpecificationAttributeId.Equals(new Guid(item.Value + "")));
                }
            }
            return await query.ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SpecificationAttributeOption>> Filter(Guid id)
        {
            return await DbSet.Where(x => x.SpecificationAttributeId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
        }
        public void Update(IEnumerable<SpecificationAttributeOption> options)
        {
            DbSet.UpdateRange(options);
        }
        public void Add(IEnumerable<SpecificationAttributeOption> options)
        {
            DbSet.AddRange(options);
        }
        public void Remove(IEnumerable<SpecificationAttributeOption> t)
        {
            DbSet.RemoveRange(t);
        }

        public async Task<IEnumerable<SpecificationAttributeOption>> GetByParentId(Guid id)
        {
            return await DbSet.Where(x => x.SpecificationAttributeId == id).ToListAsync();
        }
    }
}
