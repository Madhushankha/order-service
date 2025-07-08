using OrderService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Data;

namespace OrderService.Tests
{
    public class SharedTestBase
    {
        //protected readonly Mock<IConfigurationTests> _mockConfiguration;
        protected OrderDbContext _OrderDbContext;

        [TestInitialize]
        public void Setup()
        {
            // Create a unique in-memory database for each test
            var dbContextOptions = new DbContextOptionsBuilder<OrderDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
            _OrderDbContext = new OrderDbContext(dbContextOptions);
        }

        #region Public Methods

        public async Task AddOrdersAsync()
        {
            foreach (var Order in GetOrders())
            {
                _OrderDbContext.Orders.Add(Order);
            }
            await _OrderDbContext.SaveChangesAsync();
        }

        #endregion Public Methods

        #region Private Methods

        private List<OrderModel> GetOrders()
        {
            return new List<OrderModel>
    {
        new OrderModel
        {
            Id = "Order_001",
            CustomerId = "Customer_101",
            OrderDate = DateTime.UtcNow,
            TotalPrice = 150m,
            Status = OrderStatus.Pending,
            Items = new List<OrderItemModel>
            {
                new OrderItemModel
                {
                    Id = "Item_001",
                    OrderId = "Order_001",
                    ProductId = "Product_01",
                    ProductName = "Gamepad",
                    UnitPrice = 50m,
                    Quantity = 2
                },
                new OrderItemModel
                {
                    Id = "Item_002",
                    OrderId = "Order_001",
                    ProductId = "Product_02",
                    ProductName = "Gaming Mouse",
                    UnitPrice = 25m,
                    Quantity = 2
                }
            }
        },
        new OrderModel
        {
            Id = "Order_002",
            CustomerId = "Customer_102",
            OrderDate = DateTime.UtcNow.AddDays(-30),
            TotalPrice = 99.99m,
            Status = OrderStatus.Shipped,
            Items = new List<OrderItemModel>
            {
                new OrderItemModel
                {
                    Id = "Item_003",
                    OrderId = "Order_002",
                    ProductId = "Product_03",
                    ProductName = "Gaming Headset",
                    UnitPrice = 99.99m,
                    Quantity = 1
                }
            }
        },
        new OrderModel
        {
            Id = "Order_003",
            CustomerId = "Customer_103",
            OrderDate = DateTime.UtcNow.AddMonths(-1),
            TotalPrice = 120m,
            Status = OrderStatus.Delivered,
            Items = new List<OrderItemModel>
            {
                new OrderItemModel
                {
                    Id = "Item_004",
                    OrderId = "Order_003",
                    ProductId = "Product_04",
                    ProductName = "Mechanical Keyboard",
                    UnitPrice = 60m,
                    Quantity = 2
                }
            }
        }
    };
        }
    }

    #endregion Private Methods
}