//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KFIRKK
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransferLog
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public System.DateTime Date { get; set; }
        public double TransferAmount { get; set; }
        public string TransferLogText { get; set; }
        public string OtherClientName { get; set; }
        public Nullable<double> PointsAfterTransfer { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
