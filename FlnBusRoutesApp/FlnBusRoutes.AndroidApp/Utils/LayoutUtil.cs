using System;
using Android.Widget;

namespace FlnBusRoutes.AndroidApp.Utils
{
	public static class LayoutUtil
	{
		public static void SetGridViewHeightBasedOnChildren(GridView view) {
            int count = view.Adapter != null ? (int)Math.Ceiling((double)view.Adapter.Count / view.NumColumns) : 0;
			SetAbsListViewHeightBasedOnChildren (view, count, view.VerticalSpacing);
		}

		public static void SetListViewHeightBasedOnChildren(ListView view) {
			SetAbsListViewHeightBasedOnChildren (view, view.Count, view.DividerHeight);
		}

		private static void SetAbsListViewHeightBasedOnChildren(AbsListView view, int iteratorCount, int divider) {
		    var adapter = view.Adapter;
		    if (adapter == null)
		        return;

		    var totalHeight = 0;
		    for (var i = 0; i < iteratorCount; i++)
		    {
		        var listItem = adapter.GetView(i, null, view);
		        listItem.Measure(0, 0);
		        totalHeight += listItem.MeasuredHeight;
		    }

		    var layoutParams = view.LayoutParameters;
		    layoutParams.Height = totalHeight + (divider*(iteratorCount - 1));
		    view.LayoutParameters = layoutParams;
		    view.RequestLayout();
		}
	}
}