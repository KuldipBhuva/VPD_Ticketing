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
    public class CategoryService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<TicketTypeModel> getCat()
        {
            try
            {
                Mapper.CreateMap<TicketTypeMaster, TicketTypeModel>();
                List<TicketTypeMaster> objCityMaster = Dbcontext.TicketTypeMasters.ToList();
                List<TicketTypeModel> objCityItem = Mapper.Map<List<TicketTypeModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TicketTypeModel> getSubCat()
        {
            try
            {
                var data = (from st in Dbcontext.TicketSubTypes
                            join ttm in Dbcontext.TicketTypeMasters on st.TypeID equals ttm.TicketTypeID
                            select new TicketTypeModel()
                            {
                                TicketTypeID = ttm.TicketTypeID,
                                TicketType = ttm.TicketType,
                                Description = ttm.Description,
                                IsActive = ttm.IsActive,

                                TTSID = st.TTSID,
                                TypeID = st.TypeID,
                                SubType = st.SubType,
                                SubDesc = st.SubDesc,
                                Status = st.Status

                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TicketTypeModel> getActiveCat()
        {
            try
            {
                Mapper.CreateMap<TicketTypeMaster, TicketTypeModel>();
                List<TicketTypeMaster> objCityMaster = Dbcontext.TicketTypeMasters.Where(m => m.IsActive == true).ToList();
                List<TicketTypeModel> objCityItem = Mapper.Map<List<TicketTypeModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Insert(TicketTypeModel model)
        {
            try
            {
                Mapper.CreateMap<TicketTypeModel, TicketTypeMaster>();
                TicketTypeMaster objUser = Mapper.Map<TicketTypeMaster>(model);
                Dbcontext.TicketTypeMasters.Add(objUser);
                return Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int InsertSubCat(TicketTypeModel model)
        {
            try
            {
                Mapper.CreateMap<TicketTypeModel, TicketSubType>();
                TicketSubType objUser = Mapper.Map<TicketSubType>(model);
                Dbcontext.TicketSubTypes.Add(objUser);
                return Dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public TicketTypeModel getByID(int id)
        {
            try
            {
                Mapper.CreateMap<TicketTypeMaster, TicketTypeModel>();
                TicketTypeMaster objCityMaster = Dbcontext.TicketTypeMasters.Where(m => m.TicketTypeID == id).SingleOrDefault();
                TicketTypeModel objCityItem = Mapper.Map<TicketTypeModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TicketTypeModel getByIDSubCat(int id)
        {
            try
            {
                Mapper.CreateMap<TicketSubType, TicketTypeModel>();
                TicketSubType objCityMaster = Dbcontext.TicketSubTypes.Where(m => m.TTSID == id).SingleOrDefault();
                TicketTypeModel objCityItem = Mapper.Map<TicketTypeModel>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(TicketTypeModel model)
        {
            Mapper.CreateMap<TicketTypeModel, TicketTypeMaster>();
            TicketTypeMaster objUser = Dbcontext.TicketTypeMasters.SingleOrDefault(m => m.TicketTypeID == model.TicketTypeID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
        public int UpdateSubCat(TicketTypeModel model)
        {
            Mapper.CreateMap<TicketTypeModel, TicketSubType>();
            TicketSubType objUser = Dbcontext.TicketSubTypes.SingleOrDefault(m => m.TTSID == model.TTSID);
            objUser = Mapper.Map(model, objUser);
            return Dbcontext.SaveChanges();
        }
    }
}
