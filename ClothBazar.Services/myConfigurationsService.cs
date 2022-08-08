using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class myConfigurationsService
    {
        #region Singleton
        private static myConfigurationsService instance { get; set; }
        private myConfigurationsService()
        {
        }
        public static myConfigurationsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new myConfigurationsService();
                }
                return instance;
            }
        }
        #endregion


        public int PageSize()
        {
            using (var context = new CBContext())
            {
                var pageSizeConfig =  context.myConfigurations.Find("PageSize");
                //return pageSize from Database if its not null if null then return default value i.e 3
                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 3 ; 
            }
        }


        public int shopPageSize()
        {
            using (var context = new CBContext())
            {
                var pageSizeConfig = context.myConfigurations.Find("shopPageSize");
                //return pageSize from Database if its not null if null then return default value i.e 3
                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 4;
            }
        }

        public myConfig GetConfig(string key) {
            using ( var context = new CBContext()) {
                return context.myConfigurations.Find(key);
            }
        }


        public List<myConfig> GetAllConfiguration()
        {
            using (var context = new CBContext())
            {
                return context.myConfigurations.ToList();
            }
        }

        public void SaveConfiguration(myConfig config)
        {

            using (var context = new CBContext())
            {
                context.myConfigurations.Add(config);
                context.SaveChanges();
            }
        }

        public void UpdateConfiguration(myConfig config)
        {

            using (var context = new CBContext())
            {
                context.Entry(config).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void DeleteConfiguration(string key)
        {

            using (var context = new CBContext())
            {
                var config = context.myConfigurations.Find(key);
                context.myConfigurations.Remove(config);
                context.SaveChanges();
            }
        }
    }
}
