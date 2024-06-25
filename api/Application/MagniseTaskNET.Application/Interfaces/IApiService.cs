using MagniseTaskNET.Core.Entities;

namespace MagniseTaskNET.Application.Interfaces
{
    public interface IApiService
    {
        Task<List<Asset>> GetAssetsDataAsync(string token);
        Task<Asset> GetAssetBySymbol(string symbol);
    }
}
