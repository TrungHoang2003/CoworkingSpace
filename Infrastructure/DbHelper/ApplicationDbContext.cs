using Domain.Entites;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbHelper;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
   IdentityDbContext<User, Role, int>(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

   }

   public DbSet<Space> Space { get; set; }
   public DbSet<Venue> Venue { get; set; }
   public DbSet<Holiday> Holiday { get; set; }
   public DbSet<VenueHoliday> VenueHoliday { get; set; }
   public DbSet<HolidayDate> HolidayDate { get; set; }
   public DbSet<Collection> Collection { get; set; }
   public DbSet<Amenity> Amenity { get; set; }
   public DbSet<VenueType> VenueType { get; set; }
   public DbSet<VenueAddress> VenueAddress { get; set; }
   public DbSet<GuestHour> GuestHour { get; set; }
}