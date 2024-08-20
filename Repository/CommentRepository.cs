using api.Data;
using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            //return await _context.Comments.ToListAsync();
            var comments = _context.Comments.Include(a => a.AppUser)
                .AsQueryable();
            if (string.IsNullOrWhiteSpace(queryObject.Symbol))
            {
                comments = comments.Where(s=>s.Stock.Symbol == queryObject.Symbol);
            };
            if(queryObject.IsDescending == true)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            //return await _context.Comments.FindAsync(id);
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(i => i.Id == id);

        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FindAsync(id);
            if(commentModel == null) 
                return null;
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto update)
        {
            var commentModel = await _context.Comments.FindAsync(id);
            if(commentModel == null) return null;
            commentModel.Title = update.Title;
            commentModel.Content = update.Content;
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}
