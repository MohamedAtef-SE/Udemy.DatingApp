using API.Pagination;

namespace API.Helpers
{
    public class UserQueryParams : PaginationParams
    {
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
        public int? MinAge { get; set; } = 18;
        public int? MaxAge { get; set; } = 100;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string OrderBy { get; set; } = "lastActive";

    }
}
