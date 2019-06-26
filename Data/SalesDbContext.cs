using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options)
            : base(options)
        {

            Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            

            var orders = new List<Order>();
            var states = new string[] { "Создан", "В работе", "На доставке", "Доставлен" };
            Random random = new Random();
            for (var i = 1; i < 100; i++)
            {
                orders.Add(new Order
                {
                    Id = i,
                    Client = "Иванов Иван",
                    Sum = random.Next(600, 5000),
                    Email = CreateEmail(),
                    Status = states.OrderBy(x => Guid.NewGuid()).Take(1).First()

                });
            }

            modelBuilder.Entity<Order>().HasData(orders);
        }

        private string CreateEmail()
        {
            var length = 6;
            string allowedChars = "abcdefhkmnprstABCDEFHKLMNOPRST123456789";
            char[] chars = new char[length];
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return $"{new string(chars)}@mail.ru";
        }
    }
}
