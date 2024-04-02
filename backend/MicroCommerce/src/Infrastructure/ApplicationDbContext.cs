using System.Reflection;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Buyer> Buyers { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartProductMap> CartProductMaps { get; set; } = null!;
    public DbSet<DeliveryOption> DeliveryOptions { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Promotion> Promotions { get; set; } = null!;
    public DbSet<Seller> Sellers { get; set; } = null!;
}