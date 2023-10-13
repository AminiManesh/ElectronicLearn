using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Discounts
{
    public class EditDiscountModel : PageModel
    {
        private readonly IOrderService _orderService;
        public EditDiscountModel(IOrderService orderService)
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
            ModelState.ClearValidationState("Discount.UsersDiscounts");
            ModelState.MarkFieldValid("Discount.UsersDiscounts");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _orderService.UpdateDiscount(Discount);
            return RedirectToPage("Index");
        }
    }
}
