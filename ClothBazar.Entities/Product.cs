using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
   public class Product : BaseEntity 
    {
        [Range(1,100000)]        //its DataValidation Topic Details in BaseEntity.cs
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        //this attribute will not create a seprate column in the Product but when we created Virtual Category property for refernceing
        //it was auto created here by EntityframeWork for tracking but as if we intentionally create this will be overwritten to the
        //auto created and also will be the Name Of the foreignKey column for category to Product.this approach can be used to minimize 
        //some burden on DB e.g get caetgoryId for the specific category or viceversa.
        //public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        // first Virtual means this class/table Category is referenced to this Product class/table so if we create contextClass obj 
        //i.e to access a table say Product table its related category table data will be also retrieved but if we are not doing
        //all this in the UsingBlock i.e   using(contextClass obj) { _______ } as using will dispose the reference to DB so if you want to 
        //do this in the using block additionally you need to use the   Include() i.e Eager loading to tell him that get category data also

    }
}
