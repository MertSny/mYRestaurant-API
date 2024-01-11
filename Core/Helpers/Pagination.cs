using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Helpers
{
    public static class Pagination
    {
        public static PagedResult<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> query, int page, int pageSize) where TEntity : class
        {
            var result = new PagedResult<TEntity>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip)
                .Take(pageSize)
                .AsEnumerable()
                .ToList();

            return result;
        }

        public async static Task<PagedResult<TEntity>> GetPagedAsync<TEntity>(this IQueryable<TEntity> query, int page, int pageSize) where TEntity : class
        {
            var result = new PagedResult<TEntity>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }
    }

    public static partial class CustomExtensions
    {
        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source, IEnumerable<string> navigationPropertyPaths)
            where TEntity : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        [Obsolete]
        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType)
        {
            var entityType = context.Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                var entityNavigations = new List<INavigation>();
                foreach (var navigation in entityType.GetNavigations())
                {
                    if (!navigation.Name.Contains("User"))
                        if (includedNavigations.Add(navigation))
                            entityNavigations.Add(navigation);
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        INavigation inverseNavigation = navigation.FindInverse();
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.GetTargetType();
            }
        }
    }

    public interface IPagedResultBase
    {
        int CurrentPage { get; set; }
        int FirstRowOnPage { get; }
        int LastRowOnPage { get; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        int RowCount { get; set; }
    }

    public abstract class PagedResultBase : IPagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }
        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }

    public interface IPagedResult<TEntity> where TEntity : class
    {
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        int RowCount { get; set; }
        IEnumerable<TEntity> Results { get; set; }
    }

    public class PagedResult<TEntity> : PagedResultBase, IPagedResult<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Results { get; set; }
        public PagedResult()
        {
            Results = new List<TEntity>();
        }
    }
}
