using System;
using Newtonsoft.Json;

namespace FlnBusRoutes.Shared.Domain
{
    public class BusRoute
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int AgencyId { get; set; }
        [JsonIgnore]
        public string FullName
        {
            get { return string.Format("{0} - {1}", ShortName, LongName); }
        }
    }
}