using ReturnProvider.Models;
using System.ComponentModel.DataAnnotations;

namespace ReturnProvider.Services
{
    public interface IReturnService
    {
        Task<IEnumerable<OrderModel>> GetEligibleOrdersAsync(Guid userId);
        Task<ValidationResult> ValidateReturnRequestAsync(ReturnRequestModel request);
        Task<Guid> CreateReturnRequestAsync(ReturnRequestModel request);
        Task<byte[]> GenerateReturnLabelAsync(Guid returnId);
        Task<ReturnStatusModel> GetReturnStatusAsync(Guid returnId);
        Task<bool> UpdateReturnStatusAsync(Guid returnId, string status);
    }
}
