using Microsoft.AspNetCore.Mvc;
using ReturnProvider.Models;
using ReturnProvider.Repositories;
using ReturnProvider.Services;

namespace ReturnProvider.Controllers
{
    [ApiController]
    [Route("api/returns")]
    public class ReturnController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReturnService _returnService;

        public ReturnController(IOrderRepository orderRepository, IReturnService returnService)
        {
            _orderRepository = orderRepository;
            _returnService = returnService;
        }

        [HttpGet("eligible-orders/{userId}")]
        public async Task<IActionResult> GetEligibleOrders(Guid userId)
        {
            var orders = await _orderRepository.GetReturnableOrdersAsync(userId);
            if (!orders.Any())
                return NotFound("No eligible orders found.");

            return Ok(orders);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitReturnRequest([FromBody] ReturnRequestModel returnRequest)
        {
            if (returnRequest == null)
            {
                return BadRequest("Request body is null.");
            }

            try
            {
                var validationResult = await _returnService.ValidateReturnRequestAsync(returnRequest);

                var returnId = await _returnService.CreateReturnRequestAsync(returnRequest);

                return Ok(new
                {
                    ReturnId = returnId,
                    Message = "Return request submitted successfully."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitReturnRequest: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("label/{returnId}")]
        public async Task<IActionResult> GenerateReturnLabel(Guid returnId)
        {
            var returnRequest = await _returnService.GenerateReturnLabelAsync(returnId);
            if (returnRequest == null)
            {
                return NotFound("Return request not found.");
            }

            return File(returnRequest, "application/pdf", "ReturnLabel.pdf");
        }


        [HttpGet("status/{returnId}")]
        public async Task<IActionResult> TrackReturnStatus(Guid returnId)
        {
            var status = await _returnService.GetReturnStatusAsync(returnId);
            if (status == null)
            {
                return NotFound("Return request not found.");
            }

            return Ok(status);
        }

        [HttpPatch("update-status/{returnId}")]
        public async Task<IActionResult> UpdateReturnStatus(Guid returnId, [FromBody] string status)
        {
            var updated = await _returnService.UpdateReturnStatusAsync(returnId, status);
            if (!updated)
            {
                return BadRequest("Failed to update return status.");
            }

            return Ok("Return status updated successfully.");
        }
    }
}
