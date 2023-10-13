using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var model = _orderService.GetUserOrders(userId);
            return View(model);
        }

        public IActionResult ShowOrder(int id, bool finaly = false)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _orderService.GetOrderForUserPanel(userId, id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.finaly = finaly;
            return View(order);
        }

        public IActionResult FinalOrder(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            if (_orderService.FinalOrder(userId, id))
            {
                return Redirect("/UserPanel/Order/ShowOrder/" + id + "/?finaly=true");
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult ApplyDiscount(int orderId, string code)
        {
            var result = _orderService.ApplyDiscount(orderId, code);
            return Redirect($"/UserPanel/Order/ShowOrder/{orderId}/?type={result}");
        }
    }
}