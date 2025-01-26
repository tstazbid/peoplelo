
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Peoplelo.Models;

namespace Peoplelo.Data
{
    public class UserDbContext : IdentityDbContext<Users>
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
