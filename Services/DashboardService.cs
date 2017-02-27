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
    public class DashboardService
    {
        techflowEntities Dbcontext = new techflowEntities();
        public List<TicketModel> getAllTicket(int cid,int uid,int rid)
        {
            try
            {
                //Mapper.CreateMap<TicketMaster, TicketModel>();
                //List<TicketMaster> objCityMaster = Dbcontext.TicketMasters.ToList();
                //List<TicketModel> objCityItem = Mapper.Map<List<TicketModel>>(objCityMaster);
                //return objCityItem;
                var data = (from tm in Dbcontext.TicketMasters
                            join um in Dbcontext.UserMasters on tm.CreatedBy equals um.UID
                            join cm in Dbcontext.CompanyMasters on um.CompID equals cm.CompID into cmp
                            from c in cmp.DefaultIfEmpty()
                            join tt in Dbcontext.TicketTypeMasters on tm.TicketTypeID equals tt.TicketTypeID
                            join ts in Dbcontext.TicketStatusMasters on tm.TicketStatusID equals ts.TicketStatusID
                            join stt in Dbcontext.TicketSubTypes on tm.SubType equals stt.TTSID into sub
                            from st in sub.DefaultIfEmpty()
                            where (tm.CreatedBy == (rid == 1 ? tm.CreatedBy : uid) || tm.AssignTo == uid || tm.CompID == cid) && tm.TicketStatusID != 2
                            //tm.TicketStatusID!=2 && tm.CompID==(cid==0?tm.CompID:cid)

                            select new TicketModel()
                            {
                                TicketID = tm.TicketID,
                                Subject = tm.Subject,
                                Description = tm.Description,
                                AssignTo = tm.AssignTo,
                                CreatedBy = tm.CreatedBy,
                                CreatedDate = tm.CreatedDate,
                                UpdatedBy = tm.UpdatedBy,
                                UpdatedDate = tm.UpdatedDate,
                                IsActive = tm.IsActive,
                                SenderEmail = tm.SenderEmail,
                                TicketStatusID = tm.TicketStatusID,
                                TicketTypeID = tm.TicketTypeID,
                                Priority = tm.Priority,
                                Prefix = tm.Prefix,
                                UserDetails = new UserModel()
                                {
                                    Title = um.Title,
                                    FirstName = um.FirstName,
                                    LastName = um.LastName
                                },
                                CompDetails = new CompanyModel()
                                {
                                    Name = (c == null ? string.Empty : c.Name)
                                },
                                TStatusDetails = new TicketStatusModel()
                                {
                                    TicketStatus = ts.TicketStatus
                                },
                                TTypeDetails = new TicketTypeModel()
                                {
                                    TicketType = tt.TicketType,
                                    //SubType = (st == null ? string.Empty : st.SubType)
                                    SubType=(st==null?string.Empty:st.SubType)
                                }
                                //TSubTypeDetails = new TicketTypeModel()
                                //{
                                //    SubType=(st==null?string.Empty:st.SubType)
                                //}
                            }).OrderByDescending(m => m.CreatedDate).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserModel> getActiveStaff()
        {
            try
            {
                Mapper.CreateMap<UserMaster, UserModel>();
                List<UserMaster> objCityMaster = Dbcontext.UserMasters.Where(m => m.Role == 4 && m.Status == 1).ToList();
                List<UserModel> objCityItem = Mapper.Map<List<UserModel>>(objCityMaster);
                return objCityItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
