using MeTube_DevOps.Client.Models;

namespace MeTube_DevOps.Client.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<History>> GetUserHistoryAsync();
        Task AddHistoryAsync(History history);
    }
}
