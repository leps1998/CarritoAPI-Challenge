namespace CarritoAPI.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Type { get; set; } = "Common";
        public List<ItemCart> Items { get; set; } = new List<ItemCart>();
        public decimal Total { get; set; }
    }
}
