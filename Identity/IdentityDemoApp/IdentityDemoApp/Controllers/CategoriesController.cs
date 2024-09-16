using IdentityDemoApp.Data;
using IdentityDemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemoApp.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        // GET: CategoriesControllercs

        public readonly ApplicationDBContext dbContext;

        public CategoriesController(ApplicationDBContext _dbContext)
        {
            dbContext= _dbContext;  

        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            IEnumerable<Category> catList = dbContext.Categories;
            return View(catList);
        }

        // GET: CategoriesControllercs/Details/5

        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesControllercs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesControllercs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesControllercs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriesControllercs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesControllercs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriesControllercs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
