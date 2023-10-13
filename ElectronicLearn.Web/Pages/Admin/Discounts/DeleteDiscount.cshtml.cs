using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Discounts
{
    public class DeleteDiscountModel : PageModel
    {
        private readonly IOrderService _orderService;
        public DeleteDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Discount Discount { get; set; }

        public void OnGet(int discountId)
        {
            Discount = _orderService.GetDiscountById(discountId);
        }

        public IActionResult OnPost()
        {
            _orderService.RemoveDiscount(Discount.DiscountId);
            return RedirectToPage("Index");
        }
    }
}
