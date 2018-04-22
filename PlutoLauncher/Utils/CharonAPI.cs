using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace PlutoLauncher.Utils
{
    class CharonAPI
    {
        public enum ReleaseChannel
        {
            STABLE,
            TESTING,
            DEVELOP
        };
        private Dictionary<ReleaseChannel, string> gatewayUrls = new Dictionary<ReleaseChannel, string>
        {
            { ReleaseChannel.STABLE, "https://g01.prod.charon.plutonium.pw" },
            { ReleaseChannel.TESTING, "https://gateway.testing.charon.plutonium.pw" },
            { ReleaseChannel.DEVELOP, "https://gateway.dev.charon.plutonium.pw" }
        };

        // Backend response (validation lacks usertoken)
        public class UserResponse
        {
            public string usertoken;
            public string username;
            public int uid;
            public string avatar;
            public List<string> permissions;
            public string rank;
        }

        // HttpClient
        private readonly HttpClient client = new HttpClient();

        // Variables
        private string _gatewayUrl;

        // Constructor
        public CharonAPI(ReleaseChannel channel)
        {
            // Set gateway url
            _gatewayUrl = gatewayUrls[channel];
        }

        // Authenticate user
        public UserResponse UserAuth(string username, string password)
        {
            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
            var response = client.PostAsync(_gatewayUrl + "/auth/user", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<UserResponse>(result);
            }

            return null;
        }

        // Validate usertoken
        public UserResponse ValidateUserToken(string usertoken)
        {
            client.DefaultRequestHeaders.Add("UserToken", usertoken);
            var response = client.GetAsync(_gatewayUrl + "/validate/usertoken").Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(result);
                userResponse.usertoken = usertoken;
                return userResponse;
            }

            return null;
        }
    }
}
