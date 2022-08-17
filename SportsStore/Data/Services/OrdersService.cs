using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext context;
        public OrdersService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await this.context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Product).Include(n => n.User).ToListAsync();

            if (userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    ProductId = item.Product.Id,
                    OrderId = order.Id,
                    Price = item.Product.Price
                };
                await context.OrderItems.AddAsync(orderItem);
            }
            await context.SaveChangesAsync();
        }
    }
}
