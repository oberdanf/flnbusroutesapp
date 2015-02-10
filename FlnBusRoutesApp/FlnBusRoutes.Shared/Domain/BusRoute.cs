using System;

namespace FlnBusRoutes.Shared.Domain
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int AgencyId { get; set; }
    }
}