using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentService.API.DTOs;
using PaymentService.API.Services;

namespace PaymentService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IPaymentService paymentService,
            IMapper mapper,
            ILogger<PaymentsController> logger
        )
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PaymentResponseDto>> ProcessPayment(
            [FromBody] PaymentRequestDto paymentRequest
        )
        {
            try
            {
                var paymentResult = await _paymentService.ProcessPaymentAsync(paymentRequest);
                return Ok(paymentResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return StatusCode(500, "An error occurred while processing the payment");
            }
        }

        [HttpGet("{paymentId}")]
        [Authorize]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus(string paymentId)
        {
            try
            {
                var status = await _paymentService.GetPaymentStatusAsync(paymentId);
                return Ok(status);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Payment not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment status");
                return StatusCode(500, "An error occurred while retrieving the payment status");
            }
        }
    }
}
