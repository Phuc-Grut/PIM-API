using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class ServiceAddPriceSyntaxRepository : IServiceAddPriceSyntaxRepository
    {
        protected readonly SqlCoreContext Db;
        protected readonly DbSet<ServiceAddPriceSyntax> DbSet;
        public IUnitOfWork UnitOfWork => Db;
        public ServiceAddPriceSyntaxRepository(SqlCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<ServiceAddPriceSyntax>();
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public async Task<IEnumerable<ServiceAddPriceSyntax>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public void Add(ServiceAddPriceSyntax t)
        {
            DbSet.Add(t);
        }

        public void Update(ServiceAddPriceSyntax t)
        {
            DbSet.Update(t);
        }

        public void Remove(ServiceAddPriceSyntax t)
        {
            DbSet.Remove(t);
        }

        public async Task<ServiceAddPriceSyntax> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<bool> CheckExistById(Guid id)
        {
            return await DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
