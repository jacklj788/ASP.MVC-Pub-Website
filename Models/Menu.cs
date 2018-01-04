using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set;}
        // I spelt this wrong, and Local DB is a pain to rename.. so living with it. 
        public string Descritpion { get; set; }
    }
    // NuGet package Entity Framework 

    // Handles connecting to the database - Using a LocalDB Database that we can migrate to SQL Azure at a later date for publishing 
    public class MenuDBContext : DbContext
    {
        public DbSet<Menu> Menus { get; set;}
    }
}