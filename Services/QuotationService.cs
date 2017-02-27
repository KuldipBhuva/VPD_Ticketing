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
    public class QuotationService
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
        public List<UserModel> getUserByComp(int cid)
        {
            try
            {
                Mapper.CreateMap<UserMaster, UserModel>();
                List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.CompID == cid && m.Status==1).ToList();
                List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> getActiveUser()
        {
            try
            {
                Mapper.CreateMap<UserMaster, UserModel>();
                List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.Role != 1 && m.Status ==1).ToList();
                List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TicketTypeModel> getSubCatByCatID(int cid)
        {
            try
            {
                Mapper.CreateMap<TicketSubType, TicketTypeModel>();
                List<TicketSubType> objCityMaster = Dbcontext.TicketSubTypes.Where(m => m.TypeID == cid && m.Status == true).ToList();
                List<TicketTypeModel> objCityItem = Mapper.Map<List<TicketTypeModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
