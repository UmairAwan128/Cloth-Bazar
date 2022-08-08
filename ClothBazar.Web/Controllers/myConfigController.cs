using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothBazar.Services;
using ClothBazar.Web.ViewModels;
using ClothBazar.Web.Models;
using ClothBazar.Entities;
using System.Diagnostics;

namespace ClothBazar.Web.Controllers
{
    public class myConfigController : Controller
    {
        // GET: myConfig
        public ActionResult Index()
        {
            List<myConfigModel> modelList = myConfigurationsService.Instance.GetAllConfiguration().ToModelList();
            return View(modelList);
        }

        [HttpGet]
        public ActionResult create() {
            return PartialView("~/Views/myConfig/_create.cshtml");
        }

        [HttpPost]
        public ActionResult create(FormCollection data)
        {
            myConfig c = new myConfig();
            try
            {
                c.Key = data["Key"];
                c.Value = data["Value"];
                myConfigurationsService.Instance.SaveConfiguration(c);                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string key)
        {
            myConfig config = myConfigurationsService.Instance.GetConfig(key);
            myConfigModel model = new myConfigModel { Key = config.Key, Value = config.Value };
            return PartialView(model);
        }


        [HttpPost]
        public ActionResult Edit(myConfigModel model)
        {
            myConfig c = new myConfig();
            try
            {
                c.Key = model.Key;
                c.Value = model.Value;
                myConfigurationsService.Instance.UpdateConfiguration(c);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                //Trace.WriteLine($"{c.Id},{c.Name}");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(string key)
        {
            myConfig config = myConfigurationsService.Instance.GetConfig(key);
            myConfigModel model = new myConfigModel { Key=config.Key , Value=config.Value};
            return PartialView("~/Views/myConfig/Delete.cshtml", model);
        }


        [HttpPost]
        public ActionResult Delete(myConfigModel model)
        {
            try
            {
                myConfigurationsService.Instance.DeleteConfiguration(model.Key);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now.ToString("F"));
                //Trace.WriteLine($"{c.Id},{c.Name}");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
                Trace.WriteLine("------------------------------");
                Trace.Flush();

            }
            return RedirectToAction("Index");
        }


    }
}