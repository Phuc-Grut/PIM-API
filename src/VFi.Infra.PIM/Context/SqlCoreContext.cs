using System.Linq;
using System.Threading.Tasks;
using VFi.Infra.PIM.Mappings;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;
using VFi.Domain.PIM.Models;  

namespace VFi.Infra.PIM.Context
{
    public sealed class SqlCoreContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public SqlCoreContext(DbContextOptions<SqlCoreContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryRoot> CategoryRoot { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<DeliveryTime> DeliveryTime { get; set; }
        public DbSet<GroupCategory> GroupCategory { get; set; }
        public DbSet<GroupUnit> GroupUnit { get; set; }
        //public DbSet<Language> Language { get; set; }
       // public DbSet<LocaleStringResource> LocaleStringResource { get; set; }
        //public DbSet<LocalizedProperty> LocalizedProperty { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductAttributeOption> ProductAttributeOption { get; set; }
        public DbSet<ProductBrand> ProductBrand { get; set; }
        public DbSet<ProductCategoryMapping> ProductCategoryMapping { get; set; }
        public DbSet<ProductGroupCategoryMapping> ProductGroupCategoryMapping { get; set; }
        public DbSet<ProductInventory> ProductInventory { get; set; }
        public DbSet<ProductInventoryDetail> ProductInventoryDetail { get; set; }
        public DbSet<ProductMedia> ProductMedia { get; set; }
        public DbSet<ProductOrigin> ProductOrigin { get; set; }
        public DbSet<ProductPackage> ProductPackage { get; set; }
        public DbSet<ProductProductAttributeMapping> ProductProductAttributeMapping { get; set; }
        public DbSet<ProductProductTagMapping> ProductProductTagMapping { get; set; }
        public DbSet<ProductReview> ProductReview { get; set; }
        public DbSet<ProductReviewHelpfulness> ProductReviewHelpfulness { get; set; }
        public DbSet<ProductServiceAdd> ProductServiceAdd { get; set; }
        public DbSet<ProductSpecificationAttributeMapping> ProductSpecificationAttributeMapping { get; set; }
        public DbSet<ProductStoreMapping> ProductStoreMapping { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<ProductTopic> ProductTopic { get; set; }
        public DbSet<ProductTopicDetail> ProductTopicDetail { get; set; }
        public DbSet<ProductTopicPage> ProductTopicPage { get; set; }
        public DbSet<ProductTopicPageMap> ProductTopicPageMap { get; set; }
        public DbSet<ProductTopicQuery> ProductTopicQuery { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouse { get; set; }
        public DbSet<RelatedProduct> RelatedProduct { get; set; }
        public DbSet<ServiceAdd> ServiceAdd { get; set; }
        public DbSet<ShippingMethod> ShippingMethod { get; set; }
        public DbSet<SpecificationAttribute> SpecificationAttribute { get; set; }
        public DbSet<SpecificationAttributeOption> SpecificationAttributeOption { get; set; }
        public DbSet<ProductSpecificationCode> ProductSpecificationCodeMapping { get; set; }
        public DbSet<Store> Store { get; set; } 
        public DbSet<TaxCategory> TaxCategory { get; set; }
        public DbSet<TierPrice> TierPrice { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<ServiceAddPriceSyntax> ServiceAddPriceSyntax { get; set; }
        public DbSet<SP_GET_TOP_NEW_PRODUCT> SP_GET_TOP_NEW_PRODUCT { get; set; }
        public DbSet<SP_COUNT_PRODUCTS> SP_COUNT_PRODUCTS { get; set; }
        public DbSet<SP_COUNT_PRODUCT_BY_PRODUCTTYPE> SP_COUNT_PRODUCT_BY_PRODUCTTYPE { get; set; }
        public DbSet<SP_GET_TOP_CATEGORY> SP_GET_TOP_CATEGORY { get; set; }
        public DbSet<SP_GET_TOP_MANUFACTURER> SP_GET_TOP_MANUFACTURER { get; set; }
        public DbSet<SP_GET_TOP_PRODUCT_BRAND> SP_GET_TOP_PRODUCT_BRAND { get; set; }
        public DbSet<SP_GET_TOP_PRODUCTS_INVENTORY> SP_GET_TOP_PRODUCTS_INVENTORY { get; set; }
        public DbSet<SP_GET_INVENTORY_DETAIL> SP_GET_INVENTORY_DETAIL { get; set; }
        public DbSet<SP_GET_INVENTORY_DETAIL_BY_LISTID> SP_GET_INVENTORY_DETAIL_BY_LISTID { get; set; }
        public DbSet<SP_GET_INVENTORY_BY_LISTCODE> SP_GET_INVENTORY_BY_LISTCODE { get; set; }
        public DbSet<SP_GET_PRODUCT_PRICE_COST_BY_LISTID> SP_GET_PRODUCT_PRICE_COST_BY_LISTID { get; set; }
        public DbSet<SP_GET_INVENTORY_BY_LISTID> SP_GET_INVENTORY_BY_LISTID { get; set; }
        public DbSet<SP_GET_PRODUCT_PRICE_BY_LISTID> SP_GET_PRODUCT_PRICE_BY_LISTID { get; set; } 
        public DbSet<SP_GET_PRODUCT_BY_STOREID> SP_GET_PRODUCT_BY_STOREID { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<VFi.NetDevPack.Domain.ValidationResult>(); 
            modelBuilder.Ignore<Event>();

            //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            //    property.SetColumnType("nvarchar(100)");

            modelBuilder.ApplyConfiguration(new Mappings.Configurations.CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.CategoryRootConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.DeliveryTimeConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.GroupCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.GroupUnitConfiguration());
            //modelBuilder.ApplyConfiguration(new Mappings.Configurations.LanguageConfiguration());
            //modelBuilder.ApplyConfiguration(new Mappings.Configurations.LocaleStringResourceConfiguration());
           // modelBuilder.ApplyConfiguration(new Mappings.Configurations.LocalizedPropertyConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ManufacturerConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductAttributeOptionConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductBrandConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductCategoryMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductGroupCategoryMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductInventoryConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductInventoryDetailConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductMediaConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductOriginConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductPackageConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductProductAttributeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductProductTagMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductReviewConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductReviewHelpfulnessConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductServiceAddConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductSpecificationAttributeMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductSpecificationCodeConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductStoreMappingConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTagConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTopicConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTopicDetailConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTopicPageConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTopicPageMapConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTopicQueryConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductVariantAttributeValueConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ProductWarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.RelatedProductConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ServiceAddConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ShippingMethodConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.SpecificationAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.SpecificationAttributeOptionConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.StoreConfiguration());
           // modelBuilder.ApplyConfiguration(new Mappings.Configurations.StoredEventConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.TaxCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.TierPriceConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.UnitConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.WarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new Mappings.Configurations.ServiceAddPriceSyntaxConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed

            var success = await SaveChangesAsync() > 0;

            return success;
        }

        [DbFunction("SplitString", "pim")]
        public IQueryable<SplitStringResult> SplitString(string List, string SplitOn)
        {
            return FromExpression(() => SplitString(List, SplitOn));
        }

        protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SplitStringResult>().HasNoKey();
        }

    }
    public static class DbContextExtensions
    {
        public static async Task<List<T>> SqlQueryAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default) where T : class
        {
            if (parameters is null)
            {
                parameters = new object[] { };
            }

            if (typeof(T).GetProperties().Any())
            {
                return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
            }
            else
            {
                await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
                return default;
            }
        }
    }

    
    public static class MediatorExtension
    {
        public static async Task PublishDomainEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
