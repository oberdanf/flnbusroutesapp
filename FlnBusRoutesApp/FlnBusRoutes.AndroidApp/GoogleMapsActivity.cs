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
using Android.GoogleMaps;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;

namespace FlnBusRoutes.AndroidApp
{
	[Activity(Label = "GoogleMapsActivity")]			
	public class GoogleMapsActivity : Activity, IGenericActivity, IOnMapReadyCallback
	{
		private GoogleMap GoogleMap { get; set; }

		private static readonly LatLng FlorianopolisLatLng = new LatLng(-27.593064, -48.543764);

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.GoogleMapsLayout);

			FindViews();
			SetEvents();
			UpdateLayout();
		}

		#region IGenericActivity implementation

		public void FindViews()
		{
			var map = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googleMapsFragment);
			if (GoogleMap == null)
				FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googleMapsFragment).GetMapAsync(this);
		}

		public void SetEvents()
		{

		}

		public void UpdateLayout()
		{
		}

		#endregion

		public void OnMapReady(GoogleMap googleMap)
		{
			googleMap.MapType = GoogleMap.MapTypeNormal;
			googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(FlorianopolisLatLng, 16));
			googleMap.MapClick += OnGoogleMapClick;
			GoogleMap = googleMap;
		}

		private async void OnGoogleMapClick(object sender, GoogleMap.MapClickEventArgs e)
		{ 
			var geoCoder = new Geocoder(this);
			var addresses = await geoCoder.GetFromLocationAsync(e.Point.Latitude, e.Point.Longitude, 1); //limit query for only one item
			if (addresses.Any()) {
				var streetName = addresses.FirstOrDefault().GetAddressLine(0).Split(',').FirstOrDefault();
				var intent = new Intent(this, typeof(MainActivity));
				intent.PutExtra("streetName", streetName);

				StartActivity(intent);
			}
		}
	}
}