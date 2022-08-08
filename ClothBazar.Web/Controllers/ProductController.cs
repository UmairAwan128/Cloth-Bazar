using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//ctrl + m + o  is shortcut key to minimze the function blocks
//Edit -> Advanced -> Format Document  for auto format the code


namespace ClothBazar.Web.Controllers
{

    public class ProductController : Controller
    {
        
        // GET: Product
        public ActionResult Index()
        {
            //if while creating the View for this action we changed the name of the view then we
            //will be returning here that view i.e we will write that View name in return as   return View("name");
            return View(); //by default it will be returning the view which has the same name as Action Name..
        }

        //this action will return table/data of Product Table according to PageNo passed to it
        // Pagination: Let say we have created our project and we have added thousands of products in it now in the View of ProductTable
        //i.e when we call to show/get all the products it will take very long time to get all products and then show them in the table
        //also in this case the ProductTable will be very lengthy so the solution is pagination i.e so request to get only first few records
        //say first 10 so it will only get first 10 and show them we will do this as by telling the controler a pageNo and accroding to this pageNo
        //the next 10 records are differntiated i.e say we again request it to get the next 10 records so now it will skip first 10 and take
        //next 10 records and this time we will be passed pageNo=2 so we gotted that skip first 10 retrieve the next 10 record ..
        public ActionResult ProductTable(string search, int? pageNo) {  // we set pageNo as nullable so that we can also call this action without passing pageNo also 
            
            ProductSearchViewModel model = new ProductSearchViewModel();
            model.searchTerm = search;
            pageNo = (pageNo.HasValue && pageNo.Value>0) ? pageNo : 1; //here we are using conditionalOperator i.e if(pageNo Has a Value and its > 0){ then assign it that value} else{ i.e not passed so assign it 1 } 
                                                                       // pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo : 1 : 1;  //is same as above but it has nexted if so two seperate if and two else bot have 1 

            //now as the View(ProductTable.cshtml) will be calling this Action which has two buttons previous,Next page but how can he identify that currently we are on which page
            //so for this we will pass this pageNo to View by putting it in the Model class so we will assign the pageNo-1 value to Previous and pageNo+1 to Next Button so 

            //for simple pagination
            //model.PageNo = pageNo.Value;

            var totalRecords = ProductsService.Instance.GetProductsCount(search);

            int pageSize = myConfigurationsService.Instance.PageSize(); //pagesize is defined in Db we are getting that value

            model.products = ProductsService.Instance.GetProducts(search ,pageNo.Value,pageSize); //get the products from product table according to this pageNo 

            if (model.products != null)
            {//if there we have any products in Db then do this

                model.pager = new Pager(totalRecords, pageNo, pageSize);
                return PartialView(model);
            }
            else {
                return HttpNotFound();     //show built in error that no products found 
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            var categories = CategoryService.Instance.GetCategories();
            return PartialView(categories);
        }

        //we recieved the data in the ViewModel inpite of Entity class as when we added Category dropdown in Create Product View and
        //set its value to Id of the categpry as we want to store category Id in table of Product for referencing but in case of
        //Entity class Product it requires an category object reference inspite of Id so to resolve this problem we
        // created  ProductModel class as here we recieve CategoryId and all other attributes of Product class we will latter
        // assign all these values to the Entity class and also category ref by getting category by this Id..  
        [HttpPost]
        public ActionResult Create(NewProductViewModel productModel)
        {
            //   first read the first part of DataValidation topic in BaseEntity.cs 
            //   DataValidations on Client side we may be using JavaScript code or C# for showing Error Messsage we should apply both 
            //   as on Client side or Browser we can also disable JavaScript so inthis case this JavaScript Validation will be useless
            //   but in this case Server side validation will be there so either way we will have one Validation..
            //   SO for the Client/BrowserSide Validation we also write these same DataAnnotations what we write there in Entities also we write 
            //   them in the ModelClass(NewProductViewModel) as this Model class object is recieved by Create() or on Creation of Product so now 
            //   if(ModelState.isValid){ } written in Create() of Product becomes applicable and now it will only add a valid Product to DB
            if (ModelState.IsValid) {
                var product = new Product();
                product.Name = productModel.Name;
                product.Description = productModel.Description;
                product.Price = productModel.Price;
                product.Category = CategoryService.Instance.GetCategory(productModel.CategoryID);

                if (productModel.ImageURL != null) { 
                product.ImageURL = productModel.ImageURL;
                }
                ProductsService.Instance.SaveProduct(product);
                return RedirectToAction("ProductTable");
            }
            else {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            EditProductViewModel editModel = new EditProductViewModel();
            editModel.product = ProductsService.Instance.GetProduct(Id);
            editModel.categories = CategoryService.Instance.GetCategories();
            return PartialView(editModel);
        }

        [HttpPost]
        public ActionResult Edit(EditedProductViewModel productModel)
        {
            var product = new Product();
            product.Id = productModel.Id;
            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Price = productModel.Price;
            product.Category = CategoryService.Instance.GetCategory(productModel.CategoryID);
            if (productModel.ImageURL != null) { 
                product.ImageURL = productModel.ImageURL;
             }
            ProductsService.Instance.UpdateProduct(product);
            return RedirectToAction("ProductTable");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ProductsService.Instance.DeleteProduct(id);
            return RedirectToAction("ProductTable");
        }


        [HttpGet]
        public ActionResult Details(int Id)
        {
            ProductViewModel Model = new ProductViewModel();
            Model.product = ProductsService.Instance.GetProduct(Id);
            return View(Model);
        }

    }
}
