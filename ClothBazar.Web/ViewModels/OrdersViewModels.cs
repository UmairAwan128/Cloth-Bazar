using ClothBazar.Entities;
using ClothBazar.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.ViewModels
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public string searchTerm { get; set; }
        public Pager pager { get; set; }
        public string Status { get; set; }
    }

    public class OrderDetailsViewModel
    {
        public List<string> allOrderStatus { get; set; }

        public Order Order { get; set; }
        public ApplicationUser OrderedBy { get; set; }
    }

}