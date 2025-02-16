using System.ComponentModel.DataAnnotations;

namespace CQRS.Domain.Entities
{
    public class Group
    {
        [Key]
        public required string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();
    }
}
