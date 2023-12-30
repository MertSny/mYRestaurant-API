using Core.DataAccess.Abstract;
using Core.Entites;
using Core.Entites.Concrete;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Core.DataAccess.Concrete
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
  where TEntity : class, IEntity, new()
  where TContext : DbContext, new()
    {

        public void Add(TEntity entity)
        {
            using var context = new TContext();
            var added = context.Entry(entity);
            added.State = EntityState.Added;
            context.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            using var context = new TContext();
            await context.Set<TEntity>().AddAsync(entity);
            var i = await context.SaveChangesAsync();
            context.Entry(entity).State = EntityState.Detached;

            context.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Unchanged)
                .ToList()
                .ForEach(x => { x.State = EntityState.Detached; });
            context.Entry(entity).State = EntityState.Detached;
            return i;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            return filter == null
                ? context.Set<TEntity>().ToList()
                : context.Set<TEntity>().Where(filter).ToList();
        }

        public virtual async Task<int> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            using var context = new TContext();
            var query = context.Set<TEntity>()
                .AsQueryable()
                .AsNoTracking()
                .Where(predicate);

            var result = await query.CountAsync();

            return result;
        }

        //dt using blogu sonlanınca otomatikman dispose ediliyor.ToList() kullanmadiginiz hali bir IQueryable, icerik enumerate edilmeye baslanmadan once materyalize edilmemiş durumda, yani dt'ye ihtiyacı var. IEnumerable olarak döndürmek farketmiyor, çünkü IEnumerable zaten IQueryable'in üst sinifi(List'in de). Yani en üst sınıf olan object de döndürseniz, onun IQueryable olduğu gercegini ortadan kaldirmis olmuyorsunuz, o object hala IQueryable'a cast edilebilir durumda.

        public virtual async Task<IEnumerable<TEntity>> Search(bool eager = false, IEnumerable<string> includes = null)
        {
            using (var _dbContext = new TContext())
            {
                var query = QueryWithInclude<TEntity>(_dbContext, eager);

                if (includes != null)
                    foreach (var include in includes)
                        query = query.Include(include);

                var result = await Task.Run(() =>
                query.AsEnumerable().ToList()
                );
                return result;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate, bool eager = false, IEnumerable<string> includes = null)
        {

            using (var _dbContext = new TContext())
            {
                var query = QueryWithInclude<TEntity>(_dbContext, eager);

                if (predicate != null)
                    query = query.Where(predicate);

                if (includes != null)
                    foreach (var include in includes)
                        query = query.Include(include);

                var result = await Task.Run(() => query.AsEnumerable().ToList());

                return result;
            }

        }

        public virtual async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate, bool eager = false, IEnumerable<string> includes = null, Expression<Func<TEntity, object>> orderSelector = null, bool orderAsc = false)
        {

            using (var _dbContext = new TContext())
            {
                var query = QueryWithInclude<TEntity>(_dbContext, eager);

                if (predicate != null)
                    query = query.Where(predicate);

                if (includes != null)
                    foreach (var include in includes)
                        query = query.Include(include);

                if (null != orderSelector)
                    query = orderAsc ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);

                var result = await Task.Run(() => query.AsEnumerable().ToList());

                return result;
            }

        }

        public virtual async Task<PagedResult<TEntity>> Search(int page, int pageSize, Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, object>> orderSelector = null, bool orderAsc = false, bool eager = false, IEnumerable<string> includes = null)
        {
            using (var _dbContext = new TContext())
            {
                var query = QueryWithInclude<TEntity>(_dbContext, eager);

                if (includes != null && includes.Any())
                    foreach (var include in includes)
                        query = query.Include(include);

                if (null != predicate)
                    query = query.Where(predicate);

                if (null != orderSelector)
                    query = orderAsc ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);

                var result = await query.GetPagedAsync(page, pageSize);

                return result;
            }
        }

        public async Task<int> UpdateFieldsSave(TEntity entity, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using (var _dbContext = new TContext())
            {
                var dbEntry = _dbContext.Entry(entity);
                foreach (var includeProperty in includeProperties)
                {
                    dbEntry.Property(includeProperty).IsModified = true;
                }
                int i = await _dbContext.SaveChangesAsync();
                return i;
            }
        }

        public virtual async Task<int> UpdateAsync(TEntity entity, bool withCollections = true)
        {
            using var _dbContext = new TContext();
            var userId = entity.GetType().GetProperty(nameof(EntityMain.UpdatedBy))?.GetValue(entity);

            int id = (int)entity.GetType().GetProperty("Id")?.GetValue(entity, null);

            var dbEntity = await _dbContext.FindAsync<TEntity>(id); //Değiştirilmeden önceki kayıt çekiliyor
            var dbEntry = _dbContext.Entry(dbEntity);

            dbEntry.CurrentValues.SetValues(entity); //Yeni gelen değerler ile güncelleniyor

            if (withCollections)
            {
                var navigations = _dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations() //Json ignore işaretli olmayan collectionlar çekiliyor
              .Where(property =>
              typeof(TEntity).GetProperty(property.Name).GetCustomAttribute<JsonIgnoreAttribute>() == null &&
              property.IsCollection).AsEnumerable();

                //her navigation için yenisi varsa ekleniyor, var olan güncelleniyor, öncekine göre kaydı olmayan siliniyor
                foreach (var property in navigations)
                {
                    var propertyName = property.Name;
                    var dbItemsEntry = dbEntry.Collection(propertyName);
                    var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                    await dbItemsEntry.LoadAsync(); //Veritabanından önceki kaydı çekiliyor

                    var dbItemsMap = new Dictionary<int, object>();
                    foreach (var item in (System.Collections.IEnumerable)dbItemsEntry.CurrentValue) //her bir kayıt guid,object olarak dictionary'ye ekleniyor (dbItemsMap)
                    {
                        var keyName = _dbContext.Model.FindEntityType(item.GetType()).FindPrimaryKey().Properties.Select(x => x.Name).FirstOrDefault();
                        var key = (int)item.GetType().GetProperty(keyName)?.GetValue(item, null);

                        item.GetType().GetProperty(nameof(EntityMain.UpdatedDate))?.SetValue(item, DateTime.Now);
                        item.GetType().GetProperty(nameof(EntityMain.UpdatedBy))?.SetValue(item, userId);
                        dbItemsMap.Add(key, item);
                    }

                    var items = (IEnumerable<EntityMain>)accessor.GetOrCreate(entity, true); //Güncellenen kayıtlar Collection'a ekleniyor

                    foreach (var item in items) //Güncel kayıtlar dönülerek, önceki kayıtlarda aranıyor
                    {
                        var keyName = _dbContext.Model.FindEntityType(item.GetType()).FindPrimaryKey().Properties.Select(x => x.Name).Single();
                        var key = (int)item.GetType().GetProperty(keyName)?.GetValue(item, null);

                        if (!dbItemsMap.TryGetValue(key, out var oldItem)) //Güncel kayıtlar dbItemsMap'te yok ise entityState added olarak ekleniyor
                        {
                            item.GetType().GetProperty(nameof(EntityMain.CreatedDate))?.SetValue(item, DateTime.Now);
                            item.GetType().GetProperty(nameof(EntityMain.CreatedBy))?.SetValue(item, userId);
                            item.GetType().GetProperty(nameof(EntityMain.IsDeleted))?.SetValue(item, false);
                            _dbContext.Entry(item).State = EntityState.Added;

                            accessor.Add(dbEntity, item, true);
                        }
                        else  //Daha önceden var olan kayıtlar güncelleniyor
                        {
                            item.GetType().GetProperty(nameof(EntityMain.CreatedBy))?.SetValue(item, oldItem.GetType().GetProperty(nameof(EntityMain.CreatedBy)).GetValue(oldItem, null));
                            item.GetType().GetProperty(nameof(EntityMain.CreatedDate))?.SetValue(item, oldItem.GetType().GetProperty(nameof(EntityMain.CreatedDate)).GetValue(oldItem, null));
                            _dbContext.Entry(oldItem).CurrentValues.SetValues(item);
                            oldItem.GetType().GetProperty(nameof(EntityMain.UpdatedDate))?.SetValue(oldItem, DateTime.Now);
                            oldItem.GetType().GetProperty(nameof(EntityMain.UpdatedBy))?.SetValue(oldItem, userId);
                            dbItemsMap.Remove(key);
                        }
                    }

                    //foreach (var oldItem in dbItemsMap.Values) //güncelleme sonrası dictionary'de kalan kayıtlar siliniyor (dbItemsMap)
                    //    accessor.Remove(dbEntity, oldItem);

                    foreach (var oldItem in dbItemsMap.Values)//güncelleme sonrası dictionary'de kalan kayıtlar siliniyor (dbItemsMap)
                    {
                        oldItem.GetType().GetProperty(nameof(EntityMain.UpdatedDate))?.SetValue(oldItem, DateTime.Now);
                        oldItem.GetType().GetProperty(nameof(EntityMain.UpdatedBy))?.SetValue(oldItem, userId);
                        oldItem.GetType().GetProperty(nameof(EntityMain.IsDeleted))?.SetValue(oldItem, true);
                    }
                }
            }

            int i = await _dbContext.SaveChangesAsync();
            _dbContext.Entry(entity).State = EntityState.Detached;
            return i;
        }

        internal virtual IQueryable<TEntity> QueryWithInclude<T>(TContext _dbContext, bool eager = false)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable().AsNoTracking();
            if (eager)
                foreach (var property in _dbContext.Model.FindEntityType(typeof(T)).GetNavigations())
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(property.Name);
                    var jsonIgnore = propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>();
                    if (jsonIgnore == null)
                        query = query.Include(property.Name);
                }
            return query;
        }

    }
}
