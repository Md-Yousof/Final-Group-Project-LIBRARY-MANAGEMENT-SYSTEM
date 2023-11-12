using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI_R53_A.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<BookCopy> Copies { get; set; }
        //public DbSet<Fine> Fines { get; set; }
        public DbSet<BookFloor> BookFloors { get; set; }
        //public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Shelf> Shelfs { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        public DbSet<SubscriptionUser> SubscriptionUsers { get; set; }

        public DbSet<BookRack> BookRacks { get; set; }


        //public DbSet<ApplicationUser> UserInfos { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<BookWishlist> BookWishlists { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.AuthorId).IsRequired();
            });



            //Book Entity Relation
            modelBuilder.Entity<Book>().HasOne(p => p.Publisher).WithMany(b=>b.Books).HasForeignKey(p => p.PublisherId);
            modelBuilder.Entity<Book>().HasOne(p => p.Category).WithMany(b => b.Books).HasForeignKey(p => p.CategoryId);
            

            //Book Author 

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AuthorId });
                entity.HasOne(e => e.Book)
                 .WithMany(e => e.BookAuthor)
                 .HasForeignKey(e => e.BookId) ;

                entity.HasOne(e => e.Author)
                    .WithMany(e=>e.BookAuthor)
                    .HasForeignKey(e => e.AuthorId);
            });

            // Subscription User
            modelBuilder.Entity<SubscriptionUser>()
                .HasOne(e => e.ApplicationUser)
                 .WithMany(e => e.SubscriptionUsers)
                 .HasForeignKey(e => e.ApplicationUserId);


            modelBuilder.Entity<SubscriptionUser>()
                .HasOne(e => e.SubscriptionPlan)
                    .WithMany(e => e.SubscriptonUsers)
                    .HasForeignKey(e => e.SubscriptionPlanId);


            //BookCopy
            modelBuilder.Entity<BookCopy>().HasOne(p => p.Book).WithMany(b => b.Copies).HasForeignKey(p => p.BookId);
            modelBuilder.Entity<BookCopy>().HasOne(p => p.Shelf).WithMany(b => b.Copies).HasForeignKey(p => p.ShelfId);

            //Book Rack
            modelBuilder.Entity<BookRack>().HasOne(p => p.Shelf).WithMany(b => b.BookRacks).HasForeignKey(p => p.ShelfId);

            //Book Review
            modelBuilder.Entity<BookReview>().HasOne(p => p.Book).WithMany(b => b.BookReviews).HasForeignKey(p => p.BookId);
            modelBuilder.Entity<BookReview>().HasOne(p => p.UserInfo).WithMany(b => b.BookReviews).HasForeignKey(p => p.UserId);

            //BookWishlist
            modelBuilder.Entity<BookWishlist>().HasOne(p => p.UserInfo).WithMany(b => b.BookWishlists).HasForeignKey(p => p.UserId);
                modelBuilder.Entity<BookWishlist>().HasOne(p => p.Book).WithMany(b => b.BookWishlists).HasForeignKey(p => p.BookId);

            //BorrowedBook
            modelBuilder.Entity<BorrowedBook>().HasOne(p => p.UserInfo).WithMany(b => b.BorrowedBooks).HasForeignKey(p => p.UserId);
            modelBuilder.Entity<BorrowedBook>().HasOne(p => p.Book).WithMany(b => b.BorrowedBooks).HasForeignKey(p => p.BookId);
            modelBuilder.Entity<BorrowedBook>().HasOne(p => p.BookCopy).WithMany(b => b.BorrowBook).HasForeignKey(p => p.BookCopyId);

            
            ////Inspection
            //modelBuilder.Entity<Inspection>().HasOne(p => p.BookCopy).WithMany(b => b.Inspections).HasForeignKey(p => p.BookCopyId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Inspection>().HasOne(p => p.BorrowedBook).WithMany(b => b.Inspection).HasForeignKey(p => p.BorrowBookId);

            //Shelf
            modelBuilder.Entity<Shelf>().HasOne(p => p.BookFloor).WithMany(b => b.Shelves).HasForeignKey(p => p.BookFloorId);

            //Subcategory
            modelBuilder.Entity<Subcategory>().HasOne(p => p.Category).WithMany(b => b.Subcategories).HasForeignKey(p => p.CategoryId);


            //ApplicationUser
            //modelBuilder.Entity<ApplicationUser>().HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId).OnDelete(DeleteBehavior.Restrict);


            //UserPreference
            modelBuilder.Entity<UserPreference>().HasOne(p => p.UserInfo).WithMany(b => b.UserPreferences).HasForeignKey(p => p.UserInfoId);
            modelBuilder.Entity<UserPreference>().HasOne(p => p.Category).WithMany(b => b.UserPreferences).HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<UserPreference>().HasOne(p => p.Author).WithMany(b => b.UserPreferences).HasForeignKey(p => p.AuthorId);

            //Invoice
            modelBuilder.Entity<Invoice>().HasOne(i => i.BorrowedBook).WithMany().HasForeignKey(b => b.BorrowId);
            modelBuilder.Entity<Invoice>().HasOne(i => i.User).WithMany(i=>i.Invoices).HasForeignKey(b => b.UserId);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}