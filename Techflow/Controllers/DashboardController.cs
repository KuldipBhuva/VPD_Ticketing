using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            DashboardService objService = new DashboardService();
            TicketModel objModel = new TicketModel();
            List<TicketModel> objTList = new List<TicketModel>();

            int uid = 0;
            int rid = 0;
            int cid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            if (Session["CompID"] != null)
            {
                cid = Convert.ToInt32(Session["CompID"].ToString());
            }
            objTList = objService.getAllTicket(cid, uid, rid);
            objModel.ListTicket = new List<TicketModel>();
            objModel.ListTicket.AddRange(objTList);

            TicketService objServiceTS = new TicketService();
            List<TicketStatusModel> objTStatus = new List<TicketStatusModel>();
            objTStatus = objServiceTS.getTicketStatus();
            objModel.ListTstatus = new List<TicketStatusModel>();
            objModel.ListTstatus.AddRange(objTStatus);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objService.getActiveStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objUserList);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(TicketModel model)
        {
            try
            {
                //int status = Convert.ToInt32(All["ddlStatus"]);
                //int tid = Convert.ToInt32(All["TicketID"]);
                int uid = 0;
                int rid = 0;
                if (Session["UID"] != null)
                {
                    uid = Convert.ToInt32(Session["UID"].ToString());
                    rid = Convert.ToInt32(Session["Role"].ToString());
                }

                TicketMaster tm = Dbcontext.TicketMasters.Where(m => m.TicketID == model.TicketID).SingleOrDefault();
                if (model.TicketStatusID != null)
                {
                    var status = Dbcontext.TicketMasters.Where(m => m.TicketID == model.TicketID && m.TicketStatusID == model.TicketStatusID).SingleOrDefault();
                    if (status == null)
                    {
                        try
                        {

                            var cby = Dbcontext.UserMasters.Where(m => m.UID == tm.CreatedBy).SingleOrDefault();
                            var user = Dbcontext.UserMasters.Where(m => m.CompID == @cby.CompID && m.Role == 2).ToList();
                            int adminID = 0;
                            if (Session["UID"] != null)
                            {
                                adminID = Convert.ToInt32(Session["UID"].ToString());
                            }
                            var admin = Dbcontext.UserMasters.Where(m => m.UID == adminID).SingleOrDefault();
                            string priority = "";
                            if (model.Priority == 1)
                            {
                                priority = "High";
                            }
                            else if (model.Priority == 2)
                            {
                                priority = "Medium";
                            }
                            else
                            {
                                priority = "Normal";
                            }
                            var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == cby.CompID).SingleOrDefault();
                            var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", "Newtech Ticketing Soultion");
                            var toAddress = new MailAddress(@user[0].Email, @user[0].FirstName + ' ' + @user[0].LastName);
                            //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                            const string fromPassword = "Info@#12345";
                            var tt = Dbcontext.TicketTypeMasters.Where(a => a.TicketTypeID == tm.TicketTypeID).SingleOrDefault();
                            var bts = Dbcontext.TicketStatusMasters.Where(m => m.TicketStatusID == tm.TicketStatusID).SingleOrDefault();
                            var ats = Dbcontext.TicketStatusMasters.Where(m => m.TicketStatusID == model.TicketStatusID).SingleOrDefault();
                            int tid = model.TicketID;
                            string ticketID = tm.Prefix + "" + tm.TicketID;

                            string subject = "Status Changed from " + bts.TicketStatus + " To " + ats.TicketStatus + " of " + ticketID;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<html>");
                            sb.Append("<Body>");
                            sb.Append("<table><tr><td><b>Dear sir/madam,</b></td></tr><tr><td><br/><b>Ticket Created By : </b></td><td><br/>" + cby.Title + ' ' + cby.FirstName + ' ' + cby.LastName + "</td></tr><tr><td><b>Ticket ID : </b></td><td>" + ticketID + "</td></tr><tr><td><b>Description : </b></td><td>" + tm.Description + "</td></tr><tr><td><b>Ticket Type : </b></td><td>" + tt.TicketType + "</td></tr><tr><td><b>Ticket Priority : </b></td><td>" + priority + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + admin.Title + ' ' + admin.FirstName + ' ' + admin.LastName + "</td></tr><tr><td>Newtech Infosoft Pvt. Ltd.</td></tr></table>");
                            sb.Append("</Body>");
                            sb.Append("</html>");

                            string body = sb.ToString();

                            var smtp = new SmtpClient
                            {
                                Host = "smtp.mail.yahoo.com",
                                Port = 25,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                            };
                            using (var message = new MailMessage(fromAddress, toAddress)
                            {

                                Subject = subject,
                                Body = body
                            })
                            {
                                //foreach (HttpPostedFileBase file in files)
                                //{
                                //    if (file != null)
                                //    {
                                //        string fileName = Path.GetFileName(file.FileName);
                                //        message.Attachments.Add(new Attachment(file.InputStream, fileName));
                                //    }
                                //}
                                message.IsBodyHtml = true;
                                //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                                //var user = Dbcontext.UserMasters.Where(m => m.CompID == @cby.CompID && m.Role == 2).ToList();
                                //foreach (var u in user)
                                //{
                                //    message.CC.Add(new MailAddress(Convert.ToString(u.Email), u.FirstName));
                                //}
                                //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Kuldip"));
                                //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                                smtp.Send(message);
                                TempData["Msg"] = "Mail Successfully Sent.";
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["Msg"] = "Sorry!!Mail not sent.";
                        }
                    }
                }

                if (model.AssignTo != null)
                {
                    var assign = Dbcontext.TicketMasters.Where(m => m.AssignTo == model.AssignTo && m.TicketID == model.TicketID).SingleOrDefault();
                    if (assign == null)
                    {
                        try
                        {
                            var user = Dbcontext.UserMasters.Where(m => m.UID == model.AssignTo).SingleOrDefault();
                            var cby = Dbcontext.UserMasters.Where(m => m.UID == tm.CreatedBy).SingleOrDefault();
                            int adminID = 0;
                            if (Session["UID"] != null)
                            {
                                adminID = Convert.ToInt32(Session["UID"].ToString());
                            }
                            var admin = Dbcontext.UserMasters.Where(m => m.UID == adminID).SingleOrDefault();
                            string priority = "";
                            if (model.Priority == 1)
                            {
                                priority = "High";
                            }
                            else if (model.Priority == 2)
                            {
                                priority = "Medium";
                            }
                            else
                            {
                                priority = "Normal";
                            }
                            //var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                            var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", "Newtech Infosoft Pvt. Ltd.");
                            var toAddress = new MailAddress(@user.Email, @user.FirstName);
                            //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                            const string fromPassword = "Info@#12345";
                            var tt = Dbcontext.TicketTypeMasters.Where(a => a.TicketTypeID == tm.TicketTypeID).SingleOrDefault();
                            int tid = model.TicketID;
                            string ticketID = tm.Prefix + "" + model.TicketID;

                            string subject = "New Ticket-" + tm.Subject;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<html>");
                            sb.Append("<Body>");
                            sb.Append("<table><tr><td><b>Dear " + user.Title + ' ' + @user.FirstName + ' ' + @user.LastName + ",</b></td></tr><tr><td><br/><b>Ticket Created By : </b></td><td><br/>" + cby.Title + ' ' + cby.FirstName + ' ' + cby.LastName + "</td></tr><tr><td><b>Ticket ID : </b></td><td>" + ticketID + "</td></tr><tr><td><b>Description : </b></td><td>" + tm.Description + "</td></tr><tr><td><b>Ticket Type : </b></td><td>" + tt.TicketType + "</td></tr><tr><td><b>Ticket Priority : </b></td><td>" + priority + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + admin.Title + ' ' + admin.FirstName + ' ' + admin.LastName + "</td></tr><tr><td>Newtech Infosoft Pvt. Ltd.</td></tr></table>");
                            sb.Append("</Body>");
                            sb.Append("</html>");

                            string body = sb.ToString();

                            var smtp = new SmtpClient
                            {
                                Host = "smtp.mail.yahoo.com",
                                Port = 25,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                            };
                            using (var message = new MailMessage(fromAddress, toAddress)
                            {

                                Subject = subject,
                                Body = body
                            })
                            {
                                //foreach (HttpPostedFileBase file in files)
                                //{
                                //    if (file != null)
                                //    {
                                //        string fileName = Path.GetFileName(file.FileName);
                                //        message.Attachments.Add(new Attachment(file.InputStream, fileName));
                                //    }
                                //}
                                message.IsBodyHtml = true;
                                //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                                message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Kuldip"));
                                //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                                smtp.Send(message);
                                TempData["Msg"] = "Mail sent successfully.";
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["Msg"] = "Sorry!!Mail not sent.";
                        }
                    }
                }



                tm.TicketStatusID = model.TicketStatusID;
                tm.UpdatedDate = System.DateTime.Now;
                if (rid == 1)
                {
                    tm.AssignTo = model.AssignTo;
                }
                tm.UpdatedBy = uid;
                Dbcontext.SaveChanges();
                TempData["AMsg"] = "Updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["AMsg"] = "Status Not Updated";
            }
            //return Content("<script language='javascript' type='text/javascript'>alert('Status updated successfully.');</script>");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ChangePwd(FormCollection All)
        {
            int uid = 0;
            string opwd = All["txtOpwd"];
            string email = All["txtEmail"];
            string npwd = All["txtNpwd"];
            string ncpwd = All["txtNCpwd"];
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
            }

            var lst = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();

            if (lst.Password == opwd && lst.Email == email)
            {

                if (npwd == ncpwd)
                {
                    UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();
                    um.Password = ncpwd;
                    Dbcontext.SaveChanges();
                    ////TempData["AMsg"] = "Password changed successfully.";

                    TempData["UserMsg"] = "Password changed successfully.";

                    Response.Redirect("/Login/Index");
                    return Content("<script language='javascript' type='text/javascript'>alert('Password changed successfully.');</script>");
                }
                else
                {
                    //TempData["AMsg"] = "Status Not Updated";
                    //return Content("<script language='javascript' type='text/javascript'>alert('New Password and Confirm New Password Not Matched.');</script>");
                    TempData["AMsg"] = "New Password and Confirm New Password Not Matched.";
                    return RedirectToAction("Index");
                }
            }
            else if (lst.Password != opwd)
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Old Password Not Matched.');</script>");
                TempData["AMsg"] = "Old Password Not Matched.";
                return RedirectToAction("Index");
            }
            else if (lst.Email != email)
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Email Not Matched.');</script>");
                TempData["AMsg"] = "Email Not Matched.";
                return RedirectToAction("Index");
            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Error occured, Try again!!!');</script>");
                TempData["AMsg"] = "Error occured, Try again!!!";
                return RedirectToAction("Index");
            }

        }
    }
}
