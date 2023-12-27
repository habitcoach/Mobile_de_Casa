using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mobile_de_Casa.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile_de_Casa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Brand { get; set; }
        public DbSet<SubCategory> Mobile { get; set; }

        public DbSet<Products> Case { get; set; }
        public object Products { get; internal set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }


    }
}
