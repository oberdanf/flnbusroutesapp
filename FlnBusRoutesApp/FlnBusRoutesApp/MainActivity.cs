using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace FlnBusRoutesApp
{
	[Activity (Label = "FlnBusRoutesApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			TextView textStreet = FindViewById<TextView> (Resource.Id.textStreet);
			Button button = FindViewById<Button> (Resource.Id.buttonSearch);
			
			button.Click += delegate {
				textStreet.Text = "Clicked";
			};
		}
	}
}