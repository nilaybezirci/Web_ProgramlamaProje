namespace Eczane.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem>? CartItems { get; set; }
        public decimal total { get; set; }
    }
}
