using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    //this file has all model classes related to Produts

    //we created this ViewModel as when we added Category dropdown in Create Product View and set its value to Id of the categpry as
    //we want to store category Id in table of Product for referencing but in case of Entity class Product it requires an category 
    //object reference inspite of Id so to resolve this problem we created this Model class as here we recieve CategoryId and all 
    //other attributes of Product class we will latter assign all these values to the Entity class and also category ref by getting
    //category by this Id..  
    public class NewProductViewModel
    {
        //   first read the first part of DataValidation topic in BaseEntity.cs 
        //   DataValidations on Client side we may be using JavaScript code or C# for showing Error Messsage we should apply both 
        //   as on Client side or Browser we can also disable JavaScript so inthis case this JavaScript Validation will be useless
        //   but in this case Server side validation will be there so either way we will have one Validation..
        //   SO for the Client/BrowserSide Validation we also write these same DataAnnotations what we write there in Entities also we write 
        //   them here in this ModelClass as this Model class object is recieved by Create() or on Creation of Product so 
        //   if(ModelState.isValid){ } written in Create() of Product becomes applicable..
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public int  CategoryID { get; set; } 

    }

    public class EditProductViewModel //this viewModel will be used for passing data to Edit.cshtml 
    {
        public Product product { get; set; }
        public List<Category> categories { get; set; } //all categories
    }

    public class EditedProductViewModel : NewProductViewModel
    {//this viewModel will be used for recieving data from Edit.cshtml
        public int Id { get; set; }//will have the ProductId to Edit
    }

    public class ProductSearchViewModel
    {
        public List<Product> products { get; set; }
        public string searchTerm { get; set; }

        //for simple pagination
        //public int PageNo { get; set; }

        public Pager pager { get; set; }
    }


    public class ProductViewModel
    {
        public Product product { get; set; }
    }


}