using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class TicketSubTypeModel
    {
        public int TTSID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string SubType { get; set; }
        public string SubDesc { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
