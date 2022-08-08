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
    public class OrdersController : Controller
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


        // GET: Orders
        public ActionResult Index(string searchTerm, string status, int? pageNo)
        {
            OrderViewModel model = new OrderViewModel();
            model.searchTerm = searchTerm;
            model.Status = status;
            pageNo = (pageNo.HasValue && pageNo.Value > 0) ? pageNo : 1; //here we are using conditionalOperator i.e if(pageNo Has a Value and its > 0){ then assign it that value} else{ i.e not passed so assign it 1 } 
            int pageSize = myConfigurationsService.Instance.PageSize(); //pagesize is defined in Db we are getting that value
            model.Orders = OrdersService.Instance.SearchOrders(searchTerm, status, pageNo.Value, pageSize);

            var totalRecords = OrdersService.Instance.SearchOrdersCount(searchTerm,status);
            
            if (model.Orders != null)
            {//if there we have any products in Db then do this

                model.pager = new Pager(totalRecords, pageNo, pageSize);
                return View(model);
            }
            else
            {
                return HttpNotFound();     //show built in error that no products found 
            }
            
        }

        public ActionResult Details(int Id)
        {
            OrderDetailsViewModel model = new OrderDetailsViewModel();
            model.Order = OrdersService.Instance.GetOrderById(Id);
            if (model.Order!=null) { 
                model.OrderedBy = UserManager.FindById(model.Order.UserId);
            }
            model.allOrderStatus = new List<string> { "pending","In Progress","Delivered" };
            return View(model);
           
        }

        public JsonResult ChangeStatus(string status,int Id)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { Success = OrdersService.Instance.UpdateOrderStatus(Id,status) };
            
            return result;

        }
    }
}