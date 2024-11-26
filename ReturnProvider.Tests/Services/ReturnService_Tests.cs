using iText.Kernel.Pdf;
using Moq;
using ReturnProvider.Models;
using ReturnProvider.Repositories;
using ReturnProvider.Services;

namespace ReturnProvider.Tests.Services;

public class ReturnService_Tests
{
    private readonly Mock<IReturnRepository> _mockRepository;
    private readonly ReturnService _returnService;

    public ReturnService_Tests()
    {
        _mockRepository = new Mock<IReturnRepository>();
        _returnService = new ReturnService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateReturnRequestAsync_ShouldReturnId_WhenSuccessful()
    {
        // Arrange
        var returnRequest = new ReturnModel
        {
            OrderId = 123,
            CustomerEmail = "test@example.com",
            ReturnReason = "Damaged item",
            ResolutionType = "Refund",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.CreateReturnAsync(returnRequest))
            .ReturnsAsync(1);

        // Act
        var result = await _returnService.CreateReturnRequestAsync(returnRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetReturnByIdAsync_ShouldReturnReturnModel_WhenFound()
    {
        // Arrange
        var returnId = 1;
        var returnModel = new ReturnModel
        {
            Id = returnId,
            OrderId = 123,
            CustomerEmail = "test@example.com",
            ReturnReason = "Damaged item",
            ResolutionType = "Refund",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetReturnByIdAsync(returnId))
            .ReturnsAsync(returnModel);

        // Act
        var result = await _returnService.GetReturnByIdAsync(returnId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(returnId, result.Id);
    }

    [Fact]
    public async Task GenerateLabelPdfAsync_ShouldReturnPdfBytes_WhenReturnFound()
    {
        // Arrange
        var returnId = 1;
        var returnModel = new ReturnModel
        {
            Id = returnId,
            OrderId = 123,
            CustomerEmail = "test@example.com",
            ReturnReason = "Damaged item",
            ResolutionType = "Refund",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetReturnByIdAsync(returnId))
            .ReturnsAsync(returnModel);

        // Act
        var result = await _returnService.GenerateLabelPdfAsync(returnId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);

        using var memoryStream = new MemoryStream(result);
        using var pdfReader = new PdfReader(memoryStream);
        using var pdfDocument = new PdfDocument(pdfReader);

        var extractedText = new StringWriter();
        for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            var page = pdfDocument.GetPage(i);
            var text = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(page);
            extractedText.WriteLine(text);
        }

        var pdfContent = extractedText.ToString();
        Assert.Contains("Return Label", pdfContent);
        Assert.Contains($"Return ID: {returnId}", pdfContent);
        Assert.Contains($"Order ID: {returnModel.OrderId}", pdfContent);
        Assert.Contains($"Return Reason: {returnModel.ReturnReason}", pdfContent);
        Assert.Contains($"Resolution Type: {returnModel.ResolutionType}", pdfContent);
    }

}
