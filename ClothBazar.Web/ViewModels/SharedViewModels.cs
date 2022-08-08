using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    //the follwing file will have all the model class that is shared or may be used by more then
    //one Action/Controller e.g in case of listing/displaying the Products/Categories with Pagination
    //we will have some attributes that will be same in both of their Model class so we will be here creating a 
    //Model class some other Model class will be inheriting these...
    
    //this class is for the Pagination it has the complete logic for Google like pagination this Model
    //class object will be used by more then one Model Class
    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize = 10) //we have set the pageSize so no need to pass
        {//if we pass the pageSize then that value will be assigned otherwise this 10 will be assigned
            if (pageSize == 0) pageSize = 10;

            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}

