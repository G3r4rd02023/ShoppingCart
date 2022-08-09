using ShoppingCart.Common;
using ShoppingCart.Models;

namespace ShoppingCart.Helpers
{
    public interface IOrderHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> CancelOrderAsync(int id);
    }
}
