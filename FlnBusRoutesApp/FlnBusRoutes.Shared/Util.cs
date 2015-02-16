using System;
using System.Linq;
using System.Collections.Generic;

namespace FlnBusRoutes.Shared
{
	public static class Util
	{
		public static IList<string> EmptyEnumerableToDash(IList<string> enumerable)
		{
			return enumerable.Any() ? enumerable : new List<string> { "-" };
		}
	}
}