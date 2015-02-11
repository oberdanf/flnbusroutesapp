using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FlnBusRoutes.Shared;
using FlnBusRoutes.Shared.Domain;

namespace FlnBusRoutes.AndroidApp
{
    [Activity(Label = "FLN Bus Routes", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public TextView StreetText { get; set; }
        public Button SearchButton { get; set; }
        public ListView RoutesListView { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            StreetText = FindViewById<TextView>(Resource.Id.textStreet);
            SearchButton = FindViewById<Button>(Resource.Id.buttonSearch);
            RoutesListView = FindViewById<ListView>(Resource.Id.routesListView);

            SearchButton.Click += OnSearchButtonClick;
            RoutesListView.ItemClick += OnRoutesListViewItemClick;
        }

        void OnRoutesListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof (RouteTrackActivity));
            var busRouteAdapter = RoutesListView.Adapter as BusRouteAdapter;
            if (busRouteAdapter != null)
            {
                var busRoute = busRouteAdapter[e.Position];
                intent.PutExtra("routeId", busRoute.Id);
                intent.PutExtra("routeFullName", busRoute.FullName);
            }

            StartActivity(intent);
        }

        void OnSearchButtonClick(object sender, EventArgs e)
        {
            var busRouteAdapter = new BusRouteAdapter(this, Resource.Layout.BusRouteRow,
                BusRoutesService.Service.FindRoutesByStopName(StreetText.Text));

            RoutesListView.Adapter = busRouteAdapter;
        }
    }
}

