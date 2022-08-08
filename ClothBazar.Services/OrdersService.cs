using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity; //for using Include in LINQ query

namespace ClothBazar.Services
{
    public class OrdersService
    {
        #region Singleton
        private static OrdersService instance { get; set; }
        private OrdersService()
        {
        }
        public static OrdersService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrdersService();
                }
                return instance;
            }

        }

        #endregion

        //why searchTerm? User calls us to know status of its order we say him to tell UserId and then search by UserId and tell him 
        //searchTerm will be used for searching order of specifc user by email/UserId we are assuming that the email
        //will be unique for each user and will be enterd by user while registraion. we can also use any other thing like orderId,eamil.
        //but we will get UserId from user and search by UserId only
        public List<Order> SearchOrders(string searchTerm, string status, int pageNo, int pageSize)
        {
            //as here we are recieving 3 kinds of parameters so we may be getting either one parameter or all 
            //but in case if we get more then one or all we will be filtering products according to all the
            // parameters but in case of all parameters recieved which parameter will be used first to filter
            //data  which will be used last as this will effect the performance also so order also matters so
            //filter the data by category then on that products apply search and then apply min and max price..
            using (var context = new CBContext())
            {
                var orders = context.Orders.ToList(); //first get all products also it will be like else case if no parameter passed 
             
                if (!string.IsNullOrEmpty(searchTerm))
                {//then filter products by searchTerm if passed
                    orders = orders.Where(x => x.UserId.ToLower().Contains(searchTerm.ToLower())).ToList();
                }


                if (!string.IsNullOrEmpty(status))
                {//then filter products by searchTerm if passed
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Skip((pageNo - 1) * pageSize) //skip some records accrding to PageNo 
                       .Take(pageSize).ToList();   //take some records accrding to PageSize 
            
            }
            
        }



        public int SearchOrdersCount(string searchTerm, string status)
        {
            using (var context = new CBContext())
            {
                var orders = context.Orders.ToList(); //first get all products also it will be like else case if no parameter passed 

                if (!string.IsNullOrEmpty(searchTerm))
                {//then filter products by searchTerm if passed
                    orders = orders.Where(x => x.UserId.ToLower().Contains(searchTerm.ToLower())).ToList();
                }


                if (!string.IsNullOrEmpty(status))
                {//then filter products by searchTerm if passed
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count();
            }

        }

        public Order GetOrderById(int id)
        {
            using (var context = new CBContext())
            {
                //here Include used two times i.e we wanted to include first OrderItems from the Orders Entity
                //then we Included "product" from the OrderItems Entity to do nested implementation  we cannot
                //directly access these nested entity so wrote string for this as Include("OrderItems.product") if 
                //additionally we want to include also "Order" from OrderItems Entity then again write include
                //after this as Include("OrderItems.Order") and also nested so.
                return context.Orders.Where(x=>x.Id == id).Include(x=>x.OrderItems).Include("OrderItems.product").FirstOrDefault();
            }

        }


        public bool UpdateOrderStatus(int id, string status)
        {
            using (var context = new CBContext())
            {
                var order = context.Orders.Find(id);
                order.Status = status;
                context.Entry(order).State = EntityState.Modified;
                //context.SaveChanges() returns no of rows affected in DB so if greater then 0 return true
                return context.SaveChanges() > 0;
            }

        }

    }
}
