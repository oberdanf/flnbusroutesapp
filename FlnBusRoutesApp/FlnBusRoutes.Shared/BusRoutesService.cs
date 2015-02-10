using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FlnBusRoutes.Shared.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Json;

namespace FlnBusRoutes.Shared
{
    public class BusRoutesService
    {
        private const string FindRoutesByStopNameUrl = "https://api.appglu.com/v1/queries/findRoutesByStopName/run";
        private const string FindStopsByRouteIdUrl = "https://api.appglu.com/v1/queries/findStopsByRouteId/run";
        private const string FindDeparturesByRouteIdUrl = "https://api.appglu.com/v1/queries/findDeparturesByRouteId/run";

        private static BusRoutesService _busRoutesService;

        public static BusRoutesService Service
        {
            get { return _busRoutesService ?? (_busRoutesService = new BusRoutesService()); }
        }

        private WebClient GetDefaultWebClient()
        {
            return new WebClient
            {
                Credentials = new NetworkCredential("WKD4N7YMA1uiM8V", "DtdTtzMLQlA0hk2C1Yi5pLyVIlAQ68"),
                Headers =
                {
                    {HttpRequestHeader.ContentType, "application/json"},
                    {"X-AppGlu-Environment", "staging"}
                }
            };
        }

        public IEnumerable<BusRoute> FindRoutesByStopName(string stopName)
        {
            string returnValue;

            var jsonString = "{\"params\": {\"stopName\": \"%" + stopName + "%\"}}";

            using (var webClient = GetDefaultWebClient())
                returnValue = webClient.UploadString(FindRoutesByStopNameUrl, WebRequestMethods.Http.Post, jsonString);

            if (!string.IsNullOrWhiteSpace(returnValue))
                return JsonConvert.DeserializeObject<BusRouteJson>(returnValue).Rows;

            return Enumerable.Empty<BusRoute>();
        }

        public IEnumerable<BusStop> FindStopsByRouteId(int routeId)
        {
            string returnValue;

            var jsonString = "{\"params\": {\"routeId\": " + routeId + "}}";

            using (var webClient = GetDefaultWebClient())
                returnValue = webClient.UploadString(FindStopsByRouteIdUrl, WebRequestMethods.Http.Post, jsonString);

            if (!string.IsNullOrWhiteSpace(returnValue))
                return JsonConvert.DeserializeObject<BusStopJson>(returnValue).Rows;

            return Enumerable.Empty<BusStop>();
        }

        public IEnumerable<BusDeparture> FindDeparturesByRouteId(int routeId)
        {
            string returnValue;

            var jsonString = "{\"params\": {\"routeId\": " + routeId + "}}";

            using (var webClient = GetDefaultWebClient())
                returnValue = webClient.UploadString(FindDeparturesByRouteIdUrl, WebRequestMethods.Http.Post, jsonString);

            if (!string.IsNullOrWhiteSpace(returnValue))
                return JsonConvert.DeserializeObject<BusDepartureJson>(returnValue).Rows;

            return Enumerable.Empty<BusDeparture>();
        }

        /*
         * Due to Xamarin known limitations I can't create a generic method like this
         * See Generic Methods are not allowed: http://developer.xamarin.com/guides/ios/under_the_hood/api_design/nsobject_generics/
        private IEnumerable<T> DoPostWithJson(string url, string jsonString)
        {
            string returnValue;
            using (var webClient = GetDefaultWebClient())
               returnValue = webClient.UploadString(FindStopsByRouteIdUrl, WebRequestMethods.Http.Post, jsonString);
            if (!string.IsNullOrWhiteSpace(returnValue))
                return JsonConvert.DeserializeObject<JsonGeneric<T>>(returnValue).Rows;
        }*/
    }
}