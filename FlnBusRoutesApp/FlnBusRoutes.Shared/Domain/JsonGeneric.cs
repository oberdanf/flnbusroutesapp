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

namespace FlnBusRoutes.Shared.Domain
{
    public abstract class JsonGeneric<T>
    {
        public List<T> Rows { get; set; }
        public int RowsAffected { get; set; }
    }
}