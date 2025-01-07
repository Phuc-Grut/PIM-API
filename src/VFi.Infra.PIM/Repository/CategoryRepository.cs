using Consul.Filtering;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Formats.Asn1.AsnWriter;

namespace VFi.Domain.PIM.Interfaces
{
    public partial class CategoryRepository : ICategoryRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<Category> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public CategoryRepository(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            Db = serviceProvider.GetRequiredService<SqlCoreContext>();
            DbSet = Db.Set<Category>();
        }
        public void Add(Category category)
        {
            DbSet.Add(category);
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Category> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public async Task<Category> GetByCode(string code, string groupcode)
        {
            return await DbSet.Where(x=>x.Code.Equals(code) && x.GroupCategoryCode.Equals(groupcode)).OrderBy(x=>x.Level).FirstAsync();
        }
        public async Task<IEnumerable<Category>> GetByIds(IEnumerable<ProductCategoryMapping> productCategory)
        {
            var query = DbSet.AsQueryable();
            var productCategoryList = productCategory.ToList().Select(x => x.CategoryId).ToList();
            return await query.Where(x => productCategoryList.Contains(x.Id)).ToListAsync();
        }
        public void Remove(Category category)
        {
          
            DbSet.Remove(category);
        }

        public void Update(Category category)
        {
            
            DbSet.Update(category);
        }
        public void Update(IEnumerable<Category> stores)
        {
          
            DbSet.UpdateRange(stores);
        }
        public async Task<bool> CheckExist(string? code, Guid? id)
        {
            if (id == null)
            {
                if(String.IsNullOrEmpty(code))
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

        public async Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword) || x.Keywords.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("group"))
                {
                    query = query.Where(x => x.GroupCategoryCode.Equals(item.Value + ""));
                }
                if (item.Key.Equals("parentId"))
                {
                    if (item.Value.Equals("null"))
                    {
                        query = query.Where(x => x.ParentCategoryId == null);
                    }
                    else
                    {
                        query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                    }

                }
            }
            return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword) || x.Keywords.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("group"))
                {
                    query = query.Where(x => x.GroupCategoryCode.Equals(item.Value + ""));
                }
                if (item.Key.Equals("parentId"))
                {
                    if (item.Value.Equals("null"))
                    {
                        query = query.Where(x => x.ParentCategoryId == null);
                    }
                    else
                    {
                        query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                    }

                }
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<Category>> GetListListBox(Dictionary<string, object> filter, string? keyword)
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
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("parentId"))
                {
                    if (item.Value.Equals("null"))
                    {
                        query = query.Where(x => x.ParentCategoryId == null);
                    }
                    else
                    {
                        query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                    }

                }
            }
            return await query.OrderBy(x => x.DisplayOrder).ToListAsync();

        }
        public async Task<IEnumerable<Category>> GetCombobox(Dictionary<string, object> filter, string? keyword)
        {
            var query = DbSet.AsQueryable();
                query = query.Where(x => (keyword == null || keyword == "" || x.Name.Contains(keyword)) && x.Status == 1);

            foreach (var item in filter)
            {
                if (item.Key.Equals("groupId"))
                {
                        var ListGroupCategoryIds = item.Value.ToString().Split(',').ToList();
                        query = query.Where(x => ListGroupCategoryIds.Contains(x.GroupCategoryId.ToString()) || item.Value.Equals("null"));
                }
            }
            return await query.OrderBy(x => x.FullName).Skip(0).Take(100).ToListAsync();
        }
      

        public  async Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter)
        {
            var query = DbSet.AsQueryable();

            var levelStart = 0; 
            var level = 1; 
            var levelCount = 1;
            Guid parentId = Guid.Empty;
            var levelMin = 0; var levelMax = 0;string parentIds = "";
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || x.Name.Contains(keyword));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                { 
                    if (string.IsNullOrEmpty(item.Value.ToString())) continue;
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                    continue;  
                }
                if (item.Key.Equals("group"))
                {
                    query = query.Where(x => x.GroupCategoryCode.Equals(item.Value.ToString()));
                    continue;
                }
                if (item.Key.Equals("groupid"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(Guid.Parse(item.Value.ToString())));
                    continue;
                }
                if (item.Key.Equals("parent"))
                {
                    if (item.Value ==null || string.IsNullOrEmpty(item.Value.ToString())) continue;
                    levelStart = 1;
                   
                    if(!Guid.TryParse(item.Value.ToString(), out parentId))
                    {
                        try
                        {
                            var cate = DbSet.Where(x => x.Code.Equals(item.Value.ToString())).OrderBy(x=>x.Level).Select(x => new { x.Id, x.Level,x.ParentIds }).First();
                            parentId = cate.Id;
                            levelStart = cate.Level.HasValue ? cate.Level.Value+1: 1;
                            parentIds = cate.ParentIds;
                        }
                        catch (Exception)
                        {}
                    }
                    else
                    {
                        try
                        {
                            var cate = DbSet.FindAsync(parentId).Result;
                            parentId = cate.Id; parentIds = cate.ParentIds;
                            levelStart = cate.Level.HasValue ? cate.Level.Value+1  : 1;
                        }
                        catch (Exception)
                        { }
                    }
                   
                    continue;

                }
                if (item.Key.Equals("level"))
                {
                    
                    if (item.Value!=null) { 
                        level = Convert.ToInt32(item.Value);
                    }
                    levelMin = level - 1 + levelStart;
                   
                    continue;
                }
                if (item.Key.Equals("levelCount"))
                {

                    if (item.Value != null)
                    {
                        levelCount = Convert.ToInt32(item.Value);
                    }
                    levelMax = level - 1 + levelStart + levelCount - 1;
                  
                    continue;
                }
            }
            if (parentId.Equals(Guid.Empty))
            {
                query = query.Where(x => x.Level >= levelMin);
                query = query.Where(x => x.Level <= levelMax);
            }
            else
            {
                if (levelMin == levelMax)
                {
                    query = query.Where(x => x.ParentCategoryId.Equals(parentId));
                }else if (levelMax - levelMin > 0)
                {
                    query = query.Where(x => x.ParentIds.StartsWith(parentIds));
                }
                query = query.Where(x => x.Level >= levelMin);
                query = query.Where(x => x.Level <= levelMax);
            }


            return await query.OrderBy(x => x.FullName).ToListAsync();
            
        }

        public async Task<(IEnumerable<Category>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            foreach (var item in filter)
            {
                if (item.Key.Equals("status"))
                {
                    query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
                }
                if (item.Key.Equals("groupId"))
                {
                    query = query.Where(x => x.GroupCategoryId.Equals(new Guid(item.Value + "")));
                }
                if (item.Key.Equals("parentId"))
                {
                    if (item.Value.Equals("null"))
                    {
                        query = query.Where(x => x.ParentCategoryId == null);
                    }
                    else
                    {
                        query = query.Where(x => x.ParentCategoryId.Equals(new Guid(item.Value + "")));
                    }

                }
            }

            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }

        public async Task<IEnumerable<Category>> GetBreadcrumb(string group, string category)
        {
            var query = DbSet.AsQueryable();
            Guid cateId = Guid.Empty;
            string parentIds = "";

            if (!Guid.TryParse(category, out cateId))
            {
                try
                {
                    var cate = DbSet.Where(x => x.Code.Equals(category) && x.GroupCategoryCode.Equals(group)).OrderBy(x => x.Level).Select(x => new { x.ParentIds }).First();
                    parentIds = cate.ParentIds;
                }
                catch (Exception)
                { }
            }
            else
            {
                try
                {
                    var cate = DbSet.FindAsync(cateId).Result;
                    parentIds = cate.ParentIds;
                }
                catch (Exception)
                { }
            }
            var listParentId = parentIds.Split(',').Select(x=> new Guid(x));    
            query = query.Where(x => listParentId.Contains(x.Id) && x.GroupCategoryCode.Equals(group));

            return await query.OrderBy(x => x.Level).ToListAsync();

        }

        public async Task<(IEnumerable<Category>, int)> Filter(string? keyword, IFopRequest request)
        {
            var query = DbSet.AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
            }

            var (filtered, totalCount) = query.OrderBy(x => x.DisplayOrder).ApplyFop(request);
            return (await filtered.ToListAsync(), totalCount);
        }

        public void UpdateField(Category category, params Expression<Func<Category, object>>[] properties)
        {
            foreach (var property in properties)
            {
                Db.Entry(category).Property(property).IsModified = true;
            }
        }
    }
}
