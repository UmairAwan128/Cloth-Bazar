using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
    //Data Validation: Validating data while reciving info from user to save it to DB e.g in Create Product form we should apply condition
    //  on Name, Image field as required and Description as optional if we don, t apply such conditions we will also be able to save
    //  a product which doesnot have a Name and Image i.e Empty Product so Validations are important and should be applied....
    //  We Can Apply Such Validations in Two Ways 
    //  1. By using DataAnnotations in Entity Classes i.e using [Required] on field u want as required and don,t write it for optional behaviour
    //     and[MinLength(5)] for defining the minimum lenght for a field   For all MVC DataAnnotations Search on google we refer the folling link
    //   https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6 
    //   one Important thing that these DataAnnotations that we write in Entity Classes will be like Validation for the Server side only so for
    //   DataValidations on Client side we may be using JavaScript code or C# for showing Error Messsage we should apply both 
    //   as on Client side or Browser we can also disable JavaScript so inthis case this JavaScript Validation will be useless
    //   but in this case Server side validation will be there so either way we will have one Validation..
    //  2.Fluent API i.e done by creating onModelCreation() method in Context class and in this we will define
    //    conditions/Attributes or relations for the fields or tables this mehtod willbe called while creaing Database
 
    //    so fluent API/DataAnnotation is for Validation along the Server side but for the validation on client
    //    side we will be either using JSCode/HTML
      
    public class BaseEntity
    {
        public int Id { get; set; }
        [Required]

        //this validation is only applied on server side so user can provide Name with length less then 5
        //or greater then 50 so at that time runtime error/exception occur i.e like kind of validation error 
        //which means that the Name you provided does not fullfill this requirment i.e its length is not according
        // to requirments given.so solution is to test the length of Name at client side and then pass the 
        //valid Name here so no error occurs e.g
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        //in MVC to Validate Email don,t need to write Code just use  [EmailAddress]
    }
}
