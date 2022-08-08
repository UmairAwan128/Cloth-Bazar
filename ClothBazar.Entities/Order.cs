using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    public class Order
    {
        public int Id { get; set; }

        //UserId will have the Id of the user who ordered it is string as the User table is not created by
        //us and so byDefault and its PK i.e Id is of type string inspite of int so to reference to that table
        //we should also have the same type i.e string.
        public string UserId { get; set; }
        public DateTime OrderedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
