using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace FlnBusRoutes.Shared.Domain
{
    public class BusStop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public int RouteId { get; set; }
        [JsonIgnore]
        public string Description
        {
            get { return string.Format("{0} - {1}", Sequence, Name); }
        }
    }
}