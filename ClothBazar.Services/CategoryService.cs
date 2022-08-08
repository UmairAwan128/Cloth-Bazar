using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class CategoryService
    {
        //Singleton Pattern is here
        #region Singleton
        private static CategoryService instance { get; set; }
        private CategoryService()
        {
        }
        public static CategoryService Instance {
            get {
                if (instance == null) {
                    instance = new CategoryService();
                }
                return instance;
            }
        }
        #endregion

        public List<Category> GetCategories()
        {
            using (var context = new CBContext())
            {
                return context.Categories.ToList();
            }
        }
        //Featured Categories/Products mean than out of all categories/Products we will be showing only some of the categories 
        //on the HomePage e.g 5 out of 60 so these categories will be here and  Admin will select some categories as featured
        public List<Category> GetFeaturedCategories()
        {

            using (var context = new CBContext())
            {
                //retrn the categoried which are featured
                //(x=>x.isFeatured == true) is same as (x=>x.isFeatured) also for checking false it can be (x=> !x.isFeatured)
                return context.Categories.Where(x=>x.isFeatured == true && x.ImageURL!=null).ToList();
                // return context.Categories.Where(x => x.isFeatured == true && x.ImageURL != null).Take(4).ToList(); //means take first 5 only
               //to get newly addded categories/product list 
               //return context.Categories.Where(x=>x.isFeatured == true && x.ImageURL!=null).OrderBy(x=>x.createdOn).ToList();
                
            }
        } 

        public Category GetCategory(int id)
        {

            using (var context = new CBContext())
            {
                return context.Categories.Where(x => x.Id == id).Include(x => x.Products).FirstOrDefault();
            }
        }
        public void SaveCategory(Category category) {

            using (var context = new CBContext()) {
                context.Entry(category).State = System.Data.Entity.EntityState.Unchanged;
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {

            using (var context = new CBContext())
            {
                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void DeleteCategory(int Id)
        {

            using (var context = new CBContext())
            {
                //we wrote here a Line to Find the category we can also call the function GetCategory( id)
                //but if we do that we will recieve an error like the Object not found which is due
                //to that we access the record from any table by first creating Context class object
                //then we can access records from table anywhere we can access that contextClass obj
                //but if someWhere that contextClass obj is not accessible we cannot access table 
                //same is here GetCategory(id) method is having a using{ } block within which the
                //object of ContextClass obj is destroyed which gave us the category so if that obj
                //is destroyed then this category will also become useless so we write here that Find() line
                //not to recieve any error..
                var category = context.Categories.Where(x => x.Id == Id).Include(x => x.Products).FirstOrDefault();  //first write using System.Data.Entity;
                //if we want to delete a category which has products so first remove all products in that
                //category then delete that category so this will delete all products connected to the specific category
                context.Products.RemoveRange(category.Products);
                //now deletion will work for both if category has products or not products
                context.Categories.Remove(category);
                //or this both are same
                //context.Entry(category).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
