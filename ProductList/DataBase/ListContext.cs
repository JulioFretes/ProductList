
using ProductList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductList.DataBase
{
    public class ListContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Item> Item { get; set; }        

        public ListContext(DbContextOptions op) : base(op)
        {
        }
    }
}
