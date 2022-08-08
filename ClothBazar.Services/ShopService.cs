using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class ShopService
    {
        #region Singleton
        private static ShopService instance { get; set; }
        private ShopService()
        {
        }
        public static ShopService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShopService();
                }
                return instance;
            }

        }

        #endregion

        public int SaveOrder(Order order)
        {

            using (var context = new CBContext())
            {

                //context.Entry(product).State = System.Data.Entity.EntityState.Unchanged;
                context.Orders.Add(order);
                return context.SaveChanges(); // context.SaveChanges(); tells how many rows effected in table
            }
        }

    }
}
