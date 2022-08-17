using Microsoft.AspNetCore.Mvc;
using SportsStore.Data.Cart;

namespace SportsStore.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart shoppingCart;
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            this.shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = shoppingCart.GetShoppingCartItems();

            return View(items.Count);
        }
    }
}
