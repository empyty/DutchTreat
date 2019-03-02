using DutchTreat.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            logger.LogInformation("GetAllProducts was called");

            return context.Products
                .OrderBy(p => p.Title)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return context.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();
        }
    }
}
