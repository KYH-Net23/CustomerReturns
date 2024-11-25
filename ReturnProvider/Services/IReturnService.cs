using ReturnProvider.Models;

namespace ReturnProvider.Services;

public interface IReturnService
{
    Task<int?> CreateReturnRequestAsync(ReturnModel returnRequest);
    Task<ReturnModel?> GetReturnByIdAsync(int returnId);
    Task<byte[]> GenerateLabelPdfAsync(int returnId);
}
