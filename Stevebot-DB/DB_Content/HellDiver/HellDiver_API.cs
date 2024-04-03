using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.IO;
using Stevebot_DB.DB_Content.HellDiver.API_Objects;

namespace Stevebot_DB.DB_Content.HellDiver
{
    public static class API {
        public static NewsFeedMessage[] GetFeed() {
            int warid = GetWarID();
            string result = CallAPI($"/api/{warid}/feed");
            return JsonConvert.DeserializeObject<NewsFeedMessage[]>(result)!;
        }
        public static Planet[] GetPlanets() {
            int warid = GetWarID();
            string result = CallAPI($"/api/{warid}/planets");
            return JsonConvert.DeserializeObject<Planet[]>(result)!;
        }
        public static Event[] GetWarEvents() {
            int warid = GetWarID();
            string result = CallAPI($"/api/{warid}/events");
            return JsonConvert.DeserializeObject<Event[]>(result)!;
        }
        public static WarStatus GetWarStatus() {
            int warid = GetWarID();
            string result = CallAPI($"/api/{warid}/status");
            File.WriteAllText("DEBUG.txt",result);
            return JsonConvert.DeserializeObject<WarStatus>(result)!;
        }
        public static int GetWarID() {
            string result = CallAPI("/api");
            WarID warID = JsonConvert.DeserializeObject<WarID>(result)!;
            if (warID == null) {
                return 0;
            }
            return warID.current;
        }
        private static string CallAPI(string uriEndpoint) {
            HttpClient client = new() {
                BaseAddress = new Uri("https://helldivers-2.fly.dev")
            };
            return client.GetAsync(uriEndpoint).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
