using MagniseTaskNET.Application.Interfaces;
using MagniseTaskNET.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace MagniseTaskNET.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;

        private const string TokenCacheKey = "JwtToken";
        private const string TokenExpiryCacheKey = "JwtTokenExpiry";

        public AuthService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        public async Task<TokenResponse> GetJwtTokenAsync(string username, string password)
        {
            if (!_memoryCache.TryGetValue(TokenCacheKey, out TokenResponse tokenResponse) ||
                !_memoryCache.TryGetValue(TokenExpiryCacheKey, out DateTime tokenExpiry) ||
                DateTime.UtcNow >= tokenExpiry)
            {
                tokenResponse = await FetchJwtTokenAsync(username, password);

                tokenExpiry = DecodeJwtTokenExpiry(tokenResponse.AccessToken);

                _memoryCache.Set(TokenCacheKey, tokenResponse, tokenExpiry);
                _memoryCache.Set(TokenExpiryCacheKey, tokenExpiry);
            }

            return tokenResponse;
        }

        private DateTime DecodeJwtTokenExpiry(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            return expiry.ToUniversalTime();
        }

        private async Task<TokenResponse> FetchJwtTokenAsync(string username, string password)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var requestUrl = "https://platform.fintacharts.com/identity/realms/fintatech/protocol/openid-connect/token";

                var loginData = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", "app-cli" },
                    { "username", username },
                    { "password", password }
                };

                var content = new FormUrlEncodedContent(loginData);

                var response = await client.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return token;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching JWT token: {ex.Message}");
                throw;
            }
        }
    }
}
