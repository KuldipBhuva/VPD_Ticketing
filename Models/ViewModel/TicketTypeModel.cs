using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketTypeModel
    {
        //[Required(ErrorMessage = "Category Required")]
        //public int TicketTypeID { get; set; }
        //[Required(ErrorMessage = "Category Required")]
        //public string TicketType { get; set; }
        //public Nullable<bool> IsActive { get; set; }
        //public string Description { get; set; }

        public int TicketTypeID { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public string TicketType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Description { get; set; }

        #region SubType
        public int TTSID { get; set; }
        [Required(ErrorMessage = "Sub Category Required")]
        public string SubType { get; set; }
        public Nullable<bool> Status { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public Nullable<int> TypeID { get; set; }
        public string SubDesc { get; set; }
        #endregion

        public List<TicketTypeModel> ListCat { get;set;}
        public List<TicketTypeModel> ListSubCat { get; set; }
        public List<TicketTypeModel> ListActiveCat { get; set; }
        public TicketSubTypeModel SubTypeDetails { get; set; }
    }
}
