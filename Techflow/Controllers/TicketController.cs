using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.ViewModel;
using Services;

namespace Techflow.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/
        techflowEntities Dbcontext = new techflowEntities();
        public ActionResult Index(string status)
        {
            TicketService objService = new TicketService();
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
            //objList = objService.getTicket(uid,rid,cid);
            //objModel.ListTicket = new List<TicketModel>();
            //objModel.ListTicket.AddRange(objList);
            
            if (rid == 1 || rid == 4)
            {
                objTList = objService.getSAStaffTicket(uid,rid);
                objModel.ListTicket = new List<TicketModel>();
                objModel.ListTicket.AddRange(objTList);
            }
            else if (rid == 2)
            {
                objTList = objService.getCATicket(cid);
                objModel.ListTicket = new List<TicketModel>();
                objModel.ListTicket.AddRange(objTList);
            }
            else if (rid == 3)
            {
                objTList = objService.getAUTicket(uid);
                objModel.ListTicket = new List<TicketModel>();
                objModel.ListTicket.AddRange(objTList);
            }

            List<TicketTypeModel> ListTT = new List<TicketTypeModel>();
            ListTT = objService.getTicketType();
            objModel.ListTT = new List<TicketTypeModel>();
            objModel.ListTT.AddRange(ListTT);

            List<TicketStatusModel> ListTS = new List<TicketStatusModel>();
            ListTS = objService.getTicketStatus();
            objModel.ListTS = new List<TicketStatusModel>();
            objModel.ListTS.AddRange(ListTS);

            
            List<CompanyModel> objCompList = new List<CompanyModel>();
            objCompList = objService.getActiveComp();
            objModel.ListComp = new List<CompanyModel>();
            objModel.ListComp.AddRange(objCompList);

            List<UserModel> objUserList = new List<UserModel>();
            objUserList = objService.getActiveUser();
            objModel.UserList = new List<UserModel>();
            objModel.UserList.AddRange(objUserList);

            List<TicketTypeModel> lstACat = new List<TicketTypeModel>();
            lstACat = objService.getActiveSubCat();
            objModel.ListActiveSubCat = new List<TicketTypeModel>();
            objModel.ListActiveSubCat.AddRange(lstACat);

            return View(objModel);
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files, TicketModel model, TicketTranModel model1)
        {
            TicketService objService = new TicketService();
            model.TicketStatusID = 1;
            model.IsActive = true;
            int uid = 0;
            int tid = 0;
            int cid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                if (model.CreatedBy == null)
                {
                    model.CreatedBy = uid;
                }
                if (Session["CompID"] != null)
                {
                    cid = Convert.ToInt32(Session["CompID"].ToString());
                }
                if (model.comp == null || model.comp == 0)
                {
                    model.CompID = cid;
                }
                else
                {
                    model.CompID = model.comp;
                }
                //if (model.TicketTypeID == 1)
                //{
                //    var TList = Dbcontext.TicketMasters.Where(m => m.TicketNo.Contains("REQ")).Max(m => m.TicketNo).ToList();

                //}
                model.CreatedDate = System.DateTime.Now;

                var lstCat = Dbcontext.TicketTypeMasters.Where(m => m.TicketTypeID == model.TicketTypeID).SingleOrDefault();
                string spre = "";
                if (model.SubType != null && model.SubType!=0)
                {
                    var lstSubCat = Dbcontext.TicketSubTypes.Where(m => m.TTSID == model.SubType).SingleOrDefault();
                    spre = lstSubCat.SubType.Substring(0, 3);
                }
                
                string pre = lstCat.TicketType.Substring(0, 3);
                string result = pre + "" + spre;
                //var prefix = Regex.Replace(result, @"\b(\w)", m => m.Value.ToUpper());
                var prefix = Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(result);
                model.Prefix = prefix;
                model1.Prefix = prefix;
                //if (model.TicketTypeID == 1)
                //{
                //    model.Prefix = "REQ";
                //    model1.Prefix = "REQ";
                //}
                //else if (model.TicketTypeID == 2)
                //{
                //    model.Prefix = "INC";
                //    model1.Prefix = "INC";
                //}
                //else
                //{
                //    model.Prefix = "CHG";
                //    model1.Prefix = "CHG";
                //}
                tid = objService.InsertTicket(model);
                TempData["Msg"] = "Ticket Sent Successfully.";

                if (files != null)
                {
                    //string k = objService.Getid();
                    //int s = Convert.ToInt32(k);
                    //string strtext = "Customer";
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            string filename = System.IO.Path.GetFileName(file.FileName);
                            string folderPath = Server.MapPath("~/Attachment/") + model.TicketID;
                            //  obj.EmpId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"].ToString());
                            string destinationPath = folderPath;
                            string employeeFolderPath = destinationPath;
                            // create folder if it is not exist
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                                if (!Directory.Exists(employeeFolderPath))
                                {
                                    Directory.CreateDirectory(employeeFolderPath);
                                    // create emp id folder of not exist
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(employeeFolderPath))
                                {
                                    Directory.CreateDirectory(employeeFolderPath);
                                    // create emp id folder of not exist
                                }
                            }
                            destinationPath = employeeFolderPath;
                            /*Saving the file in server folder*/
                            //file.SaveAs(Server.MapPath("~/Images/" + filename));
                            string fileNewName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                            file.SaveAs(Path.Combine(destinationPath, fileNewName));
                            model1.TicketID = tid;
                            model1.Comment = model.Description;
                            model1.FileURL = Path.Combine("/Attachment/" + model.TicketID + "/", fileNewName);
                            model1.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                            if (model.CreatedBy == null)
                            {
                                model1.CreatedBy = uid;
                            }
                            else
                            {
                                model1.CreatedBy = model.CreatedBy;
                            }
                            model1.CreatedDate = System.DateTime.Now;
                            //model1.ReplayBy = uid;
                            //model1.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model1);
                            TempData["Msg"] = "Ticket Sent Successfully.";
                        }
                        else
                        {
                            model1.TicketID = tid;
                            model1.Comment = model.Description;
                            if (model.CreatedBy == null)
                            {
                                model1.CreatedBy = uid;
                            }
                            else
                            {
                                model1.CreatedBy = model.CreatedBy;
                            }
                            model1.CreatedDate = System.DateTime.Now;
                            //model1.ReplayBy = uid;
                            //model1.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model1);
                            TempData["Msg"] = "Ticket Sent Successfully.";
                        }
                    }
                }

                try
                {
                    var user = Dbcontext.UserMasters.Where(m => m.UID == model.CreatedBy).SingleOrDefault();
                    var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
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
                    var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", comp.Name);
                    var toAddress = new MailAddress("newtechinfosoft@gmail.com", "Niraj Shah");
                    //var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                    const string fromPassword = "Info@#12345";
                    var tt = Dbcontext.TicketTypeMasters.Where(a => a.TicketTypeID == model.TicketTypeID).SingleOrDefault();

                    string subject = "New Ticket-" + model.Subject;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<Body>");
                    sb.Append("<table><tr><td><b>Dear Newtech Administrator,</b></td></tr><tr><td><br/><b>Ticket Created By : </b></td><td><br/>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Ticket ID : </b></td><td>" + model.Prefix + "" + tid + "</td></tr><tr><td><b>Description : </b></td><td>" + model.Description + "</td></tr><tr><td><b>Ticket Type : </b></td><td>" + tt.TicketType + "</td></tr><tr><td><b>Ticket Priority : </b></td><td>"+ priority+ "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td>" + comp.Name + "</td></tr></table>");
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
                        foreach (HttpPostedFileBase file in files)
                        {
                            if (file != null)
                            {
                                string fileName = Path.GetFileName(file.FileName);
                                message.Attachments.Add(new Attachment(file.InputStream, fileName));
                            }
                        }
                        message.IsBodyHtml = true;
                        
                        message.CC.Add(new MailAddress(Convert.ToString("ketan@newtechinfosoft.in"), "Ketan Panchal"));
                        message.CC.Add(new MailAddress(Convert.ToString("pflketan@yahoo.co.in"), "Ketan-2"));
                        message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Kuldip Patel"));
                        smtp.Send(message);
                        TempData["Msg"] = "Your Ticket Successfully Sent.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Sorry!!Ticket not sent try after some time.";
                }



            }
            return RedirectToAction("Index");
        }
        public ActionResult Replay(int id, int uid)
        {
            TicketService objService = new TicketService();
            TicketModel objModel = new TicketModel();
            objModel = objService.getTicketByID(id);

            UserService objUService = new UserService();
            CompanyService objCompSer = new CompanyService();

            objModel.UserDetails = objUService.getByID(uid);
            objModel.CompDetails = objCompSer.getByID(Convert.ToInt32(objModel.UserDetails.CompID));

            int rid = 0;
            if (Session["Role"] != null)
            {
                rid = Convert.ToInt32(Session["Role"].ToString());
            }
            List<TicketTranModel> ListTicketTran = new List<TicketTranModel>();

            ListTicketTran = objService.getTicketTran(uid, id,rid);
            objModel.ListTicketTran = new List<TicketTranModel>();
            objModel.ListTicketTran.AddRange(ListTicketTran);

            List<TicketStatusModel> objTStatus = new List<TicketStatusModel>();
            objTStatus = objService.getTicketStatus();
            objModel.ListTstatus = new List<TicketStatusModel>();
            objModel.ListTstatus.AddRange(objTStatus);

            List<UserModel> objStaff = new List<UserModel>();
            objStaff = objService.getStaff();
            objModel.StaffList = new List<UserModel>();
            objModel.StaffList.AddRange(objStaff);

            //objModel.Quotedetails = objService.getQuoteByTIcketID(id);
            //objModel.Deldetails = objService.getDelByTIcketID(id);
            //objModel.Accessdetails = objService.getAccessByTIcketID(id);


            //List<TicketAttachmentModel> ListTicketAtt = new List<TicketAttachmentModel>();
            //ListTicketAtt = objService.getTicketAttachments(uid);
            //objModel.ListTA = new List<TicketAttachmentModel>();
            //objModel.ListTA.AddRange(ListTicketAtt);

            TempData["uid"] = uid;
            TempData["tid"] = id;
            return View(objModel);
        }
        [HttpPost]
        public ActionResult Replay(HttpPostedFileBase[] files, TicketTranModel model, TicketModel model1)
        {
            TicketService objService = new TicketService();
            int uid = 0;
            int rid = 0;
            if (Session["UID"] != null)
            {
                uid = Convert.ToInt32(Session["UID"].ToString());
                rid = Convert.ToInt32(Session["Role"].ToString());
                TicketMaster tm = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();

                //mailto CA


                
                if (model1.TicketStatusID != null)
                {
                    var status = Dbcontext.TicketMasters.Where(m => m.TicketID == model.TicketID && m.TicketStatusID == model1.TicketStatusID).SingleOrDefault();
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
                            if (model1.Priority == 1)
                            {
                                priority = "High";
                            }
                            else if (model1.Priority == 2)
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
                            var ats = Dbcontext.TicketStatusMasters.Where(m => m.TicketStatusID == model1.TicketStatusID).SingleOrDefault();
                            int tid = tm.TicketID;
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

                if (model1.AssignTo != null)
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
                            if (model1.Priority == 1)
                            {
                                priority = "High";
                            }
                            else if (model1.Priority == 2)
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
                            int tid = tm.TicketID;
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

                //mail to CA end


                tm.TicketStatusID = model1.TicketStatusID;
                tm.UpdatedDate = System.DateTime.Now;
                tm.UpdatedBy = uid;
                tm.AssignTo = model1.AssignTo;
                Dbcontext.SaveChanges();
                
                if (files != null)
                {
                    //string k = objService.Getid();
                    //int s = Convert.ToInt32(k);
                    //string strtext = "Customer";
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            string filename = System.IO.Path.GetFileName(file.FileName);
                            string folderPath = Server.MapPath("~/Attachment/") + model.TicketID;
                            //  obj.EmpId = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"].ToString());
                            string destinationPath = folderPath;
                            string employeeFolderPath = destinationPath;
                            // create folder if it is not exist
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                                if (!Directory.Exists(employeeFolderPath))
                                {
                                    Directory.CreateDirectory(employeeFolderPath);
                                    // create emp id folder of not exist
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(employeeFolderPath))
                                {
                                    Directory.CreateDirectory(employeeFolderPath);
                                    // create emp id folder of not exist
                                }
                            }
                            destinationPath = employeeFolderPath;
                            /*Saving the file in server folder*/
                            //file.SaveAs(Server.MapPath("~/Images/" + filename));
                            string fileNewName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                            file.SaveAs(Path.Combine(destinationPath, fileNewName));

                            //objModel.CID = Convert.ToInt32(s);
                            model.FileURL = Path.Combine("/Attachment/" + model.TicketID + "/", fileNewName);
                            model.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                            model.TicketID = model1.TicketID;
                            model.Prefix = model1.Prefix;
                            model.Comment = model1.Comment;
                            model.CreatedBy = model.CreatedBy;
                            model.CreatedDate = model.CreatedDate;
                            model.ReplayBy = uid;
                            model.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model);
                            //objModel.BID = Convert.ToInt32(Session["BranchID"].ToString());
                            //objService.InsertTicketAttachment(model1);

                            //string filepathtosave = "Images/" + filename;
                            /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE*/
                        }
                        else if (model1.Comment != null)
                        {
                            model.TicketID = model1.TicketID;
                            model.Prefix = model1.Prefix;
                            model.Comment = model1.Comment;
                            model.CreatedBy = model.CreatedBy;
                            model.CreatedDate = model.CreatedDate;
                            model.ReplayBy = uid;
                            model.ReplayDate = System.DateTime.Now;
                            objService.InsertTicketTran(model);                           
                        }
                        else
                        {
                            TempData["AMsg"] = "Status updated.But message not sent, Enter your message or Attach any files in attachment.";
                        }
                    }
                }
                if (model1.Comment != null || files[0]!=null)
                {
                    try
                    {
                        if (rid != 1)
                        {
                            //var user = Dbcontext.UserMasters.Where(m => m.UID == model.ReplayBy).SingleOrDefault();
                            //var comp = Dbcontext.CompanyMasters.Where(m => m.CompID == user.CompID).SingleOrDefault();
                            //var ts = Dbcontext.TicketStatusMasters.Where(m => m.TicketStatusID == model1.TicketStatusID).SingleOrDefault();
                            //var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", comp.Name);
                            //var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                            ////var toAddress = new MailAddress("newtech_infosoft@yahoo.com", "Administrator of Techflow");
                            //const string fromPassword = "Info@#12345";

                            //var ticket = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();
                            //string subject = ticket.Subject;
                            //StringBuilder sb = new StringBuilder();
                            //sb.Append("<html>");
                            //sb.Append("<Body>");
                            //sb.Append("<table><tr><td><b>Dear Techflow Administrator,</b></td></tr><tr><td><br/><b>Ticket ID : </b></td><td><br/>" + model.Prefix + "" + model.TicketID + "</td></tr><tr><td><b>Reply By : </b></td><td>" + user.Title + ' ' + user.FirstName + ' ' + user.LastName + "</td></tr><tr><td><b>Ticket Status : </b></td><td>" + ts.TicketStatus + "</td></tr><tr><td><b>Comment : </b></td><td>" + model.Comment + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>" + user.FirstName + ' ' + user.LastName + "</td></tr></table>");
                            //sb.Append("</Body>");
                            //sb.Append("</html>");

                            //string body = sb.ToString();

                            //var smtp = new SmtpClient
                            //{
                            //    Host = "smtp.mail.yahoo.com",
                            //    Port = 25,
                            //    EnableSsl = true,
                            //    DeliveryMethod = SmtpDeliveryMethod.Network,
                            //    UseDefaultCredentials = false,
                            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                            //};
                            //using (var message = new MailMessage(fromAddress, toAddress)
                            //{

                            //    Subject = subject,
                            //    Body = body
                            //})
                            //{
                            //    foreach (HttpPostedFileBase file in files)
                            //    {
                            //        if (file != null)
                            //        {
                            //            string fileName = Path.GetFileName(file.FileName);
                            //            message.Attachments.Add(new Attachment(file.InputStream, fileName));
                            //        }
                            //    }
                            //    message.IsBodyHtml = true;
                            //    //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                            //    message.CC.Add(new MailAddress(Convert.ToString("kuldip@newtechinfosoft.in"), "Kuldip"));
                            //    //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                            //    smtp.Send(message);
                            //    TempData["Msg"] = "Your Reply Successfully Sent.";
                            //}
                        }
                        else
                        {
                            //var user = Dbcontext.UserMasters.Where(m => m.UID == model1.CreatedBy).SingleOrDefault();
                            //var fromAddress = new MailAddress("newtech_infosoft@yahoo.com", "Techflow Administrator");
                            ////var toAddress = new MailAddress("admin@techflow.com.au", "Administrator of Techflow");
                            //var toAddress = new MailAddress(user.Email, user.FirstName+" "+user.LastName);
                            //const string fromPassword = "Info@#12345";

                            //var ticket = Dbcontext.TicketMasters.Where(m => m.TicketID == model1.TicketID).SingleOrDefault();
                            //string subject = ticket.Subject;
                            //StringBuilder sb = new StringBuilder();
                            //sb.Append("<html>");
                            //sb.Append("<Body>");
                            //sb.Append("<table><tr><td><b>Dear User,</b></td></tr><tr><td><br/><b>Ticket ID : </b></td><td><br/>" + model.Prefix + "" + model.TicketID + "</td></tr><tr><td><b>Reply By : </b></td><td>Techflow Administrator</td></tr><tr><td><b>Comment : </b></td><td>" + model.Comment + "</td></tr><tr><td><br/><b>Thanks and Regards,</b><br/></td></tr><tr><td>Techflow</td></tr></table>");
                            //sb.Append("</Body>");
                            //sb.Append("</html>");

                            //string body = sb.ToString();

                            //var smtp = new SmtpClient
                            //{
                            //    Host = "smtp.mail.yahoo.com",
                            //    Port = 25,
                            //    EnableSsl = true,
                            //    DeliveryMethod = SmtpDeliveryMethod.Network,
                            //    UseDefaultCredentials = false,
                            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                            //};
                            //using (var message = new MailMessage(fromAddress, toAddress)
                            //{

                            //    Subject = subject,
                            //    Body = body
                            //})
                            //{
                            //    foreach (HttpPostedFileBase file in files)
                            //    {
                            //        if (file != null)
                            //        {
                            //            string fileName = Path.GetFileName(file.FileName);
                            //            message.Attachments.Add(new Attachment(file.InputStream, fileName));
                            //        }
                            //    }
                            //    message.IsBodyHtml = true;
                            //    //message.CC.Add(new MailAddress(Convert.ToString("kuldipbhuva5@gmail.com"), "Distributer Order"));
                            //    message.CC.Add(new MailAddress(Convert.ToString("kuldip@newtechinfosoft.in"), "Kuldip"));
                            //    //message.CC.Add(new MailAddress(Convert.ToString("info@sugamhealthcare.com"), "Info-Sugam Health Care"));
                            //    smtp.Send(message);
                            //    TempData["Msg"] = "Your Reply Successfully Sent.";
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Msg"] = "Sorry!!Reply not sent try after some time.";
                    }
                }

            }
            return RedirectToAction("Replay", new { @id = TempData["tid"], @uid = TempData["uid"] });
        }
        public ActionResult FillUser(int cid)
        {
            List<UserModel> lstModel = new List<UserModel>();
            UserModel objModel = new UserModel();
            QuotationService objService = new QuotationService();
            lstModel = objService.getUserByComp(cid);
            objModel.ListUser = new List<UserModel>();
            objModel.ListUser.AddRange(lstModel);
            System.Threading.Thread.Sleep(5000);
            return Json(objModel.ListUser, JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(objModel.ListUser, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
        public ActionResult FillSubCat(int cid)
        {
            List<TicketTypeModel> lstModel = new List<TicketTypeModel>();
            UserModel objModel = new UserModel();
            QuotationService objService = new QuotationService();
            lstModel = objService.getSubCatByCatID(cid);
            objModel.ListSubCat = new List<TicketTypeModel>();
            objModel.ListSubCat.AddRange(lstModel);
            System.Threading.Thread.Sleep(5000);
            return Json(objModel.ListSubCat, JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(objModel.ListUser, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
    }
}
