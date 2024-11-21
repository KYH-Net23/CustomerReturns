using ReturnProvider.Models.Entities;

namespace ReturnProvider.Context
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Sample Users
            var user1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var user2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // Sample Orders
            var order1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var order2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var order3Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

            // Seed Orders with Items
            var testOrders = new List<OrderEntity>
            {
                new OrderEntity
                {
                    OrderId = order1Id,
                    UserId = user1Id,
                    IsReturnable = true,
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity
                        {
                            ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                            Name = "Sample Product 1",
                            Price = 100.00m,
                            Quantity = 1
                        },
                        new OrderItemEntity
                        {
                            ItemId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                            Name = "Sample Product 2",
                            Price = 50.00m,
                            Quantity = 2
                        }
                    }
                },
                new OrderEntity
                {
                    OrderId = order2Id,
                    UserId = user1Id,
                    IsReturnable = true,
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity
                        {
                            ItemId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                            Name = "Sample Product 3",
                            Price = 75.00m,
                            Quantity = 1
                        }
                    }
                },
                new OrderEntity
                {
                    OrderId = order3Id,
                    UserId = user2Id,
                    IsReturnable = false, // Non-returnable order
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity
                        {
                            ItemId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                            Name = "Sample Product 4",
                            Price = 150.00m,
                            Quantity = 1
                        }
                    }
                }
            };

            // Seed Returns
            var testReturns = new List<ReturnEntity>
            {
                new ReturnEntity
                {
                    ReturnId = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                    OrderId = order1Id,
                    UserId = user1Id,
                    ReturnReason = "Damaged/Defective Item",
                    ResolutionType = "Refund",
                    Status = "Requested",
                    CreatedAt = DateTime.UtcNow
                },
                new ReturnEntity
                {
                    ReturnId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OrderId = order2Id,
                    UserId = user1Id,
                    ReturnReason = "Incorrect Order",
                    ResolutionType = "Exchange",
                    Status = "In Transit",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new ReturnEntity
                {
                    ReturnId = Guid.Parse("12121212-1212-1212-1212-121212121212"),
                    OrderId = order3Id,
                    UserId = user2Id,
                    ReturnReason = "Did Not Meet Expectations",
                    ResolutionType = "Refund",
                    Status = "Refunded",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            // Add data to the context
            context.Orders.AddRange(testOrders);
            context.Returns.AddRange(testReturns);

            // Save changes to the database
            context.SaveChanges();
        }
    }
}
