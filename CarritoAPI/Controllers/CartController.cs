using System.Runtime.CompilerServices;
using CarritoAPI.DTOs;
using CarritoAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarritoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateCart([FromQuery] string dni)
        {
            try
            {
                var cartId = await _cartService.CreateCartAsync(dni);
                return Ok( new { CartId = cartId });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("delete/{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            try
            {
                await _cartService.DeleteCartAsync(cartId);
                return Ok("Cart deleted.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("{carId}/add-product")]
        public async Task<IActionResult> AddProduct(int cartId, [FromBody] ProductDTO productDTO)
        {
            try
            {
                await _cartService.AddProductAsync(cartId, productDTO.Id, productDTO.Quantity);
                var status = await _cartService.GetCartStatus(cartId);
                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{cartId}/delete-product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int cartId, int productId)
        {
            try
            {
                await _cartService.DeleteProductAsync(cartId, productId);
                var status = await _cartService.GetCartStatus(cartId);
                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{cartId}/status")]
        public async Task<IActionResult> GetCartStatus(int cartId)
        {
            try
            {
                var status = await _cartService.GetCartStatus(cartId);
                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("user/{dni}/expensive-products")]
        public async Task<IActionResult> GetExpensiveProductsPerUser(string dni)
        {
            try
            {
                var products = await _cartService.GetExpensiveProductPerUserAsync(dni);
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
