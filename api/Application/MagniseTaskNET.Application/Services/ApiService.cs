using MagniseTaskNET.Application.Interfaces;
using MagniseTaskNET.Core.Entities;
using MagniseTaskNET.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MagniseTaskNET.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MagniseTaskNETContext _dbContext;

        public ApiService(IHttpClientFactory httpClientFactory, MagniseTaskNETContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task<Asset> GetAssetBySymbol(string symbol)
        {
            try
            {
                var asset = await _dbContext.Assets
                    .Include(a => a.Mappings)
                    .FirstOrDefaultAsync(a => a.Symbol == symbol);

                return asset;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving asset with symbol '{symbol}': {ex.Message}", ex);
            }
        }

        public async Task<List<Asset>> GetAssetsDataAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var dataUrl = "https://platform.fintacharts.com/api/instruments/v1/instruments?provider=oanda&kind=forex";

            var response = await client.GetAsync(dataUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonObject = JObject.Parse(jsonString);
            var data = jsonObject["data"].Children();
            var assets = new List<Asset>();

            foreach (var item in data)
            {
                var assetId = Guid.Parse(item["id"].ToString());
                var existingAsset = await _dbContext.Assets.Include(a => a.Mappings)
                    .FirstOrDefaultAsync(a => a.Id == assetId);

                if (existingAsset != null)
                {
                    existingAsset.Symbol = item["symbol"].ToString();
                    existingAsset.Kind = item["kind"].ToString();
                    existingAsset.TickSize = item["tickSize"].ToObject<decimal>();
                    existingAsset.Currency = item["currency"].ToString();
                    existingAsset.BaseCurrency = item["baseCurrency"].ToString();
                    existingAsset.UpdateTime = DateTime.UtcNow;

                    _dbContext.Mappings.RemoveRange(existingAsset.Mappings);
                    foreach (var mapping in item["mappings"])
                    {
                        var mappingType = ((JProperty)mapping).Name;
                        var mappingDetails = mapping.First;

                        var newMapping = new Mapping
                        {
                            Id = Guid.NewGuid(),
                            MappingType = mappingType,
                            Symbol = mappingDetails["symbol"].ToString(),
                            Exchange = mappingDetails["exchange"].ToString(),
                            DefaultOrderSize = mappingDetails["defaultOrderSize"].ToObject<int>(),
                            AssetId = existingAsset.Id
                        };

                        existingAsset.Mappings.Add(newMapping);
                    }

                    _dbContext.Entry(existingAsset).State = EntityState.Modified;
                }
                else
                {
                    var newAsset = new Asset
                    {
                        Id = assetId,
                        Symbol = item["symbol"].ToString(),
                        Kind = item["kind"].ToString(),
                        TickSize = item["tickSize"].ToObject<decimal>(),
                        Currency = item["currency"].ToString(),
                        BaseCurrency = item["baseCurrency"].ToString(),
                        UpdateTime = DateTime.UtcNow,
                        Mappings = new List<Mapping>()
                    };

                    foreach (var mapping in item["mappings"])
                    {
                        var mappingType = ((JProperty)mapping).Name;
                        var mappingDetails = mapping.First;

                        var newMapping = new Mapping
                        {
                            Id = Guid.NewGuid(),
                            MappingType = mappingType,
                            Symbol = mappingDetails["symbol"].ToString(),
                            Exchange = mappingDetails["exchange"].ToString(),
                            DefaultOrderSize = mappingDetails["defaultOrderSize"].ToObject<int>(),
                            AssetId = newAsset.Id
                        };

                        newAsset.Mappings.Add(newMapping);
                    }

                    _dbContext.Assets.Add(newAsset); ;
                }
            }
            assets = await _dbContext.Assets.Include(a => a.Mappings).ToListAsync();

            return assets;
        }
    }
}
