using ReturnProvider.Models;
using ReturnProvider.Models.Entities;

namespace ReturnProvider.Repositories;

public class ReturnRepository(ApplicationDbContext context) : IReturnRepository
{
    public async Task<int?> CreateReturnAsync(ReturnModel returnRequest)
    {
        var entity = new ReturnEntity
        {
            OrderId = returnRequest.OrderId,
            CustomerEmail = returnRequest.CustomerEmail,
            ReturnReason = returnRequest.ReturnReason,
            ResolutionType = returnRequest.ResolutionType,
            Status = returnRequest.Status,
            CreatedAt = returnRequest.CreatedAt
        };

        context.Returns.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<ReturnModel?> GetReturnByIdAsync(int returnId)
    {
        var entity = await context.Returns.FindAsync(returnId);
        if (entity == null) return null;

        return new ReturnModel
        {
            Id = entity.Id,
            OrderId = entity.OrderId,
            CustomerEmail = entity.CustomerEmail,
            ReturnReason = entity.ReturnReason,
            ResolutionType = entity.ResolutionType,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}
