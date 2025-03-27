using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbHelper;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
   IdentityDbContext<User, IdentityRole<int>, int>(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      
      base.OnModelCreating(modelBuilder);
   }

   public DbSet<Space> WorkingSpaces { get; set; }
   public DbSet<Venue> Venues { get; set; }
   public DbSet<Collection> Collections { get; set; }
   public DbSet<Amenity> Amenities { get; set; }
   public DbSet<VenueType> VenueTypes { get; set; }
   public DbSet<VenueAddress> VenueAddresses { get; set; }
   
}