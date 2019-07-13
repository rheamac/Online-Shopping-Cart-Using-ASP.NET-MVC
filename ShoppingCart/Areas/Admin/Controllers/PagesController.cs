using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewsModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare List of PageVM
            List<PagesVM> pageList;

            using(Db db = new Db())
            {
                // Initialize the list
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PagesVM(x)).ToList();
            }
            // return view with the list
            return View(pageList);
        }
    }
}