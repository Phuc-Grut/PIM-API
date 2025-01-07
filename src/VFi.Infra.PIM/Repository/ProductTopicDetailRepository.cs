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
    public partial class ProductTopicDetailRepository : IProductTopicDetailRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ProductTopicDetail> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ProductTopicDetailRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<ProductTopicDetail>();
        }

        public void Add(ProductTopicDetail warehouse)
        {
            DbSet.Add(warehouse);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ProductTopicDetail>> GetAll()
        {
            return await DbSet.ToListAsync();
        }
        public async Task<IEnumerable<ProductTopicDetail>> GetAllByStatus(int status)
        {
            return await DbSet.Where(x=>x.Status.Equals(status)).ToListAsync();
        }
        public async Task<ProductTopicDetail> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(ProductTopicDetail warehouse)
        {
            DbSet.Remove(warehouse);
        }

        public void Update(ProductTopicDetail warehouse)
        {
            DbSet.Update(warehouse);
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
        public async Task<IEnumerable<ProductTopicDetail>> GetListListBox(string? keyword)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            return await query.ToListAsync();
        }
        public void Update(IEnumerable<ProductTopicDetail> warehouses)
        {
            DbSet.UpdateRange(warehouses);
        }


        public async Task<IEnumerable<ProductTopicDetail>> Filter(string? keyword, Dictionary<string, object> filter, Guid? productTopicId, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            if (productTopicId.HasValue)
            {
                query = query.Where(x => x.ProductTopicId.Equals(productTopicId.Value));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("channel"))
                {
                     query = query.Where(x => x.Channel.Equals(item.Value + ""));
                } 
                if (item.Key.Equals("topic"))
                {
                    query = query.Where(x => x.ProductTopic.Equals(item.Value + ""));
                }
                if (item.Key.Equals("keyword"))
                {
                    query = query.Where(x => x.Name.Equals(item.Value + ""));
                }
            }
            query = query.Where(x => x.Exp.HasValue && x.Exp.Value > DateTime.Now.AddMinutes(-2));
            return await query.OrderByDescending(x => x.PublishDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter, Guid? productTopicId)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }

            if (productTopicId.HasValue)
            {
                query = query.Where(x => x.ProductTopicId.Equals(productTopicId.Value));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("channel"))
                {
                    query = query.Where(x => x.Channel.Equals(item.Value + ""));
                }
                if (item.Key.Equals("topic"))
                {
                    query = query.Where(x => x.ProductTopic.Equals(item.Value + ""));
                }
                if (item.Key.Equals("keyword"))
                {
                    query = query.Where(x => x.Name.Equals(item.Value + ""));
                }
            }
            query = query.Where(x => x.Exp.HasValue && x.Exp.Value > DateTime.Now.AddMinutes(-2));
            return await query.CountAsync();
        }

       

        public async Task<IEnumerable<ProductTopicDetail>> FilterByPage(string? keyword, Dictionary<string, object> filter, List<Guid> productTopicListId, int pagesize, int pageindex)
        {

            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            if (productTopicListId.Any())
            {
                query = query.Where(x => productTopicListId.Contains(x.ProductTopicId));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("channel"))
                {
                    query = query.Where(x => x.Channel.Equals(item.Value + ""));
                }
                if (item.Key.Equals("topic"))
                {
                    query = query.Where(x => x.ProductTopic.Equals(item.Value + ""));
                }
                if (item.Key.Equals("keyword"))
                {
                    query = query.Where(x => x.Name.Equals(item.Value + ""));
                }
            }
            query = query.Where(x => x.Exp.HasValue && x.Exp.Value > DateTime.Now.AddMinutes(-2));
            return await query.OrderByDescending(x=>x.PublishDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }
        public async Task<int> FilterByPageCount(string? keyword, Dictionary<string, object> filter, List<Guid> productTopicListId)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }

            if (productTopicListId.Any())
            {
                query = query.Where(x => productTopicListId.Contains(x.ProductTopicId));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("channel"))
                {
                    query = query.Where(x => x.Channel.Equals(item.Value + ""));
                }
                if (item.Key.Equals("topic"))
                {
                    query = query.Where(x => x.ProductTopic.Equals(item.Value + ""));
                }
                if (item.Key.Equals("keyword"))
                {
                    query = query.Where(x => x.Name.Equals(item.Value + ""));
                }
            }
            query = query.Where(x => x.Exp.HasValue && x.Exp.Value > DateTime.Now.AddMinutes(-2));
            return await query.CountAsync();
        }
    }
}
