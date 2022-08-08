using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Database
{
    public class CBContext : DbContext, IDisposable //for better performance inherit the CBContext from 
                                                    // IDisposable so that the Context class object will be disposed once after its
                                                    // used or has accessed the Data from DB
    {
        //after creating DB context class and ConnectionString in the startup project
        //in package Manager Console type Enable-Migrations    then    Add-Migration anyName 
        // then Update-Database  after these the Database will be created in both ServerExplorer and Sql Server Object Explorer
        //if DB is not Created in server explorer but in Sql Server Object Explorer only you did some mistake..

        public CBContext() : base("ClothBazarConnection") {
        }

        //for details see DataValidation topic in BaseEntity.cs
        //  2.Fluent API i.e done by creating onModelCreation() method in Context class and in this we will define
        //    conditions/Attributes or relations for the fields or tables this mehtod willbe called while creaing Database
        //    so fluent API/DataAnnotation is for Validation along the Server side but for the validation on client
        //    side we will be either using JSCode/HTML

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(50);
        //}

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<myConfig> myConfigurations { get; set; }
    }
}
