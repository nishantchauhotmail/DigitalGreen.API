//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DigitalGreen.Core.DataBase.DigitalGreenDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class City
    {
        public long CityId { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    
        public virtual State State { get; set; }
    }
}
