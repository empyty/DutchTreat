using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderId}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository,
            ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var order = _repository.GetOrderById(orderId);
            if (order != null)
            {
                return Ok(_mapper.Map <IEnumerable<OrderItem>, IEnumerable<OrderItemModel>>(order.Items));
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int orderId, int id)
        {
            var order = _repository.GetOrderById(orderId);
            var item = order?.Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                return Ok(_mapper.Map<OrderItem, OrderItemModel>(item));
            }
            return NotFound();
        }
    }
}
