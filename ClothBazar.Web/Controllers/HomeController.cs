using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeViewModel hvModel = new HomeViewModel();
            //Featured Categories/Products mean than out of all categories/Products we will be showing only some of the categories 
            //on the HomePage e.g 5 out of 60 so these categories will be in FeaturedCategories and also Admin will select some categories as featured
            hvModel.FeaturedCategories = CategoryService.Instance.GetFeaturedCategories();
            return View(hvModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}