using Microsoft.EntityFrameworkCore;
using ReturnProvider.Models;
using ReturnProvider.Models.Entities;

namespace ReturnProvider.Repositories
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateReturnAsync(ReturnModel returnRequest)
        {
            var entity = new ReturnEntity
            {
                ReturnId = returnRequest.ReturnId,
                OrderId = returnRequest.OrderId,
                UserId = returnRequest.UserId,
                ReturnReason = returnRequest.ReturnReason,
                ResolutionType = returnRequest.ResolutionType,
                Status = returnRequest.Status,
                CreatedAt = returnRequest.CreatedAt
            };

            _context.Returns.Add(entity);
            await _context.SaveChangesAsync();
            return entity.ReturnId;
        }

        public async Task<ReturnModel?> GetReturnByIdAsync(Guid returnId)
        {
            var entity = await _context.Returns.FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (entity == null) return null;

            return new ReturnModel
            {
                ReturnId = entity.ReturnId,
                OrderId = entity.OrderId,
                UserId = entity.UserId,
                ReturnReason = entity.ReturnReason,
                ResolutionType = entity.ResolutionType,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<ReturnStatusModel?> GetReturnStatusAsync(Guid returnId)
        {
            var entity = await _context.Returns.FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (entity == null) return null;

            return new ReturnStatusModel
            {
                Status = entity.Status,
                UpdatedAt = entity.CreatedAt, // Use UpdatedAt when available
                StatusHistory = new List<string> { entity.Status } // Add actual status history if available
            };
        }


        public async Task<bool> UpdateReturnStatusAsync(Guid returnId, string status)
        {
            var entity = await _context.Returns.FirstOrDefaultAsync(r => r.ReturnId == returnId);
            if (entity == null) return false;

            entity.Status = status;
            _context.Returns.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
