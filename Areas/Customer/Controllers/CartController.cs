using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mobile_de_Casa.Data;
using Mobile_de_Casa.Models.ViewModels;
using Mobile_de_Casa.Utility;

namespace Mobile_de_Casa.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public OrderDetailsCartViewModel orderDetailCartVM { get; set; }
        public CartController(ILogger<CartController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            orderDetailCartVM = new OrderDetailsCartViewModel()
            {
                orderMaster = new Models.OrderMaster()
            };

            orderDetailCartVM.orderMaster.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _context.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                orderDetailCartVM.listCart = cart.ToList();
            }

            foreach (var list in orderDetailCartVM.listCart)
            {
                list.Products = await _context.Case.FirstOrDefaultAsync(x => x.Id == list.ProductId);
                orderDetailCartVM.orderMaster.OrderTotal = orderDetailCartVM.orderMaster.OrderTotal + (list.Products.Price * list.Count);
                list.Products.Description = list.Products.Description;
                if (list.Products.Description.Length > 100)
                {
                    list.Products.Description = list.Products.Description.Substring(0, 99) + "...";
                }
            }
            orderDetailCartVM.orderMaster.OrderTotalOriginal = orderDetailCartVM.orderMaster.OrderTotal;
            return View(orderDetailCartVM);
        }
        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _context.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _context.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _context.ShoppingCart.Remove(cart);
                await _context.SaveChangesAsync();

                var cnt = _context.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(Utils.ShoppingCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _context.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);

            _context.ShoppingCart.Remove(cart);
            await _context.SaveChangesAsync();

            var cnt = _context.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(Utils.ShoppingCartCount, cnt);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Summary()
        {
            orderDetailCartVM = new OrderDetailsCartViewModel()
            {
                orderMaster = new Models.OrderMaster()
            };

            orderDetailCartVM.orderMaster.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _context.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                orderDetailCartVM.listCart = cart.ToList();
            }

            foreach (var list in orderDetailCartVM.listCart)
            {
                list.Products = await _context.Case.FirstOrDefaultAsync(x => x.Id == list.ProductId);
                orderDetailCartVM.orderMaster.OrderTotal = orderDetailCartVM.orderMaster.OrderTotal + (list.Products.Price * list.Count);
                list.Products.Description = list.Products.Description;
                if (list.Products.Description.Length > 100)
                {
                    list.Products.Description = list.Products.Description.Substring(0, 99) + "...";
                }
            }
            orderDetailCartVM.orderMaster.OrderTotalOriginal = orderDetailCartVM.orderMaster.OrderTotal;
            return View(orderDetailCartVM);

        }
    }
}
