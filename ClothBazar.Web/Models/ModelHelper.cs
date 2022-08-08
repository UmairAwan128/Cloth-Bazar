using ClothBazar.Entities;
using ClothBazar.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClothBazar.Web.Models
{
    //this is a helper class it willhave some actions we will be using them to convert a specific
    //entity object/list to its related Model class as per required
    public static class ModelHelper
    {
        //following both are extension methods so these methods will be like added to the class
        //whose name is written after this in its parameter List so we can call them from that class directly
        public static List<myConfigModel> ToModelList(this List<myConfig> entityList)
        {
            List<myConfigModel> modelList = new List<myConfigModel>();

            foreach (var entity in entityList)
            {
                modelList.Add(entity.ToModel());
            }
            modelList.TrimExcess();
            return modelList;
        }

        public static myConfigModel ToModel(this myConfig entity)
        {
            return new myConfigModel { Key = entity.Key, Value = entity.Value};
        }


    }
}