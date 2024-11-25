using ReturnProvider.Models;

namespace ReturnProvider.Repositories;

public interface IReturnRepository
{
    Task<int?> CreateReturnAsync(ReturnModel returnRequest);
    Task<ReturnModel?> GetReturnByIdAsync(int returnId);
}
