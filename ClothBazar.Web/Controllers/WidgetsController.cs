using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.Web.Controllers
{

    //as the HomePage and other has alot of common things so to create a widget first check out some thing
    // which is common in the same page or on other pages of the site in this case products their style and other
    //so we can create a widget for that e.g for products so we will not be writting extra html
    //but just call the widget(Action) and it will generate design(Products html) for us using the Partial View..
    public class WidgetsController : Controller
    {
        //this action will be returning either all products or products categories wise or latest
        public ActionResult Products(bool isLatestProducts, int? categoryId=0)//(if isLatest==true) passed return latest products
        {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();
             
            if (isLatestProducts) //that are added last
            {
                //4 passed so it will return the 4 products that are added last
                model.products = ProductsService.Instance.GetLatestProducts(4);
                model.isLatestProducts = isLatestProducts;
            }

            else if (categoryId.HasValue && categoryId.Value>0) //that are added last
            {
                model.products = ProductsService.Instance.GetProductsByCategory(categoryId.Value,4);
                model.isRelatedProducts = true;
            }
            else { //return products as required e.g 8 products and pageNo=1
                model.products = ProductsService.Instance.GetProducts(1,8); 
            }
            return PartialView(model);
        }
    }
}