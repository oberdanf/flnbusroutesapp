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
using FlnBusRoutes.Shared;

namespace FlnBusRoutes.AndroidApp
{
    [Activity(Label = "RouteTrackActivity")]
    public class RouteTrackActivity : Activity
    {
        public TextView RouteNameTextView { get; set; }
        public ListView RouteTrackListView { get; set; }
        public ListView RouteTimetableGridView { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RouteDetails);

            RouteNameTextView = FindViewById<TextView>(Resource.Id.routeNameText);
            RouteTrackListView = FindViewById<ListView>(Resource.Id.routeTrackListView);
            RouteTimetableGridView = FindViewById<ListView>(Resource.Id.routeTimetableGridView);

            RouteNameTextView.Text = Intent.GetStringExtra("routeName");
            var routeId = Intent.GetIntExtra("routeId", -1);

            RouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,
                BusRoutesService.Service.FindDeparturesByRouteId(routeId)
                    .OrderBy(s => s.Calendar).ThenBy(s => s.Time)
                    .Select(s => s.Time).ToList());

            RouteTrackListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,
                BusRoutesService.Service.FindStopsByRouteId(routeId)
                    .OrderBy(s => s.Sequence)
                    .Select(s => string.Format("{0} - {1}", s.Sequence, s.Name)).ToList());
        }
    }
}