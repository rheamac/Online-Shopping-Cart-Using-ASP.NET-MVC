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
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PagesVM model)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //decalre slug
                string slug;

                //Init pageDTO
                PageDTO dto = new PageDTO();

                //DTO Tile
                dto.Title = model.Title;

                //check for and set slug of needed
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                } else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //make sure ttile and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Title and slug already exixts");
                    return View(model);
                }

                //DTO rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HashSideBar = model.HashSideBar;
                dto.Sorting = 100;
              
                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            //set tempdate message
            TempData["SM"] = "You have added new page";

            //Redirect
            return RedirectToAction("AddPage");

        }


        // GET :  Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // declare pageVm
            PagesVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //confirm page exists
                if(dto == null)
                {
                    return Content("Page does not exists");
                }

                //Initialize the pageVm model
                model = new PagesVM(dto);
            }

            // return view with model
            return View(model);
        }

        // POST :  Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PagesVM model)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //get page id
                int id = model.Id;

                //init slug
                string slug ="home";

                // get the page
                PageDTO dto = db.Pages.Find(id);

                //DTO Tile
                dto.Title = model.Title;

                //check for and set slug if needed
                if(model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
               

                //make sure ttile and slug are unique
                if(db.Pages.Where(x=> x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug) )
                {
                    ModelState.AddModelError("", "Title and slug already exixts");
                    return View(model);
                }

                //DTO rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HashSideBar = model.HashSideBar;

                //Save DTO
                db.SaveChanges();
            }
            //set tempdate message
            TempData["SM"] = "You have Edited page";

            //Redirect
            return RedirectToAction("EditPage");

        }


        // GET :  Admin/Pages/PageDetails/id
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            // view pageVM
            PagesVM model;
            
            using(Db db =new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);

                //confirm page exixt
                if(dto == null)
                {
                    return Content("page does not exist");
                }

                //init the pageVm
                model = new PagesVM(dto);
            }
            //return view of model
            return View(model);
        }


        // GET :  Admin/Pages/DeletePage/id
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);

                //remove the page
                db.Pages.Remove(dto);

                //save
                db.SaveChanges();
            }
            //redirect
            return RedirectToAction("Index");
        }

        // POST :  Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db()) {
                // set initial count
                int count = 1;

                //Declare page DTO
                PageDTO dto;

                //set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    // save
                    db.SaveChanges();

                    count++;
                }
            }
        }

        // GET :  Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            // declare model
            SidebarVM model;
            using(Db db = new Db())
            {
                //get dto
                SidebarDTO dto = db.Sidebar.Find(1);

                //init model
                model = new SidebarVM(dto);
            }


            //return view model
            return View(model);
        }

        // POST :  Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
          using(Db db = new Db())
            {
                //get the dto
                SidebarDTO dto = db.Sidebar.Find(1);

                //dto the body
                dto.Body = model.Body;

                //save
                db.SaveChanges();
            }

            //set template message
            TempData["SM"] = "you have edited the side bar";

            //redirect
            return RedirectToAction("EditSidebar");
        }
    }
}