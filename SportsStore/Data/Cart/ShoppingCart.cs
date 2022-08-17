using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext context { get; set; }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(AppDbContext context)
        {
            this.context = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Product product)
        {
            var shoppingCartItem = context.ShoppingCartItems.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Amount = 1
                };

                context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            context.SaveChanges();
        }

        public void RemoveItemFromCart(Product product)
        {
            var shoppingCartItem = context.ShoppingCartItems.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Product).ToList());
        }

        public decimal GetShoppingCartTotal() => context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Product.Price * n.Amount).Sum();

        public async Task ClearShoppingCartAsync()
        {
            var items = await context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            context.ShoppingCartItems.RemoveRange(items);
            await context.SaveChangesAsync();
        }
    }
}
