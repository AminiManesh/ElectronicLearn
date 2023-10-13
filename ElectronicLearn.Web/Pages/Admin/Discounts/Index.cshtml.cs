using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Discounts
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List<Discount> Discounts { get; set; }

        public void OnGet()
        {
            Discounts = _orderService.GetAllDiscounts();
        }
    }
}
