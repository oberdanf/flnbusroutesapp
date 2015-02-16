using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FlnBusRoutes.Shared.Domain;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FlnBusRoutes.Shared
{
    public class BusRoutesService
    {
        private class CustomTimeoutWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                var webRequest = base.GetWebRequest(uri);
                webRequest.Timeout = 30000; //30s
                return webRequest;
            }
        }

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
            return new CustomTimeoutWebClient
            {
                Credentials = new NetworkCredential("WKD4N7YMA1uiM8V", "DtdTtzMLQlA0hk2C1Yi5pLyVIlAQ68"),
                Headers =
                {
                    {HttpRequestHeader.ContentType, "application/json"},
                    {"X-AppGlu-Environment", "staging"}
                }
            };
        }

		//TODO: add results to a Cache system in order to not requery too often
        public async Task<IEnumerable<BusRoute>> FindRoutesByStopName(string stopName)
        {
            var jsonString = "{\"params\": {\"stopName\": \"%" + stopName + "%\"}}";
            IEnumerable<BusRoute> busRoutes = Enumerable.Empty<BusRoute>();
            
            using (var webClient = GetDefaultWebClient())
            {
                var returnValue =
                await webClient.UploadStringTaskAsync(FindRoutesByStopNameUrl, WebRequestMethods.Http.Post, jsonString);

                if (!string.IsNullOrWhiteSpace(returnValue))
                    busRoutes = JsonConvert.DeserializeObject<BusRouteJson>(returnValue).Rows;
            }

            return busRoutes;
        }

        public async Task<IEnumerable<BusStop>> FindStopsByRouteId(int routeId)
        {
            var jsonString = "{\"params\": {\"routeId\": " + routeId + "}}";
            var stops = Enumerable.Empty<BusStop>();
            using (var webClient = GetDefaultWebClient())
            {
                var returnValue =
                await webClient.UploadStringTaskAsync(FindStopsByRouteIdUrl, WebRequestMethods.Http.Post, jsonString);

                if (!string.IsNullOrWhiteSpace(returnValue))
                    stops = JsonConvert.DeserializeObject<BusStopJson>(returnValue).Rows;
            }

            return stops;
        }

        public async Task<IEnumerable<BusDeparture>> FindDeparturesByRouteId(int routeId)
        {
            var jsonString = "{\"params\": {\"routeId\": " + routeId + "}}";
            var departures = Enumerable.Empty<BusDeparture>();

            using (var webClient = GetDefaultWebClient())
            {
                var returnValue =
                    await webClient.UploadStringTaskAsync(FindDeparturesByRouteIdUrl, WebRequestMethods.Http.Post, jsonString);
                
                if (!string.IsNullOrWhiteSpace(returnValue))
                    departures = JsonConvert.DeserializeObject<BusDepartureJson>(returnValue).Rows;
            }

            return departures;
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