using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RealEstate.Tests.Integration
{
    public static class AuthHelper
    {
        public static async Task<string> GetTokenAsync(HttpClient client)
        {
            var resp = await client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "1234" });
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadFromJsonAsync<TokenResponse>();
            return json!.token;
        }

        private class TokenResponse
        {
            public string token { get; set; } = "";
            public DateTime expiration { get; set; }
        }
    }
}
