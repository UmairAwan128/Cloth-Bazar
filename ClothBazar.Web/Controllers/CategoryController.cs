using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.Web.Controllers
{
    // [Authorize(Roles ="Admin,Manager")]
    //means that all actions in this controller are accessible to logined user only whose role is either 
    //Admin or manager if user is not logged in is not Admin or manager user will be redirected automatically
    //to login page and once loggedin he willl be redirected back to the this page.
    public class CategoryController : Controller
    {

        [HttpGet]
        //this will be the default Action for this controler default means that if in the URL user don,t
        //give Action name i.e http://localhost:58429/Category then in this case EntityFrameWork look for the
        //Index Action and if found then open otherwise an error will be shown for details check the
        //RouteConfig.cs in App_Start folder of this project
        public ActionResult Index()  //this action is for displaying the Categories
        {
            return View();
        }

        public ActionResult CategoriesTable(string search)
        {
            CategoriesSearchViewModel model = new CategoriesSearchViewModel();
            model.categories = CategoryService.Instance.GetCategories();
            int count = ProductsService.Instance.GetProductsCount(11);
             count = ProductsService.Instance.GetProductsCount(13);
            count = ProductsService.Instance.GetProductsCount(10);
            if (string.IsNullOrEmpty(search) == false)
            {
                model.categories = model.categories.Where(c => c.Name != null && c.Name.ToLower().Contains(search.ToLower())).ToList();
                model.searchTerm = search;
            }

            return PartialView(model);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                CategoryService.Instance.SaveCategory(category);
                return RedirectToAction("CategoriesTable");
            }
            else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpGet] //this will get the specific record we wnt to edit
        public ActionResult Edit(int id) 
        {
            var category = CategoryService.Instance.GetCategory(id);
            return PartialView(category);
        }
        [HttpPost]//this will update that specific record if the values are changed
        public ActionResult Edit(Category category)
        {
            CategoryService.Instance.UpdateCategory(category);
            return RedirectToAction("CategoriesTable");
        }


        public ActionResult Delete(int id)
        {
            CategoryService.Instance.DeleteCategory(id);
            return RedirectToAction("CategoriesTable");
        }


    }
}