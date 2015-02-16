using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FlnBusRoutes.Shared;
using FlnBusRoutes.Shared.Domain;
using Android.Net;
using Android.Gms.Common.Data;

namespace FlnBusRoutes.AndroidApp
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IGenericActivity
	{
		public TextView StreetText { get; set; }

		public Button SearchButton { get; set; }

		public Button SearchWithGoogleMapsButton { get; set; }

		public ListView RoutesListView { get; set; }

		public ProgressBar ProgressBar { get; set; }

		private bool HasInternetConnection 
		{
			get 
			{
				try
				{
					var connectivityManager = GetSystemService(ConnectivityService) as ConnectivityManager;
					var activeConnection = connectivityManager.ActiveNetworkInfo;
					return ((activeConnection != null) && activeConnection.IsConnected);
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			FindViews();
			SetEvents();

			string streetName = Intent.GetStringExtra(GetString(Resource.String.street_name_param));
			if (!string.IsNullOrWhiteSpace((streetName)))
			{
				streetName = streetName.Trim();
				StreetText.Text = streetName;
				QueryRoutes(streetName);
			}
			UpdateLayout();
		}

		#region IGenericActivity implementation

		public void FindViews()
		{
			StreetText = FindViewById<TextView>(Resource.Id.textStreet);
			SearchButton = FindViewById<Button>(Resource.Id.buttonSearch);
			SearchWithGoogleMapsButton = FindViewById<Button>(Resource.Id.buttonSearchWithGoogleMaps);
			RoutesListView = FindViewById<ListView>(Resource.Id.routesListView);
			ProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBarMain);
		}

		public void SetEvents()
		{
			SearchButton.Click += OnSearchButtonClick;
			SearchWithGoogleMapsButton.Click += OnSearchWithGoogleMapsClick;
			RoutesListView.ItemClick += OnRoutesListViewItemClick;
		}

		public void UpdateLayout()
		{
			//do nothing yet
		}

		#endregion

		void OnRoutesListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var intent = new Intent(this, typeof(RouteTrackActivity));
			var busRouteAdapter = RoutesListView.Adapter as BusRouteAdapter;
			if (busRouteAdapter != null)
			{
				var busRoute = busRouteAdapter[e.Position];
				intent.PutExtra(GetString(Resource.String.route_id), busRoute.Id);
				intent.PutExtra(GetString(Resource.String.route_full_name), busRoute.FullName);
			}

			StartActivity(intent);
		}

		void OnSearchButtonClick(object sender, EventArgs e)
		{
			try
			{
				QueryRoutes(StreetText.Text);
			} catch (Exception) {
				Toast.MakeText(this, GetString(Resource.String.error_search_routes), ToastLength.Long).Show();
			}
		}

		void OnSearchWithGoogleMapsClick(object sender, EventArgs e)
		{
			StartActivity(new Intent(this, typeof(GoogleMapsActivity)));
		}

		async void QueryRoutes(string street)
		{
			if (HasInternetConnection)
			{
				if (!string.IsNullOrWhiteSpace(street))
				{
					RoutesListView.Adapter = null;
					ProgressBar.Visibility = ViewStates.Visible;
					var routesByStopName = await BusRoutesService.Service.FindRoutesByStopName(street);
					ProgressBar.Visibility = ViewStates.Gone;
					if (!routesByStopName.Any())
						Toast.MakeText(this, string.Format(GetString(Resource.String.no_routes_found_for_street), street), ToastLength.Long).Show();
					RoutesListView.Adapter = new BusRouteAdapter(this, Resource.Layout.BusRouteRow, routesByStopName);
				}
				else
					Toast.MakeText(this, GetString(Resource.String.please_type_street), ToastLength.Long).Show();
			}
			else
				Toast.MakeText(this, GetString(Resource.String.error_no_internet), ToastLength.Long).Show();
		}
	}
}