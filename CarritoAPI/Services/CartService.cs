using CarritoAPI.Domain;
using CarritoAPI.DTOs;
using CarritoAPI.Repositories.Interfaces;
using CarritoAPI.Services.Interfaces;

namespace CarritoAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        public CartService(ICartRepository cartRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }
        public async Task AddProductAsync(int cartId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new Exception("Cart no fount.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new ItemCart
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _cartRepository.UpdateAsync(cart);
        }

        public async Task<CartStatusDTO> GetCartStatus(int cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new Exception("Cart no found.");

            var total = cart.Items.Sum(i => i.Product.Price * i.Quantity);

            if (cart.Items.Count == 5)
            {
                total *= 0.8m; // 20% discout
            }
            else if (cart.Items.Count > 10)
            {
                switch (cart.Type)
                {
                    case "Common":
                        total -= 200;
                        break;
                    case "SpecialDate":
                        total -= 500;
                        break;
                    case "VIP":
                        var cheaperProduct = cart.Items.Min(i => i.Product.Price);
                        total -= (700 + cheaperProduct);
                        break;
                }
            }

            return new CartStatusDTO
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new ItemDTO
                {
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList(),
                Total = total
            };
        }

        public async Task<int> CreateCartAsync(string dni)
        {
            var user = await _userRepository.GetByDniAsync(dni);
            if (user == null) throw new Exception("User no found.");

            var cartType = user.IsVip
                ? "VIP"
                : (DateTime.Now.DayOfWeek == DayOfWeek.Friday ? "SpecialDate" : "Common");

            var cart = new Cart
            {
                UserId = user.Id,
                Type = cartType,
            };
            await _cartRepository.AddAsync(cart);
            return cart.Id;
        }

        public async Task DeleteCartAsync(int cartId)
        {
            await _cartRepository.DeleteAsync(cartId);
        }

        public async Task DeleteProductAsync(int cartId, int productId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null) throw new Exception("Cart no found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _cartRepository.UpdateAsync(cart);
            }
        }

        public async Task<IEnumerable<Product>> GetExpensiveProductPerUserAsync(string dni)
        {
            var user = await _userRepository.GetByDniAsync(dni);
            if (user == null) throw new Exception("User no found.");

            return await _cartRepository.GetTopProductsByUserAsync(user.Id, 4);
        }

    }
}
