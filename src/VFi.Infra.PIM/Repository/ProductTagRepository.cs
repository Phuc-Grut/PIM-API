using Consul;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class ProductTagRepository : IProductTagRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTag> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductTagRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTag>();
        }

        public void Add(ProductTag productTag)
        {
            DbSet.Add(productTag);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductTag>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ProductTag> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductTag productTag)
        {
            DbSet.Remove(productTag);
        }

        public void Update(ProductTag productTag)
        {
            DbSet.Update(productTag);
        }

        public async Task<IEnumerable<ProductTag>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
            }
            return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
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

        public async Task<IEnumerable<ProductTag>> GetListListBox(int? status, int? type, string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            if (type != null)
            {
                query = query.Where(x => x.Type == type);
            }
            return await query.ToListAsync();
        }  
        public async Task<IEnumerable<ProductTag>> GetByIds(IEnumerable<ProductProductTagMapping> productProductTag)
        {
            var query = DbSet.AsQueryable();
            var productProductTagList = productProductTag.ToList().Select(x => x.ProductTagId).ToList();
            return await query.Where(x => productProductTagList.Contains(x.Id)).ToListAsync();
        }
        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Guid>> Filter(List<string> tags, Guid createdBy)
        {
            List<Guid> data = new List<Guid> ();
            List<ProductTag> listTagNew = new List<ProductTag>();
            var List = await DbSet.AsQueryable().ToListAsync();
            foreach(string ele in tags)
            {
                var isExisted = List.FirstOrDefault(e => string.Compare(e.Name, ele, StringComparison.OrdinalIgnoreCase) == 0);
                if (isExisted != null)
                {
                    data.Add(isExisted.Id);
                } else
                {
                    var tagId = Guid.NewGuid();
                    var tag = new ProductTag
                    {
                        Id = tagId,
                        Name = ele,
                        Status = 1,
                        CreatedDate = DateTime.Now,
                        CreatedBy = createdBy
                    };
                    listTagNew.Add(tag);
                    data.Add(tagId);
                }
            }
            if(listTagNew.Count > 0)
            {
                DbSet.AddRange(listTagNew);
            }
            return data;
        }

        public async Task<(IEnumerable<ProductTag>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }

            var (filtered, totalCount) = query.ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }
    }
}
