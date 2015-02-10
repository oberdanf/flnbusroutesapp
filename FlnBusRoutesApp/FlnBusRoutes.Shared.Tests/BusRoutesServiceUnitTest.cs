using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlnBusRoutes.Shared;

namespace FlnBusRoutes.Shared.Tests
{
    [TestClass]
    public class BusRoutesServiceUnitTest
    {
        [TestMethod]
        public void WhenFindingRoutesForLauroLinharesStreetShouldReturnMoreThanZero()
        {
            Assert.IsTrue(BusRoutesService.Service.FindRoutesByStopName("lauro linhares").Any());
        }

        [TestMethod]
        public void WhenFindingRoutesForInexistentStreetShouldReturnZeroCount()
        {
            Assert.IsTrue(!BusRoutesService.Service.FindRoutesByStopName("asdfasdfasdf").Any());
        }
    }
}
