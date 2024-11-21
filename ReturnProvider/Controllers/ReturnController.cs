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

        // Retrieve eligible orders
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
            // Check if the request is null
            if (returnRequest == null)
            {
                return BadRequest("Request body is null.");
            }

            try
            {
                // Validate the return request
                var validationResult = await _returnService.ValidateReturnRequestAsync(returnRequest);
                //if (!string.IsNullOrEmpty(validationResult.ErrorMessage))
                //{
                //    return BadRequest(validationResult.ErrorMessage);
                //}

                // Submit the return request
                var returnId = await _returnService.CreateReturnRequestAsync(returnRequest);

                // Generate confirmation
                return Ok(new
                {
                    ReturnId = returnId,
                    Message = "Return request submitted successfully."
                });
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Error in SubmitReturnRequest: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // Generate a return label (PDF)
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


        // Track return status
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

        // Update return status (admin functionality)
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
