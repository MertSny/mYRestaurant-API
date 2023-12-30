using Core.Entites.Concrete;
using Core.Utulities.IOC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace DataAccess.Concrete
{
    public class DatabaseContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DatabaseContext()
        {
            _httpContextAccessor = (IHttpContextAccessor)ServiceTool.ServiceProvider.GetService(typeof(IHttpContextAccessor));
        }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            _httpContextAccessor = (IHttpContextAccessor)ServiceTool.ServiceProvider.GetService(typeof(IHttpContextAccessor));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=SQLServer\\Instance;Database=MyCourseDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // 1. Add the IsDeleted property
                entityType.FindProperty("IsDeleted");

                // 2. Create the query filter

                var parameter = Expression.Parameter(entityType.ClrType);

                // EF.Property<bool>(post, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

                // EF.Property<bool>(post, "DeleteFlag") == false
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                // post => EF.Property<bool>(post, "DeleteFlag") == false
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<PerformanceLog> PerformanceLogs { get; set; }
    }

}
