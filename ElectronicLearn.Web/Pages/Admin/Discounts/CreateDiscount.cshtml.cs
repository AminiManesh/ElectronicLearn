using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElectronicLearn.Web.Pages.Admin.Discounts
{
    public class CreateDiscountModel : PageModel
    {
        private readonly IOrderService _orderService;
        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Discount Discount { get; set; }
        public void OnGet()
        {
            Discount = new Discount();
        }

        public IActionResult OnPost()
        {
            ModelState.ClearValidationState("Discount.UsersDiscounts");
            ModelState.MarkFieldValid("Discount.UsersDiscounts");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _orderService.AddDiscount(Discount);
            return RedirectToPage("Index");
        }
    }
}
