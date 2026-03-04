using System.Collections.Generic;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Standard pagination envelope returned by all paginated REST API endpoints.
    /// Ensures a consistent contract for any consumer (web, mobile, B2B integrations).
    /// </summary>
    /// <typeparam name="T">The item type in the page.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>Items in the current page.</summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>Total number of records matching the query (across all pages).</summary>
        public int TotalCount { get; set; }

        /// <summary>1-based current page number.</summary>
        public int Page { get; set; }

        /// <summary>Number of items per page.</summary>
        public int PageSize { get; set; }

        /// <summary>Total number of pages: ceil(TotalCount / PageSize).</summary>
        public int TotalPages => PageSize > 0 ? (TotalCount + PageSize - 1) / PageSize : 0;

        /// <summary>True if a previous page exists.</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>True if a next page exists.</summary>
        public bool HasNextPage => Page < TotalPages;

        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items      = items;
            TotalCount = totalCount;
            Page       = page;
            PageSize   = pageSize;
        }
    }

    /// <summary>
    /// Standard API response envelope for non-paginated responses.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string CorrelationId { get; set; }

        public static ApiResponse<T> Ok(T data, string message = null, string correlationId = null)
            => new ApiResponse<T> { Success = true, Data = data, Message = message, CorrelationId = correlationId };

        public static ApiResponse<T> Fail(string message, string correlationId = null)
            => new ApiResponse<T> { Success = false, Message = message, CorrelationId = correlationId };
    }
}
