using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;

namespace FlnBusRoutes.AndroidApp
{
	[Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
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
			var addresses = geoCoder.GetFromLocation(e.Point.Latitude, e.Point.Longitude, 1); //limit query for only one item
			if (addresses.Any())
			{
				var streetName = addresses.FirstOrDefault().GetAddressLine(0).Split(',').FirstOrDefault();
				int firstIndex = streetName.IndexOf('.');
				streetName = streetName.Substring(firstIndex != -1 && firstIndex + 1 < streetName.Length ? firstIndex + 1 : 0).Trim();
				if (!string.IsNullOrWhiteSpace(streetName))
				{
					var answer = await ShowOkCancelPopupDialog(string.Format(GetString(Resource.String.going_to_search_routes_for_street), streetName));
					if (answer == DialogButtonType.Positive)
					{
						var intent = new Intent(this, typeof(MainActivity));
						intent.PutExtra(GetString(Resource.String.street_name_param), streetName);
						StartActivity(intent);
					}
				}
			}
		}

		private Task<DialogButtonType> ShowOkCancelPopupDialog(string message)
		{
			var tcs = new TaskCompletionSource<DialogButtonType>();
			var dialogBuilder = new AlertDialog.Builder(this);
			var dialog = dialogBuilder
				.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon)
				.SetMessage(message)
				.SetCancelable(false)
				.SetPositiveButton(Resource.String.ok, (sender, e) => tcs.SetResult(DialogButtonType.Positive))
				.SetNegativeButton(Resource.String.cancel, (sender, e) => tcs.SetResult(DialogButtonType.Negative))
				.Create();
			dialog.Show();
			return tcs.Task;
		}
	}
}