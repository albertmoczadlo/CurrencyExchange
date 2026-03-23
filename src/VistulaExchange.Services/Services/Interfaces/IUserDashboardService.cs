using VistulaExchange.Services.Models;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IUserDashboardService
    {
        Task<UserDashboardDto> GetDashboardAsync(string userId);
    }
}
