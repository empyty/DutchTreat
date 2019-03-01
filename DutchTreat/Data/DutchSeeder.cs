using System.Collections.Generic;
using System.IO;
using System.Linq;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext context;
        private readonly IHostingEnvironment hosting;

        public DutchSeeder(DutchContext context, IHostingEnvironment hosting)
        {
            this.context = context;
            this.hosting = hosting;
        }

        public void Seed()
        {
            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                var filepath = Path.Combine(hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                IEnumerable<Product> enumerable = products as Product[] ?? products.ToArray();
                context.Products.AddRange(enumerable);

                var order = context.Orders.FirstOrDefault(o => o.Id == 1);
                if (order != null)
                {
                    order.Items = new List<OrderItem>();
                    {
                        new OrderItem()
                        {
                            Product = enumerable.First(),
                            Quantity = 5,
                            UnitPrice = enumerable.First().Price
                        };
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
