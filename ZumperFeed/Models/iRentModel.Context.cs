﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZumperFeed.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iRentEntities : DbContext
    {
        public iRentEntities()
            : base("name=iRentEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<property> properties { get; set; }
        public DbSet<unitada> unitadas { get; set; }
        public DbSet<unitfurnished> unitfurnisheds { get; set; }
        public DbSet<unitlaundry> unitlaundries { get; set; }
        public DbSet<unitparking> unitparkings { get; set; }
        public DbSet<unitpet> unitpets { get; set; }
        public DbSet<unitphoto> unitphotos { get; set; }
        public DbSet<unit> units { get; set; }
        public DbSet<unittype> unittypes { get; set; }
        public DbSet<userpropertymap> userpropertymaps { get; set; }
        public DbSet<user> users { get; set; }
    }
}
