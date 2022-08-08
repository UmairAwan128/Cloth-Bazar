using ClothBazar.Entities;
using ClothBazar.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    //this Model has only one attribute we can also send this list to View without creating
    //this model class but we added this list to Model as in future we may be sending also some
    //other additional info so created
    public class CheckoutViewModel
    {
        public List<Product> CartProducts { get; set; }

        //this will have the Id of all the proucts user have bought we passed it to View as
        //user may have bought a product in more then one quantity e.g 3 shirt so to show the quantity 
        //there we passed it and CartProduct will have only one shirt but user bought 3 so the products
        // will be in CartProduts and we will get there quantity using CartProductIDs
        public List<int> CartProductIDs{ get; set; }
        public ApplicationUser User { get; set; }

    }

    public class ShopViewModel {
        
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> Products { get; set; }
        //from the List of Products above that is for the specific page we will find out the most expensive product
        //that whose price is heighestt and that price will be placed here in maximumPrice also can do for 
        //minPrice but we let here that it will be 0.....
        public int MaximumPrice { get; set; }
        public int? categoryId { get; set; }
        public int? sortBy { get; set; }

        public string searchTerm { get; set; }
        public Pager myPager { get; set; }
    }

    //as will be used for partial view so only products are there in this.
    public class FilteredProductsViewModel
    {
        public List<Product> Products { get; set; }
        public Pager myPager { get; set; }
        public int MaximumPrice { get; set; }
        public int? categoryId { get; set; }
        public int? sortBy { get; set; }
        public int MinimumPrice { get; set; }
        public int pageNo { get; set; }
        public string searchTerm { get; set; }

    }
}