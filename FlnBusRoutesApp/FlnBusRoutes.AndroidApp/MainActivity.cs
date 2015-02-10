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
    [Activity(Label = "FlnBusRoutes.AndroidApp", MainLauncher = true, Icon = "@drawable/icon")]
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
            intent.PutExtra("routeId", 32);
            intent.PutExtra("routeName", "133 - AGRONÔMICA VIA MAURO RAMOS");
            StartActivity(intent);
        }

        void OnSearchButtonClick(object sender, EventArgs e)
        {

            RoutesListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,
                         BusRoutesService.Service.FindRoutesByStopName(StreetText.Text).Select(b => string.Format("{0} - {1}", b.ShortName, b.LongName)).ToList());
        }
    }
}

