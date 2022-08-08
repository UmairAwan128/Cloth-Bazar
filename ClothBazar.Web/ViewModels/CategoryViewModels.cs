using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    public class CategoriesSearchViewModel
    {
        public List<Category> categories { get; set; }
        public string searchTerm { get; set; }
    }
}