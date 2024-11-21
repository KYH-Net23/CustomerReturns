using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ReturnProvider.Models;
using ReturnProvider.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ReturnProvider.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReturnRepository _returnRepository;

        public ReturnService(IOrderRepository orderRepository, IReturnRepository returnRepository)
        {
            _orderRepository = orderRepository;
            _returnRepository = returnRepository;
        }

        public async Task<IEnumerable<OrderModel>> GetEligibleOrdersAsync(Guid userId)
        {
            return await _orderRepository.GetReturnableOrdersAsync(userId);
        }

        public async Task<ValidationResult> ValidateReturnRequestAsync(ReturnRequestModel request)
        {
            // Example validation logic
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null || !order.IsReturnable)
            {
                return new ValidationResult("Order not eligible for return.", new List<string> { "OrderId" });
            }

            return ValidationResult.Success!;
        }

        public async Task<Guid> CreateReturnRequestAsync(ReturnRequestModel request)
        {
            var returnRequest = new ReturnModel
            {
                ReturnId = Guid.NewGuid(),
                OrderId = request.OrderId,
                UserId = request.UserId,
                ReturnReason = request.ReturnReason,
                ResolutionType = request.ResolutionType,
                Status = "Requested",
                CreatedAt = DateTime.UtcNow
            };
            return await _returnRepository.CreateReturnAsync(returnRequest);
        }

        public async Task<byte[]> GenerateReturnLabelAsync(Guid returnId)
        {
            var returnRequest = await _returnRepository.GetReturnByIdAsync(returnId);
            if (returnRequest == null) return null;

            // Example PDF generation logic
            return await GenerateLabelPdf(returnRequest);
        }

        public async Task<ReturnStatusModel> GetReturnStatusAsync(Guid returnId)
        {
            return await _returnRepository.GetReturnStatusAsync(returnId);
        }

        public async Task<bool> UpdateReturnStatusAsync(Guid returnId, string status)
        {
            return await _returnRepository.UpdateReturnStatusAsync(returnId, status);
        }

        public async Task<byte[]> GenerateLabelPdf(ReturnModel returnRequest)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Initialize PDF writer and document
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Add title and content
                document.Add(new Paragraph("Return Label")
                    .SetFontSize(20)
                    .SimulateBold());

                document.Add(new Paragraph($"Return ID: {returnRequest.ReturnId}")
                    .SetFontSize(14));
                document.Add(new Paragraph($"Order ID: {returnRequest.OrderId}")
                    .SetFontSize(14));
                document.Add(new Paragraph($"Return Reason: {returnRequest.ReturnReason}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Resolution Type: {returnRequest.ResolutionType}")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Created At: {returnRequest.CreatedAt}")
                    .SetFontSize(12));

                document.Add(new Paragraph("\nInstructions:")
                    .SetFontSize(16)
                    .SimulateBold());
                document.Add(new Paragraph("1. Print this return label and include it in the package."));
                document.Add(new Paragraph("2. Ship the package to the nearest return center."));

                // Close the document
                document.Close();

                return memoryStream.ToArray();
            }
        }
    }
}
