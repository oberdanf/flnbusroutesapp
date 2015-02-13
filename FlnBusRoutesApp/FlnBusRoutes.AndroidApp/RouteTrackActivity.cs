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
using FlnBusRoutes.AndroidApp.Utils;
using FlnBusRoutes.Shared;

namespace FlnBusRoutes.AndroidApp
{
    [Activity(Label = "Bus Routes", Icon = "@drawable/icon")]
	public class RouteTrackActivity : Activity, IGenericActivity
    {
        public TextView RouteNameTextView { get; set; }
        public ListView RouteTrackListView { get; set; }
        public GridView WeekdayRouteTimetableGridView { get; set; }
        public GridView SaturdayRouteTimetableGridView { get; set; }
        public GridView SundayRouteTimetableGridView { get; set; }
		public Button BackButton { get; set; }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RouteDetails);

            FindViews();
			SetEvents();

            RouteNameTextView.Text = Intent.GetStringExtra("routeFullName");
            var routeId = Intent.GetIntExtra("routeId", -1);

            await LoadRouteTimetableByRouteId(routeId);
            await LoadRouteStopsByRouteId(routeId);

            UpdateLayout();
        }

		#region IGenericActivity implementation

		public void FindViews()
		{
			RouteNameTextView = FindViewById<TextView>(Resource.Id.routeNameTextView);
			WeekdayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.weekdayRouteTimetableGridView);
			SaturdayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.saturdayRouteTimetableGridView);
			SundayRouteTimetableGridView = FindViewById<GridView>(Resource.Id.sundayRouteTimetableGridView);
			RouteTrackListView = FindViewById<ListView>(Resource.Id.routeTrackListView);
			BackButton = FindViewById<Button> (Resource.Id.backButton);
		}

		public void SetEvents()
		{
			BackButton.Click += BackButtonClick;
		}

		public void UpdateLayout()
		{
			LayoutUtil.SetGridViewHeightBasedOnChildren(WeekdayRouteTimetableGridView);
			LayoutUtil.SetGridViewHeightBasedOnChildren(SaturdayRouteTimetableGridView);
			LayoutUtil.SetGridViewHeightBasedOnChildren(SundayRouteTimetableGridView);
			LayoutUtil.SetListViewHeightBasedOnChildren(RouteTrackListView);
		}

		#endregion

		private void BackButtonClick (object sender, EventArgs e)
		{
			base.OnBackPressed ();
		}

        private async Task LoadRouteStopsByRouteId(int routeId)
        {
            try
            {
                var findStopsByRouteId = await BusRoutesService.Service.FindStopsByRouteId(routeId);
                RouteTrackListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1,
                    findStopsByRouteId.OrderBy(s => s.Sequence).Select(s => s.Description).ToList());
            }
            catch (Exception)
            {
                Toast.MakeText(this, "An error ocurred whilst trying to load bus stops!", ToastLength.Long).Show();
            }
        }

        private async Task LoadRouteTimetableByRouteId(int routeId)
        {
            try
            {
                var departuresByRouteId = await BusRoutesService.Service.FindDeparturesByRouteId(routeId);
                var busDepartures = departuresByRouteId.ToList(); //create new reference to avoid multiple enumerations

                var weekdayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "weekday").OrderBy(s => s.Time);
                var saturdayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "saturday").OrderBy(s => s.Time);
                var sundayDepartures = busDepartures.Where(d => d.Calendar.ToLower() == "sunday").OrderBy(s => s.Time);

                WeekdayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow,
                    weekdayDepartures.Select(s => s.Time).ToList());
                SaturdayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow,
                    saturdayDepartures.Select(s => s.Time).ToList());
                SundayRouteTimetableGridView.Adapter = new ArrayAdapter<String>(this, Resource.Layout.BusTimetableRow,
                    sundayDepartures.Select(s => s.Time).ToList());
            }
            catch (Exception)
            {
				Toast.MakeText(this, "An error ocurred whilst trying to load the route timetable!", ToastLength.Long).Show();
            }
        }
    }
}