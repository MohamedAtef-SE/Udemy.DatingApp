using CQRS.Infrastructure.Pagination;

namespace CQRS.Infrastructure.Params
{
    public class MessageParams : PaginationParams
    {
        public string? Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
