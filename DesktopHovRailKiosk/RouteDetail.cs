//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DesktopHovRailKiosk
{
    using System;
    using System.Collections.Generic;
    
    public partial class RouteDetail
    {
        public int routeDetailID { get; set; }
        public int routeID { get; set; }
        public int destinationStationID { get; set; }
        public int stationSequenceNo { get; set; }
        public decimal travelHour { get; set; }
    
        public virtual Route Route { get; set; }
        public virtual Station Station { get; set; }
    }
}
