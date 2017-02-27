using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Models;
using Models.ViewModel;

namespace Services
{
    public class UserService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<CompanyModel> getActiveComp()
        {
            try
            {
                Mapper.CreateMap<CompanyMaster, CompanyModel>();
                List<CompanyMaster> objCityMaster = Dbcontext.CompanyMasters.Where(m => m.Status == 1).ToList();
                List<CompanyModel> objCityItem = Mapper.Map<List<CompanyModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> getUser(int cid, int rid)
        {
            try
            {
                var data = (from um in Dbcontext.UserMasters
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID into co
                            from c in co.DefaultIfEmpty()
                            where um.CompID == (rid == 1 ? um.CompID : cid)
                            select new UserModel()
                            {
                                UID = um.UID,
                                CompID = um.CompID,
                                Role = um.Role,
                                Title = um.Title,
                                FirstName = um.FirstName,
                                LastName = um.LastName,
                                UserName = um.UserName,
                                Password = um.Password,
                                Phone = um.Phone,
                                Email = um.Email,
                                Address = um.Address,
                                City = um.City,
                                PostCode = um.PostCode,
                                Country = um.Country,
                                Status = um.Status,
                                ReqQ = um.ReqQ,
                                Power = um.Power,
                                Access = um.Access,
                                Tickets = um.Tickets,
                                Portal = um.Portal,
                                Invoice = um.Invoice,
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c == null ? string.Empty : c.Name)
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public List<UserModelAPI> getUserAPI()
        {
            try
            {
                //Mapper.CreateMap<UserMaster, UserModel>();
                //List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.Role != 1 && m.CompID == (cid == 0 ? m.CompID : cid)).ToList();
                //List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                //return objCityItem;

                var data = (from um in Dbcontext.UserMasters
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID

                            select new UserModelAPI()
                            {
                                UID = um.UID,
                                CompID = um.CompID,
                                Role = um.Role,
                                Title = um.Title,
                                FirstName = um.FirstName,
                                LastName = um.LastName,
                                UserName = um.UserName,
                                Password = um.Password,
                                Phone = um.Phone,
                                Email = um.Email,
                                Address = um.Address,
                                City = um.City,
                                PostCode = um.PostCode,
                                Country = um.Country,
                                Status = um.Status,
                                ReqQ = um.ReqQ,
                                Power = um.Power,
                                Access = um.Access,
                                Tickets = um.Tickets,
                                Portal = um.Portal,
                                Invoice = um.Invoice,
                                CompDetails = new CompanyModel()
                                {
                                    Name = cm.Name
                                }
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUser(UserModel model)
        {
            try
            {
                Mapper.CreateMap<UserModel, UserMaster>();
                UserMaster objUser = Mapper.Map<UserMaster>(model);
                Dbcontext.UserMasters.Add(objUser);
                return Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UserMasterResponse InsertUserAPI(UserModelAPI model)
        {
            try
            {
                Mapper.CreateMap<UserModelAPI, UserMaster>();
                UserMaster objUser = Mapper.Map<UserMaster>(model);

                if (objUser != null)
                {


                    Dbcontext.UserMasters.Add(objUser);
                    Dbcontext.SaveChanges();
                    return new UserMasterResponse() { response = true };

                }

                else
                {

                    return new UserMasterResponse() { response = false };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel GetAuthUser(string username, string phone)
        {
            Mapper.CreateMap<UserMaster, UserModel>();
            UserMaster objComp = Dbcontext.UserMasters.SingleOrDefault(m => m.UserName == username && m.Password == phone);
            UserModel objCItem = Mapper.Map<UserModel>(objComp);
            return objCItem;
        }
        public UserModel getByID(int id)
        {
            Mapper.CreateMap<UserMaster, UserModel>();
            UserMaster objComp = Dbcontext.UserMasters.SingleOrDefault(m => m.UID == id);
            UserModel objCItem = Mapper.Map<UserModel>(objComp);
            return objCItem;
        }
        public int Update(UserModel model)
        {
            Mapper.CreateMap<UserModel, UserMaster>();
            UserMaster objUser = Dbcontext.UserMasters.SingleOrDefault(m => m.UID == model.UID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }


        public UserMasterResponse UpdateAPI(UserModelAPI model)
        {
            Mapper.CreateMap<UserModelAPI, UserMaster>();
            UserMaster objUser = Dbcontext.UserMasters.SingleOrDefault(m => m.UID == model.UID);


            if (objUser != null)
            {
                objUser = Mapper.Map(model, objUser);
                Dbcontext.SaveChanges();
                return new UserMasterResponse() { response = true };
            }
            else
            {
                return new UserMasterResponse() { response = false };

            }
        }
        public int InsertLog(UserModel model)
        {
            Mapper.CreateMap<UserModel, UserLog>();
            UserLog objCompany = Mapper.Map<UserLog>(model);
            Dbcontext.UserLogs.Add(objCompany);
            return Dbcontext.SaveChanges();
        }
        public List<UserModel> getUserLog(int uid, DateTime frmDt, DateTime toDt)
        {
            var data = (from ul in Dbcontext.UserLogs
                        join um in Dbcontext.UserMasters on ul.UID equals um.UID
                        join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID into comp
                        from c in comp.DefaultIfEmpty()
                        where ul.DateTime >= frmDt && ul.DateTime <= toDt && ul.UID == (uid == 0 ? ul.UID : uid)
                        select new UserModel()
                        {
                            ID = ul.ID,
                            UID = ul.UID,
                            DateTime = ul.DateTime,
                            IPAddress = ul.IPAddress,

                            Title = um.Title,
                            FirstName = um.FirstName,
                            LastName = um.LastName,
                            Role=um.Role,
                            CompDetails = new CompanyModel()
                            {
                                Name=(c==null?string.Empty:c.Name)
                            }
                        }).OrderByDescending(m=>m.DateTime).ToList();
            return data;
        }
    }
}
