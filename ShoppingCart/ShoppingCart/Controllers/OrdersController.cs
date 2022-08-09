using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Enums;
using ShoppingCart.Helpers;
using Vereyon.Web;

namespace ShoppingCart.Controllers
{
    public class OrdersController : Controller
    {

        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IOrderHelper _orderHelper;

        public OrdersController(DataContext context, IFlashMessage flashMessage, IOrderHelper orderHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _orderHelper = orderHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Product)
            .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Product)
            .ThenInclude(p => p.ProductImages)
            .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        public async Task<IActionResult> Dispatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden despachar pedidos que estén en estado 'nuevo'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Despachado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                
            _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'despachado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Despachado)
            {
                _flashMessage.Danger("Solo se pueden enviar pedidos que estén en estado 'despachado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Enviado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'enviado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Enviado)
            {
                _flashMessage.Danger("Solo se pueden confirmar pedidos que estén en estado 'enviado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Confirmado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'confirmado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
            return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus == OrderStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar un pedido que esté en estado 'cancelado'.");
            }
            else
            {
                await _orderHelper.CancelOrderAsync(sale.Id);
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'cancelado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Product)
            .Where(s => s.User.UserName == User.Identity.Name)
            .ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Product)
            .ThenInclude(p => p.ProductImages)
            .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }



    }
}
