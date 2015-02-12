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
using FlnBusRoutes.Shared.Domain;

namespace FlnBusRoutes.AndroidApp
{
    [Activity(Label = "Bus Routes", Icon = "@drawable/icon")]
    public class RouteTrackActivity : Activity
    {
        public TextView RouteNameTextView { get; set; }
        public ListView RouteTrackListView { get; set; }
        public GridView WeekdayRouteTimetableGridView { get; set; }
        public GridView SaturdayRouteTimetableGridView { get; set; }
        public GridView SundayRouteTimetableGridView { get; set; }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RouteDetails);

            RouteNameTextView = FindViewById<TextView>(Resource.Id.routeNameTextView);
            WeekdayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.weekdayRouteTimetableGridView);
            SaturdayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.saturdayRouteTimetableGridView);
            SundayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.sundayRouteTimetableGridView);
            RouteTrackListView = FindViewById<ListView>(Resource.Id.routeTrackListView);

            RouteNameTextView.Text = Intent.GetStringExtra("routeFullName");
            var routeId = Intent.GetIntExtra("routeId", -1);

            var departuresByRouteId = await BusRoutesService.Service.FindDeparturesByRouteId(routeId);

            var busDepartures = departuresByRouteId.ToList(); //create new reference to avoid multiple enumerations

            var weekdayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "weekday").OrderBy(s => s.Time);
            var saturdayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "saturday").OrderBy(s => s.Time);
            var sundayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "sunday").OrderBy(s => s.Time);

            WeekdayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow, weekdayDepartures.Select(s => s.Time).ToList());
            SaturdayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow, saturdayDepartures.Select(s => s.Time).ToList());
            SundayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow, sundayDepartures.Select(s => s.Time).ToList());

            IEnumerable<BusStop> findStopsByRouteId = await BusRoutesService.Service.FindStopsByRouteId(routeId);
            RouteTrackListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,
                findStopsByRouteId
                    .OrderBy(s => s.Sequence)
                    .Select(s => s.Description).ToList());
        }
    }
}