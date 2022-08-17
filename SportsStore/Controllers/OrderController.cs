using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data.Cart;
using SportsStore.Data.Services;
using SportsStore.Data.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductService productService;
        private readonly ShoppingCart shoppingCart;
        private readonly IOrdersService ordersService;

        public OrderController(IProductService productService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            this.productService = productService;
            this.shoppingCart = shoppingCart;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await productService.GetProductByIdAsync(id);

            if (item != null)
            {
                shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await productService.GetProductByIdAsync(id);

            if (item != null)
            {
                shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        [HttpPost]
        public void SendEmail(string userEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    var items = shoppingCart.GetShoppingCartItems();

                    PdfPTable table = new PdfPTable(3);

                    foreach (var item in items)
                    {
                        PdfPCell cell1 = new PdfPCell(new Phrase(item.Product.Name));
                        PdfPCell cell2 = new PdfPCell(new Phrase(item.Product.Price.ToString()));
                        PdfPCell cell3 = new PdfPCell(new Phrase(item.Amount.ToString()));

                        table.AddCell(cell1);
                        table.AddCell(cell2);
                        table.AddCell(cell3);
                    }

                    document.Add(table);

                    Paragraph par = new Paragraph("Total: " + shoppingCart.GetShoppingCartTotal().ToString());
                    document.Add(par);

                    document.Close();
                    byte[] bytes = ms.ToArray();
                    ms.Close();


                    mail.From = new MailAddress("sportsstore2022@gmail.com");
                    mail.To.Add(userEmail);
                    mail.Subject = "Сheque";
                    mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "ChequePDF.pdf"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("sportsstore2022@gmail.com", "Qwert123.");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);
            SendEmail(userEmailAddress);

            await ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");

        }
    }
}
