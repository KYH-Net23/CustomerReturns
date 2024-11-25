using Microsoft.AspNetCore.Mvc;
using ReturnProvider.Models;
using ReturnProvider.Services;

namespace ReturnProvider.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnController(IReturnService returnService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SubmitReturnRequest([FromBody] ReturnModel returnRequest)
    {
        if (returnRequest == null)
            return BadRequest("Request body is null.");

        var returnId = await returnService.CreateReturnRequestAsync(returnRequest);

        return Ok(new { ReturnId = returnId, Message = "Return request submitted successfully." });
    }

    [HttpGet("{returnId}")]
    public async Task<IActionResult> GetReturn(int returnId)
    {
        var returnData = await returnService.GetReturnByIdAsync(returnId);
        if (returnData == null)
            return NotFound("Return request not found.");

        return Ok(returnData);
    }

    //[HttpGet("eligible-orders/{userId}")]
    //public async Task<IActionResult> GetEligibleOrders(Guid userId)
    //{
    //    var orders = await _orderRepository.GetReturnableOrdersAsync(userId);
    //    if (!orders.Any())
    //        return NotFound("No eligible orders found.");

    //    return Ok(orders);
    //}

    [HttpGet("label/{returnId}")]
    public async Task<IActionResult> GenerateReturnLabel(int returnId)
    {
        try
        {
            var labelPdf = await returnService.GenerateLabelPdfAsync(returnId);
            return File(labelPdf, "application/pdf", "ReturnLabel.pdf");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
