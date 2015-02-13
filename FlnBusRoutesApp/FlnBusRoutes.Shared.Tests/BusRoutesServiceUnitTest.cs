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
        public async void WhenFindingRoutesForLauroLinharesStreetShouldReturnMoreThanZero()
        {
            var list = await BusRoutesService.Service.FindRoutesByStopName("lauro linhares");
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public async void WhenFindingRoutesForInexistentStreetShouldReturnZeroCount()
        {
            var list = await BusRoutesService.Service.FindRoutesByStopName("asdfasdfasdf");
            Assert.IsTrue(!list.Any());
        }
    }
}
