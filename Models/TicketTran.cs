//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketTran
    {
        public int TTranID { get; set; }
        public Nullable<int> TicketID { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public string Comment { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
        public Nullable<int> ReplayBy { get; set; }
        public Nullable<System.DateTime> ReplayDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Prefix { get; set; }
    }
}
