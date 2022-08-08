using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    public class ProductsWidgetViewModel
    {
        public List<Product> products { get; set; }
        public bool isLatestProducts { get; set; }
        public bool isRelatedProducts { get; set; }

    }
}