using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClothBazar.Web
{
    //1.When the user enters URL of this Website in the Browser like http://localhost:58429/Category/Create
    //first this URL/Request comesin this RouteConfig File
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //we created this new routes.MapRoute(){ } for understanding Coustomized URLHelper (its part2 and part1 is in categoriesTable.cshtml)
            //where  $("#searchBtn").click(function (){ on the  url:  @Url.Action("CategoriesTable", "Category"), }

            // @*URL Helper: (@URL("ActionName", "ControlerName")) Some Websites have Coustomized URL for aceesing various parts of Site
            // right now our default convention is "/ControlerName/ActionName/id" we can change them also this may be benificial in SEO
            //this convention is below defined  in  routes.MapRoute(){ }  whose  name: "Default" 
            //    url:  @Url.Action("CategoriesTable", "Category"),        //bothSame   //previously written      "/Category/CategoriesTable", 
            //and now as we can see to access CategoriesTable in browser we will write link as localhost/Category/CategoriesTable
            //but we want to access this View with another Name OR even Link means the Coustomized URL so for this we need to 
            //Map a new Route in here RouteConfig.cs but it should be before the Default Route as if this not valid then that will be accessed
            //routes.MapRoute(
            //    name: "AllCategories",     //name it any uniqueName 
            //    url: "Categories/all",     // this will be the URL for accessing the CategoriesTable view but additionally this URl /Category/CategoriesTable
            //                               // will also access the CategoriesTable as the      routes.MapRoute( name: "Default"...)  is still in this file
            //    defaults: new { controller = "Category", action = "CategoriesTable" }
            ////so now both these links http://localhost:58429/Category/categoriesTable and   http://localhost:58429/Categories/all
            ////will access the categoriesTable.cshtml View
            //);



            //2.then that URL is Matched with this pattern i.e  url: "{controller}/{action}/{id}"  means
            //that first will be the name of Controller(Category) then ActionName(Create()) then any parameter
            //in case of Post request
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //3.defaults mean that if in the URL user don,t give Action name i.e http://localhost:58429/Category
                //then in this case look for the Index Action and if found then open otherwise an error will be shown
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
