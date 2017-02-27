using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Services;

namespace Techflow.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {

            TicketModel objModel = new TicketModel();

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = Convert.ToInt32(model.CreatedBy);
            int cid = Convert.ToInt32(model.comp);

            lstTickets = objService.getTicketData(todt,frmdt, uid,cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);
            return View(objModel);
        }
        public ActionResult TicketLog()
        {
            TicketModel objModel = new TicketModel();

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            DashboardService objService = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult TicketLog(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = Convert.ToInt32(model.AssignTo);
            int cid = Convert.ToInt32(model.comp);

            lstTickets = objService.getTicketData(todt, frmdt, uid, cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            DashboardService objService1 = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService1.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);

            var lstOpen = lstTickets.Where(m => m.TicketStatusID == 1).ToList();
            var lstHold = lstTickets.Where(m => m.TicketStatusID == 3).ToList();

            var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == cid).SingleOrDefault();
                //lstTickets[0].CompDetails.Name;
            ViewBag.OnHold = lstHold.Count();
            ViewBag.Total = lstTickets.Count();
            //var lstTicketO = from e in lstOpen
            //                      select new
            //                      {                                      
            //                          label = e.CompDetails.Name,
            //                          y = lstOpen.Count(),
                                                            
            //                      };
            //var rowsO = lstTicketO.ToArray();
            //ViewBag.TicketOpen = JsonConvert.SerializeObject(rowsO);
            ViewBag.TicketOpen = lstOpen.Count();
            var lstResolved = lstTickets.Where(m => m.TicketStatusID == 2).ToList();
            
            //var lstTicketR = from e in lstResolved
            //                 select new
            //                 {
            //                     label = comp,
            //                     y = lstResolved.Count(),

            //                 };
            
            //var rowsR = lstTicketR;
            //ViewBag.TicketResolved = JsonConvert.SerializeObject(rowsR);
            ViewBag.TicketResolved = lstResolved.Count();
            ViewBag.Comp = comp.Name;
            return View(objModel);
        }
        public ActionResult UserLog()
        {
            UserModel objModel = new UserModel();
            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            DashboardService objService = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);
            return View(objModel);
        }
        [HttpPost]
        public ActionResult UserLog(UserModel model)
        {
            UserModel objModel = new UserModel();
            UserService objService = new UserService();
            List<UserModel> lstUserLog = new List<UserModel>();

            int uid = 0;
            int cid = 0;

            if (model.UID != null && model.UID != 0)
            {
                uid = model.UID;
            }
            else
            {
                uid = model.sid;
            }
            
            DateTime frmDt = Convert.ToDateTime(model.FromDate);
            DateTime toDt = Convert.ToDateTime(model.ToDate);
            lstUserLog = objService.getUserLog(uid, frmDt,toDt);
            objModel.ListUser = new List<UserModel>();
            objModel.ListUser.AddRange(lstUserLog);


            QuotationService objCompUSerService = new QuotationService();
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objCompUSerService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objCompUSerService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            DashboardService objService1 = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService1.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);

            return View(objModel);
        }
        public ActionResult StaffWise()
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();

            DashboardService objService1 = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService1.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult StaffWise(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = Convert.ToInt32(model.AssignTo);
            int cid = 0;

            lstTickets = objService.getTicketGroupByComp(todt, frmdt, uid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            DashboardService objService1 = new DashboardService();
            List<UserModel> objUserList1 = new List<UserModel>();
            objUserList1 = objService1.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList1);

            return View(objModel);
        }
        public ActionResult TotalStatusWise()
        {
            

            return View();
        }
        [HttpPost]
        public ActionResult TotalStatusWise(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = 0;
            int cid = 0;

            lstTickets = objService.getTicketData(todt, frmdt, uid, cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);


            var lstOpen = lstTickets.Where(m => m.TicketStatusID == 1).ToList();
            var lstHold = lstTickets.Where(m => m.TicketStatusID == 3).ToList();
            var lstResolved = lstTickets.Where(m => m.TicketStatusID == 2).ToList();
            //var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == cid).SingleOrDefault();
            var assign = lstTickets.Where(m => m.AssignTo != null).ToList();
            var unassign = lstTickets.Where(m => m.AssignTo == null).ToList();

            ViewBag.OnHold = lstHold.Count();
            ViewBag.Total = lstTickets.Count();
            ViewBag.TicketResolved = lstResolved.Count();
            ViewBag.TicketOpen = lstOpen.Count();
            ViewBag.Assign = assign.Count();
            ViewBag.UnAssign = unassign.Count();
        
            
            //ViewBag.Comp = comp.Name;
            return View(objModel);
        }
        public ActionResult ByType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ByType(TicketModel model)
        {
            ReportService objService = new ReportService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> lstTickets = new List<TicketModel>();
            DateTime todt = Convert.ToDateTime(model.ToDate);
            DateTime frmdt = Convert.ToDateTime(model.FromDate);
            int uid = 0;
            int cid = 0;

            lstTickets = objService.getTicketData(todt, frmdt, uid, cid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(lstTickets);

            var lstReq = lstTickets.Where(m => m.TicketTypeID == 1).ToList();
            var lstInc = lstTickets.Where(m => m.TicketTypeID == 2).ToList();
            var lstChg = lstTickets.Where(m => m.TicketTypeID == 3).ToList();
            //var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == cid).SingleOrDefault();
            var assign = lstTickets.Where(m => m.AssignTo != null).ToList();
            var unassign = lstTickets.Where(m => m.AssignTo == null).ToList();

            ViewBag.REQ = lstReq.Count();
            ViewBag.Total = lstTickets.Count();
            ViewBag.CHG = lstChg.Count();
            ViewBag.INC = lstInc.Count();
            ViewBag.Assign = assign.Count();
            ViewBag.UnAssign = unassign.Count();

            return View(objModel);
        }
    }
}
