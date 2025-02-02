namespace API.Pagination
{
    public class UserQueryParams
    {
        private const int MAXPageSize = 50;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set =>  pageSize = (value > MAXPageSize) ? MAXPageSize : value;
            
        }

        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
        public int? MinAge { get; set; } = 18;
        public int? MaxAge { get; set; } = 100;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string OrderBy { get; set; } = "lastActive";

    }
}
