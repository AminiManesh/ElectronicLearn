using ElectronicLearn.DataLayer.Entities.Course;
using ElectronicLearn.DataLayer.Entities.Order;
using ElectronicLearn.DataLayer.Entities.Permission;
using ElectronicLearn.DataLayer.Entities.User;
using ElectronicLearn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.DataLayer.Context
{
    public class ElectronicLearnContext : DbContext
    {
        public ElectronicLearnContext(DbContextOptions<ElectronicLearnContext> options) : base(options)
        {

        }

        #region User
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        #endregion

        #region Wallet
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        #endregion

        #region Permission
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        #region Course
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<UserCourse> UsersCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        #endregion

        #region Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        #endregion

        #region Discount
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<UserDiscount> UsersDiscounts { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Role>()
               .HasQueryFilter(r => !r.IsDeleted);

            modelBuilder.Entity<CourseGroup>()
               .HasQueryFilter(cg => !cg.IsDeleted);

            modelBuilder.Entity<Course>()
               .HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
