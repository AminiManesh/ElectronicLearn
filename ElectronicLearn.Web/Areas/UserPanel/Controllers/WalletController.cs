using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZarinpalSandbox;

namespace ElectronicLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                return View(charge);
            }

            #region Send request to zarinpal
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            int transactionId = _walletService.AddTransaction(userId, charge.Amount, "شارژ کیف پول", true);

            var payment = new Payment(charge.Amount);
            var url = $"{Request.Scheme}://{Request.Host}";
            var res = payment.PaymentRequest("شارژ کیف پول", $"{url}/ChargeWallet/{transactionId}");
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }
            #endregion
        }
    }
}
