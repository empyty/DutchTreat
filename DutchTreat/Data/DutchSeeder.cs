using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context,
            IHostingEnvironment hosting,
            UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("itsameluigi666@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Mario",
                    LastName = "Mario",
                    Email = "itsameluigi666@gmail.com",
                    UserName = "itsameluigi666@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

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
                    order.User = user;
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
