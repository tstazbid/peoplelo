using Microsoft.AspNetCore.Identity;

namespace Peoplelo.Models
{
    public class Users: IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? LastVisit { get; set; }
        public required string Status { get; set; } = "Active";

    }
}
