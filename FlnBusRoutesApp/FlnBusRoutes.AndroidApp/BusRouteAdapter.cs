using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using FlnBusRoutes.Shared.Domain;

namespace FlnBusRoutes.AndroidApp
{
    public class BusRouteAdapter : BaseAdapter<BusRoute>
    {
        private Context _context;
        private int _rowLayout;
        private IEnumerable<BusRoute> _busRoutes;

        public BusRouteAdapter(Context context, int rowLayout, IEnumerable<BusRoute> busRoutes)
        {
            _context = context;
            _rowLayout = rowLayout;
            _busRoutes = busRoutes;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View currentRow = convertView ?? LayoutInflater.From(_context).Inflate(_rowLayout, parent, false);

            var busRoute = _busRoutes.ElementAt(position);
            if (busRoute != null)
            {
                var routeNameText = currentRow.FindViewById<TextView>(Resource.Id.routeNameTextView);
                routeNameText.Text = string.Format("{0} - {1}", busRoute.ShortName, busRoute.LongName);
            }
            return currentRow;
        }

        public override int Count
        {
            get { return _busRoutes.Count(); }
        }

        public override BusRoute this[int position]
        {
            get { return _busRoutes.ElementAt(position); }
        }
    }
}