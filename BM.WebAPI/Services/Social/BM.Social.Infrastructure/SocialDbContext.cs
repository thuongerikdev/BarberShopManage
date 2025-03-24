using BM.Social.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.Infrastructure
{
    public class SocialDbContext : DbContext
    {
        public DbSet<SocialBlog> socialBlogs { get; set; }
        public DbSet<SocialMessage> socialMessages { get; set; }
        public DbSet<SocialSrc> socialSrcs { get; set; }
        public DbSet<SocialSrcBlog> socialSrcBlogs { get; set; }
        public DbSet<SocialSrcMessage> socialSrcMessages { get; set; }
        public DbSet<SocialBlogComment> socialBlogComments { get; set; }
        public DbSet<SocialGroup> socialGroups { get; set; }
        public DbSet<SocialGroupUser> socialGroupUsers { get; set; }
        public SocialDbContext(DbContextOptions<SocialDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SocialBlog>()
                .HasMany(b => b.SocialBlogComments)
                .WithOne(c => c.SocialBlog)
                .HasForeignKey(c => c.blogID); //(1-n)

            modelBuilder.Entity<SocialBlog>()
                .HasMany(b => b.SocialSrcBlogs)
                .WithOne(sb => sb.SocialBlog)
                .HasForeignKey(sb => sb.blogID); //(1-n)

            modelBuilder.Entity<SocialMessage>()
                .HasMany(m => m.SocialSrcMessages)
                .WithOne(sm => sm.SocialMessage)
                .HasForeignKey(sm => sm.messageID); //(1-n)

            modelBuilder.Entity<SocialSrc>()
                .HasMany(s => s.SocialSrcBlogs)
                .WithOne(sb => sb.SocialSrc)
                .HasForeignKey(sb => sb.srcID); //(1-n)

            modelBuilder.Entity<SocialSrc>()
                .HasMany(s => s.SocialSrcMessages)
                .WithOne(sm => sm.SocialSrc)
                .HasForeignKey(sm => sm.srcID); //(1-n)

            modelBuilder.Entity<SocialGroup>()
                .HasMany(g => g.SocialGroupUsers)
                .WithOne(gu => gu.SocialGroup)
                .HasForeignKey(gu => gu.groupID); //(1-n)
            modelBuilder.Entity<SocialGroup>()
                .HasMany(s => s.SocialMessages)
                .WithOne(s => s.SocialGroup)
                .HasPrincipalKey(s => s.groupID); //(1-n)
        }
    }
}
