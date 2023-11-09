using Microsoft.EntityFrameworkCore;

public class YourContext : DbContext
{
    public YourContext(DbContextOptions<YourContext> options) : base(options)
    {
      
    }

    public DbSet<User> Users { get; set; } // DbSet for the User entity
    public DbSet<Flight> Flights { get; set; } // DbSet for the Flight entity
    public DbSet<Booking> Bookings { get; set; } // DbSet for the Booking entity

   
}
