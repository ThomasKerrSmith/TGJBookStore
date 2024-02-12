using Microsoft.EntityFrameworkCore;

namespace TGJBookStoreWithIdentity.Models
{
    public partial class TGJShopContext: DbContext 
    {
       public TGJShopContext()
       {
       } 

        public TGJShopContext(DbContextOptions<TGJShopContext> options)
            : base(options) 
        { 
        }

        public DbSet<Books> Books { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=citizen.manukautech.info,6304;Database=FSWDS2-Group2;UID=FSWDS2-Group1;PWD=fBit$28601;encrypt=true;trustservercertificate=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
