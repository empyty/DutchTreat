using DutchTreat.Data.Entities;
using System.Collections.Generic;
using DutchTreat.Models;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        Order GetOrderById(string name, int id);
        void AddOrder(Order newOrder);

        bool SaveAll();
        void AddEntity(object model);
    }
}
