//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DesktopMotorcycleRepair
{
    using System;
    using System.Collections.Generic;
    
    public partial class MotorcycleServices
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MotorcycleServices()
        {
            this.DetailService = new HashSet<DetailService>();
        }
    
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> Cost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailService> DetailService { get; set; }
    }
}
