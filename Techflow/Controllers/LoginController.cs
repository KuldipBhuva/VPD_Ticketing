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
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index()
        {
            Session["UID"] = null;
            Session["CompID"] = null;
            Session["Name"] = null;
            Session["ReqQ"] = null;
            Session["Power"] = null;
            Session["Access"] = null;
            Session["Tickets"] = null;
            Session["Portal"] = null;
            Session["Invoice"] = null;
            Session["Role"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserModel model)
        {
            UserService objService = new UserService();
            UserModel objModel = new UserModel();

            objModel = objService.GetAuthUser(model.UserName, model.Password);
            if (objModel != null)
            {
                //if (objModel.Portal == true && objModel.Status == 1)
                if (objModel.Status == 1)
                {
                    Session["UID"] = objModel.UID;
                    Session["Role"] = objModel.Role;
                    Session["CompID"] = objModel.CompID;
                    Session["Name"] = objModel.Title + ' ' + objModel.FirstName + ' ' + objModel.LastName;
                    Session["ReqQ"] = objModel.ReqQ;
                    Session["Power"] = objModel.Power;
                    Session["Access"] = objModel.Access;
                    Session["Tickets"] = objModel.Tickets;
                    Session["Portal"] = objModel.Portal;
                    Session["Invoice"] = objModel.Invoice;

                    DateTime cdt = System.DateTime.Now;
                    DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                    model.DateTime = dt;
                    model.IPAddress = System.Web.HttpContext.Current.Request.Params["REMOTE_ADDR"];
                    model.UID = objModel.UID;
                    objService.InsertLog(model);

                    Response.Redirect("/Dashboard/Index");
                    return RedirectToAction("/Dashboard/Index");
                }
                else
                {
                    TempData["EMsg"] = "You are not able to access portal.contact to admin.";
                    return View();
                }
            }
            else
            {
                TempData["EMsg"] = "Username or Password not Matched!!";
                return View();
            }

        }
        [HttpPost]
        public ActionResult ResetPwd(FormCollection All)
        {

            string email = All["txtEmail"];

            var lst = Dbcontext.UserMasters.Where(m => m.Email == email).SingleOrDefault();
            int uid = 0;

            if (lst != null)
            {
                //uid = lst.UID;
                //UserMaster um = Dbcontext.UserMasters.Where(m => m.UID == uid).SingleOrDefault();
                //um.UserName = lst.Email;
                //um.Password = lst.Phone;
                //Dbcontext.SaveChanges();
                //////TempData["AMsg"] = "Password changed successfully.";

                //TempData["UserMsg"] = "Password has been reset, you can login with email as username and phone as password";

                //Response.Redirect("/Login/Index");


                try
                {
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == lst.CompID).SingleOrDefault();
                    string company = "";
                    if (lst.CompID == null || lst.CompID == 0)
                    {
                        company = "Administrator";
                    }
                    else
                    {
                        company = comp.Name;
                    }
                    var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", company);
                    var toAddress = new MailAddress(lst.Email, lst.Title + " " + lst.FirstName + " " + lst.LastName);
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "Info@#12345";


                    string subject = "Forgot Password Detail";
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear " + lst.Title + " " + lst.FirstName + " " + lst.LastName + ",</b></td></tr><tr><td><br/><b>Your Password : </b></td><td><br/>" + lst.Password + "</td></tr><tr><td><b>Note : </b></td><td style='color:red;'><b>Once you will log in with this password then change password immediately.</b></td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + company + "</td></tr></table>");
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
                        
                        message.CC.Add(new MailAddress(Convert.ToString("kuldip@newtechinfosoft.in"), "Kuldip Patel"));
                        //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                        smtp.Send(message);
                        TempData["EMsg"] = "Email sent successfully.";

                    }
                    Response.Redirect("/Login/Index");
                }
                catch (Exception ex)
                {
                    TempData["EMsg"] = "Sorry!! Email not sent try after some time.Error Detail : " + ex;
                    Response.Redirect("/Login/Index");
                }
                
                return Content("<script language='javascript' type='text/javascript'>alert('Email sent successfully.');</script>");
            }
            else
            {
                //TempData["AMsg"] = "Status Not Updated";
                //return Content("<script language='javascript' type='text/javascript'>alert('New Password and Confirm New Password Not Matched.');</script>");
                TempData["EMsg"] = "Email Not Matched.";


                Response.Redirect("/Login/Index");
                return Content("<script language='javascript' type='text/javascript'>alert('Email Not Matched.');</script>");
            }
        }

    }
}
