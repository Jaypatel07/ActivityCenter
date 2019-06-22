using Microsoft.EntityFrameworkCore;

namespace Act.Models {
    public class Context : DbContext {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity1> Activities { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
    }
}