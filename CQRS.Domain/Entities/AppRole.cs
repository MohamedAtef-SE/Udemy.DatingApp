using Microsoft.AspNetCore.Identity;

namespace CQRS.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = new HashSet<AppUserRole>();
    }
}
