using MeTube_DevOps.Client.Models;

namespace MeTube_DevOps.Client.Services
{
    public interface IAdminHistoryService
    {
        Task<List<HistoryAdmin>> GetHistoryByUserAsync(int userId);
        Task<HistoryAdmin?> CreateHistoryAsync(HistoryAdmin newHistory);
        Task<bool> UpdateHistoryAsync(HistoryAdmin history);
        Task<bool> DeleteHistoryAsync(int historyId);
    }
}
