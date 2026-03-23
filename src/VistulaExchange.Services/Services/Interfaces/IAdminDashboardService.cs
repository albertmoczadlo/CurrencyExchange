using VistulaExchange.Services.Models;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();
    }
}
