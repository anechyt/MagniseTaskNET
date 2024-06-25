using MagniseTaskNET.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;

namespace MagniseTaskNET.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> GetJwtTokenAsync(string username, string password);
    }
}
