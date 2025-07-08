using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderService.Controllers;
using OrderService.DTO;
using OrderService.Model;
using OrderService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Tests
{
    [TestClass]
    public sealed class OrderControllerTest : SharedTestBase
    {
        private OrdersController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var service = new OrderService.Services.OrderService(_OrderDbContext);
            _controller = new OrdersController(service);

            _OrderDbContext.Database.EnsureDeleted();
            _OrderDbContext.Database.EnsureCreated();
        }

        #region GetById Tests

        [TestMethod]
        [DataRow("Order_001")]
        public async Task GetByIdAsync_ValidId_ReturnsOk(string id)
        {
            await AddOrdersAsync();

            var result = await _controller.GetById(id);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult, "Expected OkObjectResult");
            var model = okResult.Value as OrderModel;

            Assert.IsNotNull(model);
            Assert.AreEqual(id, model.Id);
        }

        [TestMethod]
        public async Task GetByIdAsync_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.GetById(Guid.NewGuid().ToString());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        #endregion GetById Tests

        #region Create Tests

        [TestMethod]
        public async Task Create_ValidOrder_ReturnsOkAndPersists()
        {
            var newOrder = new OrderCreateRequest
            {
                CustomerId = "Customer_999",
                Items = new List<OrderItemDto>
        {
            new OrderItemDto
            {
                ProductId = "Product_001",
                ProductName = "Gaming Chair",
                UnitPrice = 199.99m,
                Quantity = 1
            },
            new OrderItemDto
            {
                ProductId = "Product_002",
                ProductName = "4K Monitor",
                UnitPrice = 299.50m,
                Quantity = 2
            }
        }
            };

            var result = await _controller.Create(newOrder);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var created = okResult.Value as OrderModel;
            Assert.IsNotNull(created);

            var inDb = await _OrderDbContext.Orders.FindAsync(created.Id);
            Assert.IsNotNull(inDb);
        }

        #endregion Create Tests

        #region Update Tests

        [TestMethod]
        public async Task Update_ValidChange_ReturnsNoContent()
        {
            await AddOrdersAsync();
            var toUpdate = await _OrderDbContext.Orders.FindAsync("Order_001");
            toUpdate.Status = OrderStatus.Delivered;

            var result = await _controller.UpdateStatus(toUpdate.Id, toUpdate.Status);

            var updated = await _OrderDbContext.Orders.FindAsync(toUpdate.Id);
            Assert.AreEqual(OrderStatus.Delivered, updated.Status);
        }

        [TestMethod]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var result = await _controller.UpdateStatus(Guid.NewGuid().ToString(), OrderStatus.Delivered);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        #endregion Update Tests

      

        [TestCleanup]
        public void Cleanup()
        {
            _controller = null!;
            _OrderDbContext.Dispose();
        }
    }
}