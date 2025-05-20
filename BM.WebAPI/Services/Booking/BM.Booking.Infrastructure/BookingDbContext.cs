using BM.Booking.Domain;
using Microsoft.EntityFrameworkCore;

namespace BM.Booking.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        public DbSet<BookingAppointment> BookingAppointments { get; set; }
        public DbSet<BookingOrder> BookingOrders { get; set; }
        public DbSet<BookingReview> BookingReviews { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<BookingInvoice> BookingInvoices { get; set; }
        public DbSet<BookingServiceDetail> BookingServiceDetails { get; set; }
        public DbSet<BookingProduct> BookingProducts { get; set; }
        //public DbSet<BookingProductDescription> BookingProductDescriptions { get; set; }
        public DbSet<BookingProductDetail> BookingProductDetails { get; set; }
        public DbSet<BookingOrderProduct> BookingOrderProducts { get; set; }
        public DbSet<BookingCategory> BookingCategories { get; set; }
        public DbSet<BookingSupplier> BookingSuppliers { get; set; }
        public DbSet<BookingProductImage> BookingProductImages { get; set; }
        public DbSet<BookingSupplierProductDetail> BookingSupplierProductDetails { get; set; }
        public DbSet<BookingServiceDetailDescription> bookingServiceDetailDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BookingInvoice - BookingOrder (1-1)
            modelBuilder.Entity<BookingInvoice>()
                .HasOne(i => i.BookingOrder)
                .WithOne(o => o.BookingInvoice)
                .HasForeignKey<BookingInvoice>(i => i.orderID);

            // BookingAppointment - BookingServiceDetails (n-1)
            modelBuilder.Entity<BookingAppointment>()
                .HasOne(a => a.BookingServiceDetails)
                .WithMany(sd => sd.BookingAppointments)
                .HasForeignKey(a => a.serviceDetailID);

            // BookingOrder - BookingReview (1-1)
            modelBuilder.Entity<BookingReview>()
                .HasOne(r => r.BookingOrder)
                .WithOne(a => a.BookingReview)
                .HasForeignKey<BookingReview>(r => r.orderID);

            // BookingAppointment - BookingOrder (n-1)
            modelBuilder.Entity<BookingAppointment>()
                .HasOne(a => a.BookingOrder)
                .WithMany(o => o.BookingAppointments)
                .HasForeignKey(a => a.orderID);

            // BookingServiceDetail - BookingService (n-1)
            modelBuilder.Entity<BookingServiceDetail>()
                .HasOne(sd => sd.BookingService)
                .WithMany(s => s.BookingServiceDetails)
                .HasForeignKey(sd => sd.servID);

            // BookingServiceDetaial - BookingServiceDetailDescription (n-1)
            modelBuilder.Entity<BookingServiceDetailDescription>()
                .HasOne(sd => sd.BookingServiceDetails)
                .WithMany(s => s.BookingServiceDetailDescriptions)
                .HasForeignKey(sd => sd.serviceDetailID);

            // BookingProduct - BookingCategory (n-1)
            modelBuilder.Entity<BookingProduct>()
                .HasOne(p => p.BookingCategory)
                .WithMany(c => c.BookingProducts)
                .HasForeignKey(p => p.categoryID);

            // BookingProductDetail - BookingProduct (n-1)
            modelBuilder.Entity<BookingProductDetail>()
                .HasOne(pd => pd.BookingProduct)
                .WithMany(p => p.BookingProductDetails)
                .HasForeignKey(pd => pd.productID);

            // BookingProductDetail - BookingProductDescription (n-1)
            //modelBuilder.Entity<BookingProductDetail>()
            //    .HasOne(pd => pd.BookingProductDescription)
            //    .WithMany(d => d.BookingProductDetails)
            //    .HasForeignKey(pd => pd.productDescriptionID);

            //BookingProductImage-BookingProduct(n-1)
            modelBuilder.Entity<BookingProductImage>()
                .HasOne(pi => pi.BookingProduct)
                .WithMany(p => p.BookingProductImages)
                .HasForeignKey(pi => pi.productID);

            // BookingProductDetail - BookingSupplierProductDetail (1-n)
            modelBuilder.Entity<BookingSupplierProductDetail>()
                .HasOne(pd => pd.BookingProductDetail)
                .WithMany(s => s.BookingSupplierProductDetail)
                .HasForeignKey(pd => pd.supplierID);
            // BookingSupplierProductDetail - BookingSupplier (n-1)
            modelBuilder.Entity<BookingSupplierProductDetail>()
                .HasOne(sp => sp.BookingSupplier)
                .WithMany(s => s.BookingSupplierProductDetail)
                .HasForeignKey(sp => sp.supplierID);

            // BookingOrderProduct - BookingOrder (n-1)
            modelBuilder.Entity<BookingOrderProduct>()
                .HasOne(op => op.BookingOrder)
                .WithMany(o => o.BookingOrderProducts)
                .HasForeignKey(op => op.orderID);

            // BookingOrderProduct - BookingProductDetail (n-1)
            modelBuilder.Entity<BookingOrderProduct>()
                .HasOne(op => op.BookingProductDetail)
                .WithMany(pd => pd.BookingOrderProducts)
                .HasForeignKey(op => op.productDetailID);
        }
    }
}
