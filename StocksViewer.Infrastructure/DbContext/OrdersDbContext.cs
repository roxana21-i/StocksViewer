using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdersDbContext :DbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            //Seed buy orders
            string buyordersJson = System.IO.File.ReadAllText("buyorders.json");
            List<BuyOrder> buyorders = System.Text.Json.JsonSerializer.Deserialize<List<BuyOrder>>(buyordersJson);

            foreach (BuyOrder buyorder in buyorders)
            {
                modelBuilder.Entity<BuyOrder>().HasData(buyorder);
            }

            //Seed sell orders
            string sellordersJson = System.IO.File.ReadAllText("sellorders.json");
            List<SellOrder> sellorders = System.Text.Json.JsonSerializer.Deserialize<List<SellOrder>>(sellordersJson);

            foreach (SellOrder sellorder in sellorders)
            {
                modelBuilder.Entity<SellOrder>().HasData(sellorder);
            }
        }
    }
}
