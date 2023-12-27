using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mobile_de_Casa.Data;
using Mobile_de_Casa.Models;
using Mobile_de_Casa.Utility;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Mobile_de_Casa.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/Products
        
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Case.Include(p => p.Category).Include(p => p.SubCategory);
            
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var Count = _context.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).Count();//how it is taking count
                HttpContext.Session.SetInt32(Utils.ShoppingCartCount, Count);
            }

           
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Case
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
        [Authorize(Roles = "Manager")]
        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Brand, "Id", "Name");
            ViewData["SubCategoryId"] = new SelectList(_context.Mobile, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,CategoryId,SubCategoryId")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();

                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var productFromDb = await _context.Case.FindAsync(products.Id);
                if (files.Count > 0)
                {
                    //storing image to the server
                    var uploads = Path.Combine(webRootPath, "images");
                    var extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, products.Id + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productFromDb.Image = @"\images\" + products.Id + extension;
                }
                else
                {
                    var uploads = Path.Combine(webRootPath, @"images\" + Utils.DefaultImage);
                    System.IO.File.Copy(uploads, webRootPath + @"\images\" + products.Id + ".png");
                    productFromDb.Image = @"\images\" + products.Id + ".png";
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Brand, "Id", "Name", products.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Mobile, "Id", "Name", products.SubCategoryId);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Case.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Brand, "Id", "Name", products.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Mobile, "Id", "Name", products.SubCategoryId);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,CategoryId,SubCategoryId")] Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Brand, "Id", "Name", products.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.Mobile, "Id", "Name", products.SubCategoryId);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Case
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Case.FindAsync(id);
            _context.Case.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Case.Any(e => e.Id == id);
        }
        //search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string SearchString)
        {
            
           
            if (!String.IsNullOrEmpty(SearchString))
            {

                
                var applicationDbContext = _context.Case.Include(p => p.Category).Include(p => p.SubCategory).Where(p => p.Name.Contains(SearchString));
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Case.Include(p => p.Category).Include(p => p.SubCategory);
                return View(await applicationDbContext.ToListAsync());
            }
            
            ViewBag.StrSearch = SearchString;
            return View();
        }
        //Filter
        [HttpPost]
        public async Task<IActionResult> FilterAsync(IFormCollection formCollection)
        {


            string result = formCollection["FilterR"].ToString();
            //string chkPriceValue = "";
            //string chkBestSellerValue = "";
            //if (!string.IsNullOrEmpty(formCollection["chkPrice"])) { chkPrice = true; }
            //if (!string.IsNullOrEmpty(formCollection["chkBestSeller"])) { chkBestSeller = true; }

            if (result == "Price")
            {
                var applicationDbContext = _context.Case.Include(p => p.Category).Include(p => p.SubCategory).OrderByDescending(p => p.Price);
                return View(await applicationDbContext.ToListAsync());


            }


            if (result == "BestSeller")
            {

                var applicationDbContext = _context.Case.Include(p => p.Category).Include(p => p.SubCategory).OrderBy(p => p.Price);
                return View(await applicationDbContext.ToListAsync());

            }


            return View();



        }

        // feedback
        public async Task<IActionResult> Feedback(int id)
        {
            Feedback feedbackForm = new Feedback();
            feedbackForm.ProductId = id;
            //  ViewBag.ProductId = id;
            return View(feedbackForm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback(Feedback feedback)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = await _context.ApplicationUser.Where(u => u.Id == claim.Value).FirstOrDefaultAsync();
            feedback.LoginUser = userId.Id;
            feedback.Id = 0;
            feedback.FeedbackDate = DateTime.UtcNow;
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> FeedbackManager(int id)
        {
            return View(await _context.Feedbacks.Where(p => p.ProductId.Equals(id)).ToListAsync());
        }

        //CartDetails
        public async Task<IActionResult> CartDetails(int id)
        {
            var productsFromDb = await _context.Case.Include(x => x.Category).Include(x => x.SubCategory).Where(x => x.Id == id).FirstOrDefaultAsync();
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Products = productsFromDb,
                ProductId = productsFromDb.Id
            };
            
            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CartDetails(ShoppingCart shoppingCart)// to change the count
        {
            shoppingCart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;
                //checking if user alrady is in the table and also counting the count
                ShoppingCart cartFromDb = await _context.ShoppingCart.Where(x => x.ApplicationUserId == shoppingCart.ApplicationUserId
                                          && x.ProductId == shoppingCart.ProductId).FirstOrDefaultAsync();
                if (cartFromDb == null)
                {
                    await _context.ShoppingCart.AddAsync(shoppingCart);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + shoppingCart.Count;
                }
                await _context.SaveChangesAsync();
                var count = _context.ShoppingCart.Where(x => x.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32(Utils.ShoppingCartCount, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productFromDb = await _context.Case.Include(x => x.Category).Include(x => x.SubCategory).Where(x => x.Id == shoppingCart.ProductId).FirstOrDefaultAsync();
                ShoppingCart sCart = new ShoppingCart()
                {
                    Products = productFromDb,
                    ProductId = productFromDb.Id
                };
                return View(sCart);
            }
        }



    }
}
