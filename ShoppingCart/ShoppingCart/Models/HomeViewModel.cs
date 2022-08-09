using ShoppingCart.Data.Entities;

namespace ShoppingCart.Models
{
    public class HomeViewModel
    {
       
        public float Quantity { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Category> Categories { get; set; }


    }
}
