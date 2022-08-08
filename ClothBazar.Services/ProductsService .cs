using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity; //is for Include(x=>x.Category)

namespace ClothBazar.Services
{
    public class ProductsService
    {
        //Singleton Pattern is here
        #region Singleton
        private static ProductsService instance { get; set; }
        private ProductsService()
        {
        }
        public static ProductsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductsService();
                }
                return instance;
            }

        }

        #endregion




        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int pageNo, int pageSize)
        {
            //as here we are recieving 3 kinds of parameters so we may be getting either one parameter or all 
            //but in case if we get more then one or all we will be filtering products according to all the
            // parameters but in case of all parameters recieved which parameter will be used first to filter
            //data  which will be used last as this will effect the performance also so order also matters so
            //filter the data by category then on that products apply search and then apply min and max price..
            using (var context = new CBContext())
            {
                var products = context.Products.ToList(); //first get all products also it will be like else case if no parameter passed 
                 if (categoryId.HasValue) //filter products by categoryId if passed
                {//and store back those filtered products to "products"
                    products = products.Where(x => x.Category.Id == categoryId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {//then filter products by searchTerm if passed
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                if (minimumPrice.HasValue) 
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue) {

                    switch (sortBy.Value) {
                        case 2: // sortby popularity 
                             //eiter sortby Likes count
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:  // is the case 1 i.e Default-Newest
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;

                    }

                }
                return products.Skip((pageNo - 1) * pageSize) //skip some records accrding to PageNo 
                       .Take(pageSize).ToList();   //take some records accrding to PageSize 
            }
        }

        public int SearchProductsCount(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int pageNo, int pageSize)
        {
            //as here we are recieving 3 kinds of parameters so we may be getting either one parameter or all 
            //but in case if we get more then one or all we will be filtering products according to all the
            // parameters but in case of all parameters recieved which parameter will be used first to filter
            //data  which will be used last as this will effect the performance also so order also matters so
            //filter the data by category then on that products apply search and then apply min and max price..
            using (var context = new CBContext())
            {
                var products = context.Products.ToList(); //first get all products also it will be like else case if no parameter passed 
                if (categoryId.HasValue) //filter products by categoryId if passed
                {//and store back those filtered products to "products"
                    products = products.Where(x => x.Category.Id == categoryId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {//then filter products by searchTerm if passed
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {

                    switch (sortBy.Value)
                    {
                        case 2: // sortby popularity 
                                //eiter sortby Likes count
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:  // is the case 1 i.e Default-Newest
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;

                    }

                }
                return products.Count();  
            }
        }

        //We were not getting category data from Product object i.e we want to show the categoryName for each product in the ProducTable.cshtml 
        //but we are getting the category= null there this is happening because while we are getting products from Product Table using 
        // GetProducts() method in ProductService class we are not telling it to also get all Category data of the
        // related product we tell this using    include("theNameOfTable") i.e Eager Loading for this also we need
        //  to write Virtual keyword with the Category attribute in Product Entity Class and for better performance
        // inherit the CBContext from IDisposable so that the Context class object will be disposed once after its
        // used or has accessed the Data from DB
        public List<Product> GetProducts()
        {
             
            using (var context = new CBContext())
            {
                return context.Products.Include(x=>x.Category).ToList();  //1. if this property name changed it will auto changed 
                //2. can't refer another object/table i.e category i.e we refer from product to category now category to its related table not possible  

                // or return context.Products.Include("Category").ToList();    //1. if property name changed but it will not change as hardcoded
                //2. we refered product to category now we can refer category to its related table also 
            }
        }


        public int GetMaximumPrice()
        {

            using (var context = new CBContext())
            {
                return (int)(context.Products.Max(x => x.Price));
            }
        }


        public List<Product> GetProducts(int pageNo)
        {
            int PageSize = 3;
            using (var context = new CBContext())
            {
                return context.Products.OrderBy(x => x.Id).Skip((pageNo - 1) * PageSize).Take(PageSize).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProducts(string search,int pageNo, int PageSize)
        {
            //no of records this method will return on any call
            using (var context = new CBContext())
            {

                if (string.IsNullOrEmpty(search) == false) //if some thing is passed in search then do this
                {   //if we have passed Search parameter means we are calling Action
                    //from Search button Click then take only records that are filtered according to pageSize and search string provided 
                    return context.Products.Where(p => p.Name != null &&  //for each Product of product table if p.Name!=null and the name of the product in LowerForm 
                    p.Name.ToLower().Contains(search.ToLower())).// contains search string passed in LowerForm then select all those products full filling these conditions  
                    OrderBy(x => x.Id).  //and order them by Id
                    Skip((pageNo - 1) * PageSize) //skip some records accrding to PageNo 
                    .Take(PageSize)   //take some records accrding to PageSize
                    .Include(x => x.Category).ToList(); //also incluse there related Category obj
                     //then return that List of Products 
                }
                else //if search is null then start taking all records
                {
                    //now accroding to this pageNo passed we need to get specific 10 records we can do this by using two methods Skip() and take()
                    // for first time or pageNo=1 skip 0 records and take first 10 when pageNo=2 is passed  skip first 10 and take
                    //next 10 records when pageNo=3 is passed  skip first 20 and take next 10 records  so we created a formula for Skip()..
                    //and Take() needs no formula as it will pick 10 allways 
                    return context.Products.OrderBy(x=>x.Id).Skip((pageNo-1)*PageSize).Take(PageSize).Include(x => x.Category).ToList();
                    //if we don,t use take() it will pick all the next records after skiping some.
                    //for using skip() the List/Records/Products should be sorted so used OrderBy(x=>x.Id) before it..
                }
            }
        }
        
        public List<Product> GetProducts(int pageNo, int PageSize)
        {
            using (var context = new CBContext())
            {
                //as these products will be shown  on the Home Page and on home page we may be displaying only
                //a few products not all e.g 8 products so if we use here orderBy allways first 8 products will 
                //be shown either we have added new Products or not but
                //here we us OrderByDescending so that every time z new product will be added it will be like 
                //showing first and also each time we will be seeing our home page with different products and 
                //new that are in trend...
                return context.Products.OrderByDescending(x => x.Id).Skip((pageNo - 1) * PageSize).Take(PageSize).Include(x => x.Category).ToList();
            }
        }


        public List<Product> GetProductsByCategory(int categoryId, int pageSize)
        {
            using (var context = new CBContext())
            {
                //as these products will be shown  on the Home Page and on home page we may be displaying only
                //a few products not all e.g 8 products so if we use here orderBy allways first 8 products will 
                //be shown either we have added new Products or not but
                //here we us OrderByDescending so that every time z new product will be added it will be like 
                //showing first and also each time we will be seeing our home page with different products and 
                //new that are in trend...
                return context.Products.OrderByDescending(x => x.Category.Id).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public int GetProductsCount()
        {
            using (var context = new CBContext())
            {
                    return context.Products.Count();
            }
        }

        public int GetProductsCount(string search)
        {
            using (var context = new CBContext())
            {
                if (string.IsNullOrEmpty(search) == false) //if search isnot null then return count of 
                {//only those records that matches the search term
                    return context.Products.Where(p => p.Name != null &&
                    p.Name.ToLower().Contains(search.ToLower())).Count();  
                }
                else //if search is null then return count of all records
                {
                    return context.Products.Count();
                }
            }
        }

        public int GetProductsCount(int CategoryId)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(x => x.Category.Id == CategoryId).Count();
            }
        }

        public Product GetProduct(int id)
        {

            using (var context = new CBContext())
            {
                return (from p in context.Products.Include(p => p.Category)
                             where p.Id == id select p).First();
            }
        }


        //this will be used by the checkout() action in shopcontroler this method will be passed 
        //list of product ids user want to buy and it will return their related products
        public List<Product> GetSpecificProducts(List<int> IDs)
        {
            using (var context = new CBContext())
            {
                //foreach (var id in IDs ) {
                //    Products.Add(GetProduct(id));
                //}
                //or 
                return context.Products.Where(product => IDs.Contains(product.Id)).ToList();
                //first from gotted all Products i.e "context.Products"  then from this list of products
                // put 1st "product" out and compare its Id with the list "IDs" if mataches then select this
                // "product" else move to next product and follow the same procedure at the end convert these
                // selected products to a list.
                // in case of   Where(product => IDs.Contains(product.Id)) first Where() contains a condition
                //inside "Where" we have Lamda expression  i.e we have a loop which select first "product" from
                //all products in table we know that lamda expressio is a Method so this "product" is passed to a 
                //Annonymousmethod(i.e product => IDs.Contains(product.Id)) this method again has a loop which first
                //get the Id of the product passed to this AnnoymousMethod i.e "(product.Id)" then inside the loop we compare this
                //Id with the List of IDs passed to this(GetProduct()) method if found then add this to list and so 1st iteration completes 
                //now 2nd iteration again select the next product from all Products from outer loop and again compare its id with IDs and so on...
            }
        }
        

        public List<Product> GetLatestProducts(int numberOfProducts)
        {
            //to get Latest we should be first having some prodperty in table e.g createdOn/Modified on and
            //on the basis of these we should be getting the latest products but here we are getting those by
            //first ordering the products Descending then select/Take the products as many as u want.. 

            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.Id).Take(numberOfProducts).Include(x => x.Category).ToList();
            }
        }

        
        public void SaveProduct(Product product) {

            using (var context = new CBContext()) {

                context.Entry(product).State = System.Data.Entity.EntityState.Unchanged;
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {

            using (var context = new CBContext())
            {
                //updating intentionally as not updating category of the product by its own   
                var productFromDB = context.Products.Where(x => x.Id == product.Id).FirstOrDefault();

                productFromDB.Name = product.Name;
                productFromDB.Description = product.Description;
                productFromDB.Price = product.Price;
                productFromDB.Category = context.Categories.Where(x => x.Id == product.Category.Id).Include(x => x.Products).FirstOrDefault();
                if (product.ImageURL != null)
                {
                    productFromDB.ImageURL = product.ImageURL;
                }
                context.Entry(productFromDB).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void DeleteProduct(int Id)
        {

            using (var context = new CBContext())
            {
                var product = context.Products.Find(Id);
                
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }







    }
}
