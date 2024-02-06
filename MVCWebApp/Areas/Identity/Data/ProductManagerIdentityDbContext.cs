using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Areas.Identity.Configuration;

namespace MVCWebApp.Areas.Identity.Data;

public class ProductManagerIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public ProductManagerIdentityDbContext(DbContextOptions<ProductManagerIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new RoleConfiguration());
    }
}
