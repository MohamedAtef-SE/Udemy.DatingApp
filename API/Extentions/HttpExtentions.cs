using API.Helpers;
using API.Pagination;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;

namespace API.Extentions
{
    public static class HttpExtentions
    {
        public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data)
        {
            var paginationHeader = new PaginationHeader(data.CurrentPage, data.PageSize,
                                                        data.TotalCount,data.TotalPages);

            var jsonOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedPaginationHeader = JsonSerializer.Serialize(paginationHeader, jsonOptions);
            
            response.Headers.Append("Pagination", serializedPaginationHeader);
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
