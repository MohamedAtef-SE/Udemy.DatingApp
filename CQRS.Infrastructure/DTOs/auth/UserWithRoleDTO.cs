using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Infrastructure.DTOs.auth
{
    public class UserWithRoleDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public List<string?> Roles { get; set; } = new List<string?>();
    }
}
