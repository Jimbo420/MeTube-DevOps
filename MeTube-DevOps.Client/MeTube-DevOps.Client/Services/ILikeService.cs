using MeTube_DevOps.Client.Models;

namespace MeTube_DevOps.Client.Services
{
    public interface ILikeService
    {
        Task<bool> AddLikeAsync(int videoId);
        Task<bool> RemoveLikeAsync(int videoId);
        Task<bool> HasUserLikedVideoAsync(int videoId);
        Task<int> GetLikeCountForVideoAsync(int videoId);
        Task<IEnumerable<Like>> GetLikesForVideoManagementAsync(int videoId);

        // Removing likes for a video as an admin
        Task RemoveLikesForVideoAsync(int videoId, int userId);

    }
}