using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Models: are not same as the Entity classes as in Entity we have a list of attributes if in a view we want to show/send all the
//attributes of an entity or from View we want to sendback to Contler we will create an entity object in controler and pass that object
// to View or from View we will get data directly in entity class object ...
//but in case on the view we want to show only some of the attributes of Entity to the View or we want to show some selected
//attributes from Various entities e.g on HomePage we will be sending some of the Porduct Attributes some Categories and some other
//so we will create a Model class having all that attributes and create that Modelclass object and pass it to the View or We mayBe
//recieving attributes from View to Controler e.g Student Enrollment Form for the Semester in that for we will be writting studentName
//regNo other related info and also the list subjects that we are going to pick so as you can see here we get that this info is
//for two tables i.e student Personal info for StudentTable and subjects he picked this info is for SubjectTable so as two tables
//so in this case a single ModelClass will be created that will be having both these classes attributes we need and then we
//will now get the data from View in this ModelClass object and then from this Model class we will send data to the required Entities

namespace ClothBazar.Web.ViewModels
{
    //its a good practice to keep ViewModels in seperate folder these are like normal Model class as per we know
    public class HomeViewModel
    {
        //Featured Categories/Products mean than out of all categories/Products we will be showing only some of the categories 
        //on the HomePage e.g 5 out of 60 so these categories will be here and  Admin will select some categories as featured
        public List<Category> FeaturedCategories { get; set; }
        //also here we can now send two entity class objects to view at the same time
        public List<Product> FeaturedProducts { get; set; }
    }
}