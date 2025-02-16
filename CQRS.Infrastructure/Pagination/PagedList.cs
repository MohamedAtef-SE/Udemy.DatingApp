using Microsoft.EntityFrameworkCore;

namespace CQRS.Infrastructure.Pagination
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            // no need to Items.AddRange(items); you are already in class inherted from List you got
            // AddRange() already
            // this is body response 
            AddRange(items);
            // this properties for PaginationHeader respomse 'Pagination'
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        // No need to create new instence of List you already into derived class from List
        // just invoke AddRange() 
        //public List<T> Items { get; set; } = new List<T>();

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);

        }

    }
}