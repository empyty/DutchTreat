using DutchTreat.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.Models;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;

        public OrdersController(IDutchRepository repository,
            ILogger<OrdersController> logger,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(mapper.Map<IEnumerable<Order>, IEnumerable<OrderModel>>(repository.GetAllOrders()));
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = repository.GetOrderById(id);
                if (order != null)
                {
                    return Ok(mapper.Map<Order, OrderModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to get order: {e}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = mapper.Map<OrderModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    repository.AddEntity(model);
                    if (repository.SaveAll())
                    {
                        return Created($"/api/orders/{newOrder.Id}", mapper.Map<Order, OrderModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to save new order: {e}");
            }
            return BadRequest("Failed to save new order");
        }
    }
}
