using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.ViewModel;
using Newtonsoft.Json;
using Services;

namespace Techflow.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        public ActionResult Index()
        {
            CategoryService objService = new CategoryService();
            TicketTypeModel objModel = new TicketTypeModel();
            List<TicketTypeModel> lstCat = new List<TicketTypeModel>();
            List<TicketTypeModel> lstACat = new List<TicketTypeModel>();
            List<TicketTypeModel> lstSubCat = new List<TicketTypeModel>();

            lstCat = objService.getCat();
            objModel.ListCat = new List<TicketTypeModel>();
            objModel.ListCat.AddRange(lstCat);

            string jsonData = JsonConvert.SerializeObject(lstCat);
            string result = jsonData.Replace("[", string.Empty).Replace("]", string.Empty);
            ViewBag.ListCat = result;

            lstSubCat = objService.getSubCat();
            objModel.ListSubCat = new List<TicketTypeModel>();
            objModel.ListSubCat.AddRange(lstSubCat);

            lstACat = objService.getActiveCat();
            objModel.ListActiveCat = new List<TicketTypeModel>();
            objModel.ListActiveCat.AddRange(lstACat);
            
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(TicketTypeModel model, string cmd)
        {
            CategoryService objService = new CategoryService();
            if (cmd == "Save Category")
            {
                model.IsActive = true;
                objService.Insert(model);
                TempData["Msg"] = "Category Successfully Added.";
            }
            else
            {
                model.Status = true;
                objService.InsertSubCat(model);
                TempData["Msg"] = "Sub Category Successfully Added.";
            }

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            CategoryService objService = new CategoryService();
            TicketTypeModel objModel = new TicketTypeModel();
            objModel = objService.getByID(id);           

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Edit(TicketTypeModel model)
        {
            CategoryService objService = new CategoryService();
            objService.Update(model);
            return RedirectToAction("Index");
        }
        public ActionResult EditSubCat(int id)
        {
            CategoryService objService = new CategoryService();
            TicketTypeModel objModel = new TicketTypeModel();
            objModel = objService.getByIDSubCat(id);

            List<TicketTypeModel> lstACat = new List<TicketTypeModel>();
            lstACat = objService.getActiveCat();
            objModel.ListActiveCat = new List<TicketTypeModel>();
            objModel.ListActiveCat.AddRange(lstACat);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult EditSubCat(TicketTypeModel model)
        {
            CategoryService objService = new CategoryService();
            objService.UpdateSubCat(model);
            return RedirectToAction("Index");
        }
    }
}
