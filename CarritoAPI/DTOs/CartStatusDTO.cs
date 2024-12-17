namespace CarritoAPI.DTOs
{
    public class CartStatusDTO
    {
        public int Id { get; set; }
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
        public decimal Total { get; set; }
    }
}
