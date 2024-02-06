using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace ElectronicLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly ICourseService _courseService;
        public HomeController(IWalletService walletService, ICourseService courseService)
        {
            _walletService = walletService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            ViewBag.PopularCourses = _courseService.GetPopularCourses();
            return View(_courseService.GetCourses().Item1);
        }

        [Route("ChargeWallet/{id}")]
        public IActionResult ChargeWallet(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                var amount = _walletService.GetTransactionAmountById(id);
                var authority = HttpContext.Request.Query["Authority"].ToString();
                var payment = new Payment(amount);
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    _walletService.PayTransaction(id);
                    _walletService.UpdateUserBalance(id);
                    ViewBag.Code = res.RefId;
                    ViewBag.IsSuccess = true;
                }
            }

            return View();
        }
    }
}
