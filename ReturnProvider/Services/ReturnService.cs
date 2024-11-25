using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ReturnProvider.Models;
using ReturnProvider.Repositories;

namespace ReturnProvider.Services;

public class ReturnService(IReturnRepository returnRepository) : IReturnService
{
    public async Task<int?> CreateReturnRequestAsync(ReturnModel returnRequest)
    {
        return await returnRepository.CreateReturnAsync(returnRequest);
    }

    public async Task<ReturnModel?> GetReturnByIdAsync(int returnId)
    {
        return await returnRepository.GetReturnByIdAsync(returnId);
    }

    public async Task<byte[]> GenerateLabelPdfAsync(int returnId)
    {
        var returnRequest = await returnRepository.GetReturnByIdAsync(returnId);
        if (returnRequest == null)
        {
            throw new Exception("Return request not found.");
        }

        using (var memoryStream = new MemoryStream())
        {
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph("Return Label").SetFontSize(20).SimulateBold());
            document.Add(new Paragraph($"Return ID: {returnRequest.Id}").SetFontSize(14));
            document.Add(new Paragraph($"Order ID: {returnRequest.OrderId}").SetFontSize(14));
            document.Add(new Paragraph($"Return Reason: {returnRequest.ReturnReason}").SetFontSize(12));
            document.Add(new Paragraph($"Resolution Type: {returnRequest.ResolutionType}").SetFontSize(12));
            document.Add(new Paragraph($"Created At: {returnRequest.CreatedAt}").SetFontSize(12));

            document.Add(new Paragraph("\nInstructions:").SetFontSize(16).SimulateBold());
            document.Add(new Paragraph("1. Print this return label and include it in the package."));
            document.Add(new Paragraph("2. Ship the package to the nearest return center."));

            document.Close();

            return memoryStream.ToArray();
        }
    }
}
