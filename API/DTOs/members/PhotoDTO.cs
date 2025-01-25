using API.Entities;

namespace API.DTOs.members
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string URL { get; set; } = null!;
        public bool IsMain { get; set; }
        //public string? PublicId { get; set; }
        //public int AppUserId { get; set; }
        //public string UserName { get; set; } = null!;
    }
}
