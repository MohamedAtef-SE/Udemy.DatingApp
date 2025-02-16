namespace CQRS.Infrastructure.Pagination
{
    public class PaginationParams
    {
        private const int MAXPageSize = 50;
        private int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MAXPageSize ? MAXPageSize : value;

        }
    }
}
