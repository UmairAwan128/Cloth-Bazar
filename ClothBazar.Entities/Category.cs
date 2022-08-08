using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    public class Category : BaseEntity
    {
        public string ImageURL { get; set; }

        public virtual List<Product> Products { get; set; }
        //Featured Categories/Products mean than out of all categories/Products we will be showing only some of the categories 
        //on the HomePage e.g 5 out of 60 so these categories will be here and  Admin will select some categories as featured
        public bool isFeatured { get; set; } //In Category Table its type will be bit which can be 0 or 1 as SQL don,t have boll type 
    }
}
