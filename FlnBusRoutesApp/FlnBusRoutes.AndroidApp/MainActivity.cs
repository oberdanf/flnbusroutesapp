using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	public class MainActivity : Activity, IGenericActivity
    {
        public TextView StreetText { get; set; }
		public Button SearchButton { get; set; }
		public Button SearchWithGoogleMapsButton { get; set; }
        public ListView RoutesListView { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
			FindViews ();
			SetEvents ();

			string streetName = Intent.GetStringExtra("streetName");
			if (!string.IsNullOrWhiteSpace((streetName)))
				QueryRoutes(streetName.Trim());
        }

		#region IGenericActivity implementation

		public void FindViews ()
		{
			StreetText = FindViewById<TextView>(Resource.Id.textStreet);
			SearchButton = FindViewById<Button>(Resource.Id.buttonSearch);
			SearchWithGoogleMapsButton = FindViewById<Button>(Resource.Id.buttonSearchWithGoogleMaps);
			RoutesListView = FindViewById<ListView>(Resource.Id.routesListView);
		}

		public void SetEvents ()
		{
			SearchButton.Click += OnSearchButtonClick;
			SearchWithGoogleMapsButton.Click += OnSearchWithGoogleMapsClick;
			RoutesListView.ItemClick += OnRoutesListViewItemClick;
		}

		public void UpdateLayout ()
		{
			//do nothing yet
		}

		#endregion

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
            try
            {
				QueryRoutes(StreetText.Text);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "An error ocurred whilst trying to search routes.", ToastLength.Long).Show();
            }
        }

		void OnSearchWithGoogleMapsClick(object sender, EventArgs e)
		{
			StartActivity (new Intent (this, typeof(GoogleMapsActivity)));
		}

		private async void QueryRoutes(string street)
		{
			if(!string.IsNullOrWhiteSpace(street))
			{
				var routesByStopName = await BusRoutesService.Service.FindRoutesByStopName(street);
				if(routesByStopName.Any())
					RoutesListView.Adapter = new BusRouteAdapter(this, Resource.Layout.BusRouteRow, routesByStopName);
				else
					Toast.MakeText(this, string.Format("No routes found for street '{0}'.", street), ToastLength.Long).Show();
			}
			else
				Toast.MakeText(this, "Please, type a street name", ToastLength.Long).Show();
		}
    }
}