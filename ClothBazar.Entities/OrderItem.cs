using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        //no need to write Order and OrderId here as we have wrote this in the Order Entity as List
        //also while initializing this object no need to pass OrderId and Order object as it will be 
        //auto done by Entity Framework.
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product product { get; set; }

        public int Quantity { get; set; }

    }
}
