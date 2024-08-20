using api.Dtos.Comment;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject);
        Task<Comment> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment> DeleteAsync(int id);
        Task<Comment?> UpdateAsync(int id, UpdateCommentDto commentDto);
    }
}
