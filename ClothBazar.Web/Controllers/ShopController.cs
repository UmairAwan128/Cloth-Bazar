using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.Web.Controllers
{
    //this controller will be having Shop related Acions
    //[Authorize] //means that all actions in this controller are accessible to logined user only
    //if user is not logged in user will be redirected automatically to login page and once loggedin
    //he willl be redirected back to the this/shop page.
    public class ShopController : Controller
    {

        #region For Accessing User Data from DB first Copy this
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        //we need ApplicationUserManager ref to get User from Db by Id if we are using Builtin Login/Signup 
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion
        
        //all the parameters here are nullable as first Time when we access Shop Page From NavBar "Shop" click
        // we will not be passing any parameter so we will use the default approach i.e show the newest products
        // otherwise user will be passing either categoryId or Lowest/Highest price or SearchTerm but not all
        //so all nullable...
        public ActionResult Index(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int? pageNo) {
            var pageSize = myConfigurationsService.Instance.shopPageSize();
            ShopViewModel model = new ShopViewModel();
            model.FeaturedCategories = CategoryService.Instance.GetFeaturedCategories();
            model.MaximumPrice = ProductsService.Instance.GetMaximumPrice();

            pageNo = (pageNo.HasValue && pageNo.Value > 0) ? pageNo.Value : 1;
            int totalProducts = ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize);

            model.Products = ProductsService.Instance.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize); //5 is the page size
            model.searchTerm = searchTerm;
            model.sortBy = sortBy;
            model.categoryId = categoryId;    //for multiple filters at a time on page e.g show product whose price 100-500 and category = men
                                              //we get here the categoryId that is currently selected on page and then passed it back again there.

            model.myPager = new Pager(totalProducts, pageNo, pageSize);
            return View(model);
        }

        public ActionResult FilterProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int? pageNo)
        {
            var pageSize = myConfigurationsService.Instance.shopPageSize();
            FilteredProductsViewModel model = new FilteredProductsViewModel();
            pageNo = (pageNo.HasValue && pageNo.Value > 0) ? pageNo.Value : 1;
            model.searchTerm = searchTerm;
            model.sortBy = sortBy;
            model.categoryId = categoryId;    //for multiple filters at a time on page e.g show product whose price 100-500 and category = men
                                              //we get here the categoryId that is currently selected on page and then passed it back again there.
            model.MaximumPrice = ProductsService.Instance.GetMaximumPrice();
            int totalProducts = ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize);
            model.Products = ProductsService.Instance.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryId, sortBy, pageNo.Value, pageSize);
            model.myPager = new Pager(totalProducts, pageNo, pageSize);

            return PartialView(model);
        }



        // GET: Shop
        [Authorize]
        public ActionResult Checkout() //this is like the server side which recieve what products user want to buy
        {
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();

            //in MVC Request is like when user add URL in Browser to access Site or it specific Page
            //we are calling an action of Application so in this Action we can access cookies data of the Site as
           var cartProductsCookie = Request.Cookies["CartProducts"];
            //first check if the User has bought something, one thing that this "CartProducts" cookie will be created and show
            //in browser only when user will buy/AddToCart something so if the user has not bought something
            //so user will be not having this cookie and what we get here in cartProductCookie is null i.e notFound so
            if (cartProductsCookie != null && !string.IsNullOrEmpty(cartProductsCookie.Value)) {

                //we get all the productID's user wants to buy here on server in a string
                //   var ProductIDsString = cartProductsCookie.Value;
                //split these produtId's into array of string
                //   var idsArray = ProductIDsString.Split('-');
                //select each element of String Array using select and convert each to int and store in list of int
                //   List<int> ProductIDs = idsArray.Select(x => int.Parse(x)).ToList();
                //or in single line
                checkoutViewModel.CartProductIDs = cartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                //now get the products against these products using action in ProductServices
                //the method requires list of IDs and returns List of products against these IDs 
                //then we assigned this list to CartProducts List in CheckoutViewModel..
                checkoutViewModel.CartProducts = ProductsService.Instance.GetSpecificProducts( checkoutViewModel.CartProductIDs );
                //if user may have bought a product in more then one quantity e.g 3 shirts in this case in cartProducts 
                //we will get only one shirt so to show the quantity of a product on View we created "CartProductsID" 
                //in the checkoutViewModel and this will have the IDs of all the product user bought

                //getting User by Id from DB as using Builtin Register/Login system so first we included
                //some code for getting UserManager ref in the top of the class
                checkoutViewModel.User = UserManager.FindById(User.Identity.GetUserId());


            }
            return View(checkoutViewModel);
        }

        public JsonResult PlaceOrder(string OrderproductIds ) {

            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (!string.IsNullOrEmpty(OrderproductIds))
            {

                List<int> allProductsIdBought = OrderproductIds.Split('-').Select(x => int.Parse(x)).ToList();
                List<int> ProductsIdBought = allProductsIdBought.Distinct().ToList(); //for removing repetations 
                List<Product> boughtProducts = ProductsService.Instance.GetSpecificProducts(ProductsIdBought);

                Order newOrder = new Order();
                newOrder.UserId = User.Identity.GetUserId();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                // getting first item boughtProducts and then taking its price and multiplying by its price
                // getting quantity by comparing this Id with all Ids in the allProductsIdBought and then so 
                //price*quantity then for next product from broughtProughts                       
                newOrder.TotalAmount = boughtProducts.Sum(x => x.Price * allProductsIdBought.Where(productId => productId == x.Id).Count());
                newOrder.OrderItems = new List<OrderItem>();

                //here initializing OrderItems List in Order for this we are getting a Prouct from boughtProducts List 
                //and assigning its id to newly OrderItem objects property ProductId we didn,t initialized other
                //properties as they will be auto assigned like OrderId and Order object as we have initialzed
                //value from one side that is we initialized OrderItems List in the Order Entity here belo as newOrder.OrderItems
                // so no need to assign Order/OrderID in OrderItem Entity as it will be auto assigned by Entity Framework.
                newOrder.OrderItems.AddRange(boughtProducts.Select(x => new OrderItem() { ProductId = x.Id, Quantity = allProductsIdBought.Where(productId => productId == x.Id).Count() }));

                var rowsEffected = ShopService.Instance.SaveOrder(newOrder);

                result.Data = new { Success = true , Rows = rowsEffected };
            }
            else {
                result.Data = new { Success = false };
            }
            return result;
        }
    }
}