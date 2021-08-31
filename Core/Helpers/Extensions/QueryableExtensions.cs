using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Models;

namespace Helpers.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginatedResult<T> ToPaginatedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            long count = source.LongCount();
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
