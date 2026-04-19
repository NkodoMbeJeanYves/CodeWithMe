using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core.DataExtensions
{
    public static class LinQIQueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>(data, count, pageNumber, pageSize);
        }

        public class PagedResult<T>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
            public IEnumerable<T> Data { get; set; }

            public PagedResult(IEnumerable<T> data, int count, int pageNumber, int pageSize)
            {
                Data = data;
                TotalItems = count;
                PageNumber = pageNumber;
                PageSize = pageSize;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            }
        }

    }

}


