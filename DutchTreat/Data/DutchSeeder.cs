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
        private readonly DutchContext _context;
        private readonly IHostingEnvironment _hosting;

        public DutchSeeder(DutchContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
            {
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                IEnumerable<Product> enumerable = products as Product[] ?? products.ToArray();
                _context.Products.AddRange(enumerable);

                var order = _context.Orders.FirstOrDefault(o => o.Id == 1);
                if (order != null)
                {
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = enumerable.First(),
                            Quantity = 5,
                            UnitPrice = enumerable.First().Price
                        }
                    };
                }
                _context.SaveChanges();
            }
        }
    }
}
