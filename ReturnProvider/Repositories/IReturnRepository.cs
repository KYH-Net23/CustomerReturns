using ReturnProvider.Models;

namespace ReturnProvider.Repositories
{
    public interface IReturnRepository
    {
        Task<Guid> CreateReturnAsync(ReturnModel returnRequest);
        Task<ReturnModel?> GetReturnByIdAsync(Guid returnId);
        Task<ReturnStatusModel?> GetReturnStatusAsync(Guid returnId);
        Task<bool> UpdateReturnStatusAsync(Guid returnId, string status);
    }
}
