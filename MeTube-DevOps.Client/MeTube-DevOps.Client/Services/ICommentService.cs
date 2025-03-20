using MeTube_DevOps.Client.Models;
using MeTube_DevOps.Client.DTO.CommentDTOs;

namespace MeTube_DevOps.Client.Services
{
    public interface ICommentService
    {
        Task<List<Comment>?> GetCommentsByVideoIdAsync(int videoId);
        Task<Comment?> AddCommentAsync(CommentDto commentDto);
        Task<Comment?> UpdateCommentAsync(CommentDto commentDto);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<string?> GetPosterUsernameAsync(int userId);
    }
}