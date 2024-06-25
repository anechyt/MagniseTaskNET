using MagniseTaskNET.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MagniseTaskNET.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IApiService _apiService;
        private readonly IMemoryCache _memoryCache;

        public AssetsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("assets")]
        public async Task<IActionResult> GetInstruments(string bearerToken)
        {
            var assets = await _apiService.GetAssetsDataAsync(bearerToken);

            return Ok(assets);
        }

        [HttpGet("asset/{symbol}")]
        public async Task<IActionResult> GetAssetBySymbol(string symbol)
        {
            symbol = WebUtility.UrlDecode(symbol);
            var asset = await _apiService.GetAssetBySymbol(symbol);

            return Ok(asset);
        }
    }
}
